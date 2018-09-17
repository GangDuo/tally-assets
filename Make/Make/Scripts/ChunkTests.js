// Test
var actual;
actual = Chunk.split([1,2,3,4,5]);
WScript.Echo([[1,2,3],[4,5]].toString() === actual.toString());

actual = Chunk.split([1,2,3,4,5], 3);
WScript.Echo(actual.length === 3);
WScript.Echo([1,2].toString() === actual[0].toString());
WScript.Echo([3,4].toString() === actual[1].toString());
WScript.Echo([5].toString() === actual[2].toString());

actual = Chunk.split([[1,2,3,4],[5,6,7,8],[9,10,11,12],[13,14,15,16],[17,18,19,20]], 2);
WScript.Echo(actual.length === 2);
WScript.Echo(actual[0].toString());
//[[1,2,3,4],[5,6,7,8],[9,10,11,12],[13,14,15,16],[17,18,19,20]]
