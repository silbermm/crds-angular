var env = require("../environment");

describe('Crossroads App', function() {

  beforeEach(function(){
    browser.get(env.baseUrl + '/#/give');
    var creditCardButton = element(by.buttonText("Credit Card")).click();
  });


   it('It should display an error message for an invalid credit card number', function() {
     var cardInput = element(by.model('give.ccNumber'));
     cardInput.sendKeys("6511000000000000");
     cardInput.sendKeys(protractor.Key.TAB);
    //expect $root.messages[$root.MESSAGES.invalidCard].message to display
   });

  it('It should display an error message for invalid cvv number', function() {
    var cvvInput = element(by.model('give.cvc'));
    cvvInput.sendKeys("12");
    cvvInput.sendKeys(protractor.Key.TAB);
    //expect $root.messages[$root.MESSAGES.invalidCvv].message to display
  });

  it('It should display an error message for invalid zip code', function() {
    var zipInput = element(by.model('give.billingZipCode'));
    zipInput.sendKeys("aaaaaa");
    zipInput.sendKeys(protractor.Key.TAB);
    expect(element(by.model('give.billingZipCode')).getAttribute('class')).toMatch('ng-invalid-invalid-zip');
  });

  it('It should not display any error messages because all data is valid', function() {
    var nameInput = element(by.model('give.nameOnCard'));
    nameInput.sendKeys("Joe F. Smith, Jr");
    nameInput.sendKeys(protractor.Key.TAB);
    expect(element(by.model('give.nameOnCard')).getAttribute('class')).toMatch('ng-valid');

    var cardInput = element(by.model('give.ccNumber'));
    cardInput.sendKeys("6011111111111117");
    cardInput.sendKeys(protractor.Key.TAB);
    //expect $root.messages[$root.MESSAGES.invalidCard].message to not diplay

    var cvvInput = element(by.model('give.cvc'));
    cvvInput.sendKeys("123");
    cvvInput.sendKeys(protractor.Key.TAB);
    //expect $root.messages[$root.MESSAGES.invalidCvv].message to not display

    var zipInput = element(by.model('give.billingZipCode'));
    zipInput.sendKeys("12345-1234");
    zipInput.sendKeys(protractor.Key.TAB);
    expect(element(by.model('give.billingZipCode')).getAttribute('class')).toMatch('ng-valid-invalid-zip');
   });

});
