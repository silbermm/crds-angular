(function() {
  'use strict';

  module.exports = ChildCareService;

  ChildCareService.$inject = ['$resource'];

  function ChildCareService($resource) {
    var childCareService = {
      Need: $resource('__API_ENDPOINT__' + 'api/childcare/need')

    };

    return childCareService;
  }
})();
