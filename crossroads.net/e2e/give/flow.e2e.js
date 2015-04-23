var env = require("../environment");

describe('Giving Flow', function() {
  
  beforeEach(function() {
    browser.get(env.baseUrl + '/#/give');
  })
  
  it('should flow between amount entry, account entry and confirmation on pressing Give button', function () {
    expect(browser.getCurrentUrl()).toMatch(/\/amount/);
    element(by.model('give.amount')).sendKeys("12345");
    element(by.binding('give.amount')).click();
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
    var logoutButton = element.all(by.css(".navbar--login")).get(0).all(by.linkText('Sign Out'));
    logoutButton.click();    
  })
  
  it('should be able to switch login to Give as Guest', function () {
    expect(browser.getCurrentUrl()).toMatch(/\/amount/);
    element(by.model('give.amount')).sendKeys("1999");
    element(by.binding('give.amount')).click();
    expect(browser.getCurrentUrl()).toMatch(/\/login/);
    var giveAsGuestButton = element.all(by.css('.btn')).get(6);
    expect(giveAsGuestButton.getText()).toBe("GIVE AS GUEST");
    giveAsGuestButton.click();
    element(by.id('give-email')).sendKeys("tim@kriz.net");
    element(by.model('give.routing')).sendKeys("042000314");
    element(by.model('give.account')).sendKeys("9876543210");
    var giveButton = element.all(by.css('.btn')).get(9);
    expect(giveButton.getText()).toBe("GIVE $1,999.00");
    giveButton.click();
    expect(browser.getCurrentUrl()).toMatch(/\/thank-you/);      
  })
  
  it('should be able to login in to give', function () {
    expect(browser.getCurrentUrl()).toMatch(/\/amount/);
    element(by.model('give.amount')).sendKeys("1");
    element(by.binding('give.amount')).click();
    expect(browser.getCurrentUrl()).toMatch(/\/login/);
    var loginButton = element.all(by.css('.btn')).get(5);
    expect(loginButton.getText()).toBe("LOGIN");
    loginButton.click();
    element(by.id('login-page-email')).sendKeys("tim@kriz.net");
    element(by.id('login-page-password')).sendKeys("password");
    var button = element.all(by.id('submit_nav')).get(2);
    button.click();
    expect(browser.getCurrentUrl()).toMatch(/\/account/); 
  })
})