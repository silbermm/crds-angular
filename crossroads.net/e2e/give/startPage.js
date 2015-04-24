var env = require("../environment");
var amountInput = element(by.model('give.amount'));

describe('Give as a guest-giver', function() {
  it('It should contain expected programs in the dropdown', function() {
    browser.get(env.baseUrl + '/#/give/amount'); 
    element.all(by.options("program.Name for program in programs track by program.ProgramId")).then(function(rows){
        expect(rows[1].evaluate().getText()).toContain("Ministry");
        expect(rows[0].evaluate().getText()).toContain("Gamechange Campaign");
        expect(rows[2].evaluate().getText()).toContain("Old St George - Clifton");
    });
  });

  it('It should reflect the amount in the button', function() {
    amountInput.sendKeys("122");
    amountInput.sendKeys(protractor.Key.TAB);
    expect(element(by.binding("giveForm.giveForm.amount.$modelValue")).getText()).toContain("122");
  });

  it('It should reject invalid entries', function() {
    amountInput.clear();
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