(function() {
  'use strict';

  module.exports = Room;

  Room.$inject = ['$resource'];

  function Room($resource) {
    return {
      ByLocation: $resource(__API_ENDPOINT__ + 'api/room/location/:congregationId')
    };
  }

})();
