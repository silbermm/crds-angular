(function(){
  "use strict()";

  module.exports = TripParticipantCard;

  TripParticipantCard.$inject = ['$log', 'Session'];

  function TripParticipantCard($log,Session){
    return {
      restrict: "EA",
      transclude: true,
      templateUrl : "tripparticipantcard/tripParticipantCard.html",
      scope : {
        tripParticipant: '='
      },
      link : link
    };
  }
  function link(scope, el, attr) {
    scope.convertToDate = convertToDate;

    function convertToDate(date){
      // date comes in as mm/dd/yyyy, convert to yyyy-mm-dd for moment to handle
      var dateNum = Number(date * 1000);
      var d = new Date(dateNum);
      return d;
    };
  };

})();
