var env = require("../environment");
var moment = require('moment');

describe('Crossroads App', function() {

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


  it('should go the serve signup page', function() { 
    expect(element(by.id("current-user")).getText()).toBe("Laks");  
    browser.get(env.baseUrl + "/#/serve-signup");
    expect(element.all(by.css(".page-header")).get(0).getText()).toBe("Sign Up To Serve");
    var today = moment();
    element.all(by.css(".serve-day")).then(function(days){
      expect(days.length).toBeGreaterThan(0);
      var onPage = days[0].element(by.css('h4')).getText();
      var onPageDate = moment(onPage, 'EEEE, MMMM dd, yyyy');
      expect(onPageDate.dayOfYear()).toBeGreaterThan(today.dayOfYear() - 1);
    });
  }); 

  it("should display the time correctly", function(){
    expect(element(by.id("current-user")).getText()).toBe("Laks");  
    browser.get(env.baseUrl + "/#/serve-signup");
    element.all(by.css(".serve-day")).then(function(days){
      days[0].all(by.css('.serve-day-time')).then(function(tabs){
        expect(tabs[0].element(by.binding('opportunity.time')).getText())
          .toMatch(/^[1]?[0-9]:[\d]{2,2}(am|pm)$/);
      });
    });
  });

  it("should open the panel when a user is clicked", function(){
    expect(element(by.id("current-user")).getText()).toBe("Laks");  
    browser.get(env.baseUrl + "/#/serve-signup");
    var panel = element(by.id("team-panel-000"));
    panel.element(by.css(".btn")).click();
    expect(panel.element(by.css(".panel-collapse")).getAttribute("class")).toMatch("in");
  });

  it("should apply the correct CSS to opportunities", function() {
    expect(element(by.id("current-user")).getText()).toBe("Laks");
    browser.get(env.baseUrl + "/#/serve-signup");
    var panel = element(by.id("team-panel-000"));
    panel.element(by.css(".btn")).click();
    expect(panel.element(by.css(".panel-collapse")).getAttribute("class")).toMatch("in");

    var opp = panel.all(by.css(".radio")).get(0);
    if(opp.all(by.css(".label")).get(0).textValue === 'Full'){ 
      console.log("element label was equal to FULL");
      expect(opp.getAttribute('class')).toContain('text-muted');
    } else {
      expect(opp.getAttribute('class')).not.toContain('text-muted');
    }
  });


});
