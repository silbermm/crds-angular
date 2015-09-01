(function() {
  'use strict()';

  module.exports = TripPrivateInviteController;

  TripPrivateInviteController.$inject = ['$rootScope', '$scope', '$window', '$log', 'MPTools', 'Trip'];

  function TripPrivateInviteController($rootScope, $scope, $window, $log, MPTools, Trip) {

    $log.debug('TripPrivateInviteController');
    var vm = this;

    vm.cancel = cancel;
    vm.fieldError = fieldError;
    vm.hasError = hasError;

    // vm.pageInfo = PageInfo;
    vm.params = MPTools.getParams();
    vm.processing = false;
    vm.save = save;
    vm.viewReady = false;

    vm.emailPrefix = 'privateInvite';
    vm.formSubmitted = false;
    // vm.invites = [];

    activate();

    //////////////////////

    function activate() {
      // $log.debug('pageInfo: ' + vm.pageInfo);
      //vm.errorMessages = errorMessages();
      // var invite = {emailAddress:'', recipientName:''};
      // vm.invites.push({rowId: 1, emailAddress:'', recipientName:''});
      // vm.invites.push({rowId: 2, emailAddress:'', recipientName:''});
      // vm.invites.push({rowId: 3, emailAddress:'', recipientName:''});
      // vm.invites.push({rowId: 4, emailAddress:'', recipientName:''});
      // vm.invites.push({rowId: 5, emailAddress:'', recipientName:''});
      vm.pledgeCampaignId = vm.params.recordId;
      vm.viewReady = true;
    }

    function errorMessages() {
      var errors = [];
      _.each(vm.pageInfo.errors, function(d) {
        errors.push(d.message);
      });

      if (vm.pageInfo.data !== undefined) {
        _.each(vm.pageInfo.data.errors, function(d) {
          errors.push(d);
        });
      }

      return errors;
    }

    function hasError() {
      return false;

      // if (vm.errorMessages.length > 0) {
      //   return true;
      // }
      //
      // return false;
    }

    function fieldError(form, field) {
      if (form[field] === undefined) {
        return false;
      }

      if (form.$submitted || form[field].$dirty) {
        return form[field].$invalid;
      }

      return false;
    }

    function cancel() {
      $window.close();
    }

    function save(form) {
      vm.processing = true;

      //$log.debug('length:' + vm.invites.length);
      $log.debug('valid:' + form.privateInviteForm.$valid);

      if (form.privateInviteForm.$invalid) {
        $log.error('please fill out all required fields correctly');
        $rootScope.$emit('notify', $rootScope.MESSAGES.generalError);
        vm.saving = false;
        vm.submitButtonText = 'Submit';
        return false;
      }

      var dto = {};
      dto.pledgeCampaignId = vm.pledgeCampaignId;
      dto.emailAddress = vm.emailAddress;
      dto.recipientName = vm.recipientName;

      Trip.GeneratePrivateInvites.save(dto, function(privateInvite) {
        $window.close();
      },

      function(err) {
        $rootScope.$emit('notify', $rootScope.MESSAGES.generalError);
        vm.processing = false;
      });

      //copy-paste
      // vm.saving = true;
      // vm.submitButtonText = 'Submitting...';
      //
      // $log.debug('you tried to save');
      // $log.debug('nameTag: ' + vm.volunteer.nameTag);
      // $log.debug('something from parent: ' + vm.volunteer.contactId);
      //
      // $log.debug('saving');
      // if(form.adult.$invalid){
      //   $log.error('please fill out all required fields correctly');
      //   $rootScope.$emit('notify',$rootScope.MESSAGES.generalError);
      //   vm.saving = false;
      //   vm.submitButtonText = 'Submit';
      //   return false;
      // }
      //copy-paste

      // var dto = {};
      //
      // dto.applicants = vm.pageInfo.applicants;
      // dto.group = vm.group.groupId;
      // dto.pledgeCampaign = vm.pageInfo.campaign;
      //
      // Trip.SaveParticipants.save(dto, function(updatedEvents) {
      //   $window.close();
      // }, function(err) {
      //
      //   $rootScope.$emit('notify', $rootScope.MESSAGES.generalError);
      //   vm.processing = false;
      // });

    }


  }

})();
