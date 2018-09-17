/*
var ExcelApplication = (function() {
  function ExcelApplication() {}

  ExcelApplication.prototype.usingExcelApplication = function(callback) {
    var app = null,
        e;

    try {
      this._app = app = WScript.CreateObject("Excel.Application");
      callback.call(app);
    } catch(e) {
    } finally {
      try {
        app.Quit();
      } catch(e) {}
    }
  };

  ExcelApplication.prototype.usingExcelWorkbook = function(pathXls, callback) {
    var app = this._app,
        book;

    try {
      book = app.Workbooks.Open(pathXls);
      callback.call(book);
    } catch(e) {
    } finally {
      try {
        book.Close();
      } catch(e) {}
    }
  };

  return ExcelApplication;
})();


var StockHistory = (function() {
  function StockHistory(raw) {
    this._raw = raw;
  }

  StockHistory.prototype.excelify = function() {
    var excel = new ExcelApplication();
    excel.usingExcelApplication(function(app) {
      excel.usingExcelWorkbook(, function(book) {
      });
    });
  };

  return StockHistory;
})();
var history = new StockHistory(data.raw);
history.excelify(pathXls);

var Aggregater = (function() {
  function Aggregater() {}

  Aggregater.prototype.excelify = function() {
  };

  return Aggregater;
})();

*/




function repeat(element, count) {
  var i,
      xs = [];

  for (i = 0; i < count; ++i) {
    xs.push(element);
  }
  return xs;
}

/**
 * @param jsArray2D
 * @return
 */
function array2dToSafeArray2d(jsArray2d) {
  var script = WScript.CreateObject('ScriptControl');
  script.Language = 'VBScript';
  // --------------------
  // コメント削除禁止
  //   VBscript本体
  // --------------------
  var code = (function () {/*
Function ConvertArray(jsArray)
    ReDim arr(jsArray.length - 1, jsArray.[0].length - 1)
    outerCount = 0
    For Each outer In jsArray
        innerCount = 0
        For Each inner In outer
            arr(outerCount, innerCount) = inner
            innerCount = innerCount + 1
        Next
        outerCount = outerCount + 1
    Next
    ConvertArray = arr
End Function
*/}).toString().match(/[\s\S]*\/\*([\s\S]*)\*\/\}/)[1];

  script.AddCode(code);
  return script.Run('ConvertArray', jsArray2d);
}

function JSArray2SafeArray(ar) {
  var i,
      dic = new ActiveXObject("Scripting.Dictionary");

  for (i = 0; i < ar.length; i++) {
    dic.add(i, ar[i]);
  }
  return dic.items();
}

function fillInto(type, atTopLeft, jsArray2d) {
  var rowLength = jsArray2d.length,
      columnLength = jsArray2d[0].length,
      safeArray2d = array2dToSafeArray2d(jsArray2d),
      range = atTopLeft.Resize(rowLength, columnLength);

  switch (type.toLowerCase()) {
    case 'value':
      range.Value = safeArray2d;
      return;
    case 'formular1c1':
      range.FormulaR1C1 = safeArray2d;
      return;
    default:
      return;
  }
}

function fillValue(atTopLeft, jsArray2d) {
  fillInto('Value', atTopLeft, jsArray2d);
}

function fillFormulaR1C1(atTopLeft, jsArray2d) {
  fillInto('FormulaR1C1', atTopLeft, jsArray2d);
}
