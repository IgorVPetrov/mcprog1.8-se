using System;
using System.Collections.Generic;
using System.Text;

namespace mcprog
{
    public class HexStringConverter
    {
        private static IDictionary<string, byte> hston = new Dictionary<string, byte>();
        private static IDictionary<byte, string> ntohs = new Dictionary<byte, string>();

        static HexStringConverter()
        {
            string[] hex = {"0","1","2","3","4","5","6","7","8","9","A","B","C","D","E","F"};
            for (byte i = 0; i < 16; i++)
            {
                hston.Add(hex[i], i);
                ntohs.Add(i, hex[i]);
            }
        }

        public static Byte HS2toByte(string s)
        {
            return (byte)((hston[s.Substring(0, 1)] << 4) + hston[s.Substring(1, 1)]);
        }

        public static string ByteToHS2(byte b)
        {
            return ntohs[(byte)(b / 16)] + ntohs[(byte)(b % 16)] ;
        }

        public static UInt16 HS4toUInt16(string s)
        {
            ushort res=hston[s.Substring(0,1)];
		    for(int i=1;i<4;i++)
			    res=(ushort)((res<<4)+hston[s.Substring(i,1)]);
	        return res;
        }

        public static string UInt16ToHS4(ushort b)
        {
            String s= "";
		    for(int i=0;i<4;i++){
			    s=ntohs[(byte)(b%16)]+s;
			    b/=16; 
		    };	
	        return s;
        }
    }
}
