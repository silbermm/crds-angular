var env = require("../environment");

var LoginPage = (function () {
  function LoginPage() {
    this.emailField = element.all(by.css(".navbar--login")).get(0).element(by.id("login-dropdown-email"));
    this.passwordField = element.all(by.css(".navbar--login")).get(0).element(by.id("login-dropdown-password"));
    this.loginButton = element.all(by.css(".navbar--login")).get(0).all(by.buttonText('Login')).get(0);
    this.submitButton = element.all(by.css(".navbar--login")).get(0).all(by.buttonText("Login")).get(1);

    this.logoutButton = element.all(by.css(".navbar--login")).get(0).all(by.linkText('Sign Out'));
    this.currentUser = element(by.id("current-user")); 
  } 
  
  LoginPage.prototype.visitPage = function () { 
    browser.get(env.baseUrl + '/#/');
    this.loginButton.click();
  };

  LoginPage.prototype.fillEmail = function (email) {
    this.emailField.sendKeys(email);
  };

  LoginPage.prototype.fillPassword = function (password) {
    if (password == null) {
      password = "password";
    }
    this.passwordField.sendKeys(password);
  };

  LoginPage.prototype.login = function () {
    this.submitButton.click();
  };

  LoginPage.prototype.logout = function() { 
    this.logoutButton.click(); 
    expect(this.loginButton.isDisplayed()).toBeTruthy();
  }
  
  LoginPage.prototype.getCurrentUser = function () { 
    return this.currentUser.getText();
  };
  
  return LoginPage;
})();

module.exports = LoginPage
