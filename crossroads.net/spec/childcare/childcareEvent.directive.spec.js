require('crds-core');
require('../../app/ang');
require('../../app/ang2');

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
  var ChildcareService;
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

    ChildcareService = $injector.get('ChildcareService');
    $httpBackend = $injector.get('$httpBackend');

    scope = $rootScope.$new();
    element = '<childcare-event children=\'children\' childcare-event=\'childcareEvent\'></childcare-event>';
    scope.children = ChildcareEvents.children;
    scope.childcareEvent = ChildcareEvents.childcareEvent;
    element = $compile(element)(scope);
    scope.$digest();
    isolate = element.isolateScope();

    spyOn($rootScope, '$emit').and.callThrough();

    $rootScope.MESSAGES = {
      chooseOne: 'chooseOne',
      childcareSaveSuccessful: 'saved',
      childcareSaveError: 'error'
    };

  }));

  afterEach(function() {
    $httpBackend.verifyNoOutstandingExpectation();
    $httpBackend.verifyNoOutstandingRequest();
  });

  it('should not save the form if no children have been selected', function() {
    expect(isolate.childcareEvent.submit()).toBe(false);
  });

  it('should display a growl notification if there are no children selected to save', function() {
    expect(isolate.childcareEvent.submit()).toBe(false);
    expect($rootScope.$emit).toHaveBeenCalled();
  });

  it('should save the form if children have been selected', function() {
    scope.children[0].selected = true;
    expect(isolate.childcareEvent.submit()).toBe(true);
  });

  it('should send the correct object to the api', function() {
    isolate.childcareEvent.children[0].selected = true;

    var participants = {
      eventId: isolate.childcareEvent.childcareEvent.EventId,
      participants: [
        scope.children[0].participantId
      ]
    };

    $httpBackend.expectPOST(window.__env__['CRDS_API_ENDPOINT'] + 'api/childcare/rsvp', participants).respond(200);
    isolate.childcareEvent.submit();

    $httpBackend.flush();
    expect($rootScope.$emit).toHaveBeenCalledWith('notify', 'saved');
  });

  it('should display an error message with save is unsuccessful', function() {
    isolate.childcareEvent.children[0].selected = true;

    var participants = {
      eventId: isolate.childcareEvent.childcareEvent.EventId,
      participants: [
        scope.children[0].participantId
      ]
    };

    $httpBackend.expectPOST(window.__env__['CRDS_API_ENDPOINT'] + 'api/childcare/rsvp', participants).respond(500);
    isolate.childcareEvent.submit();

    $httpBackend.flush();
    expect($rootScope.$emit).toHaveBeenCalledWith('notify', 'error');
  });

});
