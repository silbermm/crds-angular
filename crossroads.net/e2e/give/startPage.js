var env = require("../environment");
var amountInput = element(by.css('#amount'));

describe('Give as a guest-giver', function() {
  it('It should contain expected programs in the dropdown', function() {
    browser.get(env.baseUrl + '/#/give'); 
    expect(element(by.css("#programs")).getText()).toContain("Ministry Fund");
    expect(element(by.css("#programs")).getText()).toContain("Game Change Fund");
    expect(element(by.css("#programs")).getText()).toContain("Old St. George");
  });

  it('It should reflect the amount in the button', function() {
    amountInput.sendKeys("122");
    amountInput.sendKeys(protractor.Key.TAB);
    amountInput.clear();
    expect(element(by.css("#giveButton")).getText()).toContain("122");
  });

  it('It should reject invalid entries', function() {
    amountInput.sendKeys("a");
    amountInput.sendKeys(protractor.Key.TAB);
    expect(element(by.css("#giveForm")).getAttribute("class")).toContain('ng-invalid-natural-number');
    amountInput.clear();
    amountInput.sendKeys("0");
    amountInput.sendKeys(protractor.Key.TAB);
    expect(element(by.css("#giveForm")).getAttribute("class")).toContain('ng-invalid-natural-number');
    amountInput.clear();
    amountInput.sendKeys("-1");
    amountInput.sendKeys(protractor.Key.TAB);
    expect(element(by.css("#giveForm")).getAttribute("class")).toContain('ng-invalid-natural-number');
    amountInput.clear();
    amountInput.sendKeys("0.2");
    amountInput.sendKeys(protractor.Key.TAB);
    expect(element(by.css("#giveForm")).getAttribute("class")).toContain('ng-invalid-natural-number');
    amountInput.clear();
    amountInput.sendKeys("2 2 3");
    amountInput.sendKeys(protractor.Key.TAB);
    expect(element(by.css("#giveForm")).getAttribute("class")).toContain('ng-invalid-natural-number');
  });

});