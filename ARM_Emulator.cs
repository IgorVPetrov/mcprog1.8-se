using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mcprog
{
    class ARM_Emulator
    {
        protected uint R0;
        protected uint R1;
        protected uint R2;
        protected uint R3;
        protected uint R4;
        protected uint R5;
        protected uint R6;
        protected uint R7;
        protected uint R8;
        protected uint R9;
        protected uint R10;
        protected uint R11;
        protected uint R12;
        protected uint SP;
        protected uint LR;
        protected uint PC;

        protected bool Z;
        protected bool N;
        protected bool C;
        protected bool C1;
        protected bool C2;
        protected bool V;

        protected bool BigEndian;
        protected bool get_output;
        protected bool put_output;

        protected Dictionary<uint, Byte> memory;

        protected void put_byte(uint addr, uint d)
        {

            //String saved = "put_byte addr = "+addr.ToString("X8")+" old = "+memory[addr].ToString("X8")+" new = "+((Byte)d).ToString("X8");
            memory[addr] = (Byte)(d & 0xFF);

        }

        protected uint get_byte(uint addr)
        {

            uint result = (uint)memory[addr];
            //String saved = "get_byte addr = "+addr.ToString("X8")+" data = "+result.ToString("X8");
            return result;

        }

        protected void put_word(uint addr, uint d)
        {
            if (BigEndian)
            {
                //uint old = (((uint)memory[addr])<<8) + (uint)memory[addr+1];
                //String saved = "put_word addr = "+addr.ToString("X8")+" old = "+old.ToString("X8")+" new = "+((ushort)d).ToString("X8");
                memory[addr + 1] = (Byte)(d & 0xFF);
                memory[addr] = (Byte)((d >> 8) & 0xFF);
            }
            else
            {
                //uint old = (((uint)memory[addr+1])<<8) + (uint)memory[addr];
                //String saved = "put_word addr = "+addr.ToString("X8")+" old = "+old.ToString("X8")+" new = "+((unsigned short)d).ToString("X8");

                memory[addr] = (Byte)(d & 0xFF);
                memory[addr + 1] = (Byte)((d >> 8) & 0xFF);
            }
        }

        protected uint get_word(uint addr)
        {
            if (BigEndian)
            {
                uint result = (((uint)memory[addr]) << 8) + (uint)memory[addr + 1];
                //String^ saved = "get_word addr = "+addr.ToString("X8")+" data = "+result.ToString("X8");
                //if(this->get_output)this->infoListBox->Items->Add(saved); 
                return result;

            }
            else
            {
                uint result = (((uint)memory[addr + 1]) << 8) + (uint)memory[addr];
                //String^ saved = "get_word addr = "+addr.ToString("X8")+" data = "+result.ToString("X8");
                //if(this->get_output)this->infoListBox->Items->Add(saved); 
                return result;

            }
        }

        protected void put_long(uint addr, uint d)
        {
            if (BigEndian)
            {
                //uint old = (((uint)memory[addr])<<24) + (((uint)memory[addr+1])<<16) +(((uint)memory[addr+2])<<8) +(uint)memory[addr+3];
                //String^ saved = "put_long addr = "+addr.ToString("X8")+" old = "+old.ToString("X8")+" new = "+d.ToString("X8");
                //if(this->put_output)this->infoListBox->Items->Add(saved);               
                memory[addr] = (Byte)((d >> 24) & 0xFF);
                memory[addr + 1] = (Byte)((d >> 16) & 0xFF);
                memory[addr + 2] = (Byte)((d >> 8) & 0xFF);
                memory[addr + 3] = (Byte)(d & 0xFF);
            }
            else
            {
                //uint old = (((uint)memory[addr+3])<<24) + (((uint)memory[addr+2])<<16) +(((uint)memory[addr+1])<<8) +(uint)memory[addr];
                //String^ saved = "put_long addr = "+addr.ToString("X8")+" old = "+old.ToString("X8")+" new = "+d.ToString("X8");
                //if(this->put_output)this->infoListBox->Items->Add(saved);               
                memory[addr + 3] = (Byte)((d >> 24) & 0xFF);
                memory[addr + 2] = (Byte)((d >> 16) & 0xFF);
                memory[addr + 1] = (Byte)((d >> 8) & 0xFF);
                memory[addr] = (Byte)(d & 0xFF);
            }
        }

        protected uint get_long(uint addr)
        {
            if (BigEndian)
            {
                uint result = (((uint)memory[addr]) << 24) + (((uint)memory[addr + 1]) << 16) + (((uint)memory[addr + 2]) << 8) + (uint)memory[addr + 3];
                // String^ saved = "get_long addr = "+addr.ToString("X8")+" data = "+result.ToString("X8");
                //if(this->get_output)this->infoListBox->Items->Add(saved); 
                return result;
            }
            else
            {
                uint result = (((uint)memory[addr + 3]) << 24) + (((uint)memory[addr + 2]) << 16) + (((uint)memory[addr + 1]) << 8) + (uint)memory[addr];
                //String^ saved = "get_long addr = "+addr.ToString("X8")+" data = "+result.ToString("X8");
                //if(this->get_output)this->infoListBox->Items->Add(saved); 
                return result;
            }
        }


    }
}
