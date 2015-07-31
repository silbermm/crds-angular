(function(){
  'use strict()';

  module.exports = TripDonations;

  TripDonations.$inject = ['$log', 'Session'];

  function TripDonations($log, Session){
    return {
      restrict: 'EA',
      transclude: true,
      templateUrl : '/tripDonations.html',
      scope : {
        donation: '='
      }
    };
  }
})();
