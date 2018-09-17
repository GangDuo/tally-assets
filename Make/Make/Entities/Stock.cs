using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Make.Entities
{
    internal class Stock
    {
        // 部門 掛率  売単価(税込) 数量 売単価(税抜) 下代合計 下代
        public ulong Id { get; set; }
        public uint ItemCode { get; set; }
        public decimal SalesCostRatio { get; set; }
        public decimal PriceWithoutTax { get; set; }
        public decimal PriceIncludingTax { get; set; }
        public string JanCode { get; set; }
        public int Quantity { get; set; }
        public int Flag { get; set; }
        //public ulong BuyingPriceWithoutTax { get; set; }

        //public ulong Price
        //{
        //    get
        //    {
        //        var raw = PriceWithoutTax * 1.08m;
        //        return ulong.Parse(Math.Floor(PriceWithoutTax * 1.08m).ToString());
        //    }
        //}
        //private static readonly int ConsumptionTaxRate = 8;
    }
}
