using System;
using System.Collections.Generic;
using System.Text;

namespace mcprog
{
    public class FileWorkerCompleteEventArgs : EventArgs
    {
        private string _path;

        public string Path
        {
            get { return _path; }
        }
        private Exception _error;

        public System.Exception Error
        {
            get { return _error; }
        }

        public FileWorkerCompleteEventArgs(string path, Exception error, List<MemoryRegion> regions)
        {
            _path = path;
            _error = error;
            _regions = regions;
        }

        private List<MemoryRegion> _regions;

        public List<MemoryRegion> Regions
        {
            get { return _regions; }
        }
    }

    public delegate void FileWorkerCompleteEventHandler(Object sender, FileWorkerCompleteEventArgs e);
}
