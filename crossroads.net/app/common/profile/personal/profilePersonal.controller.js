(function() {
  'use strict';

  var moment = require('moment');

  module.exports = ProfilePersonalController;

  ProfilePersonalController.$inject = [
    '$rootScope',
    '$log',
    '$timeout',
    'MESSAGES',
    'ProfileReferenceData',
    'Profile',
    'Validation'
  ];

  function ProfilePersonalController(
      $rootScope,
      $log,
      $timeout,
      MESSAGES,
      ProfileReferenceData,
      Profile,
      Validation) {

    var vm = this;
    var attributeTypeIds = require('crds-constants').ATTRIBUTE_TYPE_IDS;

    vm.allowPasswordChange = angular.isDefined(vm.allowPasswordChange) ?  vm.allowPasswordChange : 'true';
    vm.allowSave = angular.isDefined(vm.allowSave) ? vm.allowSave : 'true';
    vm.closeModal = closeModal;
    vm.convertHomePhone = convertHomePhone;
    vm.convertPhone = convertPhone;
    vm.dateFormat = /^(0[1-9]|1[012])[- /.](0[1-9]|[12][0-9]|3[01])[- /.]((19|20)\d\d)$/;
    vm.formatAnniversaryDate = formatAnniversaryDate;
    vm.householdForm = {};
    vm.householdInfo = {};
    vm.householdPhoneFocus = householdPhoneFocus;
    vm.isDobError = isDobError;
    vm.loading = true;
    vm.passwordPrefix = 'account-page';
    vm.phoneFormat = /^\(?(\d{3})\)?[\s.-]?(\d{3})[\s.-]?(\d{4})$/;
    vm.requireMobilePhone = angular.isDefined(vm.requireMobilePhone) ? vm.requireMobilePhone : 'false';
    vm.savePersonal = savePersonal;
    vm.showMobilePhoneError = showMobilePhoneError;
    vm.submitted = false;
    vm.validation = Validation;
    vm.viewReady = false;
    vm.zipFormat = /^(\d{5}([\-]\d{4})?)$/;

    activate();

    //Datepicker STUFF
    vm.hstep = 1;
    vm.mstep = 15;
    var now = new Date();
    vm.today = new Date(now.getFullYear(), now.getMonth(), now.getDate());
    vm.thirteenYearsAgo = new Date(now.getFullYear() - 13, now.getMonth(), now.getDate());
    vm.oneHundredFiftyYearsAgo = new Date(now.getFullYear() - 150, now.getMonth(), now.getDate());
    vm.crossroadsStartDate = new Date(1994, 0, 1);
    vm.isMeridian = true;
    vm.openBirthdatePicker = openBirthdatePicker;

    function openBirthdatePicker($event) {
      $event.preventDefault();
      $event.stopPropagation();

      vm.birthdateOpen = true;
    }

    vm.openStartAttendingDatePicker = openStartAttendingDatePicker;

    function openStartAttendingDatePicker($event) {
      $event.preventDefault();
      $event.stopPropagation();

      vm.startAttendingOpen = true;
    }

    //END Datepicker STUFF

    ////////////////////////////////
    //// IMPLEMENTATION DETAILS ////
    ////////////////////////////////

    function activate() {
      ProfileReferenceData.getInstance().then(function(response) {
        vm.genders = response.genders;
        vm.maritalStatuses = response.maritalStatuses;
        vm.serviceProviders = response.serviceProviders;
        vm.states = response.states;
        vm.countries = response.countries;
        vm.crossroadsLocations = response.crossroadsLocations;
        if (!vm.profileData) {
          Profile.Personal.get(function(data) {
            vm.profileData = { person: data };
            vm.viewReady = true;
          });
        } else {
          configurePerson();
          vm.viewReady = true;
        }
      });

      vm.buttonText = vm.buttonText !== undefined ? vm.buttonText : 'Save';
    }

    function configurePerson() {

      if ((vm.profileData.person.dateOfBirth !== undefined) && (vm.profileData.person.dateOfBirth !== '')) {
        var newBirthDate = vm.profileData.person.dateOfBirth.replace(vm.dateFormat, '$3 $1 $2');
        var mBdate = moment(newBirthDate, 'YYYY MM DD');
        vm.profileData.person.dateOfBirth = mBdate.format('MM/DD/YYYY');
      }

      if ((vm.profileData.person.anniversaryDate !== undefined) && (vm.profileData.person.anniversaryDate !== '')) {
        var mAdate = moment(new Date(vm.profileData.person.anniversaryDate));
      }

      vm.ethnicities = vm.profileData.person.attributeTypes[attributeTypeIds.ETHNICITY].attributes;
      vm.startAttendReason = vm.profileData.person.singleAttributes[attributeTypeIds.START_ATTEND_REASON];
      vm.startAttendReasons = _.find(vm.attributeTypes, function(attributeType) {
        return attributeType.attributeTypeId === attributeTypeIds.START_ATTEND_REASON;
      });
    }

    function convertHomePhone() {
      if (vm.pform['home-phone'].$valid) {
        vm.profileData.person.homePhone = vm.profileData.person.homePhone.replace(vm.phoneFormat, '$1-$2-$3');
      }
    }

    function closeModal(success) {
      if (success) {
        vm.updatedPerson.emailAddress = vm.profileData.person.emailAddress;
        vm.updatedPerson.firstName = vm.profileData.person.firstName;
        vm.updatedPerson.nickName =
            vm.profileData.person.nickName === '' ?
            vm.profileData.person.firstName :
            vm.profileData.person.nickName;
        vm.updatedPerson.lastName = vm.profileData.person.lastName;
      }

      vm.modalInstance.close(vm.updatedPerson);
    }

    function formatAnniversaryDate(anniversaryDate) {
      var tmp = moment(anniversaryDate);
      var month = tmp.month() + 1;
      var year = tmp.year();
      return month + '/' + year;
    }

    function householdPhoneFocus() {
      $rootScope.$emit('homePhoneFocus');
    }

    function isDobError() {
      return (vm.pform.birthdate.$touched ||
        vm.pform.$submitted) &&
        vm.pform.birthdate.$invalid;
    }

    function savePersonal() {
      //force genders field to be dirty
      vm.pform.$submitted = true;
      vm.householdForm.$submitted = true;
      $timeout(function() {
        vm.submitted = true;
        if (vm.pform.$invalid) {
          $rootScope.$emit('notify', $rootScope.MESSAGES.generalError);
          vm.submitted = false;
          return;
        }

        if (vm.householdForm.$invalid) {
          $rootScope.$emit('notify', $rootScope.MESSAGES.generalError);
          return;
        }

        vm.profileData.person['State/Region'] = vm.profileData.person.State;
        if (vm.submitFormCallback !== undefined) {
          vm.submitFormCallback({profile: vm.profileData });
        } else {
          vm.profileData.person.$save(function() {
            $rootScope.$emit('notify', $rootScope.MESSAGES.profileUpdated);
            $log.debug('person save successful');
            if (vm.modalInstance !== undefined) {
              vm.closeModal(true);
            }
          }, function() {
            //TODO: Should we be emiting error message here?
            $log.debug('person save unsuccessful');
          });
        }
      }, 550);
    }

    function convertPhone() {
      if (vm.pform['mobile-phone'].$valid) {
        vm.profileData.person.mobilePhone = vm.profileData.person.mobilePhone.replace(vm.phoneFormat, '$1-$2-$3');
      }
    }

    function showMobilePhoneError() {
      var show = vm.validation.showErrors(vm.pform, 'mobile-phone') && vm.requireMobilePhone;
      return show;
    }

  }

})();
