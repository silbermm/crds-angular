(function() {
  'use strict';

  var moment = require('moment');

  module.exports = ProfilePersonalController;

  ProfilePersonalController.$inject = [
    '$rootScope',
    '$log',
    '$timeout',
    '$location',
    '$anchorScroll',
    'MESSAGES',
    'ProfileReferenceData',
    'Profile',
    'Validation',
    '$sce'
  ];

  function ProfilePersonalController(
      $rootScope,
      $log,
      $timeout,
      $location,
      $anchorScroll,
      MESSAGES,
      ProfileReferenceData,
      Profile,
      Validation,
      $sce) {

    var vm = this;
    var attributeTypeIds = require('crds-constants').ATTRIBUTE_TYPE_IDS;
    var now = new Date();

    vm.allowPasswordChange = angular.isDefined(vm.allowPasswordChange) ?  vm.allowPasswordChange : 'true';
    vm.allowSave = angular.isDefined(vm.allowSave) ? vm.allowSave : 'true';
    vm.closeModal = closeModal;
    vm.convertHomePhone = convertHomePhone;
    vm.convertPhone = convertPhone;
    vm.crossroadsStartDate = new Date(1994, 0, 1);
    vm.dateFormat = /^(0[1-9]|1[012])[- /.](0[1-9]|[12][0-9]|3[01])[- /.]((19|20)\d\d)$/;
    vm.formatAnniversaryDate = formatAnniversaryDate;
    vm.householdForm = {};
    vm.householdInfo = {};
    vm.householdPhoneFocus = householdPhoneFocus;
    vm.isCrossroadsAttendee = isCrossroadsAttendee;
    vm.isHouseholdCollapsed = true;
    vm.hstep = 1;
    vm.isDobError = isDobError;
    vm.isMeridian = true;
    vm.loading = true;
    vm.minBirthdate = new Date(now.getFullYear(), now.getMonth(), now.getDate());
    vm.mstep = 15;
    vm.oneHundredFiftyYearsAgo = new Date(now.getFullYear() - 150, now.getMonth(), now.getDate());
    vm.openBirthdatePicker = openBirthdatePicker;
    vm.openStartAttendingDatePicker = openStartAttendingDatePicker;
    vm.passwordPrefix = 'account-page';
    vm.phoneFormat = /^\(?(\d{3})\)?[\s.-]?(\d{3})[\s.-]?(\d{4})$/;
    vm.requireEmail = true;
    vm.requireMobilePhone = angular.isDefined(vm.requireMobilePhone) ? vm.requireMobilePhone : 'false';
    vm.savePersonal = savePersonal;
    vm.showMobilePhoneError = showMobilePhoneError;
    vm.submitted = false;
    vm.today = moment();
    vm.underThirteen = underThirteen;
    vm.validation = Validation;
    vm.viewReady = false;
    vm.zipFormat = /^(\d{5}([\-]\d{4})?)$/;

    activate();

    ////////////////////////////////
    //// IMPLEMENTATION DETAILS ////
    ////////////////////////////////

    function activate() {

      if (vm.enforceAgeRestriction) {
        vm.minBirthdate.setFullYear(vm.minBirthdate.getFullYear() - vm.enforceAgeRestriction);
      }

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
            setDate();
            underThirteen();
            vm.viewReady = true;
          });
        } else {
          configurePerson();
          setDate();
          underThirteen();
          vm.viewReady = true;
        }

      });

      vm.buttonText = vm.buttonText !== undefined ? vm.buttonText : 'Save';
    }

    function setDate() {
      if (vm.profileData.person.attendanceStartDate) {
        vm.profileData.person.attendanceStartDate = new Date(vm.profileData.person.attendanceStartDate);
      }
    }

    function configurePerson() {

      vm.profileData.person.dateOfBirth = convertDate(vm.profileData.person.dateOfBirth);

      vm.ethnicities = vm.profileData.person.attributeTypes[attributeTypeIds.ETHNICITY].attributes;
      vm.startAttendReason = vm.profileData.person.singleAttributes[attributeTypeIds.START_ATTEND_REASON];
      vm.startAttendReasons = _.find(vm.attributeTypes, function(attributeType) {
        return attributeType.attributeTypeId === attributeTypeIds.START_ATTEND_REASON;
      });
    }

    function convertDate(date) {
      if ((date !== undefined) && (date !== '')) {
        if (typeof date === 'string' ||
            date instanceof String) {
          var newDate = date.replace(vm.dateFormat, '$3 $1 $2');
          var mDate = moment(newDate, 'YYYY MM DD');
          return mDate.format('MM/DD/YYYY');
        } else {
          var formatedDate = moment(date);
          return formatedDate.format('MM/DD/YYYY');
        }
      }
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

    function convertPhone() {
      if (vm.pform['mobile-phone'].$valid) {
        vm.profileData.person.mobilePhone = vm.profileData.person.mobilePhone.replace(vm.phoneFormat, '$1-$2-$3');
      }
    }

    function formatAnniversaryDate(anniversaryDate) {
      var tmp = moment(anniversaryDate);
      var month = tmp.month() + 1;
      var year = tmp.year();
      return month + '/' + year;
    }

    function householdPhoneFocus() {
      vm.isHouseholdCollapsed = false;
      $location.hash('homephonecont');
      $timeout(function() {
        $anchorScroll();
      }, 500);
    }

    function isDobError() {
      return (vm.pform.birthdate.$touched ||
        vm.pform.$submitted) &&
        vm.pform.birthdate.$invalid;
    }

    function openBirthdatePicker($event) {
      $event.preventDefault();
      $event.stopPropagation();

      vm.birthdateOpen = true;
    }

    function openStartAttendingDatePicker($event) {
      $event.preventDefault();
      $event.stopPropagation();

      vm.startAttendingOpen = true;
    }

    function savePersonal() {
      //force genders field to be dirty
      vm.pform.$submitted = true;
      vm.householdForm.$submitted = true;

      $timeout(function() {
        vm.submitted = true;

        if (vm.householdForm.$invalid) {
          $rootScope.$emit('notify', $rootScope.MESSAGES.generalError);
          vm.isHouseholdCollapsed = false;
          vm.submitted = false;
          return;
        }

        if (vm.pform.$invalid) {
          $rootScope.$emit('notify', $rootScope.MESSAGES.generalError);
          vm.submitted = false;
          return;
        }

        vm.profileData.person['State/Region'] = vm.profileData.person.State;
        if (vm.submitFormCallback !== undefined) {
          vm.submitFormCallback({profile: vm.profileData });
        } else {
          vm.profileData.person.$save(function() {
            vm.submitted = false;
            $rootScope.$emit('notify', $rootScope.MESSAGES.profileUpdated);
            $log.debug('person save successful');
            if (vm.profileParentForm) {
              vm.profileParentForm.$setPristine();
            }

            if (vm.modalInstance !== undefined) {
              vm.closeModal(true);
            }
          },

          function() {
            $rootScope.$emit('notify', $rootScope.MESSAGES.generalError);
            $log.debug('person save unsuccessful');
          });
        }
      }, 550);
    }

    function showMobilePhoneError() {
      var show = vm.validation.showErrors(vm.pform, 'mobile-phone') && vm.requireMobilePhone;
      return show;
    }

    function underThirteen() {
      if (vm.profileData.person.dateOfBirth !== '') {
        var birthdate = crds_utilities.convertStringToDate(vm.profileData.person.dateOfBirth);
        if (birthdate) {
          var thirteen = new Date();
          thirteen.setFullYear(thirteen.getFullYear() - 13);
          vm.requireEmail = birthdate.getTime() < thirteen.getTime();
        } else {
          vm.requireEmail = true;
        }
      } else {
        vm.requireEmail = true;
      }
    }

    function isCrossroadsAttendee() {
      var nonCrossroadsLocations = require('crds-constants').NON_CROSSROADS_LOCATIONS;
      return vm.profileData.person.congregationId
        && vm.profileData.person.congregationId != nonCrossroadsLocations.I_DO_NOT_ATTEND_CROSSROADS
        && vm.profileData.person.congregationId != nonCrossroadsLocations.NOT_SITE_SPECIFIC;
    }

  }

})();
