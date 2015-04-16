var env = require("../environment");
var moment = require('moment');
var EC = protractor.ExpectedConditions; 
describe('Serve Page', function() {

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

  it("should display success message", function(){
    expect(element(by.id("current-user")).getText()).toBe("Laks");
    browser.get(env.baseUrl + "/#/serve-signup");
    var panel = element(by.id("team-panel-000"));
    panel.element(by.buttonText("Lux")).click();
    expect(panel.element(by.css(".panel-collapse")).getAttribute("class")).toMatch("in");
    
    var radioBtn = panel.all(by.model("currentMember.serveRsvp.roleId"));
    expect(radioBtn.isDisplayed()).toBeTruthy();
    radioBtn.get(0).click();
    
    var signedupBtn = panel.all(by.model("currentMember.serveRsvp.attending"));
    expect(signedupBtn.isDisplayed()).toBeTruthy();
    signedupBtn.get(0).click(); 

    panel.all(by.tagName('option')).then(function(options){
      options[1].click();
      panel.element(by.buttonText("Save")).click(); 
    });
    
    // Right now we can't test the growl messages with protractor... 
    // see https://github.com/JanStevens/angular-growl-2/issues/81 for more info, but basically,
    // the library is using a $timeout for the message with protractor waits for before moving on
    // to the expectation. The expectation fails because by the time it runs the message is gone.
    //
    // Leave this code so when it is fixed, we can uncomment and have a passing test...

    /*var growl = element(by.binding("message.title"));*/
    /*expect(growl.textValue).toBe('Great!  You have successfully updated your serve information');   */
  });

});
