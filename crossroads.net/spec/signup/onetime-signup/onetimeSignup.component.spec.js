require('crds-core');
require('../../../app/common/common.module');
require('../../../app/signup/onetime_event');

describe('Onetime Signup Component', function() {

  var CONSTANTS = require('crds-constants');
  var MODULE = CONSTANTS.MODULES.ONETIME_SIGNUP;
  var helpers = require('../signup.helpers');

  var $compile;
  var $rootScope;
  var element;
  var scope;
  var $httpBackend;
  var isolated;

  beforeEach(function() {
    angular.mock.module(MODULE);
  });

  beforeEach(angular.mock.module(function($provide) {
    $provide.value('$state', { get: function() {} });
  }));

  beforeEach(inject(function(_$compile_, _$rootScope_, $injector) {
    $compile = _$compile_;
    $rootScope = _$rootScope_;

    scope = $rootScope.$new();
    scope.cmsInfo = helpers.cmsInfo.pages[0];
    scope.group = helpers.group;
    scope.family = helpers.family;
    console.log('before: ' + scope.family);
    element = '<onetime-event cms-info=\'cmsInfo\' group=\'group\' family=\'family\'></onetime-event>';
    element = $compile(element)(scope);

    scope.$digest();
    isolated = element.isolateScope();
  }));

  it('should get a title from the cmsInfo', function() {
    expect(isolated.onetimeEvent.cmsInfo).toEqual(helpers.cmsInfo.pages[0]);
  });

  it('should get the group', function() {
    expect(isolated.onetimeEvent.group).toEqual(helpers.group);
  });

  it('should filter out people under minimum age', function() {
    console.log(isolated.onetimeEvent);
    console.log('in spec ' + isolated.onetimeEvent.family);
    expect(isolated.onetimeEvent.family.length).toEqual(1);
  });
});
