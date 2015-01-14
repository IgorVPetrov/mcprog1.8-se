using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mcprog
{
    class Constants
    {

        public const byte MODE_INIT = 0x09;    //Primary
        public const byte IDLE_INIT = 0x00;    //Second
        public const byte ONEWIRE_INIT = 0x01;    //Second
        public const byte I2C_INIT = 0x02;    //Second
        public const byte SPI_INIT = 0x03;    //Second
        public const byte MICROWIRE_INIT = 0x04;    //Second  
        public const byte RFID125_INIT = 0x05;    //Second



        public const byte RW1990_WRITE = 0x0A;    //Primary
        public const byte DS1990_READ = 0x0B;    //Primary
        public const byte ONEWIRE_COMMAND = 0x0C;    //Primary
        public const byte ONEWIRE_COMMAND_2 = 0x0D;    //Primary

        public const byte I2C_READ = 0x0E;    //Primary
        public const byte I2C_WRITE = 0x0F;    //Primary

        public const byte RFID125_READ = 0x10;   //Primary
        public const byte RFID125_GET_DATA = 0x11;   //Primary
        public const byte RFID125_TEST = 0x12;//Primary
        public const byte RFID125_WRITE = 0x13;//Primary
    }
}
