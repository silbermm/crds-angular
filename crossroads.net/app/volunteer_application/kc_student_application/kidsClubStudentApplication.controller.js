"use strict";

(function() {

  module.exports = KidsClubStudentApplicationController;

  KidsClubStudentApplicationController.$inject = ['$scope', '$log', 'VolunteerService', 'studentFields'];

  function KidsClubStudentApplicationController($scope, $log, VolunteerService, studentFields) {
    $log.debug("Inside Kids-Club-Student-Application-Controller");
    var vm = this;

    vm.save = save;
    //vm.school = "my school";

    activate();

    ///////////////////////////////////////////

    function activate() {

    }

    function save(form) {
      $log.debug('you tried to save');
      $log.debug('school: ' + vm.school);
      $log.debug('something from parent: ' + $scope.volunteer.contactId);

      var student = new VolunteerService.SaveStudent();
      student.contactId = $scope.volunteer.contactId;
      student.formId = 16; // get this from CMS in pageInfo
      student.opportunityId = $scope.volunteer.pageInfo.opportunity;
      student.responseOpportunityId = $scope.volunteer.responseId;

      student.firstName = {
        Value: $scope.volunteer.person.firstName,
        CrossroadsId: studentFields.firstName
      };

      student.lastName = {
        Value: $scope.volunteer.person.lastName,
        CrossroadsId: studentFields.lastName
      };

      student.middleInitial = {
        Value: $scope.volunteer.person.middleName.substring(0,1),
        CrossroadsId: studentFields.middleInitial
      };

      student.email = {
        Value: $scope.volunteer.person.emailAddress,
        CrossroadsId: studentFields.email
      };

      student.birthDate = {
        Value: $scope.volunteer.person.dateOfBirth,
        CrossroadsId: studentFields.birthDate
      };

      student.gender = {
        Value: $scope.volunteer.person.genderId,
        CrossroadsId: studentFields.gender
      };

      student.nameForNameTag = {
        Value: vm.nameTag,
        CrossroadsId: studentFields.nameForNameTag
      };

      student.site = {
        Value: vm.site,
        CrossroadsId: studentFields.site
      };

      student.school = {
        Value: vm.school,
        CrossroadsId: studentFields.school
      };

      student.$save(function(saved) {
        //need to inject rootScope
        //$rootScope.$emit("notify", $rootScope.MESSAGES.serveSignupSuccess);
        return true;
      }, function(err) {
        //$rootScope.$emit("notify", $rootScope.MESSAGES.generalError);
        return false;
      });
    }


  }
})();
