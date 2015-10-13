var attributeTypes = require('crds-constants').ATTRIBUTE_TYPE_IDS;
var attributes = require('crds-constants').ATTRIBUTE_IDS;
(function() {
  'use strict';

  module.exports = SignupStepController;

  SignupStepController.$inject = [
    '$stateParams',
    'TripsSignupService',
    'AttributeTypeService',
    '$scope',

    // For the dropdowns
    'DietaryRestrictions',
    'WorkTeams',
    'ScrubTopSizes',
    'ScrubBottomSizes',
    'TshirtSizes',
    'SpiritualJourney',
    'TripSkills',
    'FrequentFlyers',
  ];

  function SignupStepController(
    $stateParams,
    TripsSignupService,
    AttributeTypeService,
    $scope,
    DietaryRestrictions,
    WorkTeams,
    ScrubTopSizes,
    ScrubBottomSizes,
    TshirtSizes,
    SpiritualJourney,
    TripSkills,
    FrequentFlyers) {

    var vm = this;

    vm.dietaryRestrictions = DietaryRestrictions;
    vm.frequentFlyers = FrequentFlyers;
    vm.scrubBottomSizes = ScrubBottomSizes;
    vm.scrubTopSizes = ScrubTopSizes;
    vm.signupService = $scope.$parent.tripsSignup.signupService;
    vm.spiritualJourney = SpiritualJourney;
    vm.step = $stateParams.stepId;
    vm.tripSkills = TripSkills;
    vm.tshirtSizes = TshirtSizes;
    vm.workTeams = WorkTeams;

    vm.signupService.pageId = $stateParams.stepId;
    vm.signupService.pages[$stateParams.stepId] = {dirty: false};

    // populate dropdowns and select defaults
    switch (vm.signupService.pageId) {
      case '2':
        evaluateScrubSizeBottom();
        evaluateScrubSizeTop();
        evaluateSpiritualLife();
        evaluateTshirtSize();
        evaluateVegetarian();
        evaluateFoodAllergies();
        break;
      case '5':
        evaluatePreviousTripExp();
        evaluateTripSkills();
        break;
      case '6':
        evaluateFrequentFlyers();
      default:
        break;
    }

    function evaluateFoodAllergies() {
      if (vm.signupService.page2.allergies) {
        return;
      }

      var found = _.find(vm.signupService.person.attributes, function(attr) {
        return attr.attributeId === attributes.FOOD_ALLERGIES;
      });

      if (found) {
        vm.signupService.page2.allergies = found.notes;
      }

    }

    function evaluateFrequentFlyers() {
      if (vm.signupService.page6.frequentFlyers){
        return;
      }
      
      var attrs = AttributeTypeService.transformPersonMultiAttributes(attributeTypes.FREQUENT_FLYERS,
          vm.signupService.person.attributes,
          vm.frequentFlyers.attributes, function(attr) {
             return attr.isChecked = true; 
          });

      vm.signupService.page6.frequentFlyers = attrs;


      //if (!vm.signupService.page6.deltaFrequentFlyer) {
        //// try to get the persons delta frequent flyer
        //var found = _.find(vm.signupService.person.attributes, function(attr) {
          //return attr.attributeId === attributes.;
        //});

        //vm.signupService.page6.deltaFrequentFlyer = vm.signupService.person.attributes.
      //}

      //if (!vm.signupService.page6.southAfricanFrequentFlyer) {
        //// try to get the persons south african frequent flyer
      //}

      //if (!vm.signupService.page6.unitedFrequentFlyer) {
        //// try to get the persons south african frequent flyer
      //}

      //if (!vm.signupService.page6.usAirwaysFrequentFlyer) {
        //// try to get the persons south african frequent flyer
      /*}*/

    }

    function evaluatePreviousTripExp() {
      if (vm.signupService.page5.previousTripExperience) {
        return;
      }

      var found = _.find(vm.signupService.person.attributes, function(attr) {
        return attr.attributeId === attributes.PREVIOUS_TRIP_EXPERIENCE;
      });

      if (found) {
        vm.signupService.page5.previousTripExperience = found.notes;
      }
    }

    function evaluateScrubSizeBottom() {
      if (vm.signupService.page2.scrubSizeBottom) {
        return;
      }

      vm.signupService.page2.scrubSizeBottom = _.find(vm.signupService.person.attributes, function(attr) {
        return attr.attributeTypeId === attributeTypes.SCRUB_BOTTOM_SIZES;
      });
    }

    function evaluateScrubSizeTop() {
      if (vm.signupService.page2.scrubSizeTop) {
        return;
      }

      vm.signupService.page2.scrubSizeTop = _.find(vm.signupService.person.attributes, function(attr) {
        return attr.attributeTypeId === attributeTypes.SCRUB_TOP_SIZES;
      });
    }

    function evaluateSpiritualLife() {
      if (vm.signupService.page2.spiritualLife) { 
        return;
      }
      
      vm.signupService.page2.spiritualLife = vm.spiritualJourney.attributes;
            
    }

    function evaluateTripSkills() {
      if (vm.signupService.page5.professionalSkills) {
        return;
      }

      var attrs = AttributeTypeService.transformPersonMultiAttributes(attributeTypes.TRIP_SKILLS,
          vm.signupService.person.attributes,
          vm.tripSkills.attributes, function(attr) {
             return attr.isChecked = true; 
          });

      vm.signupService.page5.professionalSkills = attrs;
    }

    function evaluateTshirtSize() {
      if (vm.signupService.page2.tshirtSize) {
        return;
      }

      vm.signupService.page2.tshirtSize = _.find(vm.signupService.person.attributes, function(attr) {
        return attr.attributeTypeId === attributeTypes.TSHIRT_SIZES;
      });
    }

    function evaluateVegetarian() {
      if (vm.signupService.page2.vegetarian) {
        return;
      }

      var found = _.find(vm.signupService.person.attributes, function(attr) {
        return attr.attributeId === attributes.VEGETARIAN;
      });

      if (found) {
        vm.signupService.page2.vegetarian = 'yes';
      }
    }

  }
})();
