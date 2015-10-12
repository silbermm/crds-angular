var attributeTypes = require('crds-constants').ATTRIBUTE_TYPE_IDS;
var attributes = require('crds-constants').ATTRIBUTE_IDS;
(function() {
  'use strict';

  module.exports = SignupStepController;

  SignupStepController.$inject = [
    '$stateParams',
    'TripsSignupService',
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
      case "2":
        evaluateScrubSizeBottom();
        evaluateScrubSizeTop();
        evaluateTshirtSize();
        evaluateVegetarian();
        evaluateFoodAllergies();
        break;
      case "5":
        evaluatePreviousTripExp();
        evaluateTripSkills();
        // do something
        break;
      default:
        // do nothing
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

    function evaluateTripSkills() {
      // how am i going to do this?
      var tripSkills = _.filter(vm.signupService.person.attributes), function(attr) {
          return attr.attributeTypeId === attributeTypes.TRIP_SKILLS;
      });

      _.each(tripsSkills, function() {
        // check if the model has been set... how?  
      });


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

    function evaluateTshirtSize() {
      if (vm.signupService.page2.tshirtSize) {
        return;
      }
      vm.signupService.page2.tshirtSize = _.find(vm.signupService.person.attributes, function(attr) {
        return attr.attributeTypeId === attributeTypes.SCRUB_TOP_SIZES;
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
        vm.signupService.page2.vegetarian = "yes";
      }
    }

  }
})();
