"use strict";

(function() {

  module.exports = KidsClubStudentApplicationController;

  KidsClubStudentApplicationController.$inject = ['$scope', '$log', 'VolunteerService', 'studentFields'];

  function KidsClubStudentApplicationController($scope, $log, VolunteerService, studentFields) {
    $log.debug("Inside Kids-Club-Student-Application-Controller");
    var vm = this;

    vm.reference1 = {};
    vm.reference2 = {};
    vm.serveAgeKids = {};

    vm.save = save;
    vm.checkIt = checkIt;
    //vm.school = "my school";

    activate();

    ///////////////////////////////////////////

    function activate() {

    }

    function checkIt() {
      $log.debug('constant: ' + studentFields.firstName);
    }

    function save() {
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
        Value: $scope.volunteer.person.middleName.substring(0, 1),
        CrossroadsId: studentFields.middleInitial
      };

      student.email = {
        Value: $scope.volunteer.person.emailAddress,
        CrossroadsId: studentFields.email
      };

      student.nameForNameTag = {
        Value: vm.nameTag,
        CrossroadsId: studentFields.nameForNameTag
      };

      student.birthDate = {
        Value: $scope.volunteer.person.dateOfBirth,
        CrossroadsId: studentFields.birthDate
      };

      student.gender = {
        Value: $scope.volunteer.person.genderId,
        CrossroadsId: studentFields.gender
      };

      student.site = {
        Value: vm.site,
        CrossroadsId: studentFields.site
      };

      student.howLongAttending = {
        Value: vm.howLongAttending,
        CrossroadsId: studentFields.howLongAttending
      };

      student.serviceAttend = {
        Value: vm.serviceAttend,
        CrossroadsId: studentFields.serviceAttend
      };

      student.streetAddress = {
        Value: vm.streetAddress,
        CrossroadsId: studentFields.streetAddress
      };

      student.city = {
        Value: vm.city,
        CrossroadsId: studentFields.city
      };

      student.state = {
        Value: vm.state,
        CrossroadsId: studentFields.state
      };

      student.zip = {
        Value: vm.zip,
        CrossroadsId: studentFields.zip
      };

      student.mobilePhone = {
        Value: vm.mobilePhone,
        CrossroadsId: studentFields.mobilePhone
      };

      student.homePhone = {
        Value: vm.homePhone,
        CrossroadsId: studentFields.homePhone
      };

      student.school = {
        Value: vm.school,
        CrossroadsId: studentFields.school
      };

      student.grade = {
        Value: vm.grade,
        CrossroadsId: studentFields.grade
      };

      student.whereYouAre = {
        Value: vm.whereYouAre,
        CrossroadsId: studentFields.whereYouAre
      };

      student.explainFaith = {
        Value: vm.explainFaith,
        CrossroadsId: studentFields.explainFaith
      };

      student.whyServe = {
        Value: vm.whyServe,
        CrossroadsId: studentFields.whyServe
      };

      student.specialTalents = {
        Value: vm.specialTalents,
        CrossroadsId: studentFields.specialTalents
      };

      student.availabilityDuringWeek = {
        Value: vm.availabilityDuringWeek,
        CrossroadsId: studentFields.availabilityDuringWeek
      };

      student.availabilityDuringWeekend = {
        Value: vm.availabilityDuringWeekend,
        CrossroadsId: studentFields.availabilityDuringWeekend
      };

      student.serveSite = {
        Value: vm.serveSite,
        CrossroadsId: studentFields.serveSite
      };

      student.serveServiceTimes = {
        Value: vm.serveServiceTimes,
        CrossroadsId: studentFields.serveServiceTimes
      };

      student.serveAgeKids1to2 = {
        Value: vm.serveAgeKids.age1to2,
        CrossroadsId: studentFields.serveAgeKids1to2
      };

      student.serveAgeKids3toPreK = {
        Value: vm.serveAgeKids.age3toPreK,
        CrossroadsId: studentFields.serveAgeKids3toPreK
      };

      student.serveAgeKidsKto5Grade = {
        Value: vm.serveAgeKids.Kto5Grade,
        CrossroadsId: studentFields.serveAgeKidsKto5Grade
      };

      // reference 1
      student.reference1Name = {
        Value: vm.reference1.name,
        CrossroadsId: studentFields.reference1Name
      };

      student.reference1timeKnown = {
        Value: vm.reference1.timeKnown,
        CrossroadsId: studentFields.reference1timeKnown
      };

      student.reference1homePhone = {
        Value: vm.reference1.homePhone,
        CrossroadsId: studentFields.reference1homePhone
      };

      student.reference1mobilePhone = {
        Value: vm.reference1.mobilePhone,
        CrossroadsId: studentFields.reference1mobilePhone
      };

      student.reference1workPhone = {
        Value: vm.reference1.workPhone,
        CrossroadsId: studentFields.reference1workPhone
      };

      student.reference1email = {
        Value: vm.reference1.email,
        CrossroadsId: studentFields.reference1email
      };

      student.reference1association = {
        Value: vm.reference1.association,
        CrossroadsId: studentFields.reference1association
      };

      student.reference1occupation = {
        Value: vm.reference1.occupation,
        CrossroadsId: studentFields.reference1occupation
      };

      // reference 2
      student.reference2Name = {
        Value: vm.reference2.name,
        CrossroadsId: studentFields.reference2Name
      };

      student.reference2timeKnown = {
        Value: vm.reference2.timeKnown,
        CrossroadsId: studentFields.reference2timeKnown
      };

      student.reference2homePhone = {
        Value: vm.reference2.homePhone,
        CrossroadsId: studentFields.reference2homePhone
      };

      student.reference2mobilePhone = {
        Value: vm.reference2.mobilePhone,
        CrossroadsId: studentFields.reference2mobilePhone
      };

      student.reference2workPhone = {
        Value: vm.reference2.workPhone,
        CrossroadsId: studentFields.reference2workPhone
      };

      student.reference2email = {
        Value: vm.reference2.email,
        CrossroadsId: studentFields.reference2email
      };

      student.reference2association = {
        Value: vm.reference2.association,
        CrossroadsId: studentFields.reference2association
      };

      student.reference2occupation = {
        Value: vm.reference2.occupation,
        CrossroadsId: studentFields.reference2occupation
      };

      student.parentLastName = {
        Value: vm.parent.lastName,
        CrossroadsId: studentFields.parentLastName
      };

      student.parentFirstName = {
        Value: vm.parent.firstName,
        CrossroadsId: studentFields.parentFirstName
      };

      student.parentHomePhone = {
        Value: vm.parent.homePhone,
        CrossroadsId: studentFields.parentHomePhone
      };

      student.parentMobilePhone = {
        Value: vm.parent.mobilePhone,
        CrossroadsId: studentFields.parentMobilePhone
      };

      student.parentEmail = {
        Value: vm.parent.email,
        CrossroadsId: studentFields.parentEmail
      };

      student.parentSignature = {
        Value: vm.parentSignature,
        CrossroadsId: studentFields.parentSignature
      };

      student.parentSignatureDate = {
        Value: vm.parentSignatureDate,
        CrossroadsId: studentFields.parentSignatureDate
      };

      student.studentSignature = {
        Value: vm.studentSignature,
        CrossroadsId: studentFields.studentSignature
      };

      student.studentSignatureDate = {
        Value: vm.studentSignatureDate,
        CrossroadsId: studentFields.studentSignatureDate
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
