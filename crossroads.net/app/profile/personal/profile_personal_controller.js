'use strict';
(function() {

  module.exports = function($rootScope, $log, $timeout, MESSAGES, ProfileReferenceData, Validation) {
    var vm = this;

    vm.savePersonal = function() {
      $timeout(function() {
        vm.submitted = true;
        $log.debug(vm.form.personal);
        if (vm.form.personal.$invalid) {
          $rootScope.$emit('notify', $rootScope.MESSAGES.generalError);
          vm.submitted = false;
          return;
        }

        vm.person['State/Region'] = vm.person.State;
        if (vm.submitFormCallback !== undefined) {
          
        } else {
          vm.person.$save(function() {
            $rootScope.$emit('notify', $rootScope.MESSAGES.profileUpdated);
            $log.debug('person save successful');
            if (vm.modalInstance !== undefined) {
              vm.closeModal(true);
            }
          }, function() {
            $log.debug('person save unsuccessful');
          });
        }
      }, 550);
    };

    vm.isDobError = function() {
      return (vm.form.personal.birthdate.$touched ||
        vm.form.personal.$submitted) &&
        vm.form.personal.birthdate.$invalid;
    };

    vm.convertHomePhone = function() {
      if (vm.form.personal['home-phone'].$valid) {
        vm.person.homePhone = vm.person.homePhone.replace(vm.phoneFormat, '$1-$2-$3');
      }
    };

    vm.convertPhone = function() {
      if (vm.form.personal['mobile-phone'].$valid) {
        vm.person.mobilePhone = vm.person.mobilePhone.replace(vm.phoneFormat, '$1-$2-$3');
      }
    };

    vm.serviceProviderRequired = function() {
      // if (vm.person.mobilePhone === 'undefined' ||
      // vm.person.mobilePhone === null ||
      // vm.person.mobilePhone === '' ||
      // this.form.personal['mobile-phone'].$invalid) {
      //   return false;
      // }

      return true;
    };

  };

})();
