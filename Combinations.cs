using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.IO;
using System.Linq;
using System.Text;

namespace EuroJack
{
    internal class Combinations
    {
        private const string fileName = "numbers.txt";
        private const string ignorNumFileName = "ignoreNumbers.txt";
        private static long counter = 0;
        private static int addNum2 = -1;
        private static Random randomer = new Random();
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

                    counter++;
                    if (counter > 200000)
                    {
                        Console.WriteLine(str);
                        counter = 0;
                    }

                    sb.AppendLine(str);
                }
                else
                {
                    if (addNum2 != lotNum.AddNum2)
                    {
                        addNum2 = lotNum.AddNum2;

                        var str = $"{lotNum.Num1:D2},{lotNum.Num2:D2},{lotNum.Num3:D2},{lotNum.Num4:D2},{lotNum.Num5:D2}-{lotNum.AddNum1:D2},{lotNum.AddNum2:D2}";
                        Console.WriteLine(str);
                    }
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
                        retVal = ln.Num1 != ln.Num2 + i && ln.Num2 + i != ln.Num3 + i && ln.Num3 + i != ln.Num4 + i && ln.Num4 + i != ln.Num5 + i;

                        if (!retVal)
                            break;
                    }
                }

                if (retVal)
                {
                    foreach (var item in ignoreNum)
                    {

                        var conditional = randomer.Next(1, 6);


                        switch (conditional)
                        {
                            case 1:
                                retVal = !((item.Num1 == ln.Num1 || item.Num1 == ln.Num2 || item.Num1 == ln.Num3 || item.Num1 == ln.Num4 || item.Num1 == ln.Num5) &&
                                            ((item.Num2 == ln.Num1 || item.Num2 == ln.Num2 || item.Num2 == ln.Num3 || item.Num2 == ln.Num4 || item.Num2 == ln.Num5) ||
                                            (item.Num3 == ln.Num1 || item.Num3 == ln.Num2 || item.Num3 == ln.Num3 || item.Num3 == ln.Num4 || item.Num3 == ln.Num5) ||
                                            (item.Num4 == ln.Num1 || item.Num4 == ln.Num2 || item.Num4 == ln.Num3 || item.Num4 == ln.Num4 || item.Num4 == ln.Num5) ||
                                          (item.Num5 == ln.Num1 || item.Num5 == ln.Num2 || item.Num5 == ln.Num3 || item.Num5 == ln.Num4 || item.Num5 == ln.Num5)));
                                break;
                            case 2:
                                retVal = !(((item.Num1 == ln.Num1 || item.Num1 == ln.Num2 || item.Num1 == ln.Num3 || item.Num1 == ln.Num4 || item.Num1 == ln.Num5) ||
                                          (item.Num2 == ln.Num1 || item.Num2 == ln.Num2 || item.Num2 == ln.Num3 || item.Num2 == ln.Num4 || item.Num2 == ln.Num5)) &&
                                            ((item.Num3 == ln.Num1 || item.Num3 == ln.Num2 || item.Num3 == ln.Num3 || item.Num3 == ln.Num4 || item.Num3 == ln.Num5) ||
                                            (item.Num4 == ln.Num1 || item.Num4 == ln.Num2 || item.Num4 == ln.Num3 || item.Num4 == ln.Num4 || item.Num4 == ln.Num5) ||
                                            (item.Num5 == ln.Num1 || item.Num5 == ln.Num2 || item.Num5 == ln.Num3 || item.Num5 == ln.Num4 || item.Num5 == ln.Num5)));
                                break;
                            case 3:
                                retVal = !(((item.Num1 == ln.Num1 || item.Num1 == ln.Num2 || item.Num1 == ln.Num3 || item.Num1 == ln.Num4 || item.Num1 == ln.Num5) ||
                                          (item.Num2 == ln.Num1 || item.Num2 == ln.Num2 || item.Num2 == ln.Num3 || item.Num2 == ln.Num4 || item.Num2 == ln.Num5) ||
                                          (item.Num3 == ln.Num1 || item.Num3 == ln.Num2 || item.Num3 == ln.Num3 || item.Num3 == ln.Num4 || item.Num3 == ln.Num5)) &&
                                          ((item.Num4 == ln.Num1 || item.Num4 == ln.Num2 || item.Num4 == ln.Num3 || item.Num4 == ln.Num4 || item.Num4 == ln.Num5) ||
                                          (item.Num5 == ln.Num1 || item.Num5 == ln.Num2 || item.Num5 == ln.Num3 || item.Num5 == ln.Num4 || item.Num5 == ln.Num5)));
                                break;
                            case 4:
                                retVal = !(((item.Num1 == ln.Num1 || item.Num1 == ln.Num2 || item.Num1 == ln.Num3 || item.Num1 == ln.Num4 || item.Num1 == ln.Num5) ||
                                          (item.Num2 == ln.Num1 || item.Num2 == ln.Num2 || item.Num2 == ln.Num3 || item.Num2 == ln.Num4 || item.Num2 == ln.Num5) ||
                                          (item.Num3 == ln.Num1 || item.Num3 == ln.Num2 || item.Num3 == ln.Num3 || item.Num3 == ln.Num4 || item.Num3 == ln.Num5) ||
                                          (item.Num4 == ln.Num1 || item.Num4 == ln.Num2 || item.Num4 == ln.Num3 || item.Num4 == ln.Num4 || item.Num4 == ln.Num5)) &&
                                          (item.Num5 == ln.Num1 || item.Num5 == ln.Num2 || item.Num5 == ln.Num3 || item.Num5 == ln.Num4 || item.Num5 == ln.Num5));
                                break;
                            case 5:
                                retVal = !((item.Num1 == ln.Num1 || item.Num1 == ln.Num2 || item.Num1 == ln.Num3 || item.Num1 == ln.Num4 || item.Num1 == ln.Num5) &&
                                          (item.Num2 == ln.Num1 || item.Num2 == ln.Num2 || item.Num2 == ln.Num3 || item.Num2 == ln.Num4 || item.Num2 == ln.Num5) &&
                                          (item.Num3 == ln.Num1 || item.Num3 == ln.Num2 || item.Num3 == ln.Num3 || item.Num3 == ln.Num4 || item.Num3 == ln.Num5) &&
                                          (item.Num4 == ln.Num1 || item.Num4 == ln.Num2 || item.Num4 == ln.Num3 || item.Num4 == ln.Num4 || item.Num4 == ln.Num5) &&
                                          (item.Num5 == ln.Num1 || item.Num5 == ln.Num2 || item.Num5 == ln.Num3 || item.Num5 == ln.Num4 || item.Num5 == ln.Num5));
                                break;
                            case 6:
                                retVal = !((item.Num1 == ln.Num1 || item.Num1 == ln.Num2 || item.Num1 == ln.Num3 || item.Num1 == ln.Num4 || item.Num1 == ln.Num5) ||
                                          (item.Num2 == ln.Num1 || item.Num2 == ln.Num2 || item.Num2 == ln.Num3 || item.Num2 == ln.Num4 || item.Num2 == ln.Num5) ||
                                          (item.Num3 == ln.Num1 || item.Num3 == ln.Num2 || item.Num3 == ln.Num3 || item.Num3 == ln.Num4 || item.Num3 == ln.Num5) ||
                                          (item.Num4 == ln.Num1 || item.Num4 == ln.Num2 || item.Num4 == ln.Num3 || item.Num4 == ln.Num4 || item.Num4 == ln.Num5) ||
                                          (item.Num5 == ln.Num1 || item.Num5 == ln.Num2 || item.Num5 == ln.Num3 || item.Num5 == ln.Num4 || item.Num5 == ln.Num5));
                                break;

                        }

                        if (retVal)
                        {
                            retVal = (item.AddNum1 != ln.AddNum1 && item.AddNum2 != ln.AddNum2);
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
        private const int StartNum = 1;
        private const int MaxNum = 50;
        private const int MaxAddNum = 12;
        private int[] ignoreNum = { 1, 11, 50 };

        public LotteryNum()
        {
            Num1 = 1;
            Num2 = 2;
            Num3 = 3;
            Num4 = 4;
            Num5 = 5;
            AddNum1 = 1;
            AddNum2 = 2;
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
            bool wasOVerFulled = false;
            (Num1, wasOVerFulled) = IncrementValue(Num1, new[] { Num2, Num3, Num4, Num5 });

            if (wasOVerFulled)
            {
                Num1 = StartNum;

                (Num2, wasOVerFulled) = IncrementValue(Num2, new[] { Num1, Num3, Num4, Num5 });
                if (wasOVerFulled)
                {
                    Num2 = Num1 == StartNum ? StartNum + 1 : StartNum;

                    (Num3, wasOVerFulled) = IncrementValue(Num3, new[] { Num2, Num1, Num4, Num5 });
                    if (wasOVerFulled)
                    {
                        Num3 = Num2 == StartNum ? StartNum + 1 : StartNum;

                        (Num4, wasOVerFulled) = IncrementValue(Num4, new[] { Num2, Num3, Num1, Num5 });
                        if (wasOVerFulled)
                        {
                            Num4 = Num3 == StartNum ? StartNum + 1 : StartNum;

                            (Num5, wasOVerFulled) = IncrementValue(Num5, new[] { Num2, Num3, Num4, Num1 });
                            if (wasOVerFulled)
                            {
                                Num5 = Num4 == StartNum ? StartNum + 1 : StartNum;
                                AddNum1++;

                                while (AddNum1 == AddNum2)
                                    AddNum1++;

                                if (AddNum1 > MaxAddNum)
                                {
                                    AddNum1 = StartNum;
                                    AddNum2++;

                                    if (AddNum2 > MaxAddNum)
                                    {
                                        // throw new Exception("Koniec hladania");
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
            return AddNum2 > MaxAddNum;
        }

        private (int, bool) IncrementValue(int startValue, int[] existing)
        {
            var retVal = startValue;
            var wasOverfulled = false;

            while (true)
            {
                retVal++;

                if (retVal > MaxNum)
                {
                    retVal = StartNum;
                    wasOverfulled = true;
                }

                if (!ignoreNum.Contains(retVal) && !existing.Contains(retVal))
                    break;
            }

            return (retVal, wasOverfulled);
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
