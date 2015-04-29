var env = require("../environment");

describe('Giving Flow', function() {
  
  beforeEach(function() {
    browser.get(env.baseUrl + '/#/give');
  })
  
  // it('should follow full credit card flow, logging in as user', function () {
  //   expect(browser.getCurrentUrl()).toMatch(/\/amount/);
  //   element(by.model('amount')).sendKeys("12345");
  //   element(by.binding('amount')).click();
  //   expect(browser.getCurrentUrl()).toMatch(/\/login/);
  //   var loginButton = element.all(by.css('.btn')).get(5);
  //   expect(loginButton.getText()).toBe("LOGIN");
  //   loginButton.click();
  //   element(by.id('login-page-email')).sendKeys("tim@kriz.net");
  //   element(by.id('login-page-password')).sendKeys("password");
  //   var button = element.all(by.id('submit_nav')).get(2);
  //   button.click();
  //   expect(browser.getCurrentUrl()).toMatch(/\/account/); 
  //   var giveAsGuestButton = element.all(by.model('give.view')).get(1);
  //   expect(giveAsGuestButton.getText()).toBe("CREDIT CARD");
  //   giveAsGuestButton.click();
  //   element(by.model('give.nameOnCard')).sendKeys("Mr Cross Roads");
  //   element(by.model('give.ccNumber')).sendKeys("4242424242424242");
  //   element(by.model('give.expDate')).sendKeys("0118");
  //   element(by.model('give.cvc')).sendKeys("654");
  //   element(by.model('give.billingZipCode')).sendKeys("45202-5236");
  //   var giveButton = element.all(by.css('.btn')).get(8);
  //   expect(giveButton.getText()).toBe("GIVE $12,345.00");
  //   giveButton.click();
  //   expect(browser.getCurrentUrl()).toMatch(/\/thank-you/);
  //   var email = element.all(by.binding('give.email')).first();
  //   expect(email.getText()).toBe("tim@kriz.net");
  //   var logoutButton = element.all(by.css(".navbar--login")).get(0).all(by.linkText('Sign Out'));
  //   logoutButton.click();    
  // })
  
  // it('should follow full bank account flow, giving as guest', function () {
  //   expect(browser.getCurrentUrl()).toMatch(/\/amount/);
  //   element(by.model('amount')).sendKeys("1999");
  //   element(by.model('program')).sendKeys("Ministry");
  //   element(by.binding('amount')).click();
  //   expect(browser.getCurrentUrl()).toMatch(/\/login/);
  //   var giveAsGuestButton = element.all(by.css('.btn')).get(6);
  //   expect(giveAsGuestButton.getText()).toBe("GIVE AS GUEST");
  //   giveAsGuestButton.click();
  //   element(by.id('give-email')).sendKeys("cross@roads.net");
  //   element(by.model('routing')).sendKeys("042000314");
  //   element(by.model('account')).sendKeys("9876543210");
  //   var giveButton = element.all(by.css('.btn')).get(9);
  //   expect(giveButton.getText()).toBe("GIVE $1,999.00");
  //   giveButton.click();
  //   expect(browser.getCurrentUrl()).toMatch(/\/thank-you/);  
  //   var email = element.all(by.binding('give.email')).first();
  //   expect(email.getText()).toBe("cross@roads.net");
  // })

  it('should register as new user and not lose the amt or fund', function () {
    expect(browser.getCurrentUrl()).toMatch(/\/amount/);
    element(by.model('amount')).sendKeys("867539");
    expect(element(by.binding("amount")).getText()).toContain("GIVE $867,539.00");
    element(by.binding('amount')).click();
    expect(browser.getCurrentUrl()).toMatch(/\/login/);
    var regButton = element(by.linkText('create an account'));
    regButton.click();
    expect(browser.getCurrentUrl()).toMatch(/\/register/); 
    element.all(by.id('registration-firstname')).get(1).sendKeys("Jack");
    element.all(by.id('registration-lastname')).get(1).sendKeys("Protractor");
    var ranNum = Math.floor((Math.random() * 1000) + 1);
    element.all(by.id('registration-email')).get(2).sendKeys("updates+" +ranNum+ "@crossroads.net");
    element.all(by.id('registration-password')).get(2).sendKeys("protractor");
    var regButton = element.all(by.css('.btn')).get(5);
    regButton.click();
    expect(browser.getCurrentUrl()).toMatch(/\/account/);
    element(by.cssContainingText('.ng-binding', 'Ministry'));
    element(by.cssContainingText('.ng-binding', '$867,539.00'));
    
  })
   
})