var env = require("../environment");
var moment = require('moment');
var LoginPage = require('../pages/login_page.js');
var MyServePage = require('../pages/myserve_page.js');
describe('My Serve Page', function() {
  
  var loginPage = new LoginPage();
  var myServePage = new MyServePage();
  var displayName = 'protractor'; 

  beforeEach(function(){
    loginPage.visitPage();
    loginPage.fillEmail("silbermm+23@gmail.com");
    loginPage.fillPassword("secured");
    loginPage.login();
  });

  afterEach(function(){ 
    loginPage.logout(); 
  });

  it("should display a checkmark icon for family members that responded YES", function(){
    expect(loginPage.getCurrentUser()).toBe(displayName); 
    myServePage.visitPage();
    myServePage.openFirstPanel(displayName);
    myServePage.rsvp();
  });

  it("should display a cancel icon for family members that responded NO", function(){
    expect(loginPage.getCurrentUser()).toBe(displayName);
    myServePage.visitPage();
  });

  it("should not display an icon for family members that haven't responded", function(){
    expect(loginPage.getCurrentUser()).toBe(displayName);
    myServePage.visitPage();
  });

});
