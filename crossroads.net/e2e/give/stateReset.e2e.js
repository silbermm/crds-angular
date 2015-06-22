var env = require("../environment");

describe('Giving Flow State', function() {
  beforeEach(function() {
    browser.get(env.baseUrl + '/#/give');
  })

  afterEach(function() {
    var logoutButton = element.all(by.css(".navbar--login")).get(0).all(by.linkText('Sign Out'));
    logoutButton.click();
  });

  it('should be reset after logging in as existing giver, clicking change, then refreshing the browser', function() {
    expect(browser.getCurrentUrl()).toMatch(/\/amount/);

    element(by.model('amount')).sendKeys("1999");
    element(by.binding('amount')).click();
    expect(browser.getCurrentUrl()).toMatch(/\/login/);

    element(by.id('login-page-email')).sendKeys("tim@kriz.net");
    element(by.id('login-page-password')).sendKeys("password");

    var button = element.all(by.id('submit_nav')).get(2);
    button.click();
    expect(browser.getCurrentUrl()).toMatch(/\/confirm/);

    var giveButton = element(by.css("[ng-click=\"give.goToChange(give.amount, give.donor, give.email, give.program, 'cc')\"]"));
    giveButton.click();
    expect(browser.getCurrentUrl()).toMatch(/\/change/);

    var creditCardButton = element.all(by.model('give.dto.view')).get(1);
    expect(creditCardButton.getText()).toBe("Credit Card");
    creditCardButton.click();

    browser.navigate().refresh();
    expect(browser.getCurrentUrl()).toMatch(/\/amount/);
    expect(element(by.model('amount')).getText()).toBe('');
  });

  it('should be reset after giving as existing giver, logging out, then returning to give again', function() {
    expect(browser.getCurrentUrl()).toMatch(/\/amount/);

    element(by.model('amount')).sendKeys("12345");
    element(by.binding('amount')).click();
    expect(browser.getCurrentUrl()).toMatch(/\/login/);

    element(by.id('login-page-email')).sendKeys("tim@kriz.net");
    element(by.id('login-page-password')).sendKeys("password");

    var button = element.all(by.id('submit_nav')).get(2);
    button.click();
    expect(browser.getCurrentUrl()).toMatch(/\/confirm/);

    var giveButton = element(by.css("[ng-click=\"give.confirmDonation()\"]"));
    expect(giveButton.getText()).toBe("GIVE $12,345.00");
    giveButton.click();
    expect(browser.getCurrentUrl()).toMatch(/\/thank-you/);

    var logoutButton = element.all(by.css(".navbar--login")).get(0).all(by.linkText('Sign Out'));
    logoutButton.click();
    expect(browser.getCurrentUrl()).toMatch(/\/amount/);

    browser.navigate().to(env.baseUrl + '/#/give').then(function() {
      expect(browser.getCurrentUrl()).toMatch(/\/amount/);
      expect(element(by.model('amount')).getText()).toBe('');
    });
  });

  it('should be reset after navigating to confirmation page as existing giver, then clicking refresh', function() {
    expect(browser.getCurrentUrl()).toMatch(/\/amount/);

    element(by.model('amount')).sendKeys("12345");

    element(by.model('ministryShow')).click();
    // This selects 'Game Change'
    element(by.model('program')).$('[value="26"]').click();

    element(by.binding('amount')).click();
    expect(browser.getCurrentUrl()).toMatch(/\/login/);

    element(by.id('login-page-email')).sendKeys("tim@kriz.net");
    element(by.id('login-page-password')).sendKeys("password");

    element.all(by.id('submit_nav')).get(2).click();
    expect(browser.getCurrentUrl()).toMatch(/\/confirm/);

    var program = element(by.binding('give.program'));
    expect(program.getText()).toBe('Game Change');

    var giveButton = element(by.css("[ng-click=\"give.confirmDonation()\"]"));
    expect(giveButton.getText()).toBe("GIVE $12,345.00");

    browser.navigate().refresh().then(function() {
      expect(browser.getCurrentUrl()).toMatch(/\/amount/);
      expect(element(by.model('amount')).getText()).toBe('');
      element(by.model('amount')).sendKeys("56789");

      element(by.binding('amount')).click();
      expect(browser.getCurrentUrl()).toMatch(/\/confirm/);

      expect(element(by.binding('give.amount')).getText()).toBe('$56,789.00');
      expect(element(by.binding('give.program')).getText()).toBe('Crossroads');
    });
  });

  it('should be reset after successfully giving as existing giver, then clicking refresh', function() {
    expect(browser.getCurrentUrl()).toMatch(/\/amount/);

    element(by.model('amount')).sendKeys("12345");

    element(by.model('ministryShow')).click();
    // This selects 'Game Change'
    element(by.model('program')).$('[value="26"]').click();

    element(by.binding('amount')).click();
    expect(browser.getCurrentUrl()).toMatch(/\/login/);

    element(by.id('login-page-email')).sendKeys("tim@kriz.net");
    element(by.id('login-page-password')).sendKeys("password");

    element.all(by.id('submit_nav')).get(2).click();
    expect(browser.getCurrentUrl()).toMatch(/\/confirm/);

    var program = element(by.binding('give.program'));
    expect(program.getText()).toBe('Game Change');

    var giveButton = element(by.css("[ng-click=\"give.confirmDonation()\"]"));
    expect(giveButton.getText()).toBe("GIVE $12,345.00");

    giveButton.click();
    expect(browser.getCurrentUrl()).toMatch(/\/thank-you/);

    browser.navigate().refresh().then(function() {
      expect(browser.getCurrentUrl()).toMatch(/\/amount/);
      expect(element(by.model('amount')).getText()).toBe('');
    });
  });

  it('should be reset after successfully giving as guest and returning to give again', function() {
    expect(browser.getCurrentUrl()).toMatch(/\/amount/);

    element(by.model('amount')).sendKeys("1999");
    element(by.binding('amount')).click();
    expect(browser.getCurrentUrl()).toMatch(/\/login/);

    var giveAsGuestButton = element.all(by.css('[type=button][href="#/give/account"]')).get(0);
    expect(giveAsGuestButton.getText()).toBe("Give as Guest");
    giveAsGuestButton.click();

    var creditCardButton = element.all(by.model('give.dto.view')).get(1);
    expect(creditCardButton.getText()).toBe("Credit Card");
    creditCardButton.click();

    element(by.id('give-email')).sendKeys("tim@kriz.net");
    element(by.model('creditCard.nameOnCard')).sendKeys("Mr Cross Roads");
    element(by.model('creditCard.ccNumber')).sendKeys("4242424242424242");
    element(by.model('creditCard.expDate')).sendKeys("0118");
    element(by.model('creditCard.cvc')).sendKeys("654");
    element(by.model('creditCard.billingZipCode')).sendKeys("45202-5236");

    var giveButton = element.all(by.css("[ng-click=\"give.submitBankInfo()\"]")).get(0);
    expect(giveButton.getText()).toBe("GIVE $1,999.00");

    giveButton.click().then(function() {
      expect(browser.getCurrentUrl()).toMatch(/\/thank-you/);
      var email = element.all(by.binding('give.email')).first();
      expect(email.getText()).toBe("tim@kriz.net");
    });

    // make sure state is cleared and we are taken back to the amount page instead of account
    browser.navigate().back();
    expect(browser.getCurrentUrl()).toMatch(/\/amount/);
    element(by.model('amount')).sendKeys("1999");

    element(by.binding('amount')).click();
    expect(browser.getCurrentUrl()).toMatch(/\/login/);

    giveAsGuestButton = element.all(by.css('[type=button][href="#/give/account"]')).get(0);
    expect(giveAsGuestButton.getText()).toBe("Give as Guest");
    giveAsGuestButton.click();

    creditCardButton = element.all(by.model('give.dto.view')).get(1);
    expect(creditCardButton.getText()).toBe("Credit Card");
    creditCardButton.click();

    expect(element(by.id('give-email')).getText()).toBe('');
    expect(element(by.model('creditCard.nameOnCard')).getText()).toBe('');
    expect(element(by.model('creditCard.ccNumber')).getText()).toBe('');
    expect(element(by.model('creditCard.expDate')).getText()).toBe('');
    expect(element(by.model('creditCard.cvc')).getText()).toBe('');
    expect(element(by.model('creditCard.billingZipCode')).getText()).toBe('');
  });

  it('should be reset after navigating to register page, entering info, and then clicking refresh', function() {
    expect(browser.getCurrentUrl()).toMatch(/\/amount/);

    element(by.model('amount')).sendKeys("1999");
    element(by.binding('amount')).click();
    expect(browser.getCurrentUrl()).toMatch(/\/login/);

    element(by.linkText('Create an account')).click();
    expect(browser.getCurrentUrl()).toMatch(/\/register/);

    element.all(by.id('registration-firstname')).get(1).sendKeys("Jack");
    element.all(by.id('registration-lastname')).get(1).sendKeys("Protractor");
    var ranNum = Math.floor((Math.random() * 1000) + 1);
    element.all(by.id('registration-email')).get(4).sendKeys("updates+" +ranNum+ "@crossroads.net");
    element.all(by.id('registration-password')).get(2).sendKeys("protractor");

    browser.navigate().refresh();
    expect(browser.getCurrentUrl()).toMatch(/\/amount/);

    expect(element(by.model('amount')).getText()).toBe('');
    element(by.model('amount')).sendKeys("1999");
    element(by.binding('amount')).click();
    expect(browser.getCurrentUrl()).toMatch(/\/login/);

    element(by.linkText('Create an account')).click();
    expect(browser.getCurrentUrl()).toMatch(/\/register/);

    expect(element.all(by.id('registration-firstname')).get(1).getText()).toBe('');
    expect(element.all(by.id('registration-lastname')).get(1).getText()).toBe('');
    expect(element.all(by.id('registration-email')).get(4).getText()).toBe('');
    expect(element.all(by.id('registration-password')).get(2).getText()).toBe('');
  });
});
