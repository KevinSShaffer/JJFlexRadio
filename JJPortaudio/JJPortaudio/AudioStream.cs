using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading;
using JJTrace;
using POpusCodec;
using PortAudioSharp;

namespace JJPortaudio
{
    public class JJAudioStream
    {
        private const uint defaultSampleRate = 48000;
        private const uint defaultBufsz = 4800;
        private Audio aud = null;

        /// <summary>
        /// buffer size used for this stream.
        /// </summary>
        public uint BufferSize { get { return aud.BufferSize; } }
        /// <summary>
        /// true if stream is active.
        /// </summary>
        public bool IsActive { get { return aud.IsActive; } }
        public Audio.AudioSentCallback AudioSent
        {
            get { return aud.AudioSent; }
            set { aud.AudioSent = value; }
        }

        /// <summary>
        /// Open this audio device.
        /// </summary>
        /// <param name="inOut">input/output</param>
        /// <param name="rate">sample rate</param>
        /// <param name="inCallback">called with input data, type input only</param>
        /// <param name="audioCallback">(optional) audio callback</param>
        /// <param name="cbPerSec">(optional) callbacks per second, default 10</param>
        /// <returns>true on success</returns>
        public bool OpenAudio(Devices.DeviceTypes inOut, uint rate, Audio.WavCallback inCallback = null,
            PortAudio.PaStreamCallbackDelegate audioCallback = null, int cbPerSec = 10)
        {
            Tracing.TraceLine("audioStream.open:" + inOut.ToString() + ' ' + rate, TraceLevel.Info);
            aud = new Audio();
            aud.WavInputHandler = inCallback;
            bool rv = aud.Open(inOut, rate, false, audioCallback, cbPerSec);
            if (!rv)
            {
                aud.Finished();
            }
            return rv;
        }

        /// <summary>
        /// Open an opus device.
        /// </summary>
        /// <param name="inOut">input/output</param>
        /// <param name="sampleRate">(optional) sample rate</param>
        /// <param name="inCallback">called with input data, type input only</param>
        /// <param name="audioCallback">(optional) audio callback</param>
        /// <param name="cbPerSec">(optional) callbacks per second, default 10</param>
        /// <returns>true on success</returns>
        public bool OpenOpus(Devices.DeviceTypes inOut, uint sampleRate, Audio.OpusCallback inCallback = null,
            PortAudio.PaStreamCallbackDelegate audioCallback=null, int cbPerSec = 10)
        {
            Tracing.TraceLine("audioStream.OpenOpus:" + sampleRate, TraceLevel.Info);

            // Open the device.
            aud = new Audio();
            aud.OpusInputHandler = inCallback;
            if (!aud.Open(inOut, sampleRate, true, audioCallback, cbPerSec))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Start an open audio device.
        /// </summary>
        /// <returns>True on success</returns>
        public bool StartAudio()
        {
            Tracing.TraceLine("audioStream.Start", TraceLevel.Info);
            bool rv = aud.Start();
            if (!rv)
            {
                aud.Finished();
            }
            return rv;
        }

        /// <summary>
        /// Stop an open audio device.
        /// </summary>
        /// <returns>True on success</returns>
        public bool StopAudio()
        {
            aud.TheQ.Clear();
            Tracing.TraceLine("audioStream.Stop", TraceLevel.Info);
            aud.Stop();
            return true;
        }

        /// <summary>
        /// Write audio data
        /// </summary>
        /// <param name="data">float data array</param>
        public void Write(float[] data)
        {
            // don't pass data directly.
            float[] buf = new float[data.Length];
            Array.Copy(data, buf, data.Length);
            aud.TheQ.Enqueue(buf);
        }

        /// <summary>
        /// Write opus data
        /// </summary>
        /// <param name="data">byte array</param>
        public void WriteOpus(byte[] data)
        {
            float[] buf = aud.Decoder.DecodePacketFloat(data);
            aud.TheQ.Enqueue(buf);
        }

        public float[] OpusDecode(byte[] buf)
        {
            return aud.Decoder.DecodePacketFloat(buf);
        }

        /// <summary>
        /// Drain the queue.
        /// </summary>
        public void Flush()
        {
                while (aud.IsActive && (aud.TheQ.Count > 0)) Thread.Sleep(1);
        }

        /// <summary>
        /// Cleanup
        /// </summary>
        public void Close()
        {
            if (aud != null) aud.Finished();
        }

#if zero
        // Region - beep
        #region beep
        /// <summary>
        /// Open a beeper
        /// </summary>
        /// <returns>true on success</returns>
        public bool BeepOpen()
        {
            Tracing.TraceLine("AudioStream.BeepOpen", TraceLevel.Info);
            return Beep.Open();
        }

        /// <summary>
        /// Start a beeper
        /// </summary>
        public void BeepStart()
        {
            Beep.Start();
        }

        /// <summary>
        /// stop a beeper, doesn't close it.
        /// </summary>
        public void BeepStop()
        {
            Beep.End();
        }

        /// <summary>
        /// sound a beep
        /// </summary>
        public void BeepOn()
        {
            Beep.On();
        }

        /// <summary>
        ///  silence the beep
        /// </summary>
        public void BeepOff()
        {
            Beep.Off();
        }

        /// <summary>
        /// Set beep frequency
        /// </summary>
        /// <param name="f">freq in HZ</param>
        public void BeepFreq(uint f)
        {
            Beep.Freq(f);
        }

        /// <summary>
        /// set beep amplitude
        /// </summary>
        /// <param name="a">amplitude, usually 1.</param>
        public void BeepAmplitude(float a)
        {
            Beep.Amplitude(a);
        }

        /// <summary>
        /// close the beeper
        /// </summary>
        public void BeepExit()
        {
            Beep.Finished();
        }
        #endregion
#endif
    }
}
