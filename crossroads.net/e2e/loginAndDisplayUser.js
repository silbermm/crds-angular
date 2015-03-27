describe('Crossroads App', function() {

  beforeEach(function(){
    browser.get('http://integration.crossroads.net/#/'); 
    var loginButton = element.all(by.css(".navbar--login")).get(0).all(by.buttonText('Login'));
    expect(loginButton.count()).toBe(2); 
    expect(loginButton.get(0).isDisplayed()).toBeTruthy();
    loginButton.get(0).click();
   
    var emailInput = element.all(by.css(".navbar--login")).get(0).element(by.id("login-dropdown-email"));
    var passwordInput = element.all(by.css(".navbar--login")).get(0).element(by.id("login-dropdown-password"));
    var submitBtn = element.all(by.css(".navbar--login")).get(0).all(by.buttonText("Login")).get(1);
    
    expect(emailInput.isDisplayed()).toBeTruthy();
    expect(passwordInput.isDisplayed()).toBeTruthy();

    emailInput.sendKeys("silbermm@ip.com");
    passwordInput.sendKeys("winter14");
    submitBtn.click(); 
  });

  it('should allow me to login', function() { 
    expect(element(by.id("current-user")).getText()).toBe("Matthew");  
  });


});
