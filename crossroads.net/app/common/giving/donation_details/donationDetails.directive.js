(function() {
  'use strict';

  module.exports = DonationDetails;

  DonationDetails.$inject = ['$log', 'GiveTransferService'];

  function DonationDetails($log, GiveTransferService) {
    return {
      restrict: 'EA',
      replace: true,
      scope: {
        amount: '=',
        amountSubmitted: '=',
        program: '=',
        programsIn: '=?',
        showInitiativeOption: '=?',
        showFrequencyOption: '=?',
        givingType: '=?',
        recurringStartDate: '=?',
      },
      templateUrl: 'donation_details/donationDetails.html',
      link: link
    };

    function link(scope, element, attrs) {

      scope.minDate = new Date();
      scope.amountError = amountError;
      scope.ministryShow = false;
      scope.setProgramList = setProgramList;
      scope.showInitiativeOption = scope.showInitiativeOption === undefined ? true : scope.showInitiativeOption;
      scope.showFrequencyOption = scope.showFrequencyOption === undefined ? true : scope.showFrequencyOption;
      scope.givingType = scope.givingType === undefined ? 'one_time' : scope.givingType;
      scope.dateOptions = {
        formatYear: 'yy',
        startingDay: 1,
        showWeeks: 'false',
      };
      scope.openRecurringStartDate = openRecurringStartDate;
      scope.recurringStartDatePickerOpened = false;
      scope.updateFrequency = updateFrequency;

      activate();

      /////////////////////////////////
      ////// IMPLMENTATION DETAILS ////
      /////////////////////////////////

      function activate() {
        if (!scope.program || !scope.program.ProgramId) {
          scope.program = scope.programsIn[0];
        }

        if (scope.programsIn !== undefined) {
          scope.ministryShow = scope.program.ProgramId !== scope.programsIn[0].ProgramId;
        }

        if (scope.showFrequencyOption) {
          scope.allowRecurring = scope.program.AllowRecurringGiving;
        }
      }

      function amountError() {
        return (scope.amountSubmitted && scope.donationDetailsForm.amount.$invalid &&
                scope.donationDetailsForm.$error.naturalNumber ||
                scope.donationDetailsForm.$dirty &&
                scope.donationDetailsForm.amount.$invalid &&
                scope.amount !== undefined ||
                scope.donationDetailsForm.$dirty &&
                scope.amount === '');
      }

      function setProgramList() {
        return scope.ministryShow ? scope.program = '' : scope.program = scope.programsIn[0];
      }

      function openRecurringStartDate($event) {
        $event.preventDefault();
        $event.stopPropagation();
        scope.recurringStartDatePickerOpened = true;
      }

      function updateFrequency() {
        if (scope.showFrequencyOption) {
          scope.allowRecurring = scope.program.AllowRecurringGiving;

          if (scope.givingType !== 'one_time' && !scope.allowRecurring) {
            scope.givingType = 'one_time';
          }
        }
      }
    }
  }
})();
