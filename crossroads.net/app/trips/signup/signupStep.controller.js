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
        break;
      default:
        break;
    }

    function evaluateFoodAllergies() {
      if (vm.signupService.page2.allergies) {
        return;
      }

      var found = _.find(vm.signupService.person.attributeTypes[attributeTypes.PERSONAL].attributes, function(attr) {
        return attr.attributeId === attributes.FOOD_ALLERGIES;
      });

      if (found) {
        vm.signupService.page2.allergies = found.notes;
      }

    }

    function evaluateFrequentFlyers() {
      if (vm.signupService.page6.frequentFlyers) {
        return;
      }

      var attrs = AttributeTypeService.transformPersonMultiAttributes(
          vm.signupService.person.attributeTypes[attributeTypes.FREQUENT_FLYERS].attributes,
          vm.frequentFlyers.attributes, function(contactAttr, attr) {
            attr.notes = contactAttr.notes;
          });

      vm.signupService.page6.frequentFlyers = attrs;
    }

    function evaluatePreviousTripExp() {
      if (vm.signupService.page5.previousTripExperience) {
        return;
      }

      var found =
        _.find(vm.signupService.person.attributeTypes[attributeTypes.TRIP_EXPERIENCE].attributes, function(attr) {
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

      if (vm.signupService.person.attributeTypes[attributeTypes.SCRUB_BOTTOM_SIZES] !== undefined) {
        vm.signupService.page2.scrubSizeBottom =
          vm.signupService.person.attributeTypes[attributeTypes.SCRUB_BOTTOM_SIZES].attributes[0];
      }
    }

    function evaluateScrubSizeTop() {
      if (vm.signupService.page2.scrubSizeTop) {
        return;
      }

      if (vm.signupService.person.attributeTypes[attributeTypes.SCRUB_TOP_SIZES] !== undefined) {
        vm.signupService.page2.scrubSizeTop =
          vm.signupService.person.attributeTypes[attributeTypes.SCRUB_TOP_SIZES].attributes[0];
      }
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

      var attrs = AttributeTypeService.transformPersonMultiAttributes(
          vm.signupService.person.attributeTypes[attributeTypes.TRIP_SKILLS].attributes,
          vm.tripSkills.attributes, function(personAttr, attr) {
            attr.isChecked = true;
          });

      vm.signupService.page5.professionalSkills = attrs;
    }

    function evaluateTshirtSize() {
      if (vm.signupService.page2.tshirtSize) {
        return;
      }

      if (vm.signupService.person.attributeTypes[attributeTypes.TSHIRT_SIZES] !== undefined) {
        vm.signupService.page2.tshirtSize =
          vm.signupService.person.attributeTypes[attributeTypes.TSHIRT_SIZES].attributes[0];
      }
    }

    function evaluateVegetarian() {
      if (vm.signupService.page2.vegetarian) {
        return;
      }

      if (vm.signupService.person.attributeTypes[attributeTypes.DIETARY_RESTRICTIONS] !== undefined) {

        var found =
          _.find(
              vm.signupService.person.attributeTypes[attributeTypes.DIETARY_RESTRICTIONS].attributes, function(attr) {
          return attr.attributeId === attributes.VEGETARIAN;
        });

        if (found) {
          vm.signupService.page2.vegetarian = 'yes';
        }
      }
    }

  }
})();
