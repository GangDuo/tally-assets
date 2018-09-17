/**
 * 
 */
var UserData = (function () {
  var buf = null;

  function UserData() {}

  /**
   * @returns 標準入力から読み取った文字列
   */
  UserData.read = function () {
    buf = buf || _readStdIn();
    return buf;
  };

  /**
   * 標準入力から読み取った文字列をインスタンス化します
   */
  UserData.parse = function () {
    // インスタンス化
    //var json = UserData.read().replace(/\r\n/g, '')
    try {
      return (new Function("return " + UserData.read() + ';'))();
    } catch(e) {
      throw new Error('標準入力形式がJSONではありません。');
    }
  }

  return UserData;

  // private
  function _readStdIn() {
    var stdin = '';

    // 標準入力からjson読込
    while (!WScript.StdIn.AtEndOfStream) {
      stdin += WScript.StdIn.ReadAll();
    }
    return stdin;
  }
})();
