(function() {
  'use strict()';

  module.exports = TripParticipantCard;

  TripParticipantCard.$inject = ['$log', 'TripsUrlService', '$state'];

  function TripParticipantCard($log, TripsUrlService, $state) {
    return {
      restrict: 'EA',
      transclude: true,
      templateUrl: 'trip_participant_card/tripParticipantCard.html',
      scope: {
        tripParticipant: '=',
      },
      link: link,
    };

    function link(scope, el, attr) {

      scope.goToGiving = goToGiving;
      scope.shareUrl = TripsUrlService.ShareUrl(scope.tripParticipant.trips[0].tripParticipantId);

      function goToGiving() {
        var pId = angular.copy(scope.tripParticipant.trips[0].tripParticipantId);
        $state.go('tripgiving.amount', {eventParticipantId: pId});
      }

    }
  }
})();
