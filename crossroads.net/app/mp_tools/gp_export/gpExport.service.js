'use strict()';
(function() {
  angular.module('crossroads.mptools').factory('GPExport', GPExport);

  GPExport.$inject = ['$resource'];

  function GPExport($resource) {
    return $resource(__API_ENDPOINT__ + 'api/gpexport');
  }

})();
