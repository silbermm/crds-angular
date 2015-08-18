(function() {
  'use strict()';

  module.exports = TripParticipantCard;

  TripParticipantCard.$inject = ['$log', 'TripsUrlService'];

  function TripParticipantCard($log, TripsUrlService) {
    return {
      restrict: 'EA',
      transclude: true,
      templateUrl: 'trip_participant_card/tripParticipantCard.html',
      scope: {
        tripParticipant: '='
      },
      link: link
    };

    function link(scope, el, attr) {
      scope.shareUrl = TripsUrlService.ShareUrl(scope.tripParticipant.trips[0].tripParticipantId);
    }
  }
})();
