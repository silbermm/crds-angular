var env = require("../environment");

var MyServePage = (function() { 
  function MyServePage() {
    this.firstPanel = element(by.id("team-panel-000"));
    this.firstPanelPersonButton = this.firstPanel.element(by.buttonText('Lux'));
  }

  MyServePage.prototype.visitPage = function () {
    browser.get(env.baseUrl + '/#/serve-signup');
  }
  
  MyServePage.prototype.openFirstPanel = function () {
    this.firstPanelPersonButton.click();   
  }

  MyServePage.prototype.chooseOpportunityAndFrequency = function () {
    
  }
  return MyServePage; 
})();

module.exports = MyServePage;
