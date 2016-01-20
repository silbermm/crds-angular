require('crds-core');
require('../../../app/ang');

require('../../../app/common/common.module');
require('../../../app/app');

describe('Admin Common Tool', function() {
  var $state;
  var MPTools;
  var GivingHistoryService;
  var AuthService;
  var goToFunction;
  var GIVE_ROLES = { StewardshipDonationProcessor: 7 };

  beforeEach(function() {
    angular.mock.module('crossroads', function($provide) {
      $state = jasmine.createSpyObj('$state', ['go']);
      GivingHistoryService = { impersonateDonorId: undefined };
      goToFunction = function(donorId) {
        GivingHistoryService.impersonateDonorId = donorId;
        $state.go('tools.adminGivingHistory');
      };

      $provide.constant('role', GIVE_ROLES.StewardshipDonationProcessor);
      $provide.value('goToFunction', goToFunction);
    });
  });

  beforeEach(function() {
    angular.mock.module('crossroads.core', function($provide) {
      AuthService = jasmine.createSpyObj('AuthService', ['isAuthenticated', 'isAuthorized']);
      $provide.value('AuthService', AuthService);
    });
  });

  var $controller;

  beforeEach(inject(function(_$controller_, $injector) {
    $controller = _$controller_;
    MPTools = $injector.get('MPTools');
  }));

  describe('Admin Giving History Controller', function() {
    var $scope;
    var fixture;
    beforeEach(function() {
      $scope = {};
    });

    describe('Function activate', function() {
      it('should not change state if access is not allowed', function() {
        AuthService.isAuthenticated.and.returnValue(false);

        fixture = $controller('AdminToolController', { $scope: $scope });
        expect($state.go).not.toHaveBeenCalled();
        expect(fixture.service.dto.noSelection).toBeUndefined();
        expect(fixture.service.dto.selectionError).toBeUndefined();
        expect(fixture.service.dto.tooManySelections).toBeUndefined();
      });

      it('should not change state if access is allowed but no selections are made', function() {
        AuthService.isAuthenticated.and.returnValue(true);
        AuthService.isAuthorized.and.returnValue(true);

        fixture = $controller('AdminToolController', { $scope: $scope });
        expect($state.go).not.toHaveBeenCalled();
        expect(fixture.service.dto.noSelection).toBeTruthy();
        expect(fixture.service.dto.selectionError).toBeUndefined();
        expect(fixture.service.dto.tooManySelections).toBeFalsy();
      });

      it('should not change state if access is allowed but too many selections are made', function() {
        AuthService.isAuthenticated.and.returnValue(true);
        AuthService.isAuthorized.and.returnValue(true);
        spyOn(MPTools, 'getParams').and.callFake(function(){
          return {
            selectedCount: 2,
            selectedRecord: 123,
          }
        });

        fixture = $controller('AdminToolController', { $scope: $scope });
        expect($state.go).not.toHaveBeenCalled();
        expect(fixture.service.dto.noSelection).toBeFalsy();
        expect(fixture.service.dto.selectionError).toBeUndefined();
        expect(fixture.service.dto.tooManySelections).toBeTruthy();
      });

      it('should go to history view when a single donor is being viewed or edited', function() {
        AuthService.isAuthenticated.and.returnValue(true);
        AuthService.isAuthorized.and.returnValue(true);
        spyOn(MPTools, 'getParams').and.callFake(function(){
          return {
            recordId: 123,
            selectedCount: 0,
            selectedRecord: 0,
          }
        });

        fixture = $controller('AdminToolController', { $scope: $scope });
        expect($state.go).toHaveBeenCalledWith('tools.adminGivingHistory');
        expect(GivingHistoryService.impersonateDonorId).toEqual(123);
      });

      it('should not go to history view when there is a problem getting selection information', function() {
        var actualParams;
        var callback = {
          get: function(parms, success, error) {
            actualParams = parms;
            error('error');
          }
        };
        AuthService.isAuthenticated.and.returnValue(true);
        AuthService.isAuthorized.and.returnValue(true);
        spyOn(MPTools, 'getParams').and.callFake(function(){
          return {
            recordId: 0,
            selectedCount: 1,
            selectedRecord: 456,
          }
        });
        MPTools.Selection = callback;

        fixture = $controller('AdminToolController', { $scope: $scope });
        expect($state.go).not.toHaveBeenCalled();
        expect(GivingHistoryService.impersonateDonorId).toBeUndefined();
        expect(actualParams).toBeDefined();
        expect(actualParams).toEqual({selectionId: 456});
        expect(fixture.service.dto.selectionError).toBeTruthy();
      });

      it('should go to history view when a single donor is selected', function() {
        var actualParams;
        var successData = {
          RecordIds: [123,789]
        };

        var callback = {
          get: function(parms, success) {
            actualParams = parms;
            success(successData);
          }
        };
        AuthService.isAuthenticated.and.returnValue(true);
        AuthService.isAuthorized.and.returnValue(true);
        spyOn(MPTools, 'getParams').and.callFake(function(){
          return {
            recordId: 0,
            selectedCount: 1,
            selectedRecord: 456,
          }
        });
        MPTools.Selection = callback;

        fixture = $controller('AdminToolController', { $scope: $scope });
        expect($state.go).toHaveBeenCalledWith('tools.adminGivingHistory');
        expect(GivingHistoryService.impersonateDonorId).toEqual(123);
        expect(actualParams).toBeDefined();
        expect(actualParams).toEqual({selectionId: 456});
      });
    });

    describe('Function allowAccess', function() {

      it('Should not allow access if user is not authenticated', function() {
        AuthService.isAuthenticated.and.returnValue(false);
        fixture = $controller('AdminToolController', { $scope: $scope });

        expect(fixture.allowAccess).toBeFalsy();

        expect(AuthService.isAuthenticated).toHaveBeenCalled();
        expect(AuthService.isAuthorized).not.toHaveBeenCalled();
      });

      it('Should not allow access if user is authenticated but not authorized', function() {
        AuthService.isAuthenticated.and.returnValue(true);
        AuthService.isAuthorized.and.returnValue(false);
        fixture = $controller('AdminToolController', { $scope: $scope });

        expect(fixture.allowAccess).toBeFalsy();

        expect(AuthService.isAuthenticated).toHaveBeenCalled();
        expect(AuthService.isAuthorized).toHaveBeenCalledWith(GIVE_ROLES.StewardshipDonationProcessor);
      });

      it('Should allow access if user is authenticated and authorized', function() {
        AuthService.isAuthenticated.and.returnValue(true);
        AuthService.isAuthorized.and.returnValue(true);
        fixture = $controller('AdminToolController', { $scope: $scope });

        expect(fixture.allowAccess).toBeTruthy();

        expect(AuthService.isAuthenticated).toHaveBeenCalled();
        expect(AuthService.isAuthorized).toHaveBeenCalledWith(GIVE_ROLES.StewardshipDonationProcessor);
      });
    });
  });
});
