require('crds-core');
require('../../../app/app');

describe('Check Batch Processor Tool', function() {
  var batchList = [
    {
      id: 22,
      name: 'GeneralFunding012948',
      scanDate: '2015-08-12T00:00:00',
    },
    {
      id: 23,
      name: 'PickUpTheSlack938747',
      scanDate: '2015-08-14T00:00:00',
    },
    {
      id: 24,
      name: 'General194200382',
      scanDate: '2015-09-12T00:00:00',
    },
    {
      id: 25,
      name: 'GetTough38294729',
      scanDate: '2015-09-13T00:00:00',
    },
  ];

  var programList = [
    {ProgramId: 1, Name: 'Crossroads'},
    {ProgramId: 2, Name: 'Game Change'},
    {ProgramId: 3, Name: 'Old St George Building'},
  ];

  beforeEach(angular.mock.module('crossroads'));

  var GIVE_ROLES = { StewardshipDonationProcessor: 123 };
  var GIVE_PROGRAM_TYPES = { Fuel: 999 };

  beforeEach(function() {
    angular.mock.module('crossroads.give', function($provide) {
      $provide.constant('GIVE_ROLES', GIVE_ROLES);
      $provide.constant('GIVE_PROGRAM_TYPES', GIVE_PROGRAM_TYPES);
    });
  });

  var AuthService;

  beforeEach(function(){
    angular.mock.module('crossroads.core', function($provide){
      AuthService = jasmine.createSpyObj('AuthService', ['isAuthenticated', 'isAuthorized']);
      $provide.value('AuthService', AuthService);
    });
  });

  var $controller;
  var $log;
  var $httpBackend;
  var MPTools;

  beforeEach(inject(function(_$controller_, _$log_, _MPTools_, $injector) {
    $controller = _$controller_;
    $log = _$log_;
    MPTools = _MPTools_;
    $httpBackend = $injector.get('$httpBackend');
  }));

  describe('Check Batch Processor Controller', function() {

    var $scope;
    var controller;
    beforeEach(function() {
      $scope = {};
      controller = $controller('CheckBatchProcessor', { $scope: $scope });
      $httpBackend.expectGET(window.__env__['CRDS_API_ENDPOINT'] + 'api/checkscanner/batches').respond(batchList);
      $httpBackend.expectGET(window.__env__['CRDS_API_ENDPOINT'] + 'api/programs/' + GIVE_PROGRAM_TYPES.Fuel).respond(programList);
    });

    describe('Function allowAccess', function() {
      it('Should not allow access if user is not authenticated', function() {
        AuthService.isAuthenticated.and.returnValue(false);

        expect(controller.allowAccess()).toBeFalsy();

        expect(AuthService.isAuthenticated).toHaveBeenCalled();
        expect(AuthService.isAuthorized).not.toHaveBeenCalled();
      });

      it('Should not allow access if user is authenticated but not authorized', function() {
        AuthService.isAuthenticated.and.returnValue(true);
        AuthService.isAuthorized.and.returnValue(false);

        expect(controller.allowAccess()).toBeFalsy();

        expect(AuthService.isAuthenticated).toHaveBeenCalled();
        expect(AuthService.isAuthorized).toHaveBeenCalledWith(GIVE_ROLES.StewardshipDonationProcessor);
      });

      it('Should not allow access if user is authenticated but not authorized', function() {
        AuthService.isAuthenticated.and.returnValue(true);
        AuthService.isAuthorized.and.returnValue(true);

        expect(controller.allowAccess()).toBeTruthy();

        expect(AuthService.isAuthenticated).toHaveBeenCalled();
        expect(AuthService.isAuthorized).toHaveBeenCalledWith(GIVE_ROLES.StewardshipDonationProcessor);
      });
    });

    describe('Initial Load', function() {
      it('should get a list of check batches', function() {
        $httpBackend.flush();

        expect(controller.batches.length).toBe(4);
        expect(controller.programs.length).toBe(3);
      });
    });


    describe('Process Batch', function() {
      var postData;
      beforeEach(function() {
        postData = {
          name: batchList[1].name,
          programId: programList[1].ProgramId
        };
      });

      it('should successfully submit the selected Batch with the selected Program', function(){
        $httpBackend.flush();
        $httpBackend.expectPOST( window.__env__['CRDS_API_ENDPOINT'] + 'api/checkscanner/batches', postData).respond(200, '');

        controller.batch = batchList[1];
        controller.program = programList[1];
        controller.processBatch();

        expect(controller.processing).toBe(true);
        $httpBackend.flush();
        expect(controller.success).toBe(true);
        expect(controller.error).toBe(false);
        expect(controller.processing).toBe(false);
      });

      it('should successfully submit the selected Batch with the selected Program', function(){
        $httpBackend.flush();
        $httpBackend.expectPOST( window.__env__['CRDS_API_ENDPOINT'] + 'api/checkscanner/batches', postData).respond(500, '');

        controller.batch = batchList[1];
        controller.program = programList[1];
        controller.processBatch();

        expect(controller.processing).toBe(true);
        $httpBackend.flush();
        expect(controller.success).toBe(false);
        expect(controller.error).toBe(true);
        expect(controller.processing).toBe(false);
      });
    });

  });
});
