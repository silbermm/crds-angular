describe('KC Applicant Tool', function(){


  beforeEach(module('crossroads'));

  beforeEach(
      inject(function(_$location_){
        var $location = _$location__;
        spyOn($location, 'search').and.returnValue({
          dg:'8b6242c9-ea32-40f7-97a2-e2bb3524ced2',
          'ug':'c29e64a5-820b-461f-a57c-5831d070d578',
          pageID:'292',
          recordID:'2923',
          recordDescription: undefined,
          s:'11467',
          sc:'1',
          p:0,
          v:387
        });
      })
  );

  var $controller, $log, $httpBackend, MPTools, $window;

  beforeEach(inject(function(_$controller_, _$log_, _MPTools_, _$window_, $injector){
    $controller = _$controller_;
    $log = _$log_;
    $window = _$window_;
    MPTools = _MPTools_;
    $httpBackend = $injector.get('$httpBackend');
  }));

  it('should ', function(){})


});
