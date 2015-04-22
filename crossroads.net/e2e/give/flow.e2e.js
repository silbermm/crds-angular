var env = require("../environment");

describe('Giving Flow', function() {
  
  beforeEach(function() {
    browser.get(env.baseUrl + '/#/give');
  })
  
  it('should flow between amount entry, account entry and confirmation on pressing Give button', function () {
    expect(browser.getCurrentUrl()).toMatch(/\/amount/);
    element(by.model('give.amount')).sendKeys("1234");
    element(by.binding('give.amount')).click();
    expect(browser.getCurrentUrl()).toMatch(/\/account/);
    var giveAsGuestButton = element.all(by.css('.btn')).get(6);
    expect(giveAsGuestButton.getText()).toBe("GIVE AS GUEST");
    giveAsGuestButton.click();
    element(by.id('give-email')).sendKeys("tim@kriz.net");
    element(by.model('give.routing')).sendKeys("042000314");
    element(by.model('give.account')).sendKeys("9876543210");
    var giveButton = element.all(by.css('.btn')).get(9);
    expect(giveButton.getText()).toBe("GIVE $1,234.00");
    giveButton.click();
    expect(browser.getCurrentUrl()).toMatch(/\/thank-you/);
  })
  
  it('should be able to switch Give as Guest to login', function () {
    expect(browser.getCurrentUrl()).toMatch(/\/amount/);
    element(by.model('give.amount')).sendKeys("1");
    element(by.binding('give.amount')).click();
    expect(browser.getCurrentUrl()).toMatch(/\/account/);
    var loginButton = element.all(by.css('.btn')).get(5);
    expect(loginButton.getText()).toBe("LOGIN");
    loginButton.click();
    expect(browser.getCurrentUrl()).toMatch(/\/login/);    
  })
  
  it('should be able to login in to give', function () {
    expect(browser.getCurrentUrl()).toMatch(/\/amount/);
    element(by.model('give.amount')).sendKeys("1");
    element(by.binding('give.amount')).click();
    expect(browser.getCurrentUrl()).toMatch(/\/account/);
    var loginButton = element.all(by.css('.btn')).get(5);
    expect(loginButton.getText()).toBe("LOGIN");
    loginButton.click();
    element(by.id('login-page-email')).sendKeys("tim@kriz.net");
    element(by.id('login-page-password')).sendKeys("123456");
    var button = element.all(by.id('submit_nav')).get(0);
    //select login button
    //enter information
    //expect(browser.getCurrentUrl()).toMatch(/\/account/);
  })
})