(function(begin, end) {
  var i;

  begin = begin || 1;
  end = end || 10;
  for(i = begin; i <= end; i++) {
    _createFolder(('00' + i.toString()).slice(-3));
  }

  function _createFolder(folder) {
    var fs,
        e;

    try {
      fs = WScript.CreateObject("Scripting.FileSystemObject")
    } catch(e) {
      WScript.StdErr.WriteLine(e.message)
      return;
    }

    try {
      if(fs.FolderExists(folder)) {
          throw new Error("�t�H���_ " + folder + " �͊��ɑ��݂��Ă��܂��B");
      } else {
          fs.CreateFolder(folder);
          WScript.StdOut.WriteLine("�t�H���_ " + folder + " ���쐬���܂����B");
      }
    } catch(e) {
      WScript.StdErr.WriteLine(e.message)
    } finally{
      fs = null;
    }
  }
}).apply(this, (function() {
	var i,
  		args = [];
	for(i = 0; i < WScript.Arguments.length; i++) {
  	args.push(WScript.Arguments(i));
  }
  return args;
})());
