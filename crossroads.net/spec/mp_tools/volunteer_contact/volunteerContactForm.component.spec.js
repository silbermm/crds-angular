require('crds-core');
require('../../../app/ang');
require('../../../app/ang2');

require('../../../app/common/common.module');
require('../../../app/app');

describe('Volunteer Contact Form Component', function() {

  var CONSTANTS = require('crds-constants');
  var MODULE = CONSTANTS.MODULES.CROSSROADS;
  var helpers = require('./contact.helpers');

  var $compile;
  var $rootScope;
  var element;
  var scope;
  var $window;
  var mockWindow;
  var $httpBackend;
  var isolated;
  var Validation;

  beforeEach(function() {
    angular.mock.module(MODULE);
  });

  beforeEach(inject(function(_$compile_, _$rootScope_, $injector) {
    $compile = _$compile_;
    $rootScope = _$rootScope_;
    scope = $rootScope.$new();

    spyOn($rootScope, '$emit').and.callThrough();

    $rootScope.MESSAGES = {
      generalError: 'generalError'
    };

    $httpBackend = $injector.get('$httpBackend');
    $window = $injector.get('$window');
    Validation = $injector.get('Validation');

    scope.group = helpers.group;
    element = '<volunteer-contact-form group=\'group\'></volunteer-contact-form>';
    element = $compile(element)(scope);

    scope.$digest();
    isolated = element.isolateScope();
  }));

  it('should be invalid if nothing is choosen', function() {
    expect(isolated.contactForm.save()).toBe(false);
    expect($rootScope.$emit).toHaveBeenCalledWith('notify', 'generalError');
  });

  it('should be valid if all required fields are filled out', function() {
    isolated.contactForm.data.eventChooser.$setViewValue('fakeData');
    isolated.contactForm.data.subject.$setViewValue('fakeData');
    isolated.contactForm.data.body.$setViewValue('fakeData');
    expect(isolated.contactForm.save()).toBe(true);
  });

  it('should not show the entire form until an event has been choosen', function() {
    expect(isolated.contactForm.eventChoosen()).toBe(false);
  });

  it('should show the entire form when an event has been choosen', function() {
    isolated.contactForm.data.eventChooser.$setViewValue('fakeData');
    expect(isolated.contactForm.eventChoosen()).toBe(true);
  });

  it('should create a valid formated timestamp', function() {
    var timestamp = isolated.contactForm.eventDateTime(helpers.group.events[0]);
    expect(timestamp).toEqual('12/02/2015 02:00PM - 04:00PM');
  });

  it('should get the list of the current volunteer recipients', function() {
    isolated.contactForm.formData.event = helpers.group.events[0];
    isolated.contactForm.formData.recipients = 'current';
    isolated.contactForm.eventChanged();
    $httpBackend.expectGET(window.__env__['CRDS_API_ENDPOINT'] +
                           'api/group/' + helpers.group.groupId +
                           '/event/' + helpers.group.events[0].eventId +
                          '?recipients=current').respond(200);
    $httpBackend.flush();
  });

  it('should get the list of the potential volunteers', function() {
    isolated.contactForm.formData.event = helpers.group.events[0];
    isolated.contactForm.formData.recipients = 'potential';
    isolated.contactForm.eventChanged();
    $httpBackend.expectGET(window.__env__['CRDS_API_ENDPOINT'] +
                           'api/group/' + helpers.group.groupId +
                           '/event/' + helpers.group.events[0].eventId +
                          '?recipients=potential').respond(200);
    $httpBackend.flush();

  });

});
