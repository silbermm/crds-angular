xdescribe('Routing Transit Number Directive', function() {

 beforeEach(module('rtnDirective'));


  describe('invalidRouting', function() {
    it('should validate an integer', function() {
      inject(function($compile, $rootScope) {
        var element = angular.element(
          '<form name="giveform">' +
            '<input ng-model="give.routing" name="routing" invalid-routing>' +
          '</form>'
          );
        $compile(element)($rootScope);
        $rootScope.$digest();
        element.find('input').val("aaa");
        expect(giveForm.routing.$error).toBe(invalidRouting);
      });
    });
  });
});

  // it("should return true when rtn is valid", function(){
  //   expect(isolated.signedup).toBe(null);
  // });
  //
  // it("should return false when rtn is invalid", function(){
  //   expect( isolated.isActiveTab(mockTeam[0].members[0])).toBe(false);
  // });
