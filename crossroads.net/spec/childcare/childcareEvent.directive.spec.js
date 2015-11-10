require('crds-core');
require('../../app/common/common.module');
require('../../app/childcare');

describe('Childcare Event Directive', function() {

  var helpers = require('./childcare.helpers');
  var MODULE = require('crds-constants').MODULES.CHILDCARE;

  var $compile;
  var $rootScope;
  var element;
  var scope;
  var mockSession;
  var $httpBackend;
  var ChildcareEvents;
  var isolate;

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

    // Mock out the messages
    $rootScope.MESSAGES = [
      generalError: {},
      chooseOne: {}
    ];

    scope = $rootScope.$new();
    element = '<childcare-event children=\'children\' childcare-event=\'childcareEvent\'></childcare-event>';
    scope.children = ChildcareEvents.children;
    scope.childcareEvent = ChildcareEvents.childcareEvent;
    element = $compile(element)(scope);
    scope.$digest();
    isolate = element.isolateScope();
  }));

  it('should not save the form if no children have been selected', function() {
    expect(isolate.childcareEvent.submit()).toBe(false);
  });

  it('should display a growl notification if there are no children selected to save', function() {
    spyOn($rootScope, '$emit').and.callThrough();     
    expect(isolate.childcareEvent.submit()).toBe(false);
    expect($rootScope.$emit).toHaveBeenCalledWith('notify', jasmine.any(Object));
  });

  it('should save the form if children have been selected', function() {
    scope.children[0].selected = true;
    expect(isolate.childcareEvent.submit()).toBe(true);
  });



});
