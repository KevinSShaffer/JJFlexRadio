using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Flex.Smoothlake.FlexLib;
using Ionic.Zip;
using JJTrace;

namespace Radios
{
    class FlexDB
    {
        private const string noFileMsg = "no import file found.";
        private const string errHdr = "Error";
        private const string statusHdr = "Status";
        private const string exportedMsg = "Export complete";
        private const string exportFailMsg = "Export failed - ";
        private const string exportTimeout = "timed out";
        private Flex rig;
        private string directoryName { get { return rig.ConfigDirectory + '\\' + rig.OperatorName + '\\' + "RigData"; } }
        private string metafileName { get { return "meta.txt"; } }

        public FlexDB(Flex r)
        {
            rig = r;
        }

        public bool Export()
        {
            Tracing.TraceLine("Flex export", TraceLevel.Info);
            bool rv = false;
            string tmpDir = directoryName + "\\tmp";
            string tmpMeta = tmpDir + "\\tmpMeta";
            try
            {
                if (!Directory.Exists(directoryName))
                {
                    Tracing.TraceLine("Flex export creating:" + directoryName, TraceLevel.Info);
                    Directory.CreateDirectory(directoryName);
                }
                else
                {
                    string[] fileList = Directory.GetFiles(directoryName);
                    foreach (string f in fileList)
                    {
                        string baseName = f.Substring(f.LastIndexOf('\\') + 1);
                        if ((baseName.Length > 11) && (baseName.Substring(0, 11) == "SSDR_Config"))
                        {
                            Tracing.TraceLine("Flex Export deleting file:" + f, TraceLevel.Info);
                            File.Delete(f);
                        }
                    }
                }

                // Now create the meta file.
                if (!Directory.Exists(tmpDir)) Directory.CreateDirectory(tmpDir);
                if (File.Exists(tmpMeta)) File.Delete(tmpMeta);

                using (StreamWriter sw = new StreamWriter(tmpMeta, true))
                {
                    sw.WriteLine("GLOBAL_PROFILES^" + Flex.JJRadioDefault + '^');
                    sw.WriteLine("TX_PROFILES^" + Flex.JJRadioDefault + '^');

                    // Export memories with an owner.
                    if (rig.theRadio.MemoryList.Count > 0)
                    {
                        string memString = "";
                        List<string> memNames = new List<string>();
                        foreach (Memory m in rig.theRadio.MemoryList)
                        {
                            string tmpStr = null;
                            if (!string.IsNullOrEmpty(m.Owner))
                            {
                                tmpStr = m.Owner + '|' + ((m.Group != null) ? m.Group : "") + '^';
                                // Add if unique.
                                if (!memNames.Contains(tmpStr))
                                {
                                    memNames.Add(tmpStr);
                                    memString += tmpStr;
                                }
                            }
                        }
                        if (memString != "") sw.WriteLine("MEMORIES^" + memString);
                    }

                    sw.WriteLine("BAND_PERSISTENCE^");
                    sw.WriteLine("MODE_PERSISTENCE^");
                    sw.WriteLine("GLOBAL_PERSISTENCE^");
                    if (rig.TNFs.Count > 0) sw.WriteLine("TNFS^");
                }
            }
            catch (Exception ex)
            {
                Tracing.ErrMessageTrace(ex, true);
                return rv;
            }

            // First. save the profile.
            rig.SaveProfile(true);

            // Now, export the data.
            rig.theRadio.DatabaseExportComplete = false;
            rig.q.Enqueue((Flex.FunctionDel)(() => { rig.theRadio.ReceiveSSDRDatabaseFile(tmpMeta, directoryName, false); }));
            if (!AllRadios.await(() => { return rig.theRadio.DatabaseExportComplete; }, 20000))
            {
                MessageBox.Show(exportFailMsg + exportTimeout, errHdr, MessageBoxButtons.OK);
            }
            else if (!string.IsNullOrEmpty(rig.theRadio.DatabaseExportException))
            {
                MessageBox.Show(exportFailMsg + rig.theRadio.DatabaseExportException, errHdr, MessageBoxButtons.OK);
            }
            else
            {
                rv = true;
                MessageBox.Show(exportedMsg, statusHdr, MessageBoxButtons.OK);
            }

            if (Directory.Exists(tmpDir)) Directory.Delete(tmpDir, true);
            return rv;
        }

        public bool Import()
        {
            Tracing.TraceLine("Flex import", TraceLevel.Info);
            bool rv = true;
            string theFile = null;
            try
            {
                string[] fileList = Directory.GetFiles(directoryName);
                foreach (string f in fileList)
                {
                    string baseName = f.Substring(f.LastIndexOf('\\') + 1);
                    if ((baseName.Length > 11) && (baseName.Substring(0, 11) == "SSDR_Config"))
                    {
                        theFile = f;
                    }
                }
            }
            catch (Exception ex)
            {
                Tracing.ErrMessageTrace(ex, true);
                return false;
            }
            if (theFile == null)
            {
                MessageBox.Show(noFileMsg, errHdr, MessageBoxButtons.OK);
                return false;
            }

            string tmpDir = directoryName + "\\tmp";
            string workingDir = Directory.GetCurrentDirectory();
            try
            {
                // unzip the flex_payload and meta_data files into a temp directory.
                if (Directory.Exists(tmpDir)) Directory.Delete(tmpDir, true);
                Directory.CreateDirectory(tmpDir);
                var options = new ReadOptions { StatusMessageWriter = System.Console.Out };
                using (ZipFile zip = ZipFile.Read(theFile, options))
                {
                    zip.ExtractAll(tmpDir);
                }
                if (!File.Exists(tmpDir + "\\meta_data"))
                {
                    throw new Exception("The archive must contain at least a meta_data file.");
                }
                // rename meta_data to meta_subset.
                File.Move(tmpDir + "\\meta_data", tmpDir + "\\meta_subset");
                // zip files to a temporary archive.
                Directory.SetCurrentDirectory(tmpDir);
                using (ZipFile zip = new ZipFile())
                {
                    string[] files = Directory.GetFiles(".");
                    foreach (string f in files)
                    {
                        zip.AddFile(f);
                    }
                    zip.Save("archive.zip");
                }
            }
            catch (Exception ex)
            {
                Tracing.ErrMessageTrace(ex);
                rv = false;
            }
            finally
            {
                Directory.SetCurrentDirectory(workingDir);
            }
            if (rv)
            {
                string tmpZip = tmpDir + "\\archive.zip";
                Tracing.TraceLine("Flex import file:" + tmpZip, TraceLevel.Info);
                rig.q.Enqueue((Flex.FunctionDel)(() => { rig.ImportProfile(tmpZip); }));
                // Indicate radio is inactive.
                //rig.raisePowerOff();
                //rig.theRadio.DatabaseImportComplete = false;
                //rig.q.Enqueue((Flex.FunctionDel)(() => { rig.theRadio.SendDBImportFile(tmpZip); }));
            }

            //if (Directory.Exists(tmpDir)) Directory.Delete(tmpDir, true);
            return rv;
        }
    }
}
