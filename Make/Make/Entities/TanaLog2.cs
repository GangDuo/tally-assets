using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Make.Entities
{
    internal class TanaLog2 : TanaLog
    {
        public TanaLog2(Config conf) : base(conf) { }

        protected override Stock ParseCsv(string csv)
        {
            var fields = ParseCsvLine(csv); ;
            //var fields = csv.Split(new string[] { "," }, StringSplitOptions.None);
            var priceWithoutTax = uint.Parse(fields[4]);
            var priceIncludingTax = Math.Floor(decimal.Parse((priceWithoutTax * 1.08).ToString()));
            var itemCode = uint.Parse(fields[5]);
            var salesCostRatio = uint.Parse(fields[6]);
            var quantity = int.Parse(fields[7]);

            return new Entities.Stock()
            {
                PriceWithoutTax = priceWithoutTax,
                PriceIncludingTax = priceIncludingTax,
                ItemCode = itemCode,
                SalesCostRatio = salesCostRatio,
                Quantity = quantity
            };
        }
    }
}
