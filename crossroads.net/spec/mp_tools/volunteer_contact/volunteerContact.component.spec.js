require('crds-core');
require('../../../app/ang');
require('../../../app/ang2');

require('../../../app/common/common.module');
require('../../../app/app');

describe('Volunteer Contact MPTools Component', function() {

  var CONSTANTS = require('crds-constants');
  var MODULE = CONSTANTS.MODULES.CROSSROADS;
  var helpers = require('./contact.helpers');

  var $compile;
  var $rootScope;
  var element;
  var scope;
  var $httpBackend;
  var isolated;
  var mockMpTools;
  var MPTools;
  var Group;

  beforeEach(function() {
    angular.mock.module(MODULE);
  });

  beforeEach(angular.mock.module(function($provide) {
    mockMpTools = jasmine.createSpyObj('MPTools', ['getParams']);
    mockMpTools.getParams.and.callFake(function() {
      return helpers.mptoolService.getParams();
    });

    $provide.value('MPTools', mockMpTools);
  }));

  beforeEach(inject(function(_$compile_, _$rootScope_, $injector) {
    $compile = _$compile_;
    $rootScope = _$rootScope_;

    scope = $rootScope.$new();
    $httpBackend = $injector.get('$httpBackend');
    MPTools = $injector.get('MPTools');
    Group = $injector.get('Group');

    $httpBackend.whenGET(window.__env__['CRDS_API_ENDPOINT'] + 'api/group/53232')
      .respond(200, helpers.group);
     
    element = '<volunteer-contact></volunteer-contact>';
    element = $compile(element)(scope);

    scope.$digest();
    isolated = element.isolateScope();

  }));

  it('should call MPTools.getParams()', function() {
    expect(mockMpTools.getParams).toHaveBeenCalled();
  });

  it('should not show an error if there is only one selected record', function() {
    expect(isolated.contact.showError()).toBe(false);
  });

  it('should show an error if there is more than one selected record', function() {
    isolated.contact.params.selectedCount = 2;
    expect(isolated.contact.showError()).toBe(true);
  });

  it('should go get the group on initialize', function() {
    $httpBackend.flush();
    expect(isolated.contact.group.groupId).toEqual(helpers.group.groupId);
  });

});
