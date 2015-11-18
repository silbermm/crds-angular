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

    element = '<onetime-event cms-info=\'cmsInfo\'></onetime-event>';
    element = $compile(element)(scope);

    scope.$digest();
    isolated = element.isolateScope();
  }));

  it('should get a title from the cmsInfo', function() {
    expect(isolated.onetimeEvent.cmsInfo).toEqual(helpers.cmsInfo.pages[0]);
  });
});
