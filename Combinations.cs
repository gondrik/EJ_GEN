﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace EuroJack
{
    internal class Combinations
    {
        private const string fileName = "numbers.txt";
        private const string ignorNumFileName = "ignoreNumbers.txt";

        static List<IgnoreNum> ignoreNum = new List<IgnoreNum>();

        public static void Run()
        {
            if (File.Exists(fileName))
                File.Delete(fileName);

            if (File.Exists(ignorNumFileName))
            {
                var dts = File.ReadAllLines(ignorNumFileName);

                foreach (var d in dts)
                {
                    var line = d.Trim().Replace(" ", string.Empty);
                    var items = line.Split("-");

                    ignoreNum.Add(new IgnoreNum(items));
                }
            }

            var lotNum = new LotteryNum();
            var sb = new StringBuilder();

            while (!lotNum.IsEnd())
            {
                if (IsCorrectNum(lotNum))
                {
                    var str = $"{lotNum.Num1:D2},{lotNum.Num2:D2},{lotNum.Num3:D2},{lotNum.Num4:D2},{lotNum.Num5:D2}-{lotNum.AddNum1:D2},{lotNum.AddNum2:D2}";

                    Console.WriteLine(str);
                    sb.AppendLine(str);
                }

                lotNum.Increase();
            }

            if (sb.Length > 0)
            {
                File.WriteAllText(fileName, sb.ToString());
            }


            Console.WriteLine("Koniec generovania.");
        }


        private static bool IsCorrectNum(LotteryNum ln)
        {
            var retVal = false;

            if (ln != null)
            {
                retVal = ln.Num1 != ln.Num2 && ln.Num2 != ln.Num3 && ln.Num3 != ln.Num4 && ln.Num4 != ln.Num5;

                if (retVal)
                {
                    retVal = ln.AddNum1 != ln.AddNum2;
                }

                if (retVal)
                {
                    for (int i = 1; i < 50; i++)
                    {
                        retVal = ln.Num1 != ln.Num2 + i && ln.Num2 + i != ln.Num3 + i && ln.Num3 + i != ln.Num4 + i && ln.Num4 + 1 != ln.Num5 + 1;

                        if (!retVal)
                            break;
                    }
                }

                if (retVal)
                {
                    foreach (var item in ignoreNum)
                    {

                        retVal = (item.Num1 == ln.Num1 || item.Num1 == ln.Num2 || item.Num1 == ln.Num3 || item.Num1 == ln.Num4 || item.Num1 == ln.Num5) &&
                                (item.Num2 == ln.Num1 || item.Num2 == ln.Num2 || item.Num2 == ln.Num3 || item.Num2 == ln.Num4 || item.Num2 == ln.Num5) &&
                                (item.Num3 == ln.Num1 || item.Num3 == ln.Num2 || item.Num3 == ln.Num3 || item.Num3 == ln.Num4 || item.Num3 == ln.Num5) &&
                                (item.Num4 == ln.Num1 || item.Num4 == ln.Num2 || item.Num4 == ln.Num3 || item.Num4 == ln.Num4 || item.Num4 == ln.Num5) &&
                                (item.Num5 == ln.Num1 || item.Num5 == ln.Num2 || item.Num5 == ln.Num3 || item.Num5 == ln.Num4 || item.Num5 == ln.Num5);

                        if (retVal)
                        {
                            retVal = (item.AddNum1 != ln.AddNum1 && item.AddNum2 != ln.Num2);
                        }

                        if (!retVal)
                            break;
                    }


                }
            }

            return retVal;
        }


    }


    internal class LotteryNum
    {
        private const int MaxNum = 50;
        private const int MaxAddNum = 10;

        public LotteryNum()
        {
            Num1 = 1;
            Num2 = 1;
            Num3 = 1;
            Num4 = 1;
            Num5 = 1;
            AddNum1 = 1;
            AddNum2 = 1;
        }

        public int Num1 { get; private set; }
        public int Num2 { get; private set; }
        public int Num3 { get; private set; }
        public int Num4 { get; private set; }
        public int Num5 { get; private set; }

        public int AddNum1 { get; private set; }
        public int AddNum2 { get; private set; }

        public void Increase()
        {
            Num1++;

            if (Num1 > MaxAddNum)
            {
                Num1 = 1;

                Num2++;
                if (Num2 > MaxNum)
                {
                    Num2 = 1;

                    Num3++;
                    if (Num3 > MaxNum)
                    {
                        Num3 = 1;
                        Num4++;
                        if (Num4 > MaxNum)
                        {
                            Num4 = 1;
                            Num5++;
                            if (Num5 > MaxNum)
                            {
                                Num5 = 1;
                                AddNum1++;
                                if (AddNum1 > MaxAddNum)
                                {
                                    AddNum1 = 1;
                                    AddNum2++;
                                    if (AddNum2 > MaxAddNum)
                                    {
                                        return;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public bool IsEnd()
        {
            return Num1 == MaxNum && Num2 == MaxAddNum && Num3 == MaxNum && Num4 == MaxNum && Num5 == MaxNum && AddNum1 == MaxAddNum && AddNum2 == MaxAddNum;
        }
    }


    internal class IgnoreNum
    {
        public IgnoreNum(string[] itemsNum)
        {
            //0 index= velke cisla
            //1 index = dodatkove

            var main = itemsNum[0].Split(",");

            Num1 = Convert.ToInt32(main[0]);
            Num2 = Convert.ToInt32(main[1]);
            Num3 = Convert.ToInt32(main[2]);
            Num4 = Convert.ToInt32(main[3]);
            Num5 = Convert.ToInt32(main[4]);

            var add = itemsNum[1].Split(",");

            AddNum1 = Convert.ToInt32(add[0]);
            AddNum2 = Convert.ToInt32(add[1]);
        }

        public int Num1 { get; private set; }
        public int Num2 { get; private set; }
        public int Num3 { get; private set; }
        public int Num4 { get; private set; }
        public int Num5 { get; private set; }

        public int AddNum1 { get; private set; }
        public int AddNum2 { get; private set; }


    }

}