require('crds-core');
require('../../app/ang');

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
    ChildcareEvents.setEvent(helpers.event);
    ChildcareEvents.setChildcareEvent(helpers.childcareEvent);
    ChildcareEvents.setChildren(helpers.children);

    scope = $rootScope.$new();
    element = '<childcare></childcare>';
    element = $compile(element)(scope);
    scope.$digest();
  }));

  it('should have an event set', function() {
    var isolated = element.isolateScope();
    expect(isolated.childcare.event).toEqual(helpers.event);
  });

  it('should have a childcareEvent set', function() {
    var isolated = element.isolateScope();
    expect(isolated.childcare.childcareEvent).toEqual(helpers.childcareEvent);
  });

  it('should have a list of children set', function() {
    var isolated = element.isolateScope();
    expect(isolated.childcare.children).toEqual(helpers.children);
  });

  it('should get the correct date in the correct format', function() {
    var isolated = element.isolateScope();
    expect(isolated.childcare.getDate()).toEqual('11/18/2015');
  });

  it('should get the correct time span', function() {
    var isolated = element.isolateScope();
    expect(isolated.childcare.getTime()).toEqual('08:00pm - 09:30pm');
  });

});
