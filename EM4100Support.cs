using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
/этот класс обрабатывает строку полученную от RFDemodulator
namespace mcprog
{
    class EM4100Support
    {
        bool _isvalid = false;
        string _codingMethod = "";
        string _errorInfo = "";
        RFDemodulator _demod;
        public delegate bool Decoder();
        List<Decoder> _decoders = new List<Decoder>();
        byte[] _data = new byte[5];

        public static string Get64BitSequense(byte[] inData)
        {
            string result = "111111111";
            int[] colsum = new int[4]{0,0,0,0};

            for (int i = 0; i < 10; i++)
            {
                int rowsum = 0;
                for (int j = 0; j < 4; j++)
                {              
                    if ((inData[i / 2] & (0x01 << ((i % 2) * 4 + j))) != 0)
                    {
                        colsum[j]++;
                        rowsum++;
                        result = result + "1";
                    }
                    else
                    {
                        result = result + "0";
                    }
                }
                result = result + ((rowsum % 2) == 0 ? "0" : "1");
            }
            for (int i = 0; i < 4; i++) result = result + ((colsum[i] % 2) == 0 ? "0" : "1");

            result = result + "0";

            return result;
        }
        
        public EM4100Support(RFDemodulator demod)
        {
            _demod=demod;
            _decoders.Add(ManchesterDecoder);
            _decoders.Add(BiphaseDecoder);
        }

        public void DecoderProcess()
        {
            foreach (Decoder decoder in _decoders)
            {
                if (decoder()) return;
            }
        }

        bool ParityTestAndExtractData(List<int> neData)
        {
            for (int i = 0; i < 5; i++) _data[i] = 0;
            
            if (neData[54] != 0) { _errorInfo = "EM code error: last count is not 0"; return false; };
            for (int i = 0; i < 10; i++)
            {
                int parity = 0;
                for (int j = 0; j < 5; j++) parity += neData[i * 5 + j];
                if (parity % 2 != 0) { _errorInfo = "EM code error: row parity error"; return false; };
            }
            for (int i = 0; i < 4; i++)
            {
                int parity = 0;
                for (int j = 0; j < 11; j++) parity += neData[j * 5 + i];
                if (parity % 2 != 0) { _errorInfo = "EM code error: column parity error"; return false; };
            }
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if(neData[i * 5 + j]==1)_data[i / 2] |= (byte)(0x01<<((i%2)*4+j));
                    

                }
            }
            
            return true;
        }
        /этот метод проверяет соответствуют ли входные данные манчестерской кодировке
        / делает проверку на чётность согласно протоколу и декодирует данные
        public bool ManchesterDecoder()
        {
           /здесь ищется стартовый маркер состоящий согласно протоколу из девяти единиц
            Regex marker = new Regex("010101010101010101");

            MatchCollection mc = marker.Matches(_demod.Output);

            

            string kvant = "";

            foreach (Match m in mc)
            {
                int position = m.Index + 18;
                bool next = false;
                List<int> data = new List<int>();

                while (data.Count < 55)
                {
                    
                    
                    try
                    {
                        kvant = _demod.Output.Substring(position, 2);
                    }
                    catch
                    {
                        next = true;
                        break;
                    }
                    
                    
                    position += 2;
                    if (kvant == "01")
                    {
                        data.Add(1);
                    }
                    else if (kvant == "10")
                    {
                        data.Add(0);
                    }
                    else { 
                        _errorInfo = "Manchester code error";
                        next = true;
                        break; 
                    };
                }
                if (next) continue;
                if (ParityTestAndExtractData(data)) { _codingMethod = "Manchester"; _isvalid = true; return true; };

            }
            _errorInfo = "No true data";
            return false;
            
        }
        /этот метод соттветственно проверяет входную строку на соответствие бифазной кодировке
        public bool BiphaseDecoder()
        {
            Regex marker1 = new Regex("010101010101010101");
            Regex marker2 = new Regex("101010101010101010");

            MatchCollection mc1 = marker1.Matches(_demod.Output);
            MatchCollection mc2 = marker1.Matches(_demod.Output);

            List<MatchCollection> mcl = new List<MatchCollection> { mc1, mc2 };

            foreach (MatchCollection mc in mcl)
            {

                foreach (Match m in mc)
                {
                    int position = m.Index + 18;

                    List<int> data = new List<int>();

                    bool next = false;

                    string kvant = "";

                    while (data.Count < 55)
                    {
                        try
                        {
                            kvant = _demod.Output.Substring(position, 2);
                        }
                        catch
                        {
                            next = true;
                            break;
                        }
                        position += 2;
                        if ((kvant == "01") || (kvant == "10"))
                        {
                            data.Add(1);
                        }
                        else if ((kvant == "00") || (kvant == "11"))
                        {
                            data.Add(0);
                        }
                        else { _errorInfo = "Bi-phase code error"; return false; };
                    }
                    if (next) continue;
                    if (ParityTestAndExtractData(data)) { _codingMethod = "Bi-phase"; _isvalid = true; return true; };

                }
            }
            _errorInfo = "No true data";
            return false;


        }
        
        public bool IsValid
        {
            get
            {
                return _isvalid;
            }
        }
        
        public string CodingMethod
        {
            get
            {
                return _codingMethod;
            }
        }
        
        public string ErrorInfo
        {
            get
            {
                return _errorInfo;
            }
        }
        
        public byte[] Data
        {
            get
            {
                return _data;
            }
        }
    }
}
