(function(){
  'use strict()';

  module.exports = MyTripCard;

  MyTripCard.$inject = ['$log', 'Session'];

  function MyTripCard($log,Session){
    return {
      restrict: 'EA',
      transclude: true,
      templateUrl : 'mytrips/mytripCard.html',
      scope : {
        trip: '='
      },
      link : link
    };

    function link(scope, el, attr){

      scope.goalMet = goalMet;

      function goalMet (totalRaised, goal) {
        return (totalRaised >= goal);
      }
    }
  }
})();
