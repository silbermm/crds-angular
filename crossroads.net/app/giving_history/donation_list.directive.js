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
        donationsInput: '=',
        donationTotalAmount: '=',
        donationStatementTotalAmount: '=',
        donationShowLimit: '=',
        donationDoNotShowLabels: '='
      },
      link: link
    };

    function link(scope) {
      scope.$watch('donationsInput', function(donations) {
        scope.donations = postProcessDonations(donations);
      });
    }

    function getCardIcon(brand) {
      switch (brand) {
        case 'Visa':
          return ('cc_visa');
        case 'MasterCard':
          return ('cc_mastercard');
        case 'Discover':
          return ('cc_discover');
        case 'AmericanExpress':
          return ('cc_american_express');
        default:
          return ('');
      }
    }

    function postProcessDonations(donationsInput) {
      var donations = _.transform(donationsInput, function(result, d) {
        var donation = _.cloneDeep(d);
        setDonationDisplayDetails(donation.source);
        result.push(donation);
      });

      return (donations);
    }

    function setDonationDisplayDetails(source) {
      switch (source.type) {
        case 'SoftCredit':
          break;
        case 'Cash':
          source.icon = 'money';
          source.viewBox = '0 0 34 32';
          break;
        case 'Bank':
        case 'Check':
          source.icon = 'library';
          source.viewBox = '0 0 32 32';
          source.name = 'ending in ' + source.last4;
          break;
        case 'CreditCard':
          source.icon = getCardIcon(source.brand);
          source.viewBox = '0 0 160 100';
          source.name = 'ending in ' + source.last4;
          break;
      }
    }
  }
})();
