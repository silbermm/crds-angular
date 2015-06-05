describe('VolunteerApplicationController', function() {

  beforeEach(module('crossroads'));

  beforeEach(module(function($provide){
    mockSession= jasmine.createSpyObj('Session', ['exists']);
    mockSession.exists.and.callFake(function(something){
      return '12345678';
    });
    $provide.value('Session', mockSession);
  }));

  var $controller, $log,  $httpBackend, Session, mockCmsInfoResource, MESSAGES, $stateParams, Opportunity, Contact;
  beforeEach(inject(function(_$controller_, _$log_, $injector){

    $controller = _$controller_;
    $log = _$log_;
    $httpBackend = $injector.get('$httpBackend');
    Session = $injector.get('Session');

    //mockCmsInfoResource = $injector.get('CmsInfo');
  }));

  describe('volunteer Application Controller', function(){

    var $scope, controller;

    beforeEach(inject(function($log, $httpBackend){
      $scope = {};

      var $stateParams = {"id" : 123456};
      var pageGetResponse = {"pages":[{
       "group":"1"
     }]};

     var contact = {"age":24};

      controller = $controller('VolunteerApplicationController',
        { '$scope': $scope,
        '$log': $log,
        'MESSAGES': MESSAGES,
        'Session': Session,
        '$stateParams': $stateParams,
        'CmsInfo': pageGetResponse,
        'Opportunity': Opportunity,
        'Contact': contact
        });

        // controller.showAdult = true;
        // controller.showStudent = false;
        // controller.showError = false;
    }));

    it('should determine adult application', function(){
      controller.applicationVersion();
      expect(controller.showAdult).toBe(true);
      expect(controller.showStudent).toBe(false);
      expect(controller.showError).toBe(false);
    });
  });
})
