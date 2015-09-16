(function() {
  'use strict';

  module.exports = SignupPage2Directive;

  SignupPage2Directive.$inject = [];

  function SignupPage2Directive() {
    return {
      restrict: 'E',
      replace: true,
      scope: {
        currentPage: '=',
        destination: '=',
        numberOfPages: '=',
      },
      templateUrl: 'page2/signupPage2.html',
      controller: 'PagesController as pages',
      bindToController: true,
      link: link
    };

    function link(scope) {
      scope.tshirtSizes = tshirtSizes();
      scope.bottomScrubSizes = bottomScrubSizes();
      scope.topScrubSizes = topScrubSizes();
    }

    function bottomScrubSizes() {
      return [{
        formFieldId: 1224,
        attributeId: 3174,
        value: 'Adult XS'
      }, {
        formFieldId: 1224,
        attributeId: 3175,
        value: 'Adult S'
      }, {
        formFieldId: 1224,
        attributeId: 3176,
        value: 'Adult M'
      }, {
        formFieldId: 1224,
        attributeId: 3177,
        value: 'Adult L'
      }, {
        formFieldId: 1224,
        attributeId: 3178,
        value: 'Adult XL'
      }, {
        formFieldId: 1224,
        attributeId: 3179,
        value: 'Adult XXL'
      }, {
        formFieldId: 1224,
        attributeId: 3180,
        value: 'Adult XXXL'
      }];
    }

    function topScrubSizes() {
      return [{
        formFieldId: 1414,
        attributeId: 3167,
        value: 'Adult XS'
      }, {
        formFieldId: 1414,
        attributeId: 3168,
        value: 'Adult S'
      }, {
        formFieldId: 1414,
        attributeId: 3169,
        value: 'Adult M'
      }, {
        formFieldId: 1414,
        attributeId: 3170,
        value: 'Adult L'
      }, {
        formFieldId: 1414,
        attributeId: 3171,
        value: 'Adult XL'
      }, {
        formFieldId: 1414,
        attributeId: 3172,
        value: 'Adult XXL'
      }, {
        formFieldId: 1414,
        attributeId: 3173,
        value: 'Adult XXXL'
      }];
    }

    function tshirtSizes() {
      return [{
        formFieldId: 1223,
        attributeId: 3157,
        value: 'Adult XS'
      }, {
        formFieldId: 1223,
        attributeId: 3158,
        value: 'Adult S'
      }, {
        formFieldId: 1223,
        attributeId: 3159,
        value: 'Adult M'
      }, {
        formFieldId: 1223,
        attributeId: 3160,
        value: 'Adult L'
      }, {
        formFieldId: 1223,
        attributeId: 3161,
        value: 'Adult XL'
      }, {
        formFieldId: 1223,
        attributeId: 3162,
        value: 'Adult XXL'
      }, {
        formFieldId: 1223,
        attributeId: 3163,
        value: 'Adult XXXL'
      }, {
        formFieldId: 1223,
        attributeId: 3164,
        value: 'Child S'
      }, {
        formFieldId: 1223,
        attributeId: 3165,
        value: 'Child M'
      }, {
        formFieldId: 1223,
        attributeId: 3166,
        value: 'Child L'
      }];
    }

  }
})();
