using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
/*
этот класс обрабатывает ответ от программатора
программатор собран на контроллере PIC18F4550 
к нему подключён модуль на микросхеме EM4095
при чтении транспондера интерес представляют 2 сигнала 
1) синхронизация - на этом выходе меандр с частотой радиосигнала 
2) выход демодулированного сигнала

контроллер подсчитывает сколько периодов радиочастоты длится 
состояние логической 1 на выходе демодулятора и сохраняет это значение
потом измеряет сколько длится состояние логического 0 и так же сохраняет
потом снова лог 1 итд
всего сохраняется 256 значений и получается 256 байтов
потом эти значения передаются пакетами по 64 байта в программу

так работает железо(это я написал чтобы было понятно в каком виде передаются
считанные с транспондера данные)
считанные данные передаются конструктору класса RFDemodulator(byte[] data)
здесь надо бы обьяснить что такое манчестерская и бифазная кодировка
но это наверное получится длинно
суть в том что в этих кодировках есть как бы две длительности 
логического 0 и логической 1 на выходе демодулятора
короткая и длинная(которая в 2 раза длиннее короткой)
но они по разному интерпретируются в разных кодировках
есть такой параметр data rate(скорость передачи данных )
который определяется как количество периодов радиочастоты
которые укладываются в длительность длинного сигнала на выходе демодулятора
есть ряд стандартных значений
RF/8 RF/16 RF/32 RF/64
для определения data rate я использую класс Tester

*/
namespace mcprog
{
    public class RFDemodulator
    {
        int _datarate=0;
        string _output="";
        bool _isvalid=false;
        //вот здесь создаются экземпляры класса Tester для каждой возможной data rate
        Tester _tester08 = new Tester(4, 11);
        Tester _tester16 = new Tester(12, 20);
        Tester _tester32 = new Tester(24, 40);
        Tester _tester64 = new Tester(56, 72);
        //количество периодов берётся c запасом так как реальный сигнал имеет фронт и спад
        Dictionary<int, Tester> _testers = new Dictionary<int, Tester>();

        public RFDemodulator(byte[] data)
        {
            //здесь они добавляются в ассоциативный массив
            _testers.Add(8, _tester08);
            _testers.Add(16, _tester16);
            _testers.Add(32, _tester32);
            _testers.Add(64, _tester64);

            int lowIndex = 64;
            int highIndex = 0;

            foreach (byte b in data)//перебираются все байты из массива, пришедшего с контроллера
            // и проверяется все ли они укладываются в заданные диапазоны
            {
                bool found=false;//эта переменная показывает обнаружен ли правильный диапазон
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
