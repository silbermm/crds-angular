require('crds-core');
require('../../../app/app');

describe('Check Batch Processor Tool', function() {
  var expectedBatchReturn = [
    {
      id: 23,
      name: 'Test081402',
      scanDate: '2015-08-14T00:00:00',
      status: 'not_exported',
      programId: null,
      checks: []
    },
    {
      id: 22,
      name: 'Test081401',
      scanDate: '2015-08-14T00:00:00',
      status: 'exported',
      programId: null,
      checks: []
    }
  ];

  beforeEach(angular.mock.module('crossroads'));

  beforeEach(inject(function(_$location_) {
    var $location = _$location_;
    spyOn($location, 'search').and.returnValue({
      dg:'8b6242c9-ea32-40f7-97a2-e2bb3524ced2',
      ug:'c29e64a5-820b-461f-a57c-5831d070d578',
      pageID:'292',
      recordID:'2923',
      recordDescription: undefined,
      s:'11467',
      sc:'1',
      p:0,
      v:387
    });
  }));

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
      $httpBackend.expectGET(window.__env__['CRDS_API_ENDPOINT'] +
        'api/checkscanner/batches').respond(expectedBatchReturn);
    });

    describe('Initial Load', function() {
      it('should get the correct query parameters', function() {
        expect(controller.params.userGuid).toBe('c29e64a5-820b-461f-a57c-5831d070d578');
      });

      it('should get a list of check batches', function() {
        $httpBackend.flush();
        expect(controller.batches.length).toBe(2);
      });
    });
  });
});
