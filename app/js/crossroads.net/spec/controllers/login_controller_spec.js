describe("LoginController", function() {
  beforeEach(module("crossroads"));
  beforeEach(inject(function($controller, $q, Auth, $rootScope) {
    this.deferred = $q.defer();
    spyOn(Auth, "login").andReturn(this.deferred.promise);
    this.scope = $rootScope.$new();
    this.scope.user = {
      username: "wut",
    password: "ever"
    };
    $controller("LoginCtrl", {
      $scope: this.scope
    });
  }));

  xit("should add an error on login failure", function() {
    this.scope.login();
    this.deferred.resolve("foo");
    this.scope.$apply();
    expect(this.scope.loginError).toEqual("Login failed.");
  });
});
