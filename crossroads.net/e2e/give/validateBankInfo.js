var env = require("../environment");

describe('Crossroads App', function() {

  beforeEach(function() {
    browser.get(env.baseUrl + '/#/give/account');
    var creditCardButton = element(by.buttonText("")).click();
  });

  
  it('It should display error messages for all invalid data fields', function() {
    var emailInput = element(by.id('give-email'));
    emailInput.sendKeys("1234.net");
    emailInput.sendKeys(protractor.Key.TAB);
    expect(element(by.id('give-email')).getAttribute('class')).toMatch('ng-invalid-email');
    var routingInput = element(by.model('routing'));
    routingInput.sendKeys("5555");
    routingInput.sendKeys(protractor.Key.TAB);  
    expect(element(by.model('routing')).getAttribute('class')).toMatch('ng-invalid-invalid-routing');
    var accountInput = element(by.model('account'));
    accountInput.sendKeys("account");
    accountInput.sendKeys(protractor.Key.TAB);
    expect(element(by.model('account')).getAttribute('class')).toMatch('ng-invalid-invalid-account');
  });

  it('It should not display an error message for a valid routing number', function() {
    var emailInput = element(by.id('give-email'));
    emailInput.sendKeys("cross@roads.net");
    emailInput.sendKeys(protractor.Key.TAB);
    expect(element(by.id('give-email')).getAttribute('class')).toMatch('ng-valid-email');
    var routingInput = element(by.model('routing'));
    routingInput.sendKeys("042000314");
    routingInput.sendKeys(protractor.Key.TAB);
    expect(element(by.model('routing')).getAttribute('class')).toMatch('ng-valid-invalid-routing');
    var accountInput = element(by.model('account'));
    accountInput.sendKeys("042099914005");
    accountInput.sendKeys(protractor.Key.TAB);
    expect(element(by.model('account')).getAttribute('class')).toMatch('ng-valid-invalid-account');
  });

 
});
