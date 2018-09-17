/**
 * Collectionをある数ごとに分割する
 */
var Chunk = (function () {
  function Chunk() { }

  /**
   * @returns IEnumerable<IEnumerable<T>>
   * @param sources IEnumerable<T>
   */
  Chunk.split = function (sources, chunkSize) {
    var i,
         n,
        newArr = [];

    chunkSize = chunkSize || 2;
    for (i = 0; i < Math.ceil(sources.length / chunkSize) ; ++i) {
      n = i * chunkSize;
      newArr.push(sources.slice(n, n + chunkSize));
    }
    return newArr;
  }

  return Chunk;
})();
