require('crds-core');
require('../../../app/common/common.module');
require('../../../app/app');

describe('Youngest Age Allowed Validation', function() {

  var scope;
  var form;
  var $rootScope;
  var $compile;
  var httpBackend;
  var template;

  angular.mock.module('crossroads.common');

  beforeEach(angular.mock.module(function($provide) {
    $provide.value('$state', { get: function() {} });
  }));

  beforeEach(inject(function($injector, _$compile_, _$rootScope_) {
    $compile = _$compile_;
    $rootScope = _$rootScope_;
    httpBackend = $injector.get('$httpBackend');

    template = '<form name=\'form\'>' +
      '<input type=\'text\' validate-youngest-age ' +
      'youngest-age=\'{{youngestAgeAllowed}}\' ng-model=\'age\' name=\'age\' >' +
      '</form>';
    scope = $rootScope.$new();
  }));

  describe('Youngest age provided', function() {

    beforeEach(function() {
      scope.age = null;
      scope.youngestAgeAllowed = 8;
      var element = $compile(template)(scope);
      form = scope.form;
    });

    it('should not be valid if i am below the youngest age allowed', function() {
      form.age.$setViewValue('04/03/2008');
      scope.$digest();
      expect(form.age.$valid).toBeFalsy();
    });

    it('should be valid if i am above the age of the youngest age allowed', function() {
      form.age.$setViewValue('02/21/1980');
      scope.$digest();
      expect(form.age.$valid).toBeTruthy();
    });

    it('should be valid if i am the exact age of the youngest age allowed', function() {
      var eightYearsAgo = moment().subtract(8, 'years');
      var stringDate = eightYearsAgo.format('MM/DD/YYYY');
      form.age.$setViewValue(stringDate);
      scope.$digest();
      expect(form.age.$valid).toBeTruthy();
    });

  });

  describe('Youngest age is null', function() {

    beforeEach(function() {
      scope.age = null;
      var element = $compile(template)(scope);
      form = scope.form;
    });

    it('should be valid when youngest age is null or undefined', function() {
      form.age.$setViewValue('02/21/1980');
      scope.$digest();
      expect(form.age.$valid).toBeTruthy();
    });

  });
});
