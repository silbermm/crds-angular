require('crds-core');
require('../../app/common/common.module');
require('../../app/childcare');

describe('Childcare Module', function() {

  var helpers = require('./childcare.helpers');
  var MODULE = require('crds-constants').MODULES.CHILDCARE;

  var $compile;
  var $rootScope;
  var element;
  var scope;
  var mockSession;
  var $httpBackend;
  var ChildcareEvents;

  beforeEach(function() {
    angular.mock.module(MODULE);
  });

  beforeEach(angular.mock.module(function($provide) {
    $provide.value('$state', { get: function() {} });
  }));

  beforeEach(inject(function(_$compile_, _$rootScope_, $injector) {
    $compile = _$compile_;
    $rootScope = _$rootScope_;

    ChildcareEvents = $injector.get('ChildcareEvents');
    ChildcareEvents.setEvent(helpers.childcareEvent);

    scope = $rootScope.$new();
    element = '<childcare></childcare>';
    element = $compile(element)(scope);
    scope.$digest();
  }));

  it('should have some Childcare Events', function() {
    var isolated = element.isolateScope();
    console.log(isolated);
    expect(isolated.childcare.event).toEqual(helpers.childcareEvent);
  });
});
