"use strict";

(function() {

  angular.module("crossroads").controller("KidsClubAdultApplicationController", KidsClubAdultApplicationController);

  KidsClubAdultApplicationController.$inject = ['$rootScope', '$scope', '$log', 'VolunteerService', 'adultFields'];

  function KidsClubAdultApplicationController($rootScope, $scope, $log, VolunteerService, adultFields) {
    $log.debug("Inside Kids-Club-Adult-Application-Controller");
    var vm = this;

    // vm.reference1 = {};
    // vm.reference2 = {};
    vm.save = save;


    function save() {
      $log.debug('you tried to save');
      $log.debug('nameTag: ' + $scope.volunteer.nameTag);
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
      adult.contactId = $scope.contactId;
      adult.formId = 17; // get this from CMS in pageInfo
      adult.opportunityId = $scope.opportunityId;
      adult.responseOpportunityId = $scope.responseId;

      adult.firstName = {
        Value: $scope.volunteer.firstName,
        CrossroadsId: adultFields.firstName
      };

      adult.lastName = {
        Value: $scope.volunteer.lastName,
        CrossroadsId: adultFields.lastName
      };

      adult.middleInitial = {
        Value: $scope.volunteer.middleName.substring(0, 1),
        CrossroadsId: adultFields.middleInitial
      };

      adult.previousName = {
        Value: $scope.volunteer.previousName,
        CrossroadsId: adultFields.previousName
      };

      adult.email = {
        Value: $scope.volunteer.emailAddress,
        CrossroadsId: adultFields.email
      };

      adult.nameForNameTag = {
        Value: $scope.volunteer.nameTag,
        CrossroadsId: adultFields.nameForNameTag
      };

      adult.birthDate = {
        Value: $scope.volunteer.dateOfBirth,
        CrossroadsId: adultFields.birthDate
      };

      adult.gender = {
        Value: $scope.volunteer.genderId,
        CrossroadsId: adultFields.gender
      };

      adult.maritalStatus = {
        Value: $scope.volunteer.maritalStatusId,
        CrossroadsId: adultFields.maritalStatus
      };

      adult.spouseName = {
        Value: $scope.volunteer.spouseName,
        CrossroadsId: adultFields.spouseName
      };

      adult.spouseGender = {
        Value: $scope.volunteer.spouseGender,
        CrossroadsId: adultFields.spouseGender
      };

      adult.siteYouAttend = {
        Value: $scope.volunteer.siteAttend,
        CrossroadsId: adultFields.site
      };

      adult.howLongAttending = {
        Value: $scope.volunteer.attending,
        CrossroadsId: adultFields.howLongAttending
      };

      adult.serviceAttend = {
        Value: $scope.volunteer.serviceAttend,
        CrossroadsId: adultFields.serviceAttend
      };

      adult.streetAddress = {
        Value: $scope.volunteer.addressLine1,
        CrossroadsId: adultFields.streetAddress
      };

      adult.city = {
        Value: $scope.volunteer.city,
        CrossroadsId: adultFields.city
      };

      adult.state = {
        Value: $scope.volunteer.state,
        CrossroadsId: adultFields.state
      };

      adult.zip = {
        Value: $scope.volunteer.postalCode,
        CrossroadsId: adultFields.zip
      };

      adult.mobilePhone = {
        Value: $scope.volunteer.mobilePhone,
        CrossroadsId: adultFields.mobilePhone
      };

      adult.homePhone = {
        Value: $scope.volunteer.homePhone,
        CrossroadsId: adultFields.homePhone
      };

      adult.companyName = {
        Value: $scope.volunteer.company,
        CrossroadsId: adultFields.companyName
      };

      adult.position = {
        Value: $scope.volunteer.position,
        CrossroadsId: adultFields.position
      };

      adult.workPhone = {
        Value: $scope.volunteer.workPhone,
        CrossroadsId: adultFields.workPhone
      };

      adult.child1Name = {
        Value: $scope.volunteer.child1.name,
        CrossroadsId: adultFields.child1Name
      };

      adult.child1Birthdate = {
        Value: $scope.volunteer.child1.dob,
        CrossroadsId: adultFields.child1Birthdate
      };

      adult.child2Name = {
        Value: $scope.volunteer.child2.name,
        CrossroadsId: adultFields.child2Name
      };

      adult.child2Birthdate = {
        Value: $scope.volunteer.child2.dob,
        CrossroadsId: adultFields.child2Birthdate
      };

      adult.child3Name = {
        Value: $scope.volunteer.child3.name,
        CrossroadsId: adultFields.child3Name
      };

      adult.child3Birthdate = {
        Value: $scope.volunteer.child3.dob,
        CrossroadsId: adultFields.child3Birthdate
      };

      adult.child4Name = {
        Value: $scope.volunteer.child4.name,
        CrossroadsId: adultFields.child4Name
      };

      adult.child4Birthdate = {
        Value: $scope.volunteer.child4.dob,
        CrossroadsId: adultFields.child4Birthdate
      };

      adult.everBeenArrest = {
        Value: $scope.volunteer.crime,
        CrossroadsId: adultFields.everBeenArrest
      };

      adult.addictionConcern = {
        Value: $scope.volunteer.addiction,
        CrossroadsId: adultFields.addictionConcern
      };

      adult.neglectingChild = {
        Value: $scope.volunteer.neglect,
        CrossroadsId: adultFields.neglectingChild
      };

      adult.psychiatricDisorder = {
        Value: $scope.volunteer.psychiatricDisorder,
        CrossroadsId: adultFields.psychiatricDisorder
      };

      adult.sexuallyActiveOutsideMarriage = {
        Value: $scope.volunteer.sexualyActive,
        CrossroadsId: adultFields.sexuallyActiveOutsideMarriage
      };

      adult.spiritualOrientation = {
        //currently checkboxes make radio buttons
        Value: $scope.volunteer.spiritualOrientation,
        CrossroadsId: adultFields.spiritualOrientation
      };

      adult.spiritualOrientationExplain = {
        Value: $scope.volunteer.explainFaith,
        CrossroadsId: adultFields.spiritualOrientationExplain
      };

      adult.whatPromptedApplication = {
        Value: $scope.volunteer.whatPromptedApplication,
        CrossroadsId: adultFields.whatPromptedApplication
      };

      adult.specialTalents = {
        Value: $scope.volunteer.specialTalents,
        CrossroadsId: adultFields.specialTalents
      };

      adult.availabilityWeek = {
        Value: $scope.volunteer.availabilityWeek,
        CrossroadsId: adultFields.availabilityWeek
      };

      adult.availabilityWeekend = {
        Value: $scope.volunteer.availabilityWeekend,
        CrossroadsId: adultFields.availabilityWeekend
      };

      adult.availabilityOakley = {
        Value: $scope.volunteer.availabilityOakley,
        CrossroadsId: adultFields.availabilityOakley
      };

      adult.availabilityFlorence = {
        Value: $scope.volunteer.availabilityFlorence,
        CrossroadsId: adultFields.availabilityFlorence
      };

      adult.availabilityWestSide = {
        Value: $scope.volunteer.availabilityWestSide,
        CrossroadsId: adultFields.availabilityWestSide
      };

      adult.availabilityMason = {
        Value: $scope.volunteer.availabilityMason,
        CrossroadsId: adultFields.availabilityMason
      };

      adult.availabilityClifton = {
        Value: $scope.volunteer.availabilityClifton,
        CrossroadsId: adultFields.availabilityClifton
      };

      adult.availabilityServiceTimes = {
        Value: $scope.volunteer.serveServiceTimes,
        CrossroadsId: adultFields.availabilityServiceTimes
      };

      adult.areaOfInterestServingInClassroom = {
        Value: $scope.volunteer.areaOfInterestServingInClassroom,
        CrossroadsId: adultFields.areaOfInterestServingInClassroom
      };

      adult.areaOfInterestWelcomingNewFamilies = {
        Value: $scope.volunteer.areaOfInterestWelcomingNewFamilies,
        CrossroadsId: adultFields.areaOfInterestWelcomingNewFamilies
      };

      adult.areaOfInterestHelpSpecialNeeds = {
        Value: $scope.volunteer.areaOfInterestHelpSpecialNeeds,
        CrossroadsId: adultFields.areaOfInterestHelpSpecialNeeds
      };

      adult.areaOfInterestTech = {
        Value: $scope.volunteer.areaOfInterestTech,
        CrossroadsId: adultFields.areaOfInterestTech
      };

      adult.areaOfInterestRoomPrep = {
        Value: $scope.volunteer.areaOfInterestRoomPrep,
        CrossroadsId: adultFields.areaOfInterestRoomPrep
      };

      adult.areaOfInterestAdminTasks = {
        Value: $scope.volunteer.areaOfInterestAdminTasks,
        CrossroadsId: adultFields.areaOfInterestAdminTasks
      };

      adult.areaOfInterestShoppingForSupplies = {
        Value: $scope.volunteer.areaOfInterestShoppingForSupplies,
        CrossroadsId: adultFields.areaOfInterestShoppingForSupplies
      };

      adult.areaOfInterestCreatingWeekendExperience = {
        Value: $scope.volunteer.areaOfInterestCreatingWeekendExperience,
        CrossroadsId: adultFields.areaOfInterestCreatingWeekendExperience
      };

      adult.whatAgeBirthToTwo = {
        Value: $scope.volunteer.birthToTwo,
        CrossroadsId: adultFields.whatAgeBirthToTwo
      };

      adult.whatAgeThreeToPreK = {
        Value: $scope.volunteer.threeToPreK,
        CrossroadsId: adultFields.whatAgeThreeToPreK
      };

      adult.whatAgeKToFifth = {
        Value: $scope.volunteer.kToFifth,
        CrossroadsId: adultFields.whatAgeKToFifth
      };

      // reference 1
      adult.reference1Name = {
        Value: $scope.volunteer.referenceName1,
        CrossroadsId: adultFields.reference1Name
      };

      adult.reference1timeKnown = {
        Value: $scope.volunteer.referenceTimeKnown1,
        CrossroadsId: adultFields.reference1timeKnown
      };

      adult.reference1homePhone = {
        Value: $scope.volunteer.referenceHomePhone1,
        CrossroadsId: adultFields.reference1homePhone
      };

      adult.reference1mobilePhone = {
        Value: $scope.volunteer.referenceMobilePhone1,
        CrossroadsId: adultFields.reference1mobilePhone
      };

      adult.reference1workPhone = {
        Value: $scope.volunteer.referenceWorkPhone1,
        CrossroadsId: adultFields.reference1workPhone
      };

      adult.reference1email = {
        Value: $scope.volunteer.referenceEmail1,
        CrossroadsId: adultFields.reference1email
      };

      adult.reference1association = {
        Value: $scope.volunteer.referenceAssociation1,
        CrossroadsId: adultFields.reference1association
      };

      adult.reference1occupation = {
        Value: $scope.volunteer.referenceOccupation1,
        CrossroadsId: adultFields.reference1occupation
      };

      // reference 2
      adult.reference2Name = {
        Value: $scope.volunteer.referenceName2,
        CrossroadsId: adultFields.reference2Name
      };

      adult.reference2timeKnown = {
        Value: $scope.volunteer.referenceTimeKnown2,
        CrossroadsId: adultFields.reference2timeKnown
      };

      adult.reference2homePhone = {
        Value: $scope.volunteer.referenceHomePhone2,
        CrossroadsId: adultFields.reference2homePhone
      };

      adult.reference2mobilePhone = {
        Value: $scope.volunteer.referenceMobilePhone2,
        CrossroadsId: adultFields.reference2mobilePhone
      };

      adult.reference2workPhone = {
        Value: $scope.volunteer.referenceWorkPhone2,
        CrossroadsId: adultFields.reference2workPhone
      };

      adult.reference2email = {
        Value: $scope.volunteer.referenceEmail2,
        CrossroadsId: adultFields.reference2email
      };

      adult.reference2association = {
        Value: $scope.volunteer.referenceAssociation2,
        CrossroadsId: adultFields.reference2association
      };

      adult.reference2occupation = {
        Value: $scope.volunteer.referenceOccupation2,
        CrossroadsId: adultFields.reference2occupation
      };

      // reference 3
      adult.reference3Name = {
        Value: $scope.volunteer.referenceName3,
        CrossroadsId: adultFields.reference3Name
      };

      adult.reference3timeKnown = {
        Value: $scope.volunteer.referenceTimeKnown3,
        CrossroadsId: adultFields.reference3timeKnown
      };

      adult.reference3homePhone = {
        Value: $scope.volunteer.referenceHomePhone3,
        CrossroadsId: adultFields.reference3homePhone
      };

      adult.reference3mobilePhone = {
        Value: $scope.volunteer.referenceMobilePhone3,
        CrossroadsId: adultFields.reference3mobilePhone
      };

      adult.reference3workPhone = {
        Value: $scope.volunteer.referenceWorkPhone3,
        CrossroadsId: adultFields.reference3workPhone
      };

      adult.reference3email = {
        Value: $scope.volunteer.referenceEmail3,
        CrossroadsId: adultFields.reference3email
      };

      adult.reference3association = {
        Value: $scope.volunteer.referenceAssociation3,
        CrossroadsId: adultFields.reference3association
      };

      adult.reference3occupation = {
        Value: $scope.volunteer.referenceOccupation3,
        CrossroadsId: adultFields.reference3occupation
      };

      adult.agree = {
        Value: $scope.volunteer.signatureAgree,
        CrossroadsId: adultFields.agree
      };

      adult.agreeDate = {
        Value: $scope.volunteer.signatureDate,
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
