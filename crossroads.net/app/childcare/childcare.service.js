(function() {
  'use strict';

  module.exports = ChildcareService;

  ChildcareService.$inject = ['$resource'];

  function ChildcareService($resource) {
    return {
      ChildcareEvent: $resource(__API_ENDPOINT__ + 'api/childcare/event/:eventId')
    };
  }

})();
