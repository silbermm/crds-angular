xdescribe('Bank Account Number Directive', function() {

  var $scope;
  var form;
  beforeEach(module('testDirective'));

  var mockAccountNumberValid = "123123123123123123123123123";
  var mockAccountNumberNotValid ="123123abcabc123123";

  it("should return false when account number is not valid", function(){
    expect(giveForm.account.$error).toBe(invalidAccount);
  });

  it("should return true when account number is valid", function(){
    expect(giveForm.account.$valid).toBe(true);
  });


});
