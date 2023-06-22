using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
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
                retVal = !ln.foundDuplicities(ln.addNumbers);

                if (retVal)
                {
                    retVal = !ln.foundDuplicities(ln.numbers);
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
                        retVal = !Enumerable.SequenceEqual(item.addNumbers.ToArray(), ln.addNumbers.ToArray());

                        if (retVal)
                        {
                            retVal = !Enumerable.SequenceEqual(item.numbers.ToArray(), ln.numbers.ToArray());
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
        private const int MaxAddNum = 12;

        public LotteryNum()
        {
            fillLists();
        }

        // init cisla 12345 usetria nadbytocne incrementy z 1 1 1 1 1  1 1 1 1 2  1 1 1 1 3 (ziadne cislo sa nevyskytuje opakovane) 
        public int Num1 = 1;
        public int Num2 = 2;
        public int Num3 = 3;
        public int Num4 = 4;
        public int Num5 = 5;
        public List<int> numbers;
        public List<int> addNumbers;
        public int AddNum1 = 1;
        public int AddNum2 = 2;

        public void Increase()
        {
            if (!incrementSignleNum(ref Num5))
                if (!incrementSignleNum(ref Num4))
                    if (!incrementSignleNum(ref Num3))
                        if (!incrementSignleNum(ref Num2))
                            if (!incrementSignleNum(ref Num1))
                                if (!incrementSignleNum(ref AddNum2, MaxAddNum))
                                    if (!incrementSignleNum(ref AddNum1, MaxAddNum))
                                    {

                                    }
            fillLists();

        }

        private void fillLists()
        {
            numbers = new List<int> { Num1, Num2, Num3, Num4, Num5 };
            addNumbers = new List<int> { AddNum1, AddNum2 };

            // zoradit list - nezalezi na poradi 1 2 3 4 5 = 5 3 1 2 4
            numbers.Sort();
            addNumbers.Sort();
        }

        public bool foundDuplicities(List<int> i_numbers)
        {
            bool retVal = false;
            for (int i = 0; i < i_numbers.Count - 1; ++i)
            {
                if (i_numbers[i] == i_numbers[i + 1])
                {
                    retVal = true;
                    break;
                }
            }
            return retVal;
        }



        private bool incrementSignleNum(ref int num, int maxNum = MaxNum)
        {
            bool retVal = true;
            // cisla sa v liste nemozu opakovat 1 2 3 5 4 (4 incrementuje kym nebude 6)
            // pri preteceni sa nastavi na 1 a vrati false (umozni increment dalsieho v poradi)
            do
            {
                num++;
            } while (this.numbers.Contains(num));
            if (num > maxNum)
            {
                num = 1;
                retVal = false;
            }
            return retVal;
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

            numbers = new List<int> { Num1, Num2, Num3, Num4, Num5 };
            numbers.Sort();
            addNumbers = new List<int> { AddNum1, AddNum2 };
            addNumbers.Sort();
        }

        public int Num1 { get; private set; }
        public int Num2 { get; private set; }
        public int Num3 { get; private set; }
        public int Num4 { get; private set; }
        public int Num5 { get; private set; }
        public List<int> numbers;
        public List<int> addNumbers;
        public int AddNum1 { get; private set; }
        public int AddNum2 { get; private set; }


    }

}
