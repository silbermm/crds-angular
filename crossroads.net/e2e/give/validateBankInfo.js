var env = require("../environment");

describe('Crossroads App', function() {

  beforeEach(function(){
    browser.get(env.baseUrl + '/#/give/account');  
  });


  it('It should display an error message for invalid account number', function() {
   var accountInput = element(by.model('give.account'));
   accountInput.sendKeys("aaa");
   accountInput.sendKeys(protractor.Key.TAB);
   expect(element(by.model('give.account')).getAttribute('class')).toMatch('ng-invalid-invalid-account');
  });

  it('It should display an error message for invalid routing transit number', function() {
    var routingNumber = element(by.model('give.routing'));
    routingNumber.sendKeys("aaa");
    routingNumber.sendKeys(protractor.Key.TAB);
    expect(element(by.model('give.routing')).getAttribute('class')).toMatch('ng-invalid-invalid-routing');
  });

  it('It should not display an error message for invalid routing transit number', function() {
    var routingNumber = element(by.model('give.routing'));
    routingNumber.sendKeys("111111111");
    routingNumber.sendKeys(protractor.Key.TAB);
    expect(element(by.model('give.routing')).getAttribute('class')).toMatch('ng-valid-invalid-routing');
  });

  it('It should not display an error message for invalid account number', function() {
    var accountInput = element(by.model('give.account'));
    accountInput.sendKeys("1234567890");
    accountInput.sendKeys(protractor.Key.TAB);
    expect(element(by.model('give.account')).getAttribute('class')).toMatch('ng-valid-invalid-account');
  });

});
