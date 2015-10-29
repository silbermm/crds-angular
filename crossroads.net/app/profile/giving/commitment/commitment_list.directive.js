(function() {
  'use strict()';

  module.exports = CommitmentList;

  CommitmentList.$inject = ['$rootScope', '$log', 'DonationService', 'GiveTransferService'];

  function CommitmentList($rootScope, $log, DonationService, GiveTransferService) {
    return {
      restrict: 'EA',
      transclude: true,
      templateUrl: 'commitment_list.html',
      scope: {
        commitmentInput: '=',
      },
      link: link
    };

    function link(scope) {

    }
  }
})();
