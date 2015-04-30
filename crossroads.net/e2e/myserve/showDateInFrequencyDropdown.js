var env = require("../environment");
var moment = require('moment');
var LoginPage = require('../pages/login_page.js');
var MyServePage = require('../pages/myserve_page.js');

describe("My Serve", function() {

  var loginPage = new LoginPage();
  var myServePage = new MyServePage();
  var displayName = 'Laks';

  beforeEach(function() {
    loginPage.visitPage();
    loginPage.fillEmail("lakshmi.maramraju@gmail.com");
    loginPage.fillPassword("123456");
    loginPage.login();
  });

  afterEach(function() {
    loginPage.logout();
  });

  //this keeps timinig out...
  it("should display date after ONCE in dropdown list position 1", function() {
    expect(loginPage.getCurrentUser()).toBe(displayName);
    myServePage.visitPage();

    var panel = element(by.id("team-panel-000"));
    panel.element(by.buttonText('Lux')).click();

    var el = element(by.model("currentMember.currentOpportunity.frequency"));
    var options = el.all(by.tagName('option'));
    el.all(by.tagName('option')).then(function(optoins) {
      expect(options[1].getText()).toBe("Once");
    })

    // el.all(by.tagName('option')).then(function(options){
    //   options[1].click();
    //   panel.element(by.buttonText("Save")).click();
    // });

    // var options = el.findElements(by.tagName('option'))
    // .then(function(options) {
    //   options[1].click();
    // });
    // expect(op.getText()).toBe("Once");


    //   var selectDropdownbyNum = function ( element, optionNum ) {
    //   if (optionNum){
    //     var options = element.findElements(by.tagName('option'))
    //       .then(function(options){
    //         options[optionNum].click();
    //       });
    //   }
    // };
    //   expect(el.getText()).toBe("We're Sorry!  There are no serving opportunities available.  Please check back soon. ");
  });


});
