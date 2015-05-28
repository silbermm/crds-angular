var env = require("../environment");
var amountInput = element(by.model('amount'));

describe('Give as a guest-giver', function() {
  it('It should contain expected programs in the dropdown', function() {
    browser.get(env.baseUrl + '/#/give/amount');
    element.all(by.options("program.Name for program in programs.slice(1) track by program.ProgramId")).then(function(rows){
        expect(rows[0].evaluate().getInnerHtml()).toContain("Choose Initiative");
        expect(rows[1].evaluate().getInnerHtml()).toContain("Game Change Campaign");
        expect(rows[2].evaluate().getInnerHtml()).toContain("Old St George Building");
    });
  });

  it('It should reflect the amount in the button', function() {
    amountInput.sendKeys("1224");
    amountInput.sendKeys(protractor.Key.TAB);
    expect(element(by.binding("amount")).getText()).toContain("GIVE $1,224.00");
  });


  it('It should reject invalid entries', function() {
    amountInput.clear();
    amountInput.sendKeys("a");
    amountInput.sendKeys(protractor.Key.TAB);
    expect(amountInput.getAttribute("class")).toContain('ng-invalid-natural-number');
    expect(element(by.model("amount")).getText()).toBe("");
    expect(element(by.binding("amount")).getText()).toBe("GIVE");
    amountInput.clear();
    amountInput.sendKeys("0");
    amountInput.sendKeys(protractor.Key.TAB);
    expect(amountInput.getAttribute("class")).toContain('ng-invalid-natural-number');
    expect(element(by.model("amount")).getText()).toBe("");
    expect(element(by.binding("amount")).getText()).toBe("GIVE");
    amountInput.clear();
    amountInput.sendKeys("-");
    amountInput.sendKeys(protractor.Key.TAB);
    expect(amountInput.getAttribute("class")).toContain('ng-invalid-natural-number');
    expect(element(by.model("amount")).getText()).toBe("");
    expect(element(by.binding("amount")).getText()).toBe("GIVE");
    amountInput.clear();
    amountInput.sendKeys(".");
    amountInput.sendKeys(protractor.Key.TAB);
    expect(amountInput.getAttribute("class")).toContain('ng-invalid-natural-number');
    expect(element(by.model("amount")).getText()).toBe("");
    expect(element(by.binding("amount")).getText()).toBe("GIVE");
    amountInput.clear();
    amountInput.sendKeys(" ");
    amountInput.sendKeys(protractor.Key.TAB);
    expect(amountInput.getAttribute("class")).toContain('ng-invalid-natural-number');
    expect(element(by.model("amount")).getText()).toBe("");
    expect(element(by.binding("amount")).getText()).toBe("GIVE");
  });

});
