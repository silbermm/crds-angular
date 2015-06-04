describe('Credit Card Info Directive', function() {
  var ccElement, scope, isolateScope, form, $timeout;

  beforeEach(function() {
    module('crossroads');
  });

  beforeEach(inject(function(_$compile_, _$rootScope_, _$templateCache_, _$timeout_) {
    var $compile = _$compile_;
    var $rootScope = _$rootScope_;
    var $templateCache = _$templateCache_;

    $timeout = _$timeout_;

    $templateCache.put('on-submit-messages', '<span ng-message="required">Required</span>');
    $templateCache.put('on-blur-messages',
      '<span ng-message="invalidRouting">Invalid routing</span>'
      + '<span ng-message="invalidAccount">Invalid account</span>'
      + '<span ng-message="naturalNumber">Not a valid number</span>'
      + '<span ng-message="invalidZip">Invalid zip</span>');

    var templateString =
      "<credit-card-info "
      +  "cvc='data.cvc' "
      +  "exp-date='data.expDate' "
      +  "cc-number='data.ccNumber' "
      +  "billing-zip-code='data.billingZipCode' "
      +  "bankinfo-submitted='data.bankinfoSubmitted' "
      +  "name-on-card='data.nameOnCard' "
      +  "default-source='data.defaultSource' "
      +  "change-account-info='false'> "
      +  "</credit-card-info>";

    scope = $rootScope.$new();
    scope.data = {
      cvc: '123',
      expDate: '1219',
      ccNumber: '4242424242424242',
      billingZipCode: '45140',
      bankInfoSubmitted: false,
      nameOnCard: 'Mr. Ed',
      defaultSource: {
        address_zip: "12345",
        brand: "Visa",
        cvc: "123",
        exp_date: "0123",
        name: "Tim Giver",
        last4: "9876",
      },
      changeAccountInfo: false,
    };

    ccElement = $compile(templateString)(scope);
    scope.$digest();
    isolateScope = ccElement.isolateScope();
    form = isolateScope.creditCardForm;
  }));

  describe('swapCreditCardExpDateFields Function', function() {
    it('should not dirty the credit card form or set focus if not changing existing account info', function() {
      var expDate = ccElement.find('input')[2];
      spyOn(expDate, 'focus');
      isolateScope.swapCreditCardExpDateFields();
      expect(form.$dirty).toBeFalsy();
      expect(expDate.focus).not.toHaveBeenCalled();
    });
  });

  describe('swapCreditCardExpDateFields Function', function() {
    it('should dirty the credit card form and set focus if changing existing account info', function() {
      var expDate = ccElement.find('input')[2];
      spyOn(expDate, 'focus');
      isolateScope.changeAccountInfo = true;
      isolateScope.swapCreditCardExpDateFields();
      $timeout.flush();
      expect(form.$dirty).toBeTruthy();
      expect(expDate.focus).toHaveBeenCalled();
    });
  });

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
      isolateScope.defaultCardPlaceholderValues = {};
      isolateScope.ccCardType();
      expect(isolateScope.ccNumberClass).toBe("");
    });
  });
});
