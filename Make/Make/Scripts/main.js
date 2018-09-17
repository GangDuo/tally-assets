//--------------------------------------------------------------
// 明細
//--------------------------------------------------------------
var StockHistory = (function() {
  function StockHistory(raw) {
    this._raw = raw;
  }

  //StockHistory.prototype.excelify = function() {
  //};

  StockHistory.excelify = function(book, data) {
    halve(data);
    var sheet = book.Worksheets("明細");
    var x = [
      '=ROUNDUP(RC[-2]/1.08,0)',
      '=RC[1]*RC[-2]',
      '=IF(AND(RC[-6]=258, RC[-5]=0, RC[-4]=0), 796, ROUNDDOWN(RC[-2]*RC[-5]%,0))'
    ];

    // 値セット
    fillValue(sheet.Range("A2"), data.left);
    fillValue(sheet.Range("I2"), data.right);
    // 数式セット
    fillFormulaR1C1(sheet.Range("A2").Offset(0, 4), repeat(x, data.left.length));
    fillFormulaR1C1(sheet.Range("I2").Offset(0, 4), repeat(x, data.right.length));
    // 罫線引く
    sheet.Range("A1").Resize(data.left.length + 1, 7).Borders.LineStyle = true;
    sheet.Range("I1").Resize(data.right.length + 1, 7).Borders.LineStyle = true;
  };

  return StockHistory;

  function halve(data) {
    // リストを2つに分割
    // TODO: Classにしたい
    var i,
        half,
        json = data.raw;

    if (true) {
      half = Math.ceil(json.length / 2);
      for (i = 0; i < half; i++) {
        data.left.push(json[i]);
      }
      for (; i < json.length; i++) {
        data.right.push(json[i]);
      }
    } else {
      half = Chunk.split(json, 2);
      WScript.Echo(half[0]);
      WScript.Echo(half[1]);
      data.left.push(half[0]);
      data.right.push(half[1]);
    }
  }
})();

//--------------------------------------------------------------
// 集計
//--------------------------------------------------------------
var Aggregater = (function() {
  function Aggregater() {}

  Aggregater.excelify = function(book, data) {
    // 並び替える
    var sortedData = data.raw.concat();
    (function () {
      sortedData.sort(function (a, b) {
        return a[0] < b[0] ? -1 : 1;
      });
    })();
    // 部門・掛け率 N/A は削除
    var remaining = [];
    (function () {
      var i;
      for (i = 0; i < sortedData.length; ++i) {
        if ('4294967295' !== sortedData[i][0].toString()) {
          remaining.push(sortedData[i]);
        }
      }
    })();
    sortedData = remaining;
    sheet = book.Worksheets("集計");
    fillValue(sheet.Range("A3"), sortedData);
    fillFormulaR1C1(sheet.Range("A3").Offset(0, 4), repeat([
      '=ROUNDUP(RC[-2]/1.08,0)',
      '=RC[-1]*RC[-2]',
      '=RC[1]*RC[-3]',
      // 部門 = 258、掛率 = 0、上代 = 0 なら 下代 = 796
      '=IF(AND(RC[-7]=258, RC[-6]=0, RC[-5]=0), 796, ROUNDDOWN(RC[-3]*RC[-6]%,0))',
      '=RC[-2]/RC[-3]'
    ], sortedData.length));
    // 小計
    (function() {
      var GroupBy = 1;//int
      //var XlConsolidationFunction ;
      var TotalList = JSArray2SafeArray([4, 6, 7]),
          Replace = true,
          PageBreaks = false,
          SummaryBelowData = xlSummaryBelow;
      sheet.Range("A2").Resize(1 + sortedData.length, 8).Subtotal(
        GroupBy, xlSum, TotalList, Replace, PageBreaks, SummaryBelowData);
    })();
    // アウトラインの詳細を折りたたむ
    (function() {
      var RowLevels = 2,
          ColumnLevels = 0;
      sheet.Outline.ShowLevels(RowLevels, ColumnLevels);
    })();
    // 列の非表示
    sheet.Range("B:C,E:E,H:H").EntireColumn.Hidden = true;
    // 粗利の計算式入れる
    fillFormulaR1C1(sheet.Range("I3"), repeat(['=RC[-2]/RC[-3]'], sheet.Range("A1").End(xlDown).Row - 2));
    // 罫線引く
    sheet.Range("A2").Resize(sheet.Range("A1").End(xlDown).Row - 1, sheet.Range("A2").End(xlToRight).Column).Borders.LineStyle = true;
    // 店舗名
    sheet.Range("A1").Value = data.city + '店　棚卸一覧';
  };

  return Aggregater;
})();

(function () {
  var ExcelApp,
      book,
      data = {
        raw: null,
        left: [],
        right: [],
        city: ''
      },
      commandLine = new CommandLine();

  try {
    commandLine.parse([
      {
        short: 'f'
      , long: 'file'
      , description: 'エクセルファイルのパス'
      , value: true
      },
      {
        short: 'c'
      , long: 'city'
      , description: '店舗所在地域 市町村'
      , value: true
      }
      ], false);
    data.city = commandLine.get('city');

    data.raw = UserData.parse();
    //WScript.Echo(UserData.read());

    ExcelApp = WScript.CreateObject("Excel.Application");
    ExcelApp.Visible = true;
    book = ExcelApp.Workbooks.Open(commandLine.get('file'));
    StockHistory.excelify(book, data);
    Aggregater.excelify(book, data);

    ExcelApp.DisplayAlerts = false;
    book.Save();
    ExcelApp.DisplayAlerts = true;
    WScript.StdOut.WriteLine("completed!");
  } catch(e) {
    WScript.StdErr.WriteLine(e.message);
  } finally {
    try {
      book.Close()
    } catch(e) {
      WScript.StdErr.WriteLine(e.message);
    }

    try {
      ExcelApp.Quit()
    } catch(e) {
      WScript.StdErr.WriteLine(e.message);
    }
  }
})();
