require('crds-core');
require('../../../app/common/common.module');
require('../../../app/app');

describe('Admin Giving History Tool', function() {
  var $state;
  var MPTools;
  var GivingHistoryService;
  var AuthService;
  var GIVE_ROLES = { StewardshipDonationProcessor: 123 };

  beforeEach(function() {
    angular.mock.module('crossroads', function($provide) {
      $state = jasmine.createSpyObj('$state', ['go', 'get']);
      MPTools = jasmine.createSpyObj('MPTools', ['getParams']);
      GivingHistoryService = {
        impersonateDonorId: undefined
      };

      $provide.constant('GIVE_ROLES', GIVE_ROLES);
      $provide.value('$state', $state);
      $provide.value('MPTools', MPTools);
      $provide.value('GivingHistoryService', GivingHistoryService);
    });
  });

  beforeEach(function() {
    angular.mock.module('crossroads.core', function($provide) {
      AuthService = jasmine.createSpyObj('AuthService', ['isAuthenticated', 'isAuthorized']);
      $provide.value('AuthService', AuthService);
    });
  });

  var $controller;

  beforeEach(inject(function(_$controller_) {
    $controller = _$controller_;
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

        fixture = $controller('AdminGivingHistoryController', { $scope: $scope });
        expect($state.go).not.toHaveBeenCalled();
        expect(fixture.noSelection).toBeUndefined();
        expect(fixture.selectionError).toBeUndefined();
        expect(fixture.tooManySelections).toBeUndefined();
      });

      it('should not change state if access is allowed but no selections are made', function() {
        AuthService.isAuthenticated.and.returnValue(true);
        AuthService.isAuthorized.and.returnValue(true);
        MPTools.getParams.and.returnValue({});

        fixture = $controller('AdminGivingHistoryController', { $scope: $scope });
        expect($state.go).not.toHaveBeenCalled();
        expect(fixture.noSelection).toBeTruthy();
        expect(fixture.selectionError).toBeUndefined();
        expect(fixture.tooManySelections).toBeFalsy();
      });

      it('should not change state if access is allowed but too many selections are made', function() {
        AuthService.isAuthenticated.and.returnValue(true);
        AuthService.isAuthorized.and.returnValue(true);
        MPTools.getParams.and.returnValue({selectedCount: 2, selectedRecord: 123});

        fixture = $controller('AdminGivingHistoryController', { $scope: $scope });
        expect($state.go).not.toHaveBeenCalled();
        expect(fixture.noSelection).toBeFalsy();
        expect(fixture.selectionError).toBeUndefined();
        expect(fixture.tooManySelections).toBeTruthy();
      });

      it('should go to history view when a single donor is being viewed or edited', function() {
        AuthService.isAuthenticated.and.returnValue(true);
        AuthService.isAuthorized.and.returnValue(true);
        MPTools.getParams.and.returnValue({recordId: 123, selectedCount: 0, selectedRecord: 0});

        fixture = $controller('AdminGivingHistoryController', { $scope: $scope });
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
        MPTools.getParams.and.returnValue({recordId: 0, selectedCount: 1, selectedRecord: 456});
        MPTools.Selection = callback;

        fixture = $controller('AdminGivingHistoryController', { $scope: $scope });
        expect($state.go).not.toHaveBeenCalled();
        expect(GivingHistoryService.impersonateDonorId).toBeUndefined();
        expect(actualParams).toBeDefined();
        expect(actualParams).toEqual({selectionId: 456});
        expect(fixture.selectionError).toBeTruthy();
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
        MPTools.getParams.and.returnValue({recordId: 0, selectedCount: 1, selectedRecord: 456});
        MPTools.Selection = callback;

        fixture = $controller('AdminGivingHistoryController', { $scope: $scope });
        expect($state.go).toHaveBeenCalledWith('tools.adminGivingHistory');
        expect(GivingHistoryService.impersonateDonorId).toEqual(123);
        expect(actualParams).toBeDefined();
        expect(actualParams).toEqual({selectionId: 456});
      });
    });

    describe('Function allowAccess', function() {
      beforeEach(function() {
        fixture = $controller('AdminGivingHistoryController', { $scope: $scope });
      });

      it('Should not allow access if user is not authenticated', function() {
        AuthService.isAuthenticated.and.returnValue(false);

        expect(fixture.allowAccess()).toBeFalsy();

        expect(AuthService.isAuthenticated).toHaveBeenCalled();
        expect(AuthService.isAuthorized).not.toHaveBeenCalled();
      });

      it('Should not allow access if user is authenticated but not authorized', function() {
        AuthService.isAuthenticated.and.returnValue(true);
        AuthService.isAuthorized.and.returnValue(false);

        expect(fixture.allowAccess()).toBeFalsy();

        expect(AuthService.isAuthenticated).toHaveBeenCalled();
        expect(AuthService.isAuthorized).toHaveBeenCalledWith(GIVE_ROLES.StewardshipDonationProcessor);
      });

      it('Should not allow access if user is authenticated but not authorized', function() {
        AuthService.isAuthenticated.and.returnValue(true);
        AuthService.isAuthorized.and.returnValue(true);

        expect(fixture.allowAccess()).toBeTruthy();

        expect(AuthService.isAuthenticated).toHaveBeenCalled();
        expect(AuthService.isAuthorized).toHaveBeenCalledWith(GIVE_ROLES.StewardshipDonationProcessor);
      });
    });
  });
});
