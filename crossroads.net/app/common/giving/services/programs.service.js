(function() {
  'use strict';
  module.exports = Programs;

  Programs.$inject = ['$resource'];

  function Programs($resource) {
    return {
      Programs: $resource(__API_ENDPOINT__ +  'api/programs/:programType', { programType: '@programType' })
    };
  }

})();
