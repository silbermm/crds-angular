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
    'WorkTeams',
    'ScrubTopSizes',
    'ScrubBottomSizes',
    'TshirtSizes',
    'InternationalExperience',
  ];

  function SignupStepController(
    $stateParams,
    TripsSignupService,
    AttributeTypeService,
    $scope,
    WorkTeams,
    ScrubTopSizes,
    ScrubBottomSizes,
    TshirtSizes,
    InternationalExperience) {

    var vm = this;

    vm.signupService = $scope.$parent.tripsSignup.signupService;

    vm.allergies = vm.signupService.person.singleAttributes[attributeTypes.ALLERGIES];

    vm.dietaryRestrictions = vm.signupService.person.attributeTypes[attributeTypes.DIETARY_RESTRICTIONS].attributes;
    vm.frequentFlyers = vm.signupService.person.attributeTypes[attributeTypes.FREQUENT_FLYERS].attributes;

    vm.internationalExpSelected = vm.signupService.person.singleAttributes[attributeTypes.INTERNATIONAL_EXPERIENCE];
    vm.interExperience = InternationalExperience;

    vm.scrubBottom = vm.signupService.person.singleAttributes[attributeTypes.SCRUB_BOTTOM_SIZES];
    vm.scrubBottomSizes = ScrubBottomSizes;

    vm.scrubTop = vm.signupService.person.singleAttributes[attributeTypes.SCRUB_TOP_SIZES];
    vm.scrubTopSizes = ScrubTopSizes;

    vm.spiritualLife = vm.signupService.person.attributeTypes[attributeTypes.SPIRITUAL_JOURNEY].attributes;

    vm.step = $stateParams.stepId;

    vm.tripExperience = vm.signupService.person.singleAttributes[attributeTypes.TRIP_EXPERIENCE];

    vm.tripSkills = vm.signupService.person.attributeTypes[attributeTypes.TRIP_SKILLS].attributes;

    vm.tshirt = vm.signupService.person.singleAttributes[attributeTypes.TSHIRT_SIZES];
    vm.tshirtSizes = TshirtSizes;

    vm.workTeams = WorkTeams;

    vm.signupService.pageId = $stateParams.stepId;
    vm.signupService.pages[$stateParams.stepId] = {dirty: false};

    // populate dropdowns and select defaults
    switch (vm.signupService.pageId) {
      case '2':
        evaluateSpiritualLife();
        break;
      case '6':
        break;
      default:
        break;
    }

    function evaluateSpiritualLife() {
      _.forEach(vm.spiritualLife, function(spirit) {
        if (spirit.selected) {
          spirit.selected = false;
          spirit.endDate = new Date();
        }
      });
    }
  }
})();
