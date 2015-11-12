(function() {
  'use strict';

  module.exports = ProfileCommunicationsController;

  ProfileCommunicationsController.$inject = [
    '$rootScope',
    'Profile',
    'Subscriptions',
    'Statement'];

  function ProfileCommunicationsController($rootScope,
                                           Profile,
                                           Subscriptions,
                                           Statement) {
    var vm = this;

    vm.savePaperless = savePaperless;
    vm.saveSubscription = saveSubscription;
    vm.statement = Statement;
    vm.subscriptions = Subscriptions;

    function savePaperless() {
      if (!vm.statement) {
        return;
      }

      Profile.Statement.save(vm.statement,
        function(data) {
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
