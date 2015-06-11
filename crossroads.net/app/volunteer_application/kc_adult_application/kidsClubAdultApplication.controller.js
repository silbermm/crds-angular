"use strict";

(function() {

  angular.module("crossroads").controller("KidsClubAdultApplicationController", KidsClubAdultApplicationController);

  KidsClubAdultApplicationController.$inject = ['$scope', '$log', 'VolunteerService', 'adultFields'];

  function KidsClubAdultApplicationController($scope, $log, VolunteerService, adultFields) {
    $log.debug("Inside Kids-Club-Adult-Application-Controller");
    var vm = this;

    // vm.reference1 = {};
    // vm.reference2 = {};
    // vm.serveAgeKids = {};
    vm.save = save;

    function save() {
      $log.debug('you tried to save');
      $log.debug('school: ' + vm.school);
      $log.debug('something from parent: ' + $scope.volunteer.contactId);

      $log.debug("saving");
      //changed below from scope to $scope
      if($scope.adult.$invalid){
        $log.error("please fill out all required fields correctly");
        $rootScope.$emit('notify',$rootScope.MESSAGES.generalError);
        return false;
      }
      //$log.debug("Thank you for filling out the form");
      //return true;

      var adult = new VolunteerService.SaveAdult();
      adult.contactId = $scope.volunteer.contactId;
      adult.formId = 16; // get this from CMS in pageInfo
      adult.opportunityId = $scope.volunteer.pageInfo.opportunity;
      adult.responseOpportunityId = $scope.volunteer.responseId;

      adult.firstName = {
        Value: $scope.volunteer.person.firstName,
        CrossroadsId: adultFields.firstName
      };

      adult.lastName = {
        Value: $scope.volunteer.person.lastName,
        CrossroadsId: adultFields.lastName
      };

      adult.middleInitial = {
        Value: $scope.volunteer.person.middleName.substring(0, 1),
        CrossroadsId: adultFields.middleInitial
      };

      adult.previousName = {
        Value: vm.previousName,
        CrossroadsId: adultFields.previousName
      };

      adult.email = {
        Value: $scope.volunteer.person.emailAddress,
        CrossroadsId: adultFields.email
      };

      adult.nameForNameTag = {
        Value: vm.nameTag,
        CrossroadsId: adultFields.nameForNameTag
      };

      adult.birthDate = {
        Value: $scope.volunteer.person.dateOfBirth,
        CrossroadsId: adultFields.birthDate
      };

      adult.gender = {
        Value: $scope.volunteer.person.genderId,
        CrossroadsId: adultFields.gender
      };

      adult.maritalStatus = {
        Value: vm.maritalStatus,
        CrossroadsId: adultFields.maritalStatus
      };

      adult.spouseName = {
        Value: vm.spouseName,
        CrossroadsId: adultFields.spouseName
      };

      adult.spouseGender = {
        Value: vm.spouseGender,
        CrossroadsId: adultFields.spouseGender
      };

      adult.site = {
        Value: vm.site,
        CrossroadsId: adultFields.site
      };

      adult.howLongAttending = {
        Value: vm.howLongAttending,
        CrossroadsId: adultFields.howLongAttending
      };

      adult.serviceAttend = {
        Value: vm.serviceAttend,
        CrossroadsId: adultFields.serviceAttend
      };

      adult.streetAddress = {
        Value: $scope.volunteer.person.addressLine1,
        CrossroadsId: adultFields.streetAddress
      };

      adult.city = {
        Value: $scope.volunteer.person.city,
        CrossroadsId: adultFields.city
      };

      adult.state = {
        Value: $scope.volunteer.person.state,
        CrossroadsId: adultFields.state
      };

      adult.zip = {
        Value: $scope.volunteer.person.postalCode,
        CrossroadsId: adultFields.zip
      };

      adult.mobilePhone = {
        Value: $scope.volunteer.person.mobilePhone,
        CrossroadsId: adultFields.mobilePhone
      };

      adult.homePhone = {
        Value: vm.homePhone,
        CrossroadsId: adultFields.homePhone
      };

      adult.companyName = {
        Value: vm.companyName,
        CrossroadsId: adultFields.companyName
      };

      adult.position = {
        Value: vm.position,
        CrossroadsId: adultFields.position
      };

      adult.workPhone = {
        Value: vm.workPhone,
        CrossroadsId: adultFields.workPhone
      };

      adult.child1Name = {
        Value: vm.child1Name,
        CrossroadsId: adultFields.child1Name
      };

      adult.child1Birthdate = {
        Value: vm.child1Birthdate,
        CrossroadsId: adultFields.child1Birthdate
      };

      adult.child2Name = {
        Value: vm.child2Name,
        CrossroadsId: adultFields.child2Name
      };

      adult.child2Birthdate = {
        Value: vm.child2Birthdate,
        CrossroadsId: adultFields.child2Birthdate
      };

      adult.child3Name = {
        Value: vm.child3Name,
        CrossroadsId: adultFields.child3Name
      };

      adult.child3Birthdate = {
        Value: vm.child3Birthdate,
        CrossroadsId: adultFields.child3Birthdate
      };

      adult.child4Name = {
        Value: vm.child4Name,
        CrossroadsId: adultFields.child4Name
      };

      adult.child4Birthdate = {
        Value: vm.child4Birthdate,
        CrossroadsId: adultFields.child4Birthdate
      };

      adult.everBeenArrest = {
        Value: vm.everBeenArrest,
        CrossroadsId: adultFields.everBeenArrest
      };

      adult.addictionConcern = {
        Value: vm.addictionConcern,
        CrossroadsId: adultFields.addictionConcern
      };

      adult.neglectingChild = {
        Value: vm.neglectingChild,
        CrossroadsId: adultFields.neglectingChild
      };

      adult.psychiatricDisorder = {
        Value: vm.psychiatricDisorder,
        CrossroadsId: adultFields.psychiatricDisorder
      };

      adult.sexuallyActiveOutsideMarriage = {
        Value: vm.sexuallyActiveOutsideMarriage,
        CrossroadsId: adultFields.sexuallyActiveOutsideMarriage
      };

      adult.spiritualOrientation = {
        Value: vm.spiritualOrientation,
        CrossroadsId: adultFields.spiritualOrientation
      };

      adult.spiritualOrientationExplain = {
        Value: vm.spiritualOrientationExplain,
        CrossroadsId: adultFields.spiritualOrientationExplain
      };

      adult.whatPromptedApplication = {
        Value: vm.whatPromptedApplication,
        CrossroadsId: adultFields.whatPromptedApplication
      };

      adult.specialTalents = {
        Value: vm.specialTalents,
        CrossroadsId: adultFields.specialTalents
      };

      adult.availabilityWeek = {
        Value: vm.availabilityWeek,
        CrossroadsId: adultFields.availabilityWeek
      };

      adult.availabilityWeekend = {
        Value: vm.availabilityWeekend,
        CrossroadsId: adultFields.availabilityWeekend
      };

      adult.availabilityOakley = {
        Value: vm.availabilityOakley,
        CrossroadsId: adultFields.availabilityOakley
      };

      adult.availabilityFlorence = {
        Value: vm.availabilityFlorence,
        CrossroadsId: adultFields.availabilityFlorence
      };

      adult.availabilityWestSide = {
        Value: vm.availabilityWestSide,
        CrossroadsId: adultFields.availabilityWestSide
      };

      adult.availabilityMason = {
        Value: vm.availabilityMason,
        CrossroadsId: adultFields.availabilityMason
      };

      adult.availabilityClifton = {
        Value: vm.availabilityClifton,
        CrossroadsId: adultFields.availabilityClifton
      };

      adult.availabilityServiceTimes = {
        Value: vm.availabilityServiceTimes,
        CrossroadsId: adultFields.availabilityServiceTimes
      };

      adult.areaOfInterestServingInClassroom = {
        Value: vm.areaOfInterestServingInClassroom,
        CrossroadsId: adultFields.areaOfInterestServingInClassroom
      };

      adult.areaOfInterestWelcomingNewFamilies = {
        Value: vm.serveAgeKids.areaOfInterestWelcomingNewFamilies,
        CrossroadsId: adultFields.areaOfInterestWelcomingNewFamilies
      };

      adult.areaOfInterestHelpSpecialNeeds = {
        Value: vm.areaOfInterestHelpSpecialNeeds,
        CrossroadsId: adultFields.areaOfInterestHelpSpecialNeeds
      };

      adult.areaOfInterestTech = {
        Value: vm.areaOfInterestTech,
        CrossroadsId: adultFields.areaOfInterestTech
      };

      adult.areaOfInterestRoomPrep = {
        Value: vm.serveAgeKids.areaOfInterestRoomPrep,
        CrossroadsId: adultFields.areaOfInterestRoomPrep
      };

      adult.areaOfInterestAdminTasks = {
        Value: vm.areaOfInterestAdminTasks,
        CrossroadsId: adultFields.areaOfInterestAdminTasks
      };

      adult.areaOfInterestShoppingForSupplies = {
        Value: vm.areaOfInterestShoppingForSupplies,
        CrossroadsId: adultFields.areaOfInterestShoppingForSupplies
      };

      adult.areaOfInterestCreatingWeekendExperience = {
        Value: vm.serveAgeKids.areaOfInterestCreatingWeekendExperience,
        CrossroadsId: adultFields.areaOfInterestCreatingWeekendExperience
      };

      adult.whatAgeBirthToTwo = {
        Value: vm.whatAgeBirthToTwo,
        CrossroadsId: adultFields.whatAgeBirthToTwo
      };

      adult.whatAgeThreeToPreK = {
        Value: vm.whatAgeThreeToPreK,
        CrossroadsId: adultFields.whatAgeThreeToPreK
      };

      adult.whatAgeKToFifth = {
        Value: vm.whatAgeKToFifth,
        CrossroadsId: adultFields.whatAgeKToFifth
      };

      // reference 1
      adult.reference1Name = {
        Value: vm.reference1.name,
        CrossroadsId: adultFields.reference1Name
      };

      adult.reference1timeKnown = {
        Value: vm.reference1.timeKnown,
        CrossroadsId: adultFields.reference1timeKnown
      };

      adult.reference1homePhone = {
        Value: vm.reference1.homePhone,
        CrossroadsId: adultFields.reference1homePhone
      };

      adult.reference1mobilePhone = {
        Value: vm.reference1.mobilePhone,
        CrossroadsId: adultFields.reference1mobilePhone
      };

      adult.reference1workPhone = {
        Value: vm.reference1.workPhone,
        CrossroadsId: adultFields.reference1workPhone
      };

      adult.reference1email = {
        Value: vm.reference1.email,
        CrossroadsId: adultFields.reference1email
      };

      adult.reference1association = {
        Value: vm.reference1.association,
        CrossroadsId: adultFields.reference1association
      };

      adult.reference1occupation = {
        Value: vm.reference1.occupation,
        CrossroadsId: adultFields.reference1occupation
      };

      // reference 2
      adult.reference2Name = {
        Value: vm.reference2.name,
        CrossroadsId: adultFields.reference2Name
      };

      adult.reference2timeKnown = {
        Value: vm.reference2.timeKnown,
        CrossroadsId: adultFields.reference2timeKnown
      };

      adult.reference2homePhone = {
        Value: vm.reference2.homePhone,
        CrossroadsId: adultFields.reference2homePhone
      };

      adult.reference2mobilePhone = {
        Value: vm.reference2.mobilePhone,
        CrossroadsId: adultFields.reference2mobilePhone
      };

      adult.reference2workPhone = {
        Value: vm.reference2.workPhone,
        CrossroadsId: adultFields.reference2workPhone
      };

      adult.reference2email = {
        Value: vm.reference2.email,
        CrossroadsId: adultFields.reference2email
      };

      adult.reference2association = {
        Value: vm.reference2.association,
        CrossroadsId: adultFields.reference2association
      };

      adult.reference2occupation = {
        Value: vm.reference2.occupation,
        CrossroadsId: adultFields.reference2occupation
      };

      // reference 3
      adult.reference3Name = {
        Value: vm.reference3.name,
        CrossroadsId: adultFields.reference3Name
      };

      adult.reference3timeKnown = {
        Value: vm.reference3.timeKnown,
        CrossroadsId: adultFields.reference3timeKnown
      };

      adult.reference3homePhone = {
        Value: vm.reference3.homePhone,
        CrossroadsId: adultFields.reference3homePhone
      };

      adult.reference3mobilePhone = {
        Value: vm.reference3.mobilePhone,
        CrossroadsId: adultFields.reference3mobilePhone
      };

      adult.reference3workPhone = {
        Value: vm.reference3.workPhone,
        CrossroadsId: adultFields.reference3workPhone
      };

      adult.reference3email = {
        Value: vm.reference3.email,
        CrossroadsId: adultFields.reference3email
      };

      adult.reference3association = {
        Value: vm.reference3.association,
        CrossroadsId: adultFields.reference3association
      };

      adult.reference3occupation = {
        Value: vm.reference3.occupation,
        CrossroadsId: adultFields.reference3occupation
      };

      adult.agree = {
        Value: vm.agree,
        CrossroadsId: adultFields.agree
      };

      adult.agreeDate = {
        Value: vm.agreeDate,
        CrossroadsId: adultFields.agreeDate
      };

      adult.$save(function(saved) {
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
