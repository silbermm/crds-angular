require('crds-core');
require('../../app/ang');

require('../../app/childcare');

describe('Childcare Event Service', function() {
  var MODULE = require('crds-constants').MODULES.CHILDCARE;
  var helper = require('./childcare.helpers');

  var $rootScope;
  var scope;
  var childcareEvent;

  beforeEach(angular.mock.module(MODULE));

  beforeEach(inject(function(_ChildcareEvents_) {
    childcareEvent = _ChildcareEvents_;
  }));

  it('should store an event in the event property', function() {
    childcareEvent.setEvent(helper.event);
    expect(childcareEvent.event).toEqual(helper.event);
  });

  it('should store an array of events in the events property', function() {
    childcareEvent.setEvents(helper.childcareEvents);
    expect(childcareEvent.events).toEqual(helper.childcareEvents);
  });

  it('should store an event in the childcareEvent property', function() {
    childcareEvent.setChildcareEvent(helper.childcareEvent);
    expect(childcareEvent.childcareEvent).toEqual(helper.childcareEvent);
  });

  it('should store an array of children', function() {
    childcareEvent.setChildren(helper.children);
    expect(childcareEvent.children).toEqual(helper.children);
  });
});
