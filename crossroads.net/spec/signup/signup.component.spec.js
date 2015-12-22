require('crds-core');
require('../../app/common/common.module');
require('../../app/signup');

describe('Signup Component', function() {
  var CONSTANTS = require('crds-constants');
  var MODULE = CONSTANTS.MODULES.SIGNUP;
  var helpers = require('./signup.helpers');

  var $compile;
  var $rootScope;
  var element;
  var scope;
  var $httpBackend;
  var SignupService;
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

    SignupService = $injector.get('SignupService');
    SignupService.cmsInfo = helpers.cmsInfo;

    scope = $rootScope.$new();
    element = '<crds-signup></crds-signup>';
    element = $compile(element)(scope);
    console.log('aaaaa');
    console.log(element);
    console.log('aaaaa');
    scope.$digest();

    console.log('bbbbb');
    console.log(element);
    console.log('bbbbb');
    isolated = element.isolateScope();

    console.log('ccccc');
    console.log(isolated);
    console.log('ccccc');
  }));

  it('should show the onetimesignup component', function() {
    console.log('isolated!');
    console.log(isolated.signup.showOnetimeEvent());
    expect(isolated.signup.showOnetimeEvent()).toBe(true);
  });

  it('should show the community group signup', function() {
    SignupService.cmsInfo.pages[0].className = CONSTANTS.CMS.PAGENAMES.COMMUNITYGROUPS;
    expect(isolated.signup.showCommunityGroups()).toBe(true);
  });

});
