using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Make.Entities
{
    internal class Outsourcing : TanaLog
    {
        public Outsourcing(Config conf) : base(conf) { }

        protected override Stock ParseCsv(string csv)
        {
            //var fields = csv.Split(new string[] { "," }, StringSplitOptions.None);
            var fields = ParseCsvLine(csv);
            int quantity;

            if (int.TryParse(fields[4], out quantity))
            {
                // 数値に変換できます
                quantity = int.Parse(fields[4]);
            }
            else
            {
                // 数字ではありません
                quantity = 0;
            }
            return new Entities.Stock()
            {
                JanCode = fields[0],
                Quantity = quantity
            };
        }

        protected override List<string> Take(string path)
        {
            var lines = new List<string>();
            using (var sr = new StreamReader(path, Encoding.GetEncoding("Shift_JIS")))
            {
                var csv = sr.ReadToEnd();
                lines.AddRange(csv.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries));
            }
            return lines;
        }
    }
}
