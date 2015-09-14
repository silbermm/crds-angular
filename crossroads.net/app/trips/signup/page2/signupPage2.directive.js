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
        pageTitle: '=',
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
    }

    function bottomScrubSizes() {
      return [{
        formFieldId: 1224,
        attributeId: 3174,
        label: 'Adult XS'
      }, {
        formFieldId: 1224,
        attributeId: 3175,
        label: 'Adult S'
      }, {
        formFieldId: 1224,
        attributeId: 3176,
        label: 'Adult M'
      }, {
        formFieldId: 1224,
        attributeId: 3177,
        label: 'Adult L'
      }, {
        formFieldId: 1224,
        attributeId: 3178,
        label: 'Adult XL'
      }, {
        formFieldId: 1224,
        attributeId: 3179,
        label: 'Adult XXL'
      }, {
        formFieldId: 1224,
        attributeId: 3180,
        label: 'Adult XXXL'
      }];
    }

    function tshirtSizes() {
      return [{
        formFieldId: 1223,
        attributeId: 3157,
        label: 'Adult XS'
      }, {
        formFieldId: 1223,
        attributeId: 3158,
        label: 'Adult S'
      }, {
        formFieldId: 1223,
        attributeId: 3159,
        label: 'Adult M'
      }, {
        formFieldId: 1223,
        attributeId: 3160,
        label: 'Adult L'
      }, {
        formFieldId: 1223,
        attributeId: 3161,
        label: 'Adult XL'
      }, {
        formFieldId: 1223,
        attributeId: 3162,
        label: 'Adult XXL'
      }, {
        formFieldId: 1223,
        attributeId: 3163,
        label: 'Adult XXXL'
      }, {
        formFieldId: 1223,
        attributeId: 3164,
        label: 'Child S'
      }, {
        formFieldId: 1223,
        attributeId: 3165,
        label: 'Child M'
      }, {
        formFieldId: 1223,
        attributeId: 3166,
        label: 'Child L'
      }];
    }

  }
})();
