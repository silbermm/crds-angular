var env = require("../environment");
var moment = require('moment');

var random = Math.floor((Math.random() * 100000) + 1);

var correctEmail = "lakshmi.maramraju@gmail.com";
var changeEmail = "luks" + random + "@gmail.com";

describe('Edit profile on myserve page', function() {

  beforeEach(function(){
    browser.get(env.baseUrl + '/#/');
    var loginButton = element.all(by.css(".navbar--login")).get(0).all(by.buttonText('Login'));
    expect(loginButton.count()).toBe(2);
    expect(loginButton.get(0).isDisplayed()).toBeTruthy();
    loginButton.get(0).click();

    var emailInput = element.all(by.css(".navbar--login")).get(0).element(by.id("login-dropdown-email"));
    var passwordInput = element.all(by.css(".navbar--login")).get(0).element(by.id("login-dropdown-password"));
    var submitBtn = element.all(by.css(".navbar--login")).get(0).all(by.buttonText("Login")).get(1);

    emailInput.sendKeys("lakshmi.maramraju@gmail.com");
    passwordInput.sendKeys("123456");
    submitBtn.click();
  });

  afterEach(function(){
    var loginButton = element.all(by.css(".navbar--login")).get(0).all(by.buttonText('Login'));
    var logoutButton = element.all(by.css(".navbar--login")).get(0).all(by.linkText('Sign Out'));
    logoutButton.click();
    expect(loginButton.get(0).isDisplayed()).toBeTruthy();
  });

  it("should allow profile edit for logged in user", function(){
    expect(element(by.id("current-user")).getText()).toBe("Laks");
    browser.get(env.baseUrl + "/#/serve-signup");
    var panel = element(by.id("team-panel-000"));
    panel.element(by.buttonText('Lux')).click();
    panel.element(by.css(".edit-btn")).click();

    //var profileModal = element(by.css(".email-modal"));
    var email = element(by.id("account-page-email"));
    email.clear();
    email.sendKeys(changeEmail); 
    element(by.id("save-personal")).click(); 
    panel.element(by.buttonText('Lux')).click();
    var currentEmail2 = panel.all(by.binding("currentMember.emailAddress"));
    expect(currentEmail2.getText()).toBe(changeEmail);

  });

});
