using System;
using System.Collections.Generic;
using System.Text;

namespace mcprog
{
    public class ProgrammingProgressInfo
    {


        public string Message;
        public bool MessageChanged;
        public bool ProgressBarDataChanged;
        public ProgressBarData ProgressBarData;

        public ProgrammingProgressInfo(string m)
        {
            Message = m;
            MessageChanged = true;
            ProgressBarDataChanged = false;
        }
        public ProgrammingProgressInfo(ProgressBarData pbd)
        {
            ProgressBarData = pbd;
            MessageChanged = false;
            ProgressBarDataChanged = true;
        } 
    }

    public struct ProgressBarData
    {
        public int Minimum;
        public int Maximum;
        public int Value;

        public ProgressBarData(int min, int val, int max)
        {
            Minimum = min;
            Maximum = max;
            Value = val;
        }
    }

    public class ProgrammingCompleteInfo
    {
        public Exception error=null;
        public string Message =null;
    }

    public class VerificationException : Exception
    {


        private uint _errorAddress;

        public uint ErrorAddress
        {
            get { return _errorAddress; }
        }
        private byte _writtenByte;

        public byte WrittenByte
        {
            get { return _writtenByte; }
        }
        private byte _readByte;

        public byte ReadByte
        {
            get { return _readByte; }
        }

        public VerificationException(uint addr, byte wbyte, byte rbyte):base("Verification error ")
        {          
            _errorAddress = addr;
            _writtenByte = wbyte;
            _readByte = rbyte;
        }
    }
}
