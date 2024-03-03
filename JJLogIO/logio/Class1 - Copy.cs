using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace LogIO
{
    public class LogIO
    {
        public string fileName;
        FileStream fs;
        BinaryReader rfs;
        BinaryWriter wfs;
        // const long nullPTR = 0xffffffffffffffff;
        const long nullPTR = -1;
        // The cursor points to the current record.
        long cursor;
        public long Position
        {
        get { return cursor; }
        }
        struct listPointers
        {
            public long prev;
            public long next;
        }
        struct recordData
        {
            public listPointers header;
            public uint Length;
            char[] data;
            public recordData(long prev, string str)
            {
                // Constructor - record to be added.
                header.prev = prev;
                header.next = nullPTR;
                Length = (uint)str.Length;
                data = str.ToCharArray(0, str.Length);
            }
            public recordData(long prev, long next, string str)
            {
                // Constructor - record to be inserted.
                header.prev = prev;
                header.next = next;
                Length = (uint)str.Length;
                data = str.ToCharArray(0, str.Length);
            }
            public void Read(BinaryReader rfs, bool justHeader)
            {
                // (overloaded) read just header or entire record.
                try
                {
                    header.prev = rfs.ReadInt64();
                    header.next = rfs.ReadInt64();
                    if (!justHeader)
                    {
                        Length = rfs.ReadUInt32();
                        data = rfs.ReadChars((Int32)Length);
                    }
                }
                catch
                {
                    throw;
                }
            }
            public void Read(BinaryReader rfs)
            {
                // (overloaded) read entire record.
                Read(rfs, false);
            }
            public string ReadString(BinaryReader rfs)
            {
                Read(rfs);
                return new string(data);
            }
            public void Write(BinaryWriter wfs, bool justHeader)
            {
                // (overloaded) write just header or entire record.
                try
                {
                    wfs.Write(header.prev);
                    wfs.Write(header.next);
                    if (!justHeader)
                    {
                        wfs.Write(Length);
                        wfs.Write(data, 0, (Int32)Length);
                    }
                }
                catch
                {
                    throw;
                }
            }
            public void Write(BinaryWriter wfs)
            {
                // (overloaded) write entire record.
                Write(wfs, false);
            }
        }
        // File header
        const int Version1 = 1;
        const int logVersion = Version1;
        const string headerMagic = "JJLG";
        struct fh
        {
            char[] magic;
            public fh(char[] m)
            {
                magic = m;
            }
        }
        struct fileHeaderData
        {
            public char m0, m1, m2, m3;
            public int version;
            public long first;
            public long last;
            public const int Length = 4 * sizeof(char) + sizeof(int) + 2 * sizeof(long);
            public fileHeaderData(long f, long l)
            {
                m0 = 'J';
                m1 = 'J';
                m2 = 'L';
                m3 = 'G';
                version = logVersion;
                first = f;
                last = l;
            }
            public void Read(BinaryReader rfs)
            {
                rfs.BaseStream.Position = 0;
                try
                {
                    m0 = rfs.ReadChar();
                    m1 = rfs.ReadChar();
                    m2 = rfs.ReadChar();
                    m3 = rfs.ReadChar();
                    if (!goodMagic(this))
                    {
                        throw new Exception();
                    }
                    version = rfs.ReadInt32();
                    first = rfs.ReadInt64();
                    last = rfs.ReadInt64();
                }
                catch
                {
                    throw;
                }
            }
            public void Write(BinaryWriter wfs)
            {
                wfs.BaseStream.Position = 0;
                    // The log header must be in fileHeader.
                    try
                    {
                        wfs.Write(m0);
                        wfs.Write(m1);
                        wfs.Write(m2);
                        wfs.Write(m3);
                        wfs.Write(version);
                        wfs.Write(first);
                        wfs.Write(last);
                    }
                    catch
                    {
                        throw;
                    }
            }
        }
        fileHeaderData fileHeader;

        static Exception notALogFile(FileStream fs)
        {
            return new Exception(@fs.Name + " is not a valid log file.");
        }
        static Exception corruptLog(FileStream fs)
        {
            string msg = "Log is corrupted at offset " + fs.Position.ToString();
            return new Exception(msg);
        }
        static Exception BadPosition(long pos)
        {
            return new Exception("Bad position: " + pos);
        }

        // Constructor
        public LogIO(string fn)
        {
            fileName = fn;
            try
            {
                fs = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                rfs = new BinaryReader(fs);
                wfs = new BinaryWriter(fs);
            }
            catch
            {
                throw;
            }
            if (fs.Length>0)
            {
                // The file exists.
                try { fileHeader.Read(rfs); }
                catch { throw notALogFile(fs); }
                cursor = fs.Position;
            }
            else
            {
                // Create a new file.
                fileHeader = new fileHeaderData(nullPTR, nullPTR);
                try { fileHeader.Write(wfs); }
                catch { throw; }
            }
        }

        static bool goodMagic(fileHeaderData hdr)
        {
            return ((hdr.m0 == 'J') && (hdr.m1 == 'J') && (hdr.m2 == 'L') && (hdr.m3 == 'G')) ? true : false;
        }

        static bool goodRecordHeader(FileStream fs, recordData rec)
        {
            return (((rec.header.prev == nullPTR) ||
                 ((rec.header.prev >= fileHeaderData.Length)) &&
                  (rec.header.prev < fs.Length)) &&
                ((rec.header.next == nullPTR) ||
                 ((rec.header.next >= fileHeaderData.Length) &&
                  (rec.header.next < fs.Length)))) ? true: false;
        }

        public long SeekToFirst()
        {
            // Seek to the first record.
            cursor = fileHeader.first;
            fs.Position = cursor;
            return cursor;
        }

        public long SeekToLast()
        {
            // Seek to the last record.
            cursor = fileHeader.last;
            fs.Position = cursor;
            return cursor;
        }

        public long SeekToPosition(long pos)
        {
            fs.Position = pos;
            recordData rec = new recordData();
            // Read the record's header.
            try { rec.Read(rfs, true); }
            catch { throw BadPosition(pos); }
            if (!goodRecordHeader(fs, rec)) {
                throw BadPosition(pos);
            }
            cursor = pos;
            fs.Position = pos;
            return cursor;
        }

        public string Read()
        {
            fs.Position = cursor;
            recordData rec = new recordData();
            string str = null;
            try { str = rec.ReadString(rfs); }
            catch { throw; }
            cursor = fs.Position;
            return str;
        }

        public void Append(string str)
        {
            // Append a record.
            // We're positioned at the new record.
            fs.Position = fs.Length;
            cursor = fs.Position;
            recordData rec = new recordData(fileHeader.last, str);
            try { rec.Write(wfs); }
            catch { throw; }
            // fileHeader.last is null means this is the first record, (i.e.) a new file.
            if (fileHeader.last != nullPTR)
            {
                // Update the previous last record's header.
                fs.Position = fileHeader.last;
                try { rec.Read(rfs, true); }
                catch { throw; }
                fs.Position = fileHeader.last;
                // Last record must have a null next pointer.
                if (rec.header.next != nullPTR)
                {
                    throw corruptLog(fs);
                }
                rec.header.next = cursor;
                try { rec.Write(wfs, true); }
                catch { throw; }
            }
            fileHeader.last = cursor;
            if (fileHeader.first == nullPTR)
            {
                fileHeader.first = cursor;
            }
            try { fileHeader.Write(wfs); }
            catch { throw; }
            fs.Position = cursor;
        }

        public void Update(string str)
        {
            // Update the record at the cursor.
            fs.Position = cursor;
            recordData rec = new recordData();
            try { rec.Read(rfs); }
            catch { throw BadPosition(fs.Position); }
            if (!goodRecordHeader(fs, rec))
            {
                throw BadPosition(fs.Position);
            }
            if (str.Length <= rec.Length)
            {
                // We can update in place.
                fs.Position = cursor;
                recordData rec2 = new recordData(rec.header.prev, rec.header.next, str);
                try { rec2.Write(wfs); }
                catch { throw; }
            }
            else
            {
                // Put the updated record at the end.
                long newcursor = fs.Length;
                fs.Position = newcursor;
                recordData rec2 = new recordData(rec.header.prev, rec.header.next, str);
                try { rec2.Write(wfs); }
                catch { throw; }
                // Update the previous record's next pointer if there is one.
                if (rec.header.prev != nullPTR)
                {
                    fs.Position = rec.header.prev;
                    try
                    {
                        rec2.Read(rfs, true);
                        rec2.header.next = newcursor;
                        rec2.Write(wfs, true);
                    }
                    catch { throw; }
                }
                // Fix the previous position of the next record if there is one.
                if (rec.header.next != nullPTR)
                {
                    fs.Position = rec.header.next;
                    try
                    {
                        rec2.Read(rfs, true);
                        rec2.header.prev = newcursor;
                        rec2.Write(wfs, true);
                    }
                    catch { throw; }
                }
            }
        }
    }
}
