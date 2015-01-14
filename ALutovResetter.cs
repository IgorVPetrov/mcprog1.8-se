using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.IO;
using Microsoft.Win32.SafeHandles;
using System.ComponentModel;
using System.Threading;
using System.Runtime.InteropServices;
using System.Data;
using System.Windows.Forms;
using System.Linq;
using System.Diagnostics;
using System.Drawing;

namespace mcprog
{
    public class ALutovResetter : I921Programmer
    {

        #region Fields

        string _devName = "ALutov Resetter";
        bool _isBusy = false;
        EventHandler _busy;
        EventHandler _ready;
        HardwareMode _hardwareMode = HardwareMode.ProgrammerMode;
        
        List<string> _readingSequense = new List<string> {"Loading driver","Checking programmer hardware","Authentication","Reading data","Saving file","Unloading driver" };
        int _readingStep = 0;

        List<string> _writingSequense = new List<string> { "Loading driver", "Checking programmer hardware", "Loading file", "Authentication", "Writing data", "Unloading driver" };
        int _writingStep = 0;        

        #endregion 
            
        #region I921Programmer Members

        ICollection<string> I921Programmer.SupportedModes
        {
            get { return new List<string>(); }
        }

        int I921Programmer.ReadChip(string mode, MemoryRegionInfo regionInfo, Action<ProgrammingCompleteInfo, MemoryRegion> completed, Action<ProgrammingProgressInfo> progress)
        {
            _isBusy = true;

            AsyncOperation asyncOp = AsyncOperationManager.CreateOperation(new Guid());

            string infoString = "";
            string errString = "";
            
            DataReceivedEventHandler processOutput = delegate(Object sender, DataReceivedEventArgs drea)
            {
                if ((drea.Data!=null)&&drea.Data.Contains(_readingSequense[_readingStep]))
                {
                    if (drea.Data.Contains("Done"))
                    {
                        if (infoString == "")
                            infoString += (_readingSequense[_readingStep] + " Ok");
                        else
                            infoString += ("\r\n" + _readingSequense[_readingStep] + " Ok");
                        _readingStep++;
                        asyncOp.Post(delegate(object args) { progress(new ProgrammingProgressInfo(infoString)); }, null);
                    }
                    else
                    {
                        errString = _readingSequense[_readingStep] + " Error";
                    }
                }
            };
            ThreadStart start = delegate()
            {
                asyncOp.Post(delegate(object args) { OnBusy(); }, null);
                MemoryRegion region = new MemoryRegion(regionInfo);
                ProgrammingCompleteInfo pcInfo = new ProgrammingCompleteInfo();              
                _readingStep = 0;
                try
                {
                    string path = Application.ExecutablePath;
                    path = path.Substring(0, path.LastIndexOf("\\"));
                    Directory.SetCurrentDirectory(path);    

                    
                    Process p = new Process();
                    p.StartInfo.UseShellExecute = false;
                    p.StartInfo.RedirectStandardOutput = true;
                    p.StartInfo.FileName = "ps3cc921.exe";
                    p.StartInfo.CreateNoWindow = true;
                    p.OutputDataReceived += processOutput;                   
                    p.Start();                    
                    p.BeginOutputReadLine();
                    p.WaitForExit();
                    p.Close();
                    if (errString != "")
                    {
                        throw new Exception(errString);
                    }
                    else
                    {
                        pcInfo.Message = "Reading Ok";
                        using (BinaryReader file = new BinaryReader(File.OpenRead("RS3CC921.BIN")))
                        {
                            file.BaseStream.Position = 0;

                            for (int i = 0; i < region.Data.Length; i++)
                            {
                                region.Data[i] = file.ReadByte();
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    pcInfo.error = e;
                }
                
                asyncOp.Post(delegate(object args) { completed(pcInfo, region); }, null);
                _isBusy = false;
                asyncOp.PostOperationCompleted(delegate(object args) { OnReady(); }, null);
            };

            (new Thread(start)).Start();

            return 0;
        }

        int I921Programmer.ProgramChip(string mode, MemoryRegion region, Action<ProgrammingCompleteInfo> completed, Action<ProgrammingProgressInfo> progress)
        {
            _isBusy = true;
            AsyncOperation asyncOp = AsyncOperationManager.CreateOperation(new Guid());

            string infoString = "";
            string errString = "";

            DataReceivedEventHandler processOutput = delegate(Object sender, DataReceivedEventArgs drea)
            {
                if ((drea.Data != null) && drea.Data.Contains(_writingSequense[_writingStep]))
                {
                    if (drea.Data.Contains("Done"))
                    {
                        if (infoString == "")
                            infoString += (_writingSequense[_writingStep] + " Ok");
                        else
                            infoString += ("\r\n" + _writingSequense[_writingStep] + " Ok");
                        _writingStep++;
                        asyncOp.Post(delegate(object args) { progress(new ProgrammingProgressInfo(infoString)); }, null);
                    }
                    else
                    {
                        errString = _writingSequense[_writingStep] + " Error";
                    }
                }    
            };


            ThreadStart start = delegate()
            {

                asyncOp.Post(delegate(object args) { OnBusy(); }, null);

                ProgrammingCompleteInfo pcInfo = new ProgrammingCompleteInfo();

                _writingStep = 0;

                try
                {
                    string path = Application.ExecutablePath;
                    path = path.Substring(0, path.LastIndexOf("\\"));
                    Directory.SetCurrentDirectory(path); 
                    
                    using (BinaryWriter file = new BinaryWriter(File.OpenWrite("RS3CC921.BIN")))
                    {
                        file.Write(region.Data);
                    }
                    
                    Process p = new Process();
                    p.StartInfo.UseShellExecute = false;
                    p.StartInfo.RedirectStandardOutput = true;
                    p.StartInfo.FileName = "ps3cc921.exe";
                    p.StartInfo.Arguments = "RS3CC921.BIN";
                    p.StartInfo.CreateNoWindow = true;
                    p.OutputDataReceived += processOutput;
                    p.Start();
                    p.BeginOutputReadLine();
                    p.WaitForExit();
                    p.Close();
                    if (errString != "")
                    {
                        throw new Exception(errString);
                    }
                    else
                    {
                        pcInfo.Message = "Programming Ok";

                    }

                }
                catch (Exception e)
                {
                    pcInfo.error = e;
                }
                
                
                asyncOp.Post(delegate(object args) { completed(pcInfo); }, null);
                _isBusy = false;
                asyncOp.PostOperationCompleted(delegate(object args) { OnReady(); }, null);
            };

            (new Thread(start)).Start();

            return 0;
        
        }

        #endregion
        
        #region OnEvent methods

        protected virtual void OnBusy()
        {
            EventHandler localHandler = _busy;
            if (localHandler != null)
            {
                localHandler(this, new EventArgs());
            }
        }

        protected virtual void OnReady()
        {
            EventHandler localHandler = _ready;
            if (localHandler != null)
            {
                localHandler(this, new EventArgs());
            }
        }
        
        #endregion


        #region IChipProgrammer Members

        event EventHandler IChipProgrammer.Busy
        {
            add
            {
                _busy = (EventHandler)Delegate.Combine(_busy, value);
            }
            remove
            {
                _busy = (EventHandler)Delegate.Remove(_busy, value);
            }
        }

        event EventHandler IChipProgrammer.Ready
        {
            add
            {
                _ready = (EventHandler)Delegate.Combine(_ready, value);
            }
            remove
            {
                _ready = (EventHandler)Delegate.Remove(_ready, value);
            }
        }

        bool IChipProgrammer.IsBusy
        {
            get { return _isBusy; }
        }

        System.Windows.Forms.Form IChipProgrammer.GetServiceWindow()
        {
            return new System.Windows.Forms.Form();
        }

        HardwareMode IChipProgrammer.GetMode()
        {
            return _hardwareMode;
        }

        #endregion
    }
}
