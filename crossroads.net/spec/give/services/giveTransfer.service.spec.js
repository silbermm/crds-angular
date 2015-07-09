describe('Give Transfer Service', function() {
  var fixture;

  beforeEach(function(){
    module('crossroads', function($provide){
    });
  });

  beforeEach(
    inject(function(GiveTransferService) {
      fixture = GiveTransferService;
    })
  );

  describe("factory object", function(){
    it("should have the expected attributes", function(){
      expect(fixture.account).toBeDefined();
      expect(fixture.account).toBe('');

      expect(fixture.amount).toBeDefined();
      expect(fixture.amount).toBe('');

      expect(fixture.donor).toBeDefined();
      expect(fixture.donor).toBe('');

      expect(fixture.email).toBeDefined();
      expect(fixture.email).toBe('');

      expect(fixture.program).toBeDefined();
      expect(fixture.program).toBe('');

      expect(fixture.routing).toBeDefined();
      expect(fixture.routing).toBe('');

      expect(fixture.view).toBeDefined();
      expect(fixture.view).toBe('');

      expect(fixture.ccNumberClass).toBeDefined();
      expect(fixture.ccNumberClass).toBe('');
    });

    it("should have the expected attributes reset", function() {
      fixture.account = '1';
      fixture.amount = '2';
      fixture.donor = '3';
      fixture.email = '4';
      fixture.program = '5';
      fixture.routing = '6';
      fixture.view = '7';
      fixture.ccNumberClass = '8';

      fixture.reset();

      expect(fixture.account).toBeDefined();
      expect(fixture.account).toBe('');

      expect(fixture.amount).toBeDefined();
      expect(fixture.amount).toBe('');

      expect(fixture.donor).toBeDefined();
      expect(fixture.donor).toBe('');

      expect(fixture.email).toBeDefined();
      expect(fixture.email).toBe('');

      expect(fixture.program).toBeDefined();
      expect(fixture.program).toBe('');

      expect(fixture.routing).toBeDefined();
      expect(fixture.routing).toBe('');

      expect(fixture.view).toBeDefined();
      expect(fixture.view).toBe('');

      expect(fixture.ccNumberClass).toBeDefined();
      expect(fixture.ccNumberClass).toBe('');

    });
  });

});
