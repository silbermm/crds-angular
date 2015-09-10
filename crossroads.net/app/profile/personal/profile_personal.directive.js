'use strict';
(function() {
  module.exports = ProfilePersonalDirective;

  ProfilePersonalDirective.$inject = ['$log', '$rootScope', 'ProfileReferenceData', 'Validation'];

  function ProfilePersonalDirective($log, $rootScope, ProfileReferenceData, Validation) {
    
    return {
      restrict: 'E',
      bindToController: true,
      scope: {
        updatedPerson: '=?',
        modalInstance: '=?',
        submitFormCallback: '&?',
        allowPasswordChange: '=',
        requireMobilePhone: '=',
        allowSave: '='
      },
      templateUrl: 'personal/profile_personal.template.html',
      controller: 'ProfilePersonalController as profile',
      link: link
    };
  
    function link(scope, el, attr, vm) {
      
      vm.closeModal = closeModal;
      vm.validation = Validation;
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

      activate();
     
      function activate() {
        vm.form = vm.pform;
        if (vm.profileForm !== undefined) {
          vm.profileForm = vm.pform;
        }
        ProfileReferenceData.getInstance().then(function(response) {
          vm.genders = response.genders;
          vm.maritalStatuses = response.maritalStatuses;
          vm.serviceProviders = response.serviceProviders;
          vm.states = response.states;
          vm.countries = response.countries;
          vm.crossroadsLocations = response.crossroadsLocations;
          configurePerson(response.person);

          vm.viewReady = true;
        });
      }

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



      function closeModal(success) {
        if (success) {
          vm.updatedPerson.emailAddress = vm.person.emailAddress;
          vm.updatedPerson.firstName = vm.person.firstName;
          vm.updatedPerson.nickName =
              vm.person.nickName === '' ?
              vm.person.firstName :
              vm.person.nickName;
          vm.updatedPerson.lastName = vm.person.lastName;
        }

        vm.modalInstance.close(vm.updatedPerson);
      }


      function householdPhoneFocus() {
        $rootScope.$emit('homePhoneFocus');
      }




    }
  }
})();
