var env = require("../environment");
var moment = require('moment');
var LoginPage = require('../pages/login_page.js');
var MyServePage = require('../pages/myserve_page.js');

describe("My Serve", function() {
  
  var loginPage = new LoginPage();
  var myServePage = new MyServePage();
  var displayName = 'Protractor'; 

  beforeEach(function(){
    loginPage.visitPage();
    loginPage.fillEmail("silbermm+28@gmail.com");
    loginPage.fillPassword("secured");
    loginPage.login();
  });

  afterEach(function(){ 
    loginPage.logout(); 
  });


  it("should display a checkmark icon for family members that responded YES", function(){
    expect(loginPage.getCurrentUser()).toBe(displayName); 
    myServePage.visitPage();
    
    var el = element(by.css(".serve-signup-cont > div > span > p"));
    expect(el.getText()).toBe("We're Sorry!  There are no serving opportunities available.  Please check back soon. "); 
  });


});
