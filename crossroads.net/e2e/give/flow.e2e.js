var env = require("../environment");

describe('Giving Flow', function() {
  
  beforeEach(function() {
    browser.get(env.baseUrl + '/#/give');
  })
  
  it('should flow between amount entry, account entry and confirmation on pressing Give button', function () {
    expect(browser.getCurrentUrl()).toMatch(/\/amount/);
    element(by.model('give.amount')).sendKeys("1");
    element(by.binding('give.amount')).click();
    expect(browser.getCurrentUrl()).toMatch(/\/account/);
    element.all(by.binding('give.amount')).get(1).click();
    expect(browser.getCurrentUrl()).toMatch(/\/thank-you/);
  })
})