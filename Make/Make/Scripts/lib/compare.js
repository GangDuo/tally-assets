var compare;
(function(compare) {
  function descAsString(a, b) { return a < b ? 1 : -1; }
  function ascAsNumber(a, b) { return a - b; }
  function descAsNumber(a, b) { return b - a; }
  // export
  compare.descAsString = descAsString;
  compare.ascAsNumber = ascAsNumber;
  compare.descAsNumber = descAsNumber;
})(compare || (compare = {}));

(function() {
  //tests();

  function tests() {
    var actual,
        expected,
        orderByAscAsString = ['a', 'b', 'c', 'd', 'e', 'f', 'g'],
        stringArray = ['f', 'b', 'd', 'c', 'a', 'g', 'e'],
        orderByAscAsNumber = [1, 5, 40, 200],
        numberArray = [40, 1, 5, 200]

    actual = stringArray.sort();
    expected = orderByAscAsString;
    WScript.Echo('asc as string:', actual.join() === expected.join());

    actual = stringArray.sort(compare.descAsString);
    expected = orderByAscAsString.reverse();
    WScript.Echo('desc as string:', actual.join() === expected.join());

    actual = numberArray.sort(compare.ascAsNumber);
    expected = orderByAscAsNumber;
    WScript.Echo('asc as number:', actual.join() === expected.join());

    actual = numberArray.sort(compare.descAsNumber);
    expected = orderByAscAsNumber.reverse();
    WScript.Echo('desc as number:', actual.join() === expected.join());
  }
})();
