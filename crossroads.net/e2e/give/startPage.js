var env = require("../environment");
var amountInput = element(by.model('give.amount'));

describe('Give as a guest-giver', function() {
  it('It should contain expected programs in the dropdown', function() {
    browser.get(env.baseUrl + '/#/give/amount'); 
    element.all(by.options("program for program in programs")).then(function(rows){
        expect(rows[0].evaluate().getText()).toContain("Ministry Fund");
        expect(rows[1].evaluate().getText()).toContain("Game Change Fund");
        expect(rows[2].evaluate().getText()).toContain("Old St. George");
    });
  });

  it('It should reflect the amount in the button', function() {
    amountInput.sendKeys("1224");
    amountInput.sendKeys(protractor.Key.TAB);
    expect(element(by.binding("give.amount")).getText()).toContain("GIVE $1,224.00");
  });

  it('It should not allow invalid entries', function() {
    amountInput.clear(); 
    amountInput.sendKeys("a");
    amountInput.sendKeys(protractor.Key.TAB);
    expect(amountInput.getAttribute("class")).toContain('ng-invalid-natural-number');
    expect(element(by.model("give.amount")).getText()).toBe("");
    expect(element(by.binding("give.amount")).getText()).toBe("GIVE");
    amountInput.clear();  
    amountInput.sendKeys("0");
    amountInput.sendKeys(protractor.Key.TAB);
    expect(amountInput.getAttribute("class")).toContain('ng-invalid-natural-number');
    expect(element(by.model("give.amount")).getText()).toBe("");
    expect(element(by.binding("give.amount")).getText()).toBe("GIVE");
    amountInput.clear();
    amountInput.sendKeys("-");
    amountInput.sendKeys(protractor.Key.TAB);
    expect(amountInput.getAttribute("class")).toContain('ng-invalid-natural-number');
    expect(element(by.model("give.amount")).getText()).toBe("");
    expect(element(by.binding("give.amount")).getText()).toBe("GIVE");
    amountInput.clear();
    amountInput.sendKeys(".");
    amountInput.sendKeys(protractor.Key.TAB);
    expect(amountInput.getAttribute("class")).toContain('ng-invalid-natural-number');
    expect(element(by.model("give.amount")).getText()).toBe("");
    expect(element(by.binding("give.amount")).getText()).toBe("GIVE");
    amountInput.clear();
    amountInput.sendKeys(" ");
    amountInput.sendKeys(protractor.Key.TAB);
    expect(amountInput.getAttribute("class")).toContain('ng-invalid-natural-number');
    expect(element(by.model("give.amount")).getText()).toBe("");
    expect(element(by.binding("give.amount")).getText()).toBe("GIVE");
  });

});