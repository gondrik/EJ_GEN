using Microsoft.ML;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace EuroJack
{
    internal class PlanetLottery
    {
        public static void Run()
        {
            var random = new Random(50);

            using (var sw = File.CreateText($"ej_{DateTime.Now:hhmmss}.txt"))
            {
                for (int j = 0; j < 36; j++)
                {
                    var sb = new StringBuilder();
                    var nums = new List<int>();
                    for (int i = 0; i < 5; i++)
                    {
                        int num = 0;
                        while (true)
                        {
                            num = random.Next(1, 50);

                            if (!nums.Contains(num))
                            {
                                nums.Add(num);
                                break;
                            }
                        }

                        sb.Append($"{num} ");
                    }

                    for (int i = 0; i < 2; i++)
                    {
                        int num = 0;
                        while (true)
                        {
                            num = random.Next(1, 10);

                            if (!nums.Contains(num))
                            {
                                nums.Add(num);
                                break;
                            }
                        }

                        sb.Append($"{num} ");
                    }

                    sw.WriteLine($"{j}. {sb.ToString()}");
                }
            }


            var ins = new InputData("eurojackpot_moon.csv", true, ";");

            var ml = new MLContext();

            using (var sw = File.CreateText($"ej_{DateTime.Now:hhmmss}.txt"))
            {

                for (int a = 0; a < 34; a++)
                {
                    var data = ml.Data.LoadFromEnumerable<DrawnNumber>(ins.DrawnNumbers);
                    var row = ml.Data.CreateEnumerable<DrawnNumber>(data, true);


                    var num1 = ml.Transforms.CopyColumns(outputColumnName: "Label", inputColumnName: "Num1")/*.Append(ml.Transforms.Categorical.OneHotEncoding(outputColumnName: "DayEncoded", inputColumnName: "Day"))*/.Append(ml.Transforms.Concatenate(outputColumnName: "Features", "DayAi", "Num1", "Num2", "Num3", "Num4", "Num5", "Num6", "Num7", "Phase")).Append(ml.Regression.Trainers.Sdca()).Append(ml.Transforms.CopyColumns(outputColumnName: "num1", inputColumnName: "Score"));
                    var num2 = ml.Transforms.CopyColumns(outputColumnName: "Label", inputColumnName: "Num2")/*.Append(ml.Transforms.Categorical.OneHotEncoding(outputColumnName: "DayEncoded", inputColumnName: "Day")) */.Append(ml.Transforms.Concatenate(outputColumnName: "Features", "DayAi", "Num1", "Num2", "Num3", "Num4", "Num5", "Num6", "Num7", "Phase")).Append(ml.Regression.Trainers.Sdca()).Append(ml.Transforms.CopyColumns(outputColumnName: "num2", inputColumnName: "Score"));
                    var num3 = ml.Transforms.CopyColumns(outputColumnName: "Label", inputColumnName: "Num3")/*.Append(ml.Transforms.Categorical.OneHotEncoding(outputColumnName: "DayEncoded", inputColumnName: "Day")) */.Append(ml.Transforms.Concatenate(outputColumnName: "Features", "DayAi", "Num1", "Num2", "Num3", "Num4", "Num5", "Num6", "Num7", "Phase")).Append(ml.Regression.Trainers.Sdca()).Append(ml.Transforms.CopyColumns(outputColumnName: "num3", inputColumnName: "Score"));
                    var num4 = ml.Transforms.CopyColumns(outputColumnName: "Label", inputColumnName: "Num4")/*.Append(ml.Transforms.Categorical.OneHotEncoding(outputColumnName: "DayEncoded", inputColumnName: "Day")) */.Append(ml.Transforms.Concatenate(outputColumnName: "Features", "DayAi", "Num1", "Num2", "Num3", "Num4", "Num5", "Num6", "Num7", "Phase")).Append(ml.Regression.Trainers.Sdca()).Append(ml.Transforms.CopyColumns(outputColumnName: "num4", inputColumnName: "Score"));
                    var num5 = ml.Transforms.CopyColumns(outputColumnName: "Label", inputColumnName: "Num5")/*.Append(ml.Transforms.Categorical.OneHotEncoding(outputColumnName: "DayEncoded", inputColumnName: "Day")) */.Append(ml.Transforms.Concatenate(outputColumnName: "Features", "DayAi", "Num1", "Num2", "Num3", "Num4", "Num5", "Num6", "Num7", "Phase")).Append(ml.Regression.Trainers.Sdca()).Append(ml.Transforms.CopyColumns(outputColumnName: "num5", inputColumnName: "Score"));
                    var num6 = ml.Transforms.CopyColumns(outputColumnName: "Label", inputColumnName: "Num6")/*.Append(ml.Transforms.Categorical.OneHotEncoding(outputColumnName: "DayEncoded", inputColumnName: "Day")) */.Append(ml.Transforms.Concatenate(outputColumnName: "Features", "DayAi", "Num1", "Num2", "Num3", "Num4", "Num5", "Num6", "Num7", "Phase")).Append(ml.Regression.Trainers.Sdca()).Append(ml.Transforms.CopyColumns(outputColumnName: "num6", inputColumnName: "Score"));
                    var num7 = ml.Transforms.CopyColumns(outputColumnName: "Label", inputColumnName: "Num7")/*.Append(ml.Transforms.Categorical.OneHotEncoding(outputColumnName: "DayEncoded", inputColumnName: "Day")) */.Append(ml.Transforms.Concatenate(outputColumnName: "Features", "DayAi", "Num1", "Num2", "Num3", "Num4", "Num5", "Num6", "Num7", "Phase")).Append(ml.Regression.Trainers.Sdca()).Append(ml.Transforms.CopyColumns(outputColumnName: "num7", inputColumnName: "Score"));
                    //            var num8 = ml.Transforms.CopyColumns(outputColumnName: "Label", inputColumnName: "Num8")/*.Append(ml.Transforms.Categorical.OneHotEncoding(outputColumnName: "DayEncoded", inputColumnName: "Day")) */.Append(ml.Transforms.Concatenate(outputColumnName: "Features", "DayAi", "Num1", "Num2", "Num3", "Num4", "Num5", "Num6", "Num7", "Num8", "Phase")).Append(ml.Regression.Trainers.Sdca()).Append(ml.Transforms.CopyColumns(outputColumnName: "num8", inputColumnName: "Score"));

                    var multipl = num1.Append(num2).Append(num3).Append(num4).Append(num5).Append(num6).Append(num7).Fit(data);//.Transform(data);

                    var pred = ml.Model.CreatePredictionEngine<DrawnNumber, PredicateNums>(multipl);

                    var tmp = pred.Predict(new DrawnNumber() { DayAi = new DateTime(2020, 3, 20).ToFileTimeUtc(), Phase = 9.9f });
                    sw.WriteLine($"{a}: {Math.Round(tmp.Num1)} {Math.Round(tmp.Num2)} {Math.Round(tmp.Num3)} {Math.Round(tmp.Num4)} {Math.Round(tmp.Num5)} {Math.Round(tmp.Num6)} {Math.Round(tmp.Num7)}");
                }
            }


            Console.WriteLine($"TotalCount:{ins.TotalCount}.{ins.Errors}");

            //Console.ReadLine();
        }
    }
}
