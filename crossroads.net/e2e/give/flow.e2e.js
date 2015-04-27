var env = require("../environment");

describe('Giving Flow', function() {
  
  beforeEach(function() {
    browser.get(env.baseUrl + '/#/give');
  })
  
  it('should follow full credit crdflow, logging in as user', function () {
    expect(browser.getCurrentUrl()).toMatch(/\/amount/);
    element(by.model('give.amount')).sendKeys("12345");
    element(by.model('give.program')).sendKeys("Ministry");
    element(by.binding('give.amount')).click();
    //element(by.model('give.program')).sendKeys("Ministry");
    expect(browser.getCurrentUrl()).toMatch(/\/login/);
    var loginButton = element.all(by.css('.btn')).get(5);
    expect(loginButton.getText()).toBe("LOGIN");
    loginButton.click();
    element(by.id('login-page-email')).sendKeys("tim@kriz.net");
    element(by.id('login-page-password')).sendKeys("password");
    var button = element.all(by.id('submit_nav')).get(2);
    button.click();
    expect(browser.getCurrentUrl()).toMatch(/\/account/); 
    var giveAsGuestButton = element.all(by.model('give.view')).get(1);
    expect(giveAsGuestButton.getText()).toBe("CREDIT CARD");
    giveAsGuestButton.click();
    element(by.model('give.nameOnCard')).sendKeys("Mr Cross Roads");
    element(by.model('give.ccNumber')).sendKeys("4242424242424242");
    element(by.model('give.expDate')).sendKeys("0118");
    element(by.model('give.cvc')).sendKeys("654");
    element(by.model('give.billingZipCode')).sendKeys("45202-5236");
    var giveButton = element.all(by.css('.btn')).get(8);
    expect(giveButton.getText()).toBe("GIVE $12,345.00");
    giveButton.click();
    expect(browser.getCurrentUrl()).toMatch(/\/thank-you/);
    var email = element.all(by.binding('give.email')).first();
    expect(email.getText()).toBe("tim@kriz.net");
    var logoutButton = element.all(by.css(".navbar--login")).get(0).all(by.linkText('Sign Out'));
    logoutButton.click();    
  })
  
  it('should follow full bank account flow, giving as guest', function () {
    expect(browser.getCurrentUrl()).toMatch(/\/amount/);
    element(by.model('give.amount')).sendKeys("1999");
    element(by.model('give.program')).sendKeys("Ministry");
    element(by.binding('give.amount')).click();
    expect(browser.getCurrentUrl()).toMatch(/\/login/);
    var giveAsGuestButton = element.all(by.css('.btn')).get(6);
    expect(giveAsGuestButton.getText()).toBe("GIVE AS GUEST");
    giveAsGuestButton.click();
    element(by.id('give-email')).sendKeys("cross@roads.net");
    element(by.model('routing')).sendKeys("042000314");
    element(by.model('account')).sendKeys("9876543210");
    var giveButton = element.all(by.css('.btn')).get(9);
    expect(giveButton.getText()).toBe("GIVE $1,999.00");
    giveButton.click();
    expect(browser.getCurrentUrl()).toMatch(/\/thank-you/);  
    var email = element.all(by.binding('give.email')).first();
    expect(email.getText()).toBe("cross@roads.net");
  })
   
})