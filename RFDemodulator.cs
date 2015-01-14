using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mcprog
{
    public class RFDemodulator
    {
        int _datarate=0;
        string _output="";
        bool _isvalid=false;
        Tester _tester08 = new Tester(4, 11);
        Tester _tester16 = new Tester(12, 20);
        Tester _tester32 = new Tester(24, 40);
        Tester _tester64 = new Tester(56, 72);
        Dictionary<int, Tester> _testers = new Dictionary<int, Tester>();

        public RFDemodulator(byte[] data)
        {
            _testers.Add(8, _tester08);
            _testers.Add(16, _tester16);
            _testers.Add(32, _tester32);
            _testers.Add(64, _tester64);

            int lowIndex = 64;
            int highIndex = 0;

            foreach (byte b in data)
            {
                bool found=false;
                foreach (KeyValuePair<int, Tester> pair in _testers)
                {
                    if (pair.Value.Test(b))
                    {
                        found = true;
                        if (pair.Key < lowIndex) lowIndex = pair.Key;
                        if (pair.Key > highIndex) highIndex = pair.Key;
                        break;
                    }
                }
                if (!found) return;
                
            }
            if (lowIndex >= highIndex) return;
            if (lowIndex != highIndex / 2) return;

            _isvalid = true;
            _datarate = highIndex;

            bool curbit = true;
            foreach (byte b in data)
            {
                _output = _output + (_testers[lowIndex].Test(b) ? (curbit ? "1" : "0") : (curbit ? "11" : "00"));
                if (curbit) curbit = false;
                else curbit = true;
            }

        }

        public int DataRate
        {
            get
            {
                return _datarate;
            }
        }

        public string Output
        {
            get
            {
                return _output;
            }
        }
        public bool IsValid
        {
            get
            {
                return _isvalid;
            }
        }

        class Tester
        {
            int _minval;
            int _maxval;

            public Tester(int min, int max)
            {
                _minval = min;
                _maxval = max;

            }
            public bool Test(int val)
            {
                if ((val <= _minval) || (val >= _maxval)) return false;
                return true;
            }



        }

    }
}
