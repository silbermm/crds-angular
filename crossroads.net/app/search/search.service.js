(function() {
  module.exports = Search;

  Search.$inject = ['$resource'];

  function Search($resource) {
    return {
      //Search: $resource(__API_ENDPOINT__+'api/search')
      Search: $resource(__AWS_SEARCH_ENDPOINT__+'search')
    };
  }
})();
