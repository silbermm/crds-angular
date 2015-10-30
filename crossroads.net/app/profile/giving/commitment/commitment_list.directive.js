(function() {
  'use strict()';

  module.exports = CommitmentList;

  CommitmentList.$inject = ['$rootScope', '$log', 'DonationService', 'GiveTransferService'];

  function CommitmentList($rootScope, $log, DonationService, GiveTransferService) {
    return {
      restrict: 'EA',
      transclude: true,
      templateUrl: 'templates/commitment_list.html',
      scope: {
        commitmentListInput: '=',
      },
      link: link
    };

    function link(scope) {
      scope.$watch('commitmentListInput', function(pledgeCommitments) {
        scope.pledgeCommitments = pledgeCommitments;
      });

    function commitmentMet(donations, commitment) {
      return (donations >= commitment);
    }
    }
  }
})();
