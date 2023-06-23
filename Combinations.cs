using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Globalization;
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
                    var str = $"{lotNum.numbers[0]:D2},{lotNum.numbers[1]:D2},{lotNum.numbers[2]:D2},{lotNum.numbers[3]:D2},{lotNum.numbers[4]:D2}-{lotNum.addNumbers[0]:D2},{lotNum.addNumbers[1]:D2}";

                    counter++;
                    if (counter > 20000)
                    {
                        Console.WriteLine(str);
                        counter = 0;
                    }

                    sb.AppendLine(str);
                }

                do
                {
                    lotNum.Increase();
                } while (lotNum.numbers.Intersect(lotNum.ignoreNum).Any());
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
                    {            // [4] [3] [2] [1] [0]
                                 // 1   2   3   4   5
                                 // 1   3   5   7   9
                                 // 1   4   7   10  13
                                 // ...
                        retVal =
                            ln.numbers[4] != ln.numbers[3] + i &&
                            ln.numbers[3] != ln.numbers[2] + i &&
                            ln.numbers[2] != ln.numbers[1] + i &&
                            ln.numbers[1] != ln.numbers[0] + i;

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
        private const int StartNum = 1;
        private const int MaxNum = 50;
        private const int MaxAddNum = 12;
        public List<int> ignoreNum = new List<int> { 1, 11, 50 };

        public LotteryNum()
        {
            numbers[0] = Num1;
            numbers[1] = Num2;
            numbers[2] = Num3;
            numbers[3] = Num4;
            numbers[4] = Num5;

            addNumbers[0] = AddNum1;
            addNumbers[1] = AddNum2;

            // zoradit list od zostupne - nezalezi na poradi 5 4 3 2 1 = 5 3 1 2 4
            numbers.Sort();
            numbers.Reverse();
            addNumbers.Sort();
            addNumbers.Reverse();
        }

        // init cisla 12345 usetria nadbytocne incrementy z 1 1 1 1 1  1 1 1 1 2  1 1 1 1 3 (ziadne cislo sa nevyskytuje opakovane) 
        public int Num1 = 5;
        public int Num2 = 4;
        public int Num3 = 3;
        public int Num4 = 2;
        public int Num5 = 1;
        public List<int> numbers = new List<int> { 5, 4, 3, 2, 1 };
        public List<int> addNumbers = new List<int> { 2, 1 };
        public int AddNum1 = 2;
        public int AddNum2 = 1;

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


        public void Increase()
        {
            bool isAddOverFlow = false;
            bool isOverFlow = false;
            for (int i_add = 0; i_add < addNumbers.Count; ++i_add)
            {
                addNumbers[i_add]++;

                if (addNumbers[i_add] + i_add <= MaxAddNum)
                {
                    if (isAddOverFlow)
                    {
                        for (int x = 1; x <= i_add; ++x)
                        {
                            addNumbers[i_add - x] = addNumbers[i_add] + x;
                        }
                        isAddOverFlow = false;
                    }
                    break;
                }
                else
                {
                    addNumbers[i_add] = MaxAddNum - i_add;
                    isAddOverFlow = true;
                }
            }

            if (isAddOverFlow) // dodatkove cisla dosiahli pre aktualne hlavne cisla max (skoncili na overflow)
            {
                // pre aktualnu paticu boli vygenerovane vsetky kombinacie dodatkov 
                // reset dodatkovych cisel
                addNumbers[0] = 2;
                addNumbers[1] = 1;

                // pokracovanie v inkrementacii hlavnych cisel
                for (int i = 0; i < numbers.Count; ++i)
                {
                    numbers[i]++;


                    if (numbers[i] + i <= MaxNum) // max kombinacia bude 50 49 48 47 46 resp 12 11
                    {
                        if (isOverFlow)
                        {
                            for (int x = 1; x <= i; ++x)
                            {
                                numbers[i - x] = numbers[i] + x;
                            }
                            isOverFlow = false;
                        }
                        break;
                    }
                    else
                    {
                        do
                        {
                            numbers[i]--;
                        } while (ignoreNum.Contains(numbers[i]) || numbers[i] > MaxNum);
                        isOverFlow = true;
                    }
                }
            }
        }

        public bool IsEnd()
        {
            return numbers[0] == 50 && numbers[1] == 49 && numbers[2] == 48 && numbers[3] == 47 && numbers[4] == 56 && addNumbers[0] == 12 && addNumbers[1] == 11;
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
            addNumbers = new List<int> { AddNum1, AddNum2 };

            numbers.Sort();
            numbers.Reverse();
            addNumbers.Sort();
            addNumbers.Reverse();
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
