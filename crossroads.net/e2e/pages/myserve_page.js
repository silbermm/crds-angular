var env = require("../environment");

var MyServePage = (function() { 
  function MyServePage() {
    this.firstPanel = element(by.id("team-panel-000"));
  }

  MyServePage.prototype.visitPage = function () {
    browser.get(env.baseUrl + '/#/serve-signup');
  }
  
  MyServePage.prototype.openFirstPanel = function (buttonName) {
    this.firstPanel.element(by.buttonText(buttonName)).click();
    expect(this.firstPanel.element(by.css(".panel-collapse")).getAttribute("class")).toMatch("in");
  }

  MyServePage.prototype.rsvp = function () {
    var radioBtn = this.firstPanel.all(by.model("currentMember.serveRsvp.roleId"));
    expect(radioBtn.isDisplayed()).toBeTruthy();
    radioBtn.get(0).click();
    
    var signedupBtn = this.firstPanel.all(by.model("currentMember.serveRsvp.attending"));
    expect(signedupBtn.isDisplayed()).toBeTruthy();
    signedupBtn.get(0).click(); 

    this.firstPanel.all(by.tagName('option')).then(function(options){
      options[0].click();
    });
    this.firstPanel.element(by.buttonText("Save")).click(); 
  }
  return MyServePage; 
})();

module.exports = MyServePage;
