'use strict';
(function() {

  module.exports = function($rootScope, $log, $timeout, MESSAGES, ProfileReferenceData) {
    var vm = this;

    vm.ProfileReferenceData = ProfileReferenceData.getInstance();
    vm.person = {};
    vm.householdPhoneFocus = householdPhoneFocus;

    //default parm values
    vm.allowPasswordChange = angular.isDefined(vm.allowPasswordChange) ? vm.allowPasswordChange : 'true';
    vm.allowSave = angular.isDefined(vm.allowSave) ? vm.allowSave : 'true';
    vm.requireMobilePhone = angular.isDefined(vm.requireMobilePhone) ? vm.requireMobilePhone : 'false';

    vm.passwordPrefix = 'account-page';
    vm.submitted = false;

    vm.phoneFormat = /^\(?(\d{3})\)?[\s.-]?(\d{3})[\s.-]?(\d{4})$/;
    vm.zipFormat = /^(\d{5}([\-]\d{4})?)$/;
    vm.dateFormat = /^(0[1-9]|1[012])[- /.](0[1-9]|[12][0-9]|3[01])[- /.]((19|20)\d\d)$/;

    vm.loading = true;
    vm.viewReady = false;

    vm.initProfile = function(form) {
      vm.form = form;
      vm.ProfileReferenceData.then(function(response) {
        vm.genders = response.genders;
        vm.maritalStatuses = response.maritalStatuses;
        vm.serviceProviders = response.serviceProviders;
        vm.states = response.states;
        vm.countries = response.countries;
        vm.crossroadsLocations = response.crossroadsLocations;
        configurePerson(response.person);

        // var household =

        vm.viewReady = true;
      });
    };

    function configurePerson(person) {
      vm.person = person;

      if (vm.person.dateOfBirth !== undefined) {
        var newBirthDate = vm.person.dateOfBirth.replace(vm.dateFormat, '$3 $1 $2');
        var mBdate = moment(newBirthDate, 'YYYY MM DD');
        vm.person.dateOfBirth = mBdate.format('MM/DD/YYYY');
      }

      if (vm.person.anniversaryDate !== undefined) {
        var mAdate = moment(new Date(vm.person.anniversaryDate));
        vm.person.anniversaryDate = mAdate.format('MM/DD/YYYY');
      }
    }

    function householdPhoneFocus(){
      $rootScope.$emit('homePhoneFocus');
    }

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
        vm.person.$save(function() {
          $rootScope.$emit('notify', $rootScope.MESSAGES.profileUpdated);
          $log.debug('person save successful');
          if (vm.modalInstance !== undefined) {
            vm.closeModal(true);
          }
        }, function() {
          $log.debug('person save unsuccessful');
        });
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
