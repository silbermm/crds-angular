var env = require("../environment");
var amountInput = element(by.model('give.amount'));

describe('Give as a guest-giver', function() {
  it('It should contain expected programs in the dropdown', function() {
    browser.get(env.baseUrl + '/#/give'); 
    element.all(by.repeater("program in programs")).then(function(rows){
        expect(rows[0].evaluate().getText()).toContain("Ministry Fund");
        expect(rows[1].evaluate().getText()).toContain("Game Change Fund");
        expect(rows[2].evaluate().getText()).toContain("Old St. George");
    });
  });

  it('It should reflect the amount in the button', function() {
    amountInput.sendKeys("122");
    amountInput.sendKeys(protractor.Key.TAB);
    amountInput.clear();
    expect(element(by.binding("give.amount")).getText()).toContain("122");
  });

  it('It should reject invalid entries', function() {
    amountInput.sendKeys("a");
    amountInput.sendKeys(protractor.Key.TAB);
    expect(amountInput.getAttribute("class")).toContain('ng-invalid-natural-number');
    amountInput.clear();
    amountInput.sendKeys("0");
    amountInput.sendKeys(protractor.Key.TAB);
    expect(amountInput.getAttribute("class")).toContain('ng-invalid-natural-number');
    amountInput.clear();
    amountInput.sendKeys("-1");
    amountInput.sendKeys(protractor.Key.TAB);
    expect(amountInput.getAttribute("class")).toContain('ng-invalid-natural-number');
    amountInput.clear();
    amountInput.sendKeys("0.2");
    amountInput.sendKeys(protractor.Key.TAB);
    expect(amountInput.getAttribute("class")).toContain('ng-invalid-natural-number');
    amountInput.clear();
    amountInput.sendKeys("2 2 3");
    amountInput.sendKeys(protractor.Key.TAB);
    expect(amountInput.getAttribute("class")).toContain('ng-invalid-natural-number');
  });

});