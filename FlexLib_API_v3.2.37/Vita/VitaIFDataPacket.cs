﻿// ****************************************************************************
///*!	\file VitaDataPacket.cs
// *	\brief Defines a Vita IF Data Packet
// *
// *	\copyright	Copyright 2012-2017 FlexRadio Systems.  All Rights Reserved.
// *				Unauthorized use, duplication or distribution of this software is
// *				strictly prohibited by law.
// *
// *	\date 2012-03-05
// *	\author Eric Wachsmann, KE5DTO
// */
// ****************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

using Flex.Util;
using System.Diagnostics;

namespace Flex.Smoothlake.Vita
{
    /// <summary>
    /// Represents a single Vita IF Data Packet as defined in the Vita 49 Standard Section 6.1.
    /// Can also represent an Extended Data Packet as seen in Section 6.2.
    /// </summary>
    public class VitaIFDataPacket
    {
        public Header header;
        public uint stream_id;
        public VitaClassID class_id;
        public uint timestamp_int;
        public ulong timestamp_frac;
        public float[] payload;
        public Int16[] payload_int16;
        public Trailer trailer;

        public int Length;

        public VitaIFDataPacket()
        {
            header = new Header();
            header.pkt_type = VitaPacketType.IFDataWithStream;
            header.c = true;
            header.t = true;
            header.tsi = VitaTimeStampIntegerType.Other;
            header.tsf = VitaTimeStampFractionalType.RealTime;
            trailer = new Trailer();
        }

        private double ONE_OVER_ZERO_DBFS = 1.0 / Math.Pow(2, 15);
        //private static int samples = 0;
        Random ran = new Random();
        public VitaIFDataPacket(byte[] data, int bytes)
        {
            Length = bytes;

            int index = 0;
            uint temp = ByteOrder.SwapBytes(BitConverter.ToUInt32(data, index));
            index += 4;

            header = new Header();
            header.pkt_type = (VitaPacketType)(temp >> 28);
            header.c = ((temp & 0x08000000) != 0);
            header.t = ((temp & 0x04000000) != 0);
            header.tsi = (VitaTimeStampIntegerType)((temp >> 22) & 0x03);
            header.tsf = (VitaTimeStampFractionalType)((temp >> 20) & 0x03);
            header.packet_count = (byte)((temp >> 16) & 0x0F);
            header.packet_size = (ushort)(temp & 0xFFFF);

            // if packet is a type with a stream id, read/save it
            if (header.pkt_type == VitaPacketType.IFDataWithStream || 
                header.pkt_type == VitaPacketType.ExtDataWithStream)
            {
                stream_id = ByteOrder.SwapBytes(BitConverter.ToUInt32(data, index));
                index += 4;
            }

            if(header.c)
            {
                temp = ByteOrder.SwapBytes(BitConverter.ToUInt32(data, index));
                index += 4;
                class_id.OUI = temp & 0x00FFFFFF;
            
                temp = ByteOrder.SwapBytes(BitConverter.ToUInt32(data, index));
                index += 4;
                class_id.InformationClassCode = (ushort)(temp >> 16);
                class_id.PacketClassCode = (ushort)temp;
            }

            if (header.tsi != VitaTimeStampIntegerType.None)
            {
                timestamp_int = ByteOrder.SwapBytes(BitConverter.ToUInt32(data, index));
                index += 4;
            }

            if (header.tsf != VitaTimeStampFractionalType.None)
            {
                timestamp_frac = ByteOrder.SwapBytes(BitConverter.ToUInt64(data, index));
                index += 8;
            }

            int payload_bytes = (header.packet_size-1)*4; // -1 for header
            switch (header.pkt_type)
            {
                case VitaPacketType.IFDataWithStream:
                case VitaPacketType.ExtDataWithStream:
                    payload_bytes -= 4;
                    break;
            }

            if(header.c) payload_bytes -= 8;            
            if(header.tsi != VitaTimeStampIntegerType.None) payload_bytes -= 4;
            if(header.tsf != VitaTimeStampFractionalType.None) payload_bytes -= 8;
            if (header.t) payload_bytes -= 4;

            // swap endianess on the bytes
            // for each sample if we are dealing with DAX audio.
            if (class_id.PacketClassCode == VitaFlex.SL_VITA_IF_NARROW_CLASS)
            {
                payload = new float[payload_bytes / sizeof(float)];

                Debug.Assert(payload_bytes % 4 == 0);

                for (int i = 0; i < payload_bytes / 4; i++)
                {
                    // swap outer bytes -- 0 and 3
                    byte b = data[index + i * 4];
                    data[index + i * 4] = data[index + i * 4 + 3];
                    data[index + i * 4 + 3] = b;

                    // swap inner bytes -- 1 and 2
                    b = data[index + i * 4 + 1];
                    data[index + i * 4 + 1] = data[index + i * 4 + 2];
                    data[index + i * 4 + 2] = b;
                }
                // copy the data as is -- it is already floating point
                Buffer.BlockCopy(data, index, payload, 0, payload_bytes);
            }
            else if (class_id.PacketClassCode == VitaFlex.SL_VITA_IF_NARROW_REDUCED_BW_CLASS)
            {
                //Int16 Mono Samples 
                float one_over_max = 1.0f / (float)Int16.MaxValue;

                Int16[] samples = new Int16[payload_bytes / sizeof(Int16)];
                payload = new float[samples.Length * 2]; // Must be twice the length since we're going from MONO to STEREO

                // Convert from network order
                for (int i = 0; i < samples.Length; i++)
                {
                    short val = BitConverter.ToInt16(data, index + i * 2);
                    samples[i] = IPAddress.NetworkToHostOrder(val);
                }

                // Convert from int16 to floats and duplicate mono channel
                for (int i = 0; i < payload.Length; i += 2)
                {
                    float val = samples[i / 2] * one_over_max;
                    payload[i] = val;
                    payload[i + 1] = val;
                }
            }
            else if (class_id.PacketClassCode == VitaFlex.SL_VITA_IF_WIDE_CLASS_192kHz ||
                         class_id.PacketClassCode == VitaFlex.SL_VITA_IF_WIDE_CLASS_96kHz ||
                         class_id.PacketClassCode == VitaFlex.SL_VITA_IF_WIDE_CLASS_48kHz ||
                         class_id.PacketClassCode == VitaFlex.SL_VITA_IF_WIDE_CLASS_24kHz)
            {
                payload = new float[payload_bytes / sizeof(float)];

                // copy the data as is -- it is already floating point
                Buffer.BlockCopy(data, index, payload, 0, payload_bytes);

                for (int i = 0; i < payload.Length; i++)
                {
                    payload[i] = payload[i] * (float)ONE_OVER_ZERO_DBFS;
                }
            }

            index += payload_bytes;

            if (header.t)
            {
                temp = ByteOrder.SwapBytes(BitConverter.ToUInt32(data, index));
                trailer.CalibratedTimeEnable = (temp & 0x80000000) != 0;
                trailer.ValidDataEnable = (temp & 0x40000000) != 0;
                trailer.ReferenceLockEnable = (temp & 0x20000000) != 0;
                trailer.AGCMGCEnable = (temp & 0x10000000) != 0;
                trailer.DetectedSignalEnable = (temp & 0x08000000) != 0;
                trailer.SpectralInversionEnable = (temp & 0x04000000) != 0;
                trailer.OverrangeEnable = (temp & 0x02000000) != 0;
                trailer.SampleLossEnable = (temp & 0x01000000) != 0;

                trailer.CalibratedTimeIndicator = (temp & 0x00080000) != 0;
                trailer.ValidDataIndicator = (temp & 0x00040000) != 0;
                trailer.ReferenceLockIndicator = (temp & 0x00020000) != 0;
                trailer.AGCMGCIndicator = (temp & 0x00010000) != 0;
                trailer.DetectedSignalIndicator = (temp & 0x00008000) != 0;
                trailer.SpectralInversionIndicator = (temp & 0x00004000) != 0;
                trailer.OverrangeIndicator = (temp & 0x00002000) != 0;
                trailer.SampleLossIndicator = (temp & 0x00001000) != 0;

                trailer.e = (temp & 0x80) != 0;
                trailer.AssociatedContextPacketCount = (byte)(temp & 0xEF);
            }
        }

        public byte[] ToBytes(bool use_int16_payload = false)
        {
            int index = 0;

            int num_bytes = 4 + 4; // Header + StreamID
            if (use_int16_payload)
            {
                num_bytes += payload_int16.Length * sizeof(Int16);
            }
            else
            {
                num_bytes += payload.Length * sizeof(float);
            }
            if (header.c) num_bytes += 8;
            if (header.t) num_bytes += 4;
            if (header.tsi != VitaTimeStampIntegerType.None) num_bytes += 4;
            if (header.tsf != VitaTimeStampFractionalType.None) num_bytes += 8;

            byte[] temp = new byte[num_bytes];
            byte b = (byte)((byte)header.pkt_type << 4 |
                Convert.ToByte(header.c) << 3 |
                Convert.ToByte(header.t) << 2);
            temp[0] = b;

            b = (byte)((byte)header.tsi << 6 |
                (byte)header.tsf << 4 |
                (byte)header.packet_count);
            temp[1] = b;

            temp[2] = (byte)(header.packet_size >> 8);
            temp[3] = (byte)header.packet_size;

            index += 4;

            Array.Copy(BitConverter.GetBytes(ByteOrder.SwapBytes(stream_id)), 0, temp, index, 4);
            index += 4;

            if (header.c)
            {
                Array.Copy(BitConverter.GetBytes(ByteOrder.SwapBytes(class_id.OUI)), 0, temp, index, 4);
                index += 4;

                Array.Copy(BitConverter.GetBytes(ByteOrder.SwapBytes(class_id.InformationClassCode)), 0, temp, index, 2);
                index += 2;

                Array.Copy(BitConverter.GetBytes(ByteOrder.SwapBytes(class_id.PacketClassCode)), 0, temp, index, 2);
                index += 2;
            }

            if (header.tsi != VitaTimeStampIntegerType.None)
            {
                Array.Copy(BitConverter.GetBytes(ByteOrder.SwapBytes(timestamp_int)), 0, temp, index, 4);
                index += 4;
            }

            if (header.tsf != VitaTimeStampFractionalType.None)
            {
                Array.Copy(BitConverter.GetBytes(ByteOrder.SwapBytes(timestamp_frac)), 0, temp, index, 8);
                index += 8;
            }

            if (use_int16_payload)
            {
                for (int i = 0; i < payload_int16.Length; i++)
                {
                    payload_int16[i] = IPAddress.HostToNetworkOrder(payload_int16[i]);
                }

                Buffer.BlockCopy(payload_int16, 0, temp, index, payload_int16.Length * sizeof(Int16));

                index += payload_int16.Length * sizeof(Int16);
            }
            else
            {
                // copy the payload
                Buffer.BlockCopy(payload, 0, temp, index, payload.Length * sizeof(float));

                // swap endianness of the samples
                for (int i = 0; i < payload.Length; i++)
                {
                    // swap outer bytes -- 0 and 3
                    b = temp[index + i * 4];
                    temp[index + i * 4] = temp[index + i * 4 + 3];
                    temp[index + i * 4 + 3] = b;

                    // swap inner bytes -- 1 and 2
                    b = temp[index + i * 4 + 1];
                    temp[index + i * 4 + 1] = temp[index + i * 4 + 2];
                    temp[index + i * 4 + 2] = b;
                }

                index += payload.Length * sizeof(float);
            }

            if(header.t)
            {
                b = (byte)(Convert.ToByte(trailer.CalibratedTimeEnable)     << 7 |
                           Convert.ToByte(trailer.ValidDataEnable)          << 6 |
                           Convert.ToByte(trailer.ReferenceLockEnable)      << 5 |
                           Convert.ToByte(trailer.AGCMGCEnable)             << 4 |
                           Convert.ToByte(trailer.DetectedSignalEnable)     << 3 |
                           Convert.ToByte(trailer.SpectralInversionEnable)  << 2 |
                           Convert.ToByte(trailer.OverrangeEnable)          << 1 |
                           Convert.ToByte(trailer.SampleLossEnable)         << 0);
                temp[index + 3] = b;
                
                b = (byte)(Convert.ToByte(trailer.CalibratedTimeIndicator)      << 3 |
                           Convert.ToByte(trailer.ValidDataIndicator)           << 2 |
                           Convert.ToByte(trailer.ReferenceLockIndicator)       << 1 |
                           Convert.ToByte(trailer.AGCMGCIndicator)              << 0);
                temp[index + 2] = b;

                b = (byte)(Convert.ToByte(trailer.DetectedSignalIndicator)      << 7 |
                           Convert.ToByte(trailer.SpectralInversionIndicator)   << 6 |
                           Convert.ToByte(trailer.OverrangeIndicator)           << 5 |
                           Convert.ToByte(trailer.SampleLossIndicator)          << 4);
                temp[index + 1] = b;

                b = (byte)(Convert.ToByte(trailer.e) << 7 |
                    trailer.AssociatedContextPacketCount);
                temp[index] = b;

                index += 4;
            }

            return temp;
        }
    }
}
