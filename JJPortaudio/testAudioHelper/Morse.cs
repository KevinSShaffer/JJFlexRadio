using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using JJTrace;
using JJPortaudio;

namespace testAudioHelper
{
    public class Morse
    {
        // dit, or element, length in ms = ParisDividend/wpm.
        private const int ParisDividend = 1200;
        private float secondsPerElement
        {
            get { return ((float)ParisDividend / (float)_Speed) / 1000f; }
        }
        private float samplesPerElement
        {
            get { return (float)sampleRate * secondsPerElement; }
        }

        // spacing
        private const byte intraChar = 1;
        private const byte interChar = 3;
        private const byte interWord = 7;

        // character elements
        private const byte ignore = 0;
        private const byte space = 1;
        private const byte dit = 2;
        private const byte dah = 3;
        private const byte etx = 4;

        /// <summary>
        /// Morse code table
        /// </summary>
        /// <remarks>
        /// This jagged array contains one array per character.
        /// Each array specifies only the active elements, (i.e.) doesn't include intracharacter spaces.
        /// </remarks>
        private static byte[][] codeTable = new byte[128][]
        {
            // 0x00-0x0f - unused
            new byte[] { ignore },
            new byte[] { ignore },
            new byte[] { ignore },
            new byte[] { ignore },
            new byte[] { ignore },
            new byte[] { ignore },
            new byte[] { ignore },
            new byte[] { ignore },
            new byte[] { ignore },
            new byte[] { ignore },
            new byte[] { ignore },
            new byte[] { ignore },
            new byte[] { ignore },
            new byte[] { ignore },
            new byte[] { ignore },
            new byte[] { ignore },
            // 0x10 unused
            new byte[] { ignore },
            new byte[] { ignore },
            new byte[] { ignore },
            new byte[] { ignore },
            new byte[] { ignore },
            new byte[] { ignore },
            new byte[] { ignore },
            new byte[] { ignore },
            new byte[] { ignore },
            new byte[] { ignore },
            new byte[] { ignore },
            new byte[] { ignore },
            new byte[] { ignore },
            new byte[] { ignore },
            new byte[] { ignore },
            new byte[] { ignore },
            // 0x20-0x2f blank ! " # $ % & ' ( ) * + , - . /
            new byte[] { space}, // blank
            new byte[] { dah, dit, dah, dit, dah, dah }, // !
            new byte[] { dit, dah, dit, dit, dah, dit }, // "
            new byte[] { space }, // #
            new byte[] { dit, dit, dit, dah, dit, dit, dah }, // $
            new byte[] { space }, // %
            new byte[] { dit, dah, dit, dit, dit }, // &
            new byte[] { dit, dah, dah, dah, dah, dit }, // '
            new byte[] { dah, dit, dah, dah, dit }, // (
            new byte[] { dah, dit, dah, dah, dit, dah }, // )
            new byte[] { space }, // *
            new byte[] { dit, dah, dit, dah, dit }, // +
            new byte[] { dah, dah, dit, dit, dah, dah }, // ,
            new byte[] { dah, dit, dit, dit, dit, dah }, // -
            new byte[] { dit, dah, dit, dah, dit, dah }, // .
            new byte[] { dah, dit, dit, dah, dit }, // /
            // 0x30-0x3f 0-9 : ; < = > ?
            new byte[] { dah, dah, dah, dah, dah }, // 0
            new byte[] { dit, dah, dah, dah, dah }, // 1
            new byte[] { dit, dit, dah, dah, dah }, // 2
            new byte[] { dit, dit, dit, dah, dah }, // 3
            new byte[] { dit, dit, dit, dit, dah }, // 4
            new byte[] { dit, dit, dit, dit, dit }, // 5
            new byte[] { dah, dit, dit, dit, dit }, // 6
            new byte[] { dah, dah, dit, dit, dit }, // 7
            new byte[] { dah, dah, dah, dit, dit }, // 8
            new byte[] { dah, dah, dah, dah, dit }, // 9
            new byte[] { dah, dah, dah, dit, dit, dit }, // :
            new byte[] { dah, dit, dah, dit, dah, dit }, // ;
            new byte[] { space }, // <
            new byte[] { dah, dit, dit, dit, dah }, // =
            new byte[] { space }, // >
            new byte[] { dit, dit, dah, dah, dit, dit }, // ?
            // 0x40-0x4f @ A-O
            new byte[] { dit, dah, dah, dit, dah, dit }, // @
            new byte[] { dit, dah }, // A
            new byte[] { dah, dit, dit, dit }, // B
            new byte[] { dah, dit, dah, dit }, // C
            new byte[] { dah, dit, dit }, // D
            new byte[] { dit }, // E
            new byte[] { dit, dit, dah, dit }, // F
            new byte[] { dah, dah, dit }, // G
            new byte[] { dit, dit, dit, dit }, // H
            new byte[] { dit, dit }, // I
            new byte[] { dit, dah, dah, dah }, // J
            new byte[] { dah, dit, dah }, // K
            new byte[] { dit, dah, dit, dit }, // L
            new byte[] { dah, dah }, // M
            new byte[] { dah, dit }, // N
            new byte[] { dah, dah, dah }, // O
            // 0x50-0x5f P-Z [ \ ] ^ _
            new byte[] { dit, dah, dah, dit }, // P
            new byte[] { dah, dah, dit, dah }, // Q
            new byte[] { dit, dah, dit }, // R
            new byte[] { dit, dit, dit }, // S
            new byte[] { dah }, // T
            new byte[] { dit, dit, dah }, // U
            new byte[] { dit, dit, dit, dah }, // V
            new byte[] { dit, dah, dah }, // W
            new byte[] { dah, dit, dit, dah }, // X
            new byte[] { dah, dit, dah, dah }, // Y
            new byte[] { dah, dah, dit, dit }, // Z
            new byte[] { space }, // [
            new byte[] { space }, // \
            new byte[] { space }, // ]
            new byte[] { space }, // ^
            new byte[] { dit, dit, dah, dah, dit, dah }, // _
            // 0x60-0x6f ` a-o 
            new byte[] { space }, // `
            new byte[] { dit, dah }, // a
            new byte[] { dah, dit, dit, dit }, // b
            new byte[] { dah, dit, dah, dit }, // c
            new byte[] { dah, dit, dit }, // d
            new byte[] { dit }, // e
            new byte[] { dit, dit, dah, dit }, // f
            new byte[] { dah, dah, dit }, // g
            new byte[] { dit, dit, dit, dit }, // h
            new byte[] { dit, dit }, // i
            new byte[] { dit, dah, dah, dah }, // j
            new byte[] { dah, dit, dah }, // k
            new byte[] { dit, dah, dit, dit }, // l
            new byte[] { dah, dah }, // m
            new byte[] { dah, dit }, // n
            new byte[] { dah, dah, dah }, // o
            // 0x70-0x7f p-z { | } ~ [delta]
            new byte[] { dit, dah, dah, dit }, // p
            new byte[] { dah, dah, dit, dah }, // q
            new byte[] { dit, dah, dit }, // r
            new byte[] { dit, dit, dit }, // s
            new byte[] { dah }, // t
            new byte[] { dit, dit, dah }, // u
            new byte[] { dit, dit, dit, dah }, // v
            new byte[] { dit, dah, dah }, // w
            new byte[] { dah, dit, dit, dah }, // x
            new byte[] { dah, dit, dah, dah }, // y
            new byte[] { dah, dah, dit, dit }, // z
            new byte[] { space }, // {
            new byte[] { space }, // |
            new byte[] { space }, // }
            new byte[] { space }, // ~
            new byte[] { space }, // [delta]
        };

        private const uint sampleRate = 24000;
        private const uint defaultFreq = 700;
        private uint _Frequency = defaultFreq;
        /// <summary>
        /// get/set frequency
        /// </summary>
        public uint Frequency
        {
            get { return _Frequency; }
            set { _Frequency = value; }
        }

        private const uint defaultSpeed = 20;
        private uint _Speed = defaultSpeed;
        /// <summary>
        /// get/set the speed
        /// </summary>
        public uint Speed
        {
            get { return _Speed; }
            set { _Speed = value; }
        }

        private float samplesPerCycle
        {
            get { return (float)sampleRate / (float)_Frequency; }
        }
        private const float omega = (2 * (float)Math.PI);
        private float sampleLength
        {
            get { return omega / samplesPerCycle; }
        }

        private JJAudioStream aud;
        private bool audOpen { get { return (aud != null); } }

        /// <summary>
        /// Morse sender
        /// </summary>
        public Morse()
        {
            Tracing.TraceLine("Morse", TraceLevel.Info);
            aud = new JJAudioStream();
            if (!aud.OpenAudio(Devices.DeviceTypes.output, sampleRate))
            {
                Tracing.TraceLine("Morse:stream didn't open", TraceLevel.Error);
                aud = null;
            }
        }

        /// <summary>
        /// Close
        /// </summary>
        public void Close()
        {
            Tracing.TraceLine("Morse.Close", TraceLevel.Info);
            aud.Close();
            aud = null;
        }

        /// <summary>
        /// Start the monitor
        /// </summary>
        /// <returns>true on success</returns>
        public bool Start()
        {
            Tracing.TraceLine("Morse.Start", TraceLevel.Info);
            return (audOpen && aud.StartAudio());
        }

        /// <summary>
        /// stop the monitor
        /// </summary>
        /// <returns>true</returns>
        public bool Stop()
        {
            Tracing.TraceLine("Morse.Stop", TraceLevel.Info);
            return (audOpen && aud.StopAudio());
        }

        /// <summary>
        /// Send a character
        /// </summary>
        /// <param name="c">the character</param>
        public void Send(char c)
        {
            Tracing.TraceLine("Morse.send:" + ((int)c).ToString(), TraceLevel.Info);
            if (c > codeTable.Length)
            {
                // Invalid input.
                return;
            }

            byte[] b = codeTable[c];
            if (b[0] == ignore) return;

            bool sound = false;
            uint elements = 0;
            // send initial element.
            sendElement(b[0], ref sound, ref elements);
            for (int i = 1; i < b.Length; i++)
            {
                sendElement(space, ref sound, ref elements); // intrachar
                sendElement(b[i], ref sound, ref elements);
            }
            // output intercharacter space
            for (int i = 0; i < interChar; i++) { sendElement(space, ref sound, ref elements); }
            writeBuf(sound, elements); // force out the space
        }

        /// <summary>
        /// Send a morse string
        /// </summary>
        /// <param name="chars">the string</param>
        public void Send(string chars)
        {
            foreach(char c in chars)
            {
                Send(c);
            }
        }

        private void sendElement(byte el,
            ref bool sound, ref uint elements)
        {
            if (sound)
            {
                if (el == space)
                {
                    writeBuf(sound, elements);
                    sound = false;
                    elements = 0;
                }
            }
            else
            {
                // !sound
                if (el != space)
                {
                    if (elements > 0) writeBuf(sound, elements);
                    sound = true;
                    elements = 0;
                }
            }
            elements += (uint)((el == dah) ? 3 : 1);
        }

        private void writeBuf(bool sound, uint elements)
        {
            float samples = elements * samplesPerElement;
            float[] buf = new float[(int)(samples * 2)]; // left and right channels
            float val = 0;
            float incr = sampleLength;
            for (int i = 0; i < buf.Length; i += 2)
            {
                if (sound)
                {
                    buf[i] = (float)Math.Sin(val);
                    val += incr;
                }
                else buf[i] = 0f;
                buf[i + 1] = buf[i];
            }
            aud.Write(buf);
        }
    }
}
