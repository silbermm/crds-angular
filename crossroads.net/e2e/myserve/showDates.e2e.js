require('jasmine-given');

var env = require("../environment");

var random = Math.floor((Math.random() * 100000) + 1);

var correctEmail = "lakshmi.maramraju@gmail.com";
var changeEmail = "luks" + random + "@gmail.com";

describe("My Serve", function() {

  beforeEach(function(){
    browser.get(env.baseUrl + '/#/');
    var loginButton = element.all(by.css(".navbar--login")).get(0).all(by.buttonText('Login'));
    expect(loginButton.count()).toBe(2);
    expect(loginButton.get(0).isDisplayed()).toBeTruthy();
    loginButton.get(0).click();

    var emailInput = element.all(by.model('credentials.username')).get(0);
    var passwordInput = element.all(by.model("passwd")).get(0);
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

  
  it("should show the frequency select dropdown", function(){  
    expect(element(by.id("current-user")).getText()).toBe("Laks");
    browser.get(env.baseUrl + "/#/serve-signup");
    var panel = element(by.id("team-panel-000"));
    panel.element(by.buttonText('Lux')).click();
    var radioBtn = panel.all(by.model("currentMember.currentOpportunity"));
    expect(radioBtn.isDisplayed()).toBeTruthy();
    radioBtn.get(0).click();
    var signedupBtn = panel.all(by.model("currentMember.currentOpportunity.signedup"));
    expect(signedupBtn.isDisplayed()).toBeTruthy();
    signedupBtn.get(0).click();  
    
  });
});
