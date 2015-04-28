describe('Unit testing donation details directive', function(){

  var $compile, $rootScope, $httpBackend;

  var onlineGivingProgramsGetResponse =
  [
    {
        "ProgramId": 26,
        "Name": "Gamechange Campaign"
    },
    {
        "ProgramId": 3,
        "Name": "Ministry"
    },
    {
        "ProgramId": 112,
        "Name": "Old St George - Clifton"
    }
]

  // Load the crossroads module, which contains the directive

  beforeEach(function(){
    module('crossroads');
  });

   // Store references to $rootScope and $compile
  // so they are available to all tests in this describe block
  beforeEach(inject(function(_$compile_, _$rootScope_,_$injector_){
    // The injector unwraps the underscores (_) from around the parameter names when matching
    $compile = _$compile_;
    $rootScope = _$rootScope_;
    $injector = _$injector_;
    $rootScope = $injector.get('$rootScope');
       scope = $rootScope.$new();
    $httpBackend = $injector.get('$httpBackend');
    giveController = function() {
         return $controller('GiveCtrl', {'$scope' : scope});
    };
    var $controller = $injector.get('$controller');
    //This is required to mock the on-blur-messages that are pulled in from the CMS
    $templateCache = $injector.get('$templateCache');
    $templateCache.put('on-blur-messages',
     '<span ng-message="naturalNumber">Amount entered does not appear to be valid.</span>');

    $httpBackend.when('GET', window.__env__['CRDS_API_ENDPOINT'] +'api/programs/1')
     .respond(onlineGivingProgramsGetResponse);
  }));

  afterEach(function() {
    $httpBackend.flush();
    $httpBackend.verifyNoOutstandingExpectation();
    $httpBackend.verifyNoOutstandingRequest();
   });

  it('should replace the element with the appropriate content and GET onlineGivingProgramsGetResponse', function(){
    $httpBackend.expectGET(window.__env__['CRDS_API_ENDPOINT'] +'api/programs/1');
    var controller = giveController();
    scope.give = controller;
    //Compile a piece of HTML containing the directive
    var element = $compile("<donation-details progtype='1' give='give'></donation-details>")(scope);

    // fire all the watches
    scope.$digest();
    // Check that the compiled element contains the templated content
    expect(element.html()).toContain("program.Name for program in programs track by program.ProgramId");
  });
});