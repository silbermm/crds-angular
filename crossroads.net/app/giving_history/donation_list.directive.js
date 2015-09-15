(function() {
  'use strict()';

  module.exports = DonationList;

  DonationList.$inject = ['$log'];

  function DonationList($log) {
    return {
      restrict: 'EA',
      transclude: true,
      templateUrl: 'templates/donation_list.html',
      scope: {
        donations: '=',
        donationTotalAmount: '=',
        donationStatementTotalAmount: '='
      },
      link: link
    };

    function link(scope, el, attr) {
      /* Nothing - leaving for future expandability */
    }
  }
})();