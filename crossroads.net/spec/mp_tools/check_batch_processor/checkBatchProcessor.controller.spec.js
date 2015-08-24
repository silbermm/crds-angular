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
      $httpBackend.expectGET(window.__env__['CRDS_API_ENDPOINT'] + 'api/programs/1').respond(programList);
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
