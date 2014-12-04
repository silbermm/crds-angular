describe("crds menu controller spec", function() {
  beforeEach(function() {
    module("crossroads");
  });

  beforeEach(inject(function($controller, $rootScope) {
    this.scope = $rootScope.$new();
    this.controller = $controller("crdsMenuCtrl", {
      $scope: this.scope
    });
  }));

  describe("toggle desktop login", function() {
    it("toggles", function() {
      expect(this.scope.loginShow).toBeFalsy();
      this.scope.toggleDesktopLogin();
      expect(this.scope.loginShow).toBeTruthy();
    });
  });
});
