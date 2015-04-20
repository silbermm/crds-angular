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
    var svg = myServePage.personIcon(displayName);
    expect(svg.element(by.css('svg.icon-check-circle')).isDisplayed()).toBeTruthy();
  });

});
