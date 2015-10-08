(function() {
  'use strict()';

  module.exports = RecurringGivingList;

  RecurringGivingList.$inject = ['$log'];

  function RecurringGivingList($log) {
    return {
      restrict: 'EA',
      transclude: true,
      templateUrl: 'templates/recurring_giving_list.html',
      scope: {
        recurringGiftsInput: '=',
      },
      link: link
    };

    function link(scope) {
      scope.$watch('recurringGiftsInput', function(recurringGifts) {
        scope.recurringGifts = postProcessRecurringGifts(recurringGifts);
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

    function postProcessRecurringGifts(recurringGiftsInput) {
      var recurringGifts = _.transform(recurringGiftsInput, function(result, rg) {
          var recurringGift = _.cloneDeep(rg);
          setRecurringGiftDisplayDetails(recurringGift.source);
          result.push(recurringGift);
        });

      return recurringGifts;
    }

    function setRecurringGiftDisplayDetails(source) {
      switch (source.type) {
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
