(function() {
  'use strict';

  module.exports = ProfileCommunicationsController;

  ProfileCommunicationsController.$inject = [
    '$rootScope',
    'Person',
    'Profile',
    'PaymentService',
    'Subscriptions',
    'Donor'];

  function ProfileCommunicationsController($rootScope,
                                           Person,
                                           Profile,
                                           PaymentService,
                                           Subscriptions,
                                           Donor) {
    var vm = this;


    vm.donor = Donor;
    vm.paperless = (Donor.Statement_Method_ID === 2 ? true : false);
    vm.saveSubscription = saveSubscription;
    vm.subscriptions = Subscriptions;

    function savePaperless() {

      //TODO: Move to resolve
      vm.subscriptions = Profile.Subscriptions.query();
      PaymentService.getDonor()
        .then(function(donor) {
          vm.donor = donor;
          vm.paperless = (donor.Statement_Method_ID === 2 ? true : false);
        },

        function(error) {
          $rootScope.$emit('notify', $rootScope.MESSAGES.generalError);
        });

      Profile.Subscriptions.save(subscription.Subscription).$promise
        .then(function(data) {
          subscription.Subscription.dp_RecordID = data.dp_RecordID;
          $rootScope.$emit('notify', $rootScope.MESSAGES.profileUpdated);
        },

        function(error) {
          $rootScope.$emit('notify', $rootScope.MESSAGES.generalError);
        });
    }

    function saveSubscription(subscription) {
      if (subscription.Subscription) {
        subscription.Subscription.Unsubscribed = !subscription.Subscribed;
      } else {
        subscription.Subscription = {
          Publication_ID: subscription.ID,
          Publication_Title: subscription.Title,
          Unsubscribed: !subscription.Subscribed
        };

      }

      Profile.Subscriptions.save(subscription.Subscription).$promise
        .then(function(data) {
          subscription.Subscription.dp_RecordID = data.dp_RecordID;
          $rootScope.$emit('notify', $rootScope.MESSAGES.profileUpdated);
        },

        function(error) {
          $rootScope.$emit('notify', $rootScope.MESSAGES.generalError);
        });
    }
  }
})();
