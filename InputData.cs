using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Text;

namespace EuroJack
{
    public class InputData
    {
        private readonly string _filePath;
        public InputData(string filePath, bool ignoreFirstRow, string spliter)
        {
            _filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filePath);

            InitFileData(ignoreFirstRow, spliter);
        }

        public int TotalCount { get; private set; }
        public Dictionary<int, string[]> Datas { get; set; }

        public List<DrawnNumber> DrawnNumbers { get; set; }
        public string Errors { get; set; }


        private void InitFileData(bool ignoreFirstRow, string spliter)
        {
            TotalCount = 0;
            Datas = new Dictionary<int, string[]>();
            DrawnNumbers = new List<DrawnNumber>();


            if (File.Exists(_filePath))
            {
                var items = File.ReadAllLines(_filePath);
                TotalCount = items.Length;


                for (int i = 0; i < TotalCount; i++)
                {
                    if (ignoreFirstRow && i == 0)
                        continue;

                    var tmpDatas = items[i].Split(spliter, StringSplitOptions.RemoveEmptyEntries);
                    Datas.Add((ignoreFirstRow ? i - 1 : i), tmpDatas);
                    if (tmpDatas.Length >= 9)
                    {
                        var tmpDraw = new DrawnNumber();
                        DateTime? tmpDate = null;
                        var tmpStrDate = tmpDatas[0].Split(".", StringSplitOptions.RemoveEmptyEntries);
                        if (tmpStrDate.Length == 3)
                        {
                            tmpDate = new DateTime(Convert.ToInt32(tmpStrDate[2]), Convert.ToByte(tmpStrDate[1]), Convert.ToByte(tmpStrDate[0]));
                        }

                        tmpDraw.DayAi = tmpDate.Value.ToFileTimeUtc();
                        tmpDraw.Num1 =(float)Convert.ToDecimal(tmpDatas[1]);
                        tmpDraw.Num2 = (float)Convert.ToDecimal(tmpDatas[2]);
                        tmpDraw.Num3 = (float)Convert.ToDecimal(tmpDatas[3]);
                        tmpDraw.Num4 = (float)Convert.ToDecimal(tmpDatas[4]);
                        tmpDraw.Num5 = (float)Convert.ToDecimal(tmpDatas[5]);
                        tmpDraw.Num6 = (float)Convert.ToDecimal(tmpDatas[6]);
                        tmpDraw.Num7 = (float)Convert.ToDecimal(tmpDatas[7]);
                        //                        tmpDraw.Num8 = Convert.ToDecimal(tmpDatas[8]);
                        tmpDraw.Phase = (float)Convert.ToDecimal(tmpDatas[8]);

                        DrawnNumbers.Add(tmpDraw);
                    }
                }
            }
            else
            {
                Errors = $"Chyby: Neexistuje File na {_filePath}";
            }
        }
    }

    public class DrawnNumber
    {
        //public DateTime Day { get; set; }
        public float DayAi { get; set; }
        public float Num1 { get; set; }
        public float Num2 { get; set; }
        public float Num3 { get; set; }
        public float Num4 { get; set; }
        public float Num5 { get; set; }
        public float Num6 { get; set; }
        public float Num7 { get; set; }
        //public float Num8 { get; set; }
        public float Phase { get; set; }
    }

    public class PredicateNums
    {
        [ColumnName("num1")]
        public float Num1 { get; set; }
        [ColumnName("num2")]
        public float Num2 { get; set; }
        [ColumnName("num3")]
        public float Num3 { get; set; }
        [ColumnName("num4")]
        public float Num4 { get; set; }
        [ColumnName("num5")]
        public float Num5 { get; set; }
        [ColumnName("num6")]
        public float Num6 { get; set; }
        [ColumnName("num7")]
        public float Num7 { get; set; }
        [ColumnName("num8")]
        public float Num8 { get; set; }
    }
}