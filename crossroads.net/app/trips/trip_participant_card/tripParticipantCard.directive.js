(function(){
  'use strict()';

  module.exports = TripParticipantCard;

  TripParticipantCard.$inject = ['$log', 'Session'];

  function TripParticipantCard($log,Session){
    return {
      restrict: 'EA',
      transclude: true,
      templateUrl : 'trip_participant_card/tripParticipantCard.html',
      scope : {
        tripParticipant: '='
      }
    };
  }
})();
