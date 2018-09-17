using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Make
{
    class CommandLine
    {
        // http://opcdiary.net/?p=26847
        public bool ShowHelp { get; private set; }
        public string StoreCode { get; private set; }
        public string StoreName { get; private set; }
        public int Year { get; private set; }
        public int Month { get; private set; }
        public string TanalogDir { get; private set; }
        public string OutsourcingDir { get; private set; }
        public string OutputFile { get; private set; }
        public List<string> Extra { get; private set; }

        private Mono.Options.OptionSet Options;

        public CommandLine()
        {
            ShowHelp = false;
            StoreCode = String.Empty;
            StoreName = String.Empty;
            Year = 2099;
            Month = 1;
            TanalogDir = String.Empty;
            OutsourcingDir = String.Empty;
            OutputFile = String.Empty;

            #region コマンドラインオプションを設定する
            Options = new Mono.Options.OptionSet() {
                //オプションとオプションの説明、そのオプションの引数に対するアクションを定義する
                {"c|store-code=", "店舗コード", v => StoreCode = v},
                {"n|store-name=", "店舗名", v => StoreName = v},
                {"y|year=", "棚卸年", (int v) => Year = v},
                {"m|month=", "棚卸月", (int v) => Month = v},
                {"t|tanalog-dir=", "TANALOG.DAT TANALOG2.DATが保存されているフォルダ", v => TanalogDir = v},
                {"d|outsourcing-dir=", "業者が納品した棚卸データが保存されているフォルダ", v => OutsourcingDir = v},
                {"o|output-file=", "エクセルの保存ファイルパス（拡張子:xlsxを含める）", v => OutputFile = v},
                //VALUEをとらないオプションは以下のようにnullとの比較をしてTrue/Falseの値をとるようにする
//                {"h|help", "show help.", v => showHelp = v != null}
            };
            #endregion
        }

        public void Parse(string[] args)
        {
            // OptionSetクラスのParseメソッドでOptionSetの内容に合わせて引数をパースする。
            // Paseメソッドはパース仕切れなかったコマンドラインのオプションの内容をstring型のリストで返す。
            // パースに失敗した場合OptionExceptionを発生させる
            Extra = Options.Parse(args);
            Extra.ForEach(t => Debug.WriteLine(t));
        }
    }
}
