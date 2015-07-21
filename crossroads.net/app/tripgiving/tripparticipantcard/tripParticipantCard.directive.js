(function(){
  'use strict()';

  module.exports = TripParticipantCard;

  TripParticipantCard.$inject = ['$log', 'Session'];

  function TripParticipantCard($log,Session){
    return {
      restrict: 'EA',
      transclude: true,
      templateUrl : 'tripparticipantcard/tripParticipantCard.html',
      scope : {
        tripParticipant: '='
      },
      link : link
    };
  }
  function link(scope, el, attr) {
    scope.convertToDate = convertToDate;

    function convertToDate(date){
      var dateNum = Number(date * 1000);
      var d = new Date(dateNum);
      return d;
    }
  }

})();
