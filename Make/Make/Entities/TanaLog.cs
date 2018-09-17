using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Text.Extensions;

namespace Make.Entities
{
    internal class TanaLog
    {
        public class Config
        {
            public int ActivityYear { get; set; }
            public int ActivityMonth { get; set; }
        }
        private Config Conf { get; set; }

        public TanaLog(Config conf)
        {
            Conf = conf;
        }

        virtual protected Csv.ICsvLine ParseCsvLine(string csv)
        {
            var lines = new List<Csv.ICsvLine>(Csv.CsvReader.ReadFromText(csv, new Csv.CsvOptions()
            {
                HeaderMode = Csv.HeaderMode.HeaderAbsent
            }).ToArray());

            if (lines.Count > 1)
            {
                throw new ArgumentException();
            }
            return lines[0];
        }

        virtual protected Entities.Stock ParseCsv(string csv)
        {
            var fields = ParseCsvLine(csv);
            //var fields = csv.Split(new string[] { "," }, StringSplitOptions.None);
            return new Entities.Stock()
            {
                JanCode = fields[4],
                Quantity = int.Parse(fields[5])
            };
        }

        virtual protected List<string> Take(string path)
        {
            var validLines = new List<string>();
            using (var sr = new StreamReader(path, Encoding.GetEncoding("Shift_JIS")))
            {
                while (!sr.EndOfStream)
                {
                    var line = sr.ReadLine();
                    // TODO: 棚卸月より前の棚データは削除する
                    //       これだとダメかも
                    string pattern = String.Empty;
                    var m = String.Format("{0:00}", Conf.ActivityMonth);
                    if (Conf.ActivityMonth < 10)
                    {
                        pattern = @"^(?=" + Conf.ActivityYear.ToString() + "0[" + Conf.ActivityMonth + "-9]).*$";
                    }
                    else
                    {
                        pattern = @"^(?=" + Conf.ActivityYear.ToString() + "1[0-2]).*$";
                    }
                    //var pattern = @"^(?=" + Conf.ActivityYear.ToString() + "0[2-9]).*$";
                    if (Regex.IsMatch(line, pattern))
                    {
                        validLines.Add(line);
                        break;
                    }
                }
                while (!sr.EndOfStream)
                {
                    validLines.Add(sr.ReadLine());
                }
            }
            return validLines;
        }

        public List<Entities.Stock> Parse(string path)
        {
            var History = new List<Entities.Stock>();
            var lines = Take(path);
            //foreach (var line in Take(path))
            for (int i = 0; i < lines.Count; i++)
            {
                var line = lines[i];
                try
                {
                    var fields = ParseCsv(line);
                    if (0 == fields.Quantity)
                    {
                        continue;
                    }
                    if (null != fields.JanCode && fields.JanCode.Length < 13)
                    {
                        fields.JanCode = ("0000000000000" + fields.JanCode).Right(13);
                    }
                    History.Add(fields);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    throw new Exception(String.Format("{0}({1}行目): {2}", path, i + 1, ex.Message));
                }
            }
            return History;
        }
    }
}
