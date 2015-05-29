describe('Credit Card Info Directive', function() {
  var scope, isolateScope, form;

  beforeEach(function() {
    module('crossroads');
  });

  beforeEach(inject(function(_$compile_, _$rootScope_, _$templateCache_) {
    var $compile = _$compile_;
    var $rootScope = _$rootScope_;
    var $templateCache = _$templateCache_;

    $templateCache.put('on-submit-messages', '<span ng-message="required">Required</span>');
    $templateCache.put('on-blur-messages',
      '<span ng-message="invalidRouting">Invalid routing</span>'
      + '<span ng-message="invalidAccount">Invalid account</span>'
      + '<span ng-message="naturalNumber">Not a valid number</span>'
      + '<span ng-message="invalidZip">Invalid zip</span>');

    var template = angular.element('<credit-card-info cvc="model.cvc" exp-date="model.expDate" cc-number="model.ccNumber" billing-zip-code="model.billingZipCode" bankinfo-submitted="model.bankinfoSubmitted" name-on-card="model.nameOnCard"></credit-card-info>');
    scope = $rootScope.$new();
    scope.model = {
      cvc: '123',
      expDate: '1219',
      ccNumber: '4242424242424242',
      billingZipCode: '45140',
      bankInfoSubmitted: false,
      nameOnCard: 'Mr. Ed'
    };

    var element = $compile(template)(scope);
    scope.$digest();
    isolateScope = element.isolateScope();
    form = isolateScope.creditCardForm;
  }));

  describe('ccCardType Function', function() {
    it('should have the visa credit card class', function(){
      form.ccNumber.$modelValue = '4242424242424242';
      isolateScope.ccCardType();
      expect(isolateScope.ccNumberClass).toBe("cc-visa");
    });

    it('should have the mastercard credit card class', function(){
      form.ccNumber.$modelValue = '5105105105105100';
      isolateScope.ccCardType();
      expect(isolateScope.ccNumberClass).toBe("cc-mastercard");
    });

    it('should have the discover credit card class', function(){
      form.ccNumber.$modelValue = '6011111111111117';
      isolateScope.ccCardType();
      expect(isolateScope.ccNumberClass).toBe("cc-discover");
    });

    it('should have the amex credit card class', function(){
      form.ccNumber.$modelValue = '378282246310005';
      isolateScope.ccCardType();
      expect(isolateScope.ccNumberClass).toBe("cc-american-express");
    });

    it('should not have a credit card class', function(){
      form.ccNumber.$modelValue = '';
      isolateScope.ccCardType();
      expect(isolateScope.ccNumberClass).toBe("");
    });
  });
});
