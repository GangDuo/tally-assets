using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;
using System.Diagnostics;
using MySQL;
using System.Data;
using System.Configuration;

namespace Make.Model
{
    internal class TanaLog
    {
        public int ActivityYear { get; set; }
        public int ActivityMonth { get; set; }
        public string StoreCode { get; set; }
        public List<Entities.Stock> Outsourcing { get; set; }
        public List<Entities.Stock> InHand { get; private set; }
        public List<Entities.Stock> Otherwise { get; private set; }
        public List<Entities.Stock> Results { get; private set; }

        public void LoadFrom(string folder)
        {
            var conf = new Entities.TanaLog.Config()
            {
                ActivityYear = ActivityYear,
                ActivityMonth = ActivityMonth
            };
            var tanalog = Path.Combine(folder, "TANALOG.DAT");
            var tanalog2 = Path.Combine(folder, "TANALOG2.DAT");

            InHand = new Entities.TanaLog(conf).Parse(tanalog);
            Otherwise = new Entities.TanaLog2(conf).Parse(tanalog2);
        }

        public void LoadOutsourcing(string folder)
        {
            var conf = new Entities.TanaLog.Config()
            {
                ActivityYear = ActivityYear,
                ActivityMonth = ActivityMonth
            };

            var xs = new List<Entities.Stock>();
            foreach (var file in Directory.GetFiles(folder))
            {
                if (String.Copy(file).ToLower().EndsWith(".csv"))
                {
                    xs.AddRange(new Entities.Outsourcing(conf).Parse(file));
                }
            }
            Outsourcing = xs;
        }

        //public void Configure() { }

        public void Make()
        {
            // 元の並び順を記憶
            ulong id = 0;
            Otherwise.ForEach(s => s.Id = ++id);
            InHand.ForEach(s => s.Id = ++id);
            Outsourcing.ForEach(s => s.Id = ++id);

            Results = new List<Entities.Stock>(Otherwise);
            // flagが 1 の商品を取り除く
            var clone = new List<Entities.Stock>(InHand);
            clone.AddRange(Outsourcing);
            // 単品管理 対商品のみ
            var products = (new Product()).Execute(new Server(), new Account(), null);
            // Full-Outer-Join
            //var xs = products.AsEnumerable().FullOuterJoin<DataRow, Entities.Stock, string, TT>(
            //    clone,
            //    a => a["jan"].ToString(),
            //    (Entities.Stock b) => b.JanCode,
            //    (a, b, id) => new TT()
            //    {
            //        ID = id,// jan
            //        Price = (a == null ? "" : a["suggested_retail_price"].ToString()),
            //        M = (a == null ? "0" : a["invt_mng"].ToString())
            //    }
            //).Where(x => tmp.Contains(x.ID)).ToArray();//.Where(x=> tmp.Contains(x.ID) && (x.M == "0")).ToArray();
            var xs = clone.LeftOuterJoin(
                products.AsEnumerable(),
                (Entities.Stock b) => b.JanCode,
                a => a["jan"].ToString(),
                (a, b) =>
                {
                    try
                    {
                        var janCode = a.JanCode;
                        var itemCode = (b == null ? 0U : uint.Parse(b["item_code"].ToString()));
                        var salesCostRatio = 0m;
                        if (b != null && !String.IsNullOrWhiteSpace(b["multiplications"].ToString()))
                        {
                            salesCostRatio = decimal.Parse(b["multiplications"].ToString());
                        }
                        var priceIncludingTax = (b == null ? 0m : decimal.Parse(b["suggested_retail_price"].ToString()));
                        var flag = (b == null ? -1 : int.Parse(b["invt_mng"].ToString()));

                        if (flag == -1)
                        {
                            flag = 0;
                            if (a.JanCode.StartsWith("04"))
                            {
                                // JANマスタに無 かつ 04 始まりのJAN
                                salesCostRatio = decimal.Parse(janCode.Substring(2, 2));
                                itemCode = uint.Parse(janCode.Substring(4, 3));
                                priceIncludingTax = uint.Parse(Math.Floor(uint.Parse(janCode.Substring(7, 5)) / 1.05 * 1.08).ToString());
                            }
                            else
                            {
                                // JANマスタに無 その他
                                // 部門・掛け率 N/A は部門コードを存在しないコードに設定
                                itemCode = uint.MaxValue;
                                priceIncludingTax = decimal.Parse(a.JanCode);
                            }
                        }

                        // フラグ2だったら
                        // 掛け率 0、売単価(税込) 0
                        // としてJScript側で
                        // 下代が 796（税抜）となるようにする
                        if (flag == 2)
                        {
                            salesCostRatio = 0;
                            priceIncludingTax = 0;
                        }

                        return new Entities.Stock()
                        {
                            Id = a.Id,
                            JanCode = janCode,
                            ItemCode = itemCode,
                            SalesCostRatio = salesCostRatio,
                            PriceIncludingTax = priceIncludingTax,
                            Quantity = a.Quantity,
                            Flag = flag
                        };
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                        throw new Exception("JanCode = " + a.JanCode);
                    }
                }).OrderBy(r => r.Id);
            Debug.Assert(clone.Select(i => i.JanCode).Count() == xs.Count());
            Results.AddRange(xs.Where(x => x.Flag == 0 || x.Flag == 2));
        }

        private static readonly string Cmd = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Scripts", "Excel.wsf");
        private static readonly string XlsTmpl = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "tmpl.xlsx");

        public void SaveAsExcel(string xlsPath)
        {
            Debug.Assert(xlsPath.EndsWith(".xlsx"));

            var stores = (new Store()).Execute(new Server(), new Account(), null).AsEnumerable().ToDictionary(
                row => row["code"].ToString(),
                row => row["name"].ToString().Replace("店", ""));

            // 原紙をコピー
            File.Copy(XlsTmpl, xlsPath);
            using (var hProcess = Process.Start(new ProcessStartInfo()
            {
                FileName = @"cscript",
                Arguments = String.Format(@"//B //Nologo ""{0}"" --file ""{1}"" --city {2}", Cmd, xlsPath, stores[StoreCode]),
                WindowStyle = ProcessWindowStyle.Hidden,
                //
                UseShellExecute = false,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
            }))
            {
                // JSON変換
                var o = new List<List<string>>();
                foreach (var item in Results)
                {
                    o.Add(new List<string>()
                    {
                        item.ItemCode.ToString(),
                        Math.Floor(item.SalesCostRatio).ToString(),
                        Math.Floor(item.PriceIncludingTax).ToString(),
                        item.Quantity.ToString()
                    });
                }
                var actual = Text.Json.ToString(o);

                //hProcess.StandardInput.Write("[[1,2,3,4],[5,6,7,8],[9,10,11,12],[13,14,15,16],[17,18,19,20]]");
                hProcess.StandardInput.Write(Text.Json.ToString(o));
                hProcess.StandardInput.Close();

                hProcess.WaitForExit();
                var stdout = hProcess.StandardOutput.ReadToEnd();
                var stderr = hProcess.StandardError.ReadToEnd();
                if (stderr.Length > 0)
                {
                    throw new Exception(stderr);
                }
            }
        }
    }

    internal class Server : MySQL.IServer
    {
        public string Address { get { return ConfigurationManager.AppSettings["DatabaseHostName"]; } }
        public uint Port { get { return uint.Parse(ConfigurationManager.AppSettings["DatabasePort"]); } }
    }
    internal class Account : MySQL.IAccount
    {
        public string Username { get { return ConfigurationManager.AppSettings["DatabaseUsername"]; } }
        public string Password { get { return ConfigurationManager.AppSettings["DatabasePassword"]; } }
    }

    internal class Product: MySQL.AbstractRunnableSql
    {
        protected override DataTable RunSql(Transcation con, object arg)
        {
            var sql = @"SELECT `t`.`jan`,
                               `t`.`suggested_retail_price`,
                               `t`.`multiplications`,
                               `t`.`item_code`,
                               `t`.`invt_mng`
                          FROM `humpty_dumpty`.`invt_products` `t`;";
            var table = con.GetTable(sql);
            foreach (DataRow row in table.Rows)
            {
                if (String.IsNullOrEmpty(row["invt_mng"].ToString()))
                {
                    row["invt_mng"] = 0;
                }
            }
            table.AcceptChanges();
            return table;
        }
    }

    internal class Store : MySQL.AbstractRunnableSql
    {
        protected override DataTable RunSql(Transcation con, object arg)
        {
            var sql = @"SELECT `t`.`code`,
                               `t`.`name`
                          FROM `humpty_dumpty`.`TStores` `t`;";
            return con.GetTable(sql);
        }
    }
}
