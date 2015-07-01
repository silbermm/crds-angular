var env = require("../environment");

describe('Giving Flow', function() {
  var checkState;

  beforeEach(function() {
    browser.get(env.baseUrl + '/#/give');
    checkState = function(stateName) {
      browser.waitForAngular();
      browser.executeScript(function() {
        return(angular.element(document).injector().get('$state').current);
      }).then(function(state) {
        expect(state.name).toBe(stateName);
      });
    };
  })

  afterEach(function() {
    var logoutButton = element.all(by.css(".navbar--login")).get(0).all(by.linkText('Sign Out'));
    logoutButton.click();
  });

  it('should follow full credit card flow, logging in as user with existing giver', function () {
    checkState('give.amount');
    element(by.model('amount')).sendKeys("12345");
    element(by.binding('amount')).click();
    checkState('give.login');
    var loginButton = element.all(by.css('.btn')).get(6);
    expect(loginButton.getText()).toBe("Login");
    loginButton.click();
    element(by.id('login-page-email')).sendKeys("tim@kriz.net");
    element(by.id('login-page-password')).sendKeys("password");
    var button = element.all(by.id('submit_nav')).get(2);
    button.click();
    checkState('give.confirm');
    var giveButton = element(by.css("[ng-click=\"give.confirmDonation()\"]"));
    expect(giveButton.getText()).toBe("GIVE $12,345.00");
    giveButton.click();
    checkState('give.thank-you');
  });

  it('should follow full flow, giving as guest', function () {
    checkState('give.amount');
    element(by.model('amount')).sendKeys("1999");
    element(by.binding('amount')).click();
    checkState('give.login');
    var giveAsGuestButton = element.all(by.css('.btn')).get(7);
    expect(giveAsGuestButton.getText()).toBe("Give as Guest");
    giveAsGuestButton.click();
    var creditCardButton = element.all(by.model('give.dto.view')).get(1);
    expect(creditCardButton.getText()).toBe("Credit Card");
    creditCardButton.click();
    element(by.id('give-email')).sendKeys("tim@kriz.net");
    element(by.model('creditCard.ccNumber')).sendKeys("4242424242424242");
    element(by.model('creditCard.expDate')).sendKeys("0118");
    element(by.model('creditCard.cvc')).sendKeys("654");
    element(by.model('creditCard.billingZipCode')).sendKeys("45202-5236");

    var giveButton = element.all(by.css("[ng-click=\"give.submitBankInfo()\"]")).get(0);
    expect(giveButton.getText()).toBe("GIVE $1,999.00");

    giveButton.click().then(function() {
      browser.waitForAngular();
      checkState('give.thank-you');
      var email = element.all(by.binding('give.email')).first();
      expect(email.getText()).toBe("tim@kriz.net");

      var amount = element.all(by.binding('give.amount')).first();
      expect(amount).toBeDefined();
      expect(amount.getText()).toBe("$1,999.00");

      var program = element.all(by.binding("give.program['Name']")).first();
      expect(program).toBeDefined();
      expect(program.getText()).toBe("Crossroads");
    });
  });

  it('should register as new user and not lose the amt or fund', function () {
    checkState('give.amount');
    element(by.model('amount')).sendKeys("867539");
    expect(element(by.binding("amount")).getText()).toContain("GIVE $867,539.00");
    element(by.binding('amount')).click();
    checkState('give.login');
    var regButton = element(by.linkText('Create an account'));
    regButton.click();
    checkState('give.register');
    element.all(by.id('registration-firstname')).get(1).sendKeys("Jack");
    element.all(by.id('registration-lastname')).get(1).sendKeys("Protractor");
    var ranNum = Math.floor((Math.random() * 1000) + 1);
    element.all(by.id('registration-email')).get(4).sendKeys("updates+" +ranNum+ "@crossroads.net");
    element.all(by.id('registration-password')).get(2).sendKeys("protractor");
    var regButton = element.all(by.css('.btn')).get(6);
    regButton.click();
    checkState('give.account');
    element(by.cssContainingText('.ng-binding', 'Ministry'));
    element(by.cssContainingText('.ng-binding', '$867,539.00'));
  });

  it('should follow full credit card flow, logging in as user with existing giver and changing account information', function () {
    checkState('give.amount');
    element(by.model('amount')).sendKeys("12345");
    element(by.binding('amount')).click();
    checkState('give.login');
    var loginButton = element.all(by.css('.btn')).get(6);
    expect(loginButton.getText()).toBe("Login");
    loginButton.click();
    element(by.id('login-page-email')).sendKeys("tim@kriz.net");
    element(by.id('login-page-password')).sendKeys("password");
    var button = element.all(by.id('submit_nav')).get(2);
    button.click();
    checkState('give.confirm');
    var giveButton = element(by.css("[ng-click=\"give.goToChange(give.amount, give.donor, give.email, give.program, give.view)\"]"));
    giveButton.click();
    checkState('give.change');
    var creditCardButton = element.all(by.model('give.dto.view')).get(1);
    expect(creditCardButton.getText()).toBe("Credit Card");
    creditCardButton.click();
    element(by.model('amount')).clear();
    element(by.model('amount')).sendKeys("54321");
    element(by.model('creditCard.ccNumber')).sendKeys("5555555555554444");
    element(by.model('creditCard.expDate')).sendKeys("0818");
    element(by.model('creditCard.cvc')).sendKeys("999");
    element(by.model('creditCard.billingZipCode')).sendKeys("45202-0818");
    var chgButton = element.all(by.css("[ng-click=\"give.submitChangedBankInfo()\"]")).get(0);
    expect(chgButton.getText()).toBe("GIVE $54,321.00");
    chgButton.click();
    checkState('give.thank-you');
    var email = element.all(by.binding('give.email')).first();
    expect(email).toBeDefined();
    expect(email.getText()).toBe("tim@kriz.net");

    var amount = element.all(by.binding('give.amount')).first();
    expect(amount).toBeDefined();
    expect(amount.getText()).toBe("$54,321.00");

    var program = element.all(by.binding("give.program['Name']")).first();
    expect(program).toBeDefined();
    expect(program.getText()).toBe("Crossroads");
  });

  it('should follow full bank account flow, logging in as user with existing giver and changing bank account information', function () {
    checkState('give.amount');
    element(by.model('amount')).sendKeys("12345");
    element(by.binding('amount')).click();
    checkState('give.login');
    var loginButton = element.all(by.css('.btn')).get(6);
    expect(loginButton.getText()).toBe("Login");
    loginButton.click();
    element(by.id('login-page-email')).sendKeys("sandi.ritter+protractor@ingagepartners.com");
    element(by.id('login-page-password')).sendKeys("winter14");
    var button = element.all(by.id('submit_nav')).get(2);
    button.click();
    checkState('give.confirm');
    var giveButton = element(by.css("[ng-click=\"give.goToChange(give.amount, give.donor, give.email, give.program, give.view)\"]"));
    giveButton.click();
    checkState('give.change');
    var bankAccountButton = element.all(by.model('give.dto.view')).get(0);
    expect(bankAccountButton.getText()).toBe("Bank Account");
    bankAccountButton.click();
    element(by.model('amount')).clear();
    element(by.model('amount')).sendKeys("89321");
    element(by.model('bankAccount.routing')).sendKeys("110000000");
    element(by.model('bankAccount.account')).sendKeys("000123456789");
    var chgButton = element.all(by.css("[ng-click=\"give.submitChangedBankInfo()\"]")).get(0);
    expect(chgButton.getText()).toBe("GIVE $89,321.00");
    chgButton.click().then(function() {
      browser.waitForAngular();
      checkState('give.thank-you');
      var email = element.all(by.binding('give.email')).first();
      expect(email).toBeDefined();
      expect(email.getText()).toBe("sandi.ritter+protractor@ingagepartners.com");

      var amount = element.all(by.binding('give.amount')).first();
      expect(amount).toBeDefined();
      expect(amount.getText()).toBe("$89,321.00");

      var program = element.all(by.binding("give.program['Name']")).first();
      expect(program).toBeDefined();
      expect(program.getText()).toBe("Crossroads");
   });
  });

  it('Giving as guest via credit card, using the change link- retain valid info and discard invalid info', function () {
    checkState('give.amount');
    element(by.model('amount')).sendKeys("1999");
    element(by.binding('amount')).click();
    checkState('give.login');
    var giveAsGuestButton = element.all(by.css('.btn')).get(7);
    expect(giveAsGuestButton.getText()).toBe("Give as Guest");
    giveAsGuestButton.click();
    var creditCardButton = element.all(by.model('give.dto.view')).get(1);
    expect(creditCardButton.getText()).toBe("Credit Card");
    creditCardButton.click();
    element(by.id('give-email')).sendKeys("tim@kriz.net");
    element(by.model('creditCard.ccNumber')).sendKeys("4242424242424242");
    element(by.model('creditCard.expDate')).sendKeys("0118");
    element(by.model('creditCard.cvc')).sendKeys("6");
    element(by.model('creditCard.billingZipCode')).sendKeys("452025236");

    var changeButton = element.all(by.css("[ng-click=\"give.processChange()\"]")).get(0);
    expect(changeButton.getText()).toBe("Change");

    changeButton.click().then(function() {
       checkState('give.amount');
       expect(element(by.model('amount'))).toBeDefined();
    });
    element(by.model('amount')).clear();
    element(by.model('amount')).sendKeys("99");
    element(by.binding('amount')).click();
    var creditCardButton = element.all(by.model('give.dto.view')).get(1);
    expect(creditCardButton.getText()).toBe("Credit Card");   
    expect(element(by.model('give-email'))).toBeDefined();
    expect(element(by.model('creditCard.ccNumber'))).toBeDefined();
    expect(element(by.model('creditCard.expDate'))).toBeDefined();
    expect(element(by.model('creditCard.cvc')).getText()).toBe("");
    expect(element(by.model('creditCard.billingZipCode')).getText()).toBe("");

    element(by.model('creditCard.cvc')).sendKeys("678");
    element(by.model('creditCard.billingZipCode')).sendKeys("45202-5236");

    var chgButton = element.all(by.css("[ng-click=\"give.submitBankInfo()\"]")).get(0);
    expect(chgButton.getText()).toBe("GIVE $99.00");
    chgButton.click().then(function() {
      browser.waitForAngular();
      checkState('give.thank-you');
      var email = element.all(by.binding('give.email')).first();
      expect(email).toBeDefined();
      expect(email.getText()).toBe("tim@kriz.net");

      var amount = element.all(by.binding('give.amount')).first();
      expect(amount).toBeDefined();
      expect(amount.getText()).toBe("$99.00");

      var program = element.all(by.binding("give.program['Name']")).first();
      expect(program).toBeDefined();
      expect(program.getText()).toBe("Crossroads");
   });
  });

  it('Giving as guest via ACH, using the change link - retain valid info and discard invalid info', function () {
    checkState('give.amount');
    element(by.model('amount')).sendKeys("999");
    element(by.binding('amount')).click();
    checkState('give.login');
    var giveAsGuestButton = element.all(by.css('.btn')).get(7);
    expect(giveAsGuestButton.getText()).toBe("Give as Guest");
    giveAsGuestButton.click();
    var creditCardButton = element.all(by.model('give.dto.view')).get(0);
    expect(creditCardButton.getText()).toBe("Bank Account");
    creditCardButton.click();
    element(by.id('give-email')).sendKeys("tim@kriz.net");
    element(by.model('bankAccount.routing')).sendKeys("111111");
    element(by.model('bankAccount.account')).sendKeys("aaa");
    
    var changeButton = element.all(by.css("[ng-click=\"give.processChange()\"]")).get(0);
    expect(changeButton.getText()).toBe("Change");

    changeButton.click().then(function() {
       browser.waitForAngular();
       checkState('give.amount');
       expect(element(by.model('amount'))).toBeDefined();
    });
    element(by.model('amount')).clear();
    element(by.model('amount')).sendKeys("199");
    element(by.binding('amount')).click();
    var creditCardButton = element.all(by.model('give.dto.view')).get(1);
    expect(creditCardButton.getText()).toBe("Credit Card");   
    expect(element(by.model('give-email'))).toBeDefined();
    expect(element(by.model('bankAccount.routing')).getText()).toBe("");
    expect(element(by.model('bankAccount.account')).getText()).toBe("");
 
    element(by.model('bankAccount.routing')).sendKeys("110000000");
    element(by.model('bankAccount.account')).sendKeys("000123456789");
   
    var giveButton = element.all(by.css("[ng-click=\"give.submitBankInfo()\"]")).get(0);
    expect(giveButton.getText()).toBe("GIVE $199.00");
    giveButton.click().then(function() {
      browser.waitForAngular();
      checkState('give.thank-you');
      var email = element.all(by.binding('give.email')).first();
      expect(email).toBeDefined();
      expect(email.getText()).toBe("tim@kriz.net");

      var amount = element.all(by.binding('give.amount')).first();
      expect(amount).toBeDefined();
      expect(amount.getText()).toBe("$199.00");

      var program = element.all(by.binding("give.program['Name']")).first();
      expect(program).toBeDefined();
      expect(program.getText()).toBe("Crossroads");

   });
  });

  it('Existing user, giving via ACH giving again via Credit Card - testing change to payment type', function () {
    checkState('give.amount');
    element(by.model('amount')).sendKeys("555");
    element(by.binding('amount')).click();
    checkState('give.login');
    var loginButton = element.all(by.css('.btn')).get(6);
    expect(loginButton.getText()).toBe("Login");
    loginButton.click();
    element(by.id('login-page-email')).sendKeys("sandi.ritter+protractor@ingagepartners.com");
    element(by.id('login-page-password')).sendKeys("winter14");
    var button = element.all(by.id('submit_nav')).get(2);
    button.click();
    checkState('give.confirm');
    var giveButton = element(by.css("[ng-click=\"give.goToChange(give.amount, give.donor, give.email, give.program, give.view)\"]"));
    giveButton.click();
    checkState('give.change');
    var bankAccountButton = element.all(by.model('give.dto.view')).get(0);
    expect(bankAccountButton.getText()).toBe("Bank Account");
    bankAccountButton.click();
    element(by.model('amount')).clear();
    element(by.model('amount')).sendKeys("444");
    element(by.model('bankAccount.routing')).sendKeys("110000000");
    element(by.model('bankAccount.account')).sendKeys("000123456789");
    var chgButton = element.all(by.css("[ng-click=\"give.submitChangedBankInfo()\"]")).get(0);
    expect(chgButton.getText()).toBe("GIVE $444.00");
    chgButton.click().then(function() {
      browser.waitForAngular();
      checkState('give.thank-you');
      var email = element.all(by.binding('give.email')).first();
      expect(email).toBeDefined();
      expect(email.getText()).toBe("sandi.ritter+protractor@ingagepartners.com");

      var amount = element.all(by.binding('give.amount')).first();
      expect(amount).toBeDefined();
      expect(amount.getText()).toBe("$444.00");

      var program = element.all(by.binding("give.program['Name']")).first();
      expect(program).toBeDefined();
      expect(program.getText()).toBe("Crossroads"); 
     });
    var logoutButton = element.all(by.css(".navbar--login")).get(0).all(by.linkText('Sign Out'));
    logoutButton.click();

    browser.get(env.baseUrl + '/#/give');
    checkState('give.amount');
    element(by.model('amount')).sendKeys("543");
    element(by.binding('amount')).click();
    checkState('give.login');
    var loginButton = element.all(by.css('.btn')).get(6);
    expect(loginButton.getText()).toBe("Login");
    loginButton.click();
    element(by.id('login-page-email')).sendKeys("sandi.ritter+protractor@ingagepartners.com");
    element(by.id('login-page-password')).sendKeys("winter14");
    var button = element.all(by.id('submit_nav')).get(2);
    button.click();
    checkState('give.confirm');
    var giveButton = element(by.css("[ng-click=\"give.goToChange(give.amount, give.donor, give.email, give.program, give.view)\"]"));
    giveButton.click();
    checkState('give.change');
    var creditCardButton = element.all(by.model('give.dto.view')).get(1);
    expect(creditCardButton.getText()).toBe("Credit Card");
    creditCardButton.click();
    element(by.model('creditCard.ccNumber')).sendKeys("5555555555554444");
    element(by.model('creditCard.expDate')).sendKeys("0818");
    element(by.model('creditCard.cvc')).sendKeys("999");
    element(by.model('creditCard.billingZipCode')).sendKeys("45202-0818");
    var chgButton = element.all(by.css("[ng-click=\"give.submitChangedBankInfo()\"]")).get(0);
    expect(chgButton.getText()).toBe("GIVE $543.00");
    chgButton.click().then(function() {
      browser.waitForAngular();
      checkState('give.thank-you');
      var email = element.all(by.binding('give.email')).first();
      expect(email).toBeDefined();
      expect(email.getText()).toBe("sandi.ritter+protractor@ingagepartners.com");

      var amount = element.all(by.binding('give.amount')).first();
      expect(amount).toBeDefined();
      expect(amount.getText()).toBe("$543.00");

      var program = element.all(by.binding("give.program['Name']")).first();
      expect(program).toBeDefined();
      expect(program.getText()).toBe("Crossroads"); 
    });
  });

  it('Exsiting user logs in, gives, and logs out.  All information has been cleared out - testing give flow reset', function () {
    checkState('give.amount');
    element(by.model('amount')).sendKeys("99");
    element(by.binding('amount')).click();
    checkState('give.login');
    var loginButton = element.all(by.css('.btn')).get(6);
    expect(loginButton.getText()).toBe("Login");
    loginButton.click();
    element(by.id('login-page-email')).sendKeys("sandi.ritter+protractor@ingagepartners.com");
    element(by.id('login-page-password')).sendKeys("winter14");
    var button = element.all(by.id('submit_nav')).get(2);
    button.click();
    checkState('give.confirm');
    var giveButton = element(by.css("[ng-click=\"give.confirmDonation()\"]"));
    giveButton.click();

    var logoutButton = element.all(by.css(".navbar--login")).get(0).all(by.linkText('Sign Out'));
    logoutButton.click();
    //navigate to give and verify amount is null
    browser.get(env.baseUrl + '/#/give');
    checkState('give.amount');
    var amount = element.all(by.binding('give.amount')).first();
    expect(amount.getText()).toBe("GIVE");
    expect(amount.getAttribute('value')).toBe(null);
  });

  it('Existing user logs in, gives, and retruns to main give page.  All information has been cleared out - testing give flow reset', function () {
    checkState('give.amount');
    element(by.model('amount')).sendKeys("765");
    element(by.binding('amount')).click();
    checkState('give.login');
    var loginButton = element.all(by.css('.btn')).get(6);
    expect(loginButton.getText()).toBe("Login");
    loginButton.click();
    element(by.id('login-page-email')).sendKeys("sandi.ritter+protractor@ingagepartners.com");
    element(by.id('login-page-password')).sendKeys("winter14");
    var button = element.all(by.id('submit_nav')).get(2);
    button.click();
    checkState('give.confirm');
    var giveButton = element(by.css("[ng-click=\"give.confirmDonation()\"]"));
    giveButton.click();
    //navigate to give and verify amount is null
    browser.get(env.baseUrl + '/#/give');
    checkState('give.amount');
    var amount = element.all(by.binding('give.amount')).first();
    expect(amount.getText()).toBe("GIVE");
    expect(amount.getAttribute('value')).toBe(null);
  });

});


