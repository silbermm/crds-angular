'use strict()';
(function(){

  angular.module('crossroads').factory('Capacity', Capacity);

  Capacity.$inject = ['$resource'];

  function Capacity($resource){
    return $resource(__API_ENDPOINT__ + 'api/serve/opp-capacity'); 
  }


})();
