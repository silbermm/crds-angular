describe('Serve Icon Directive', function() {

  var $compile, $rootScope, element, scope, isolateScope;

  var mockTrue = {
    contactId: 1,
    emailAddress: "matt.silbernagel@ingagepartners.com",
    lastName: "Silbernagel",
    name: "Matty",
    serveRsvp : {
      attending: true,
      roleId: 145
    }
  };

  var mockFalse = {
    contactId: 1,
    emailAddress: "matt.silbernagel@ingagepartners.com",
    lastName: "Silbernagel",
    name: "Matty",
    serveRsvp : {
      attending: false,
      roleId: 145
    }
  };

  var mockNull = {
    contactId: 1,
    emailAddress: "matt.silbernagel@ingagepartners.com",
    lastName: "Silbernagel",
    name: "Matty",
    serveRsvp : {
      roleId: 145
    }
  };


  beforeEach(function(){
    module('crossroads');
  });
 
  beforeEach(inject(function(_$compile_, _$rootScope_){
    $compile = _$compile_;
    $rootScope = _$rootScope_; 
    scope = $rootScope.$new();
  })); 
  

  it("should show the checkmark icon", function(){ 
    element = "<serve-icon member='member'></serve-icon>";
    scope.member = mockTrue;

    element = $compile(element)(scope);
    scope.$digest();
    isolateScope = element.isolateScope();
  
    expect(element.html()).toContain("icon-check-circle"); 
    expect(element.html()).toContain("#check-circle");
    expect(element.html()).toContain('text-success'); 
    expect(element.html()).not.toContain("text-danger"); 
  });

  it("should show the cancel icon", function(){ 
    element = "<serve-icon member='member'></serve-icon>";
    scope.member = mockFalse; 

    element = $compile(element)(scope);
    scope.$digest();
    isolateScope = element.isolateScope();
  
    expect(element.html()).toContain("icon-cancel-circle"); 
    expect(element.html()).toContain("#cancel-circle");
    expect(element.html()).toContain('text-danger'); 
    expect(element.html()).not.toContain("text-success"); 
  });

  it("should not show an icon", function(){ 
    element = "<serve-icon member='member'></serve-icon>";
    scope.member = mockNull; 

    element = $compile(element)(scope);
    scope.$digest();
    isolateScope = element.isolateScope();
  
    expect(element.html()).toBe(""); 
  });


});
