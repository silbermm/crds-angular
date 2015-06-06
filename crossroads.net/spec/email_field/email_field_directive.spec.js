describe('Email Field Directive', function() {
  var mockSession, mockUser;

  var emailModel, emailPrefix, formSubmitted, emailNotFound, onEmailNotFound, emailFound, onEmailFound, validateUnique, checkUnique, focused;

  var element, scope, isolateScope;

  var $compile, $rootScope, $templateCache, $httpBackend, $compile, $rootScope;

  beforeEach(function(){
    module('crossroads', function($provide){
      mockSession = jasmine.createSpyObj('Session', ['exists', 'isActive']);
      mockSession.exists.and.callFake(function(something){
        return undefined;
      });
      mockSession.isActive.and.callFake(function() {
        return(false);
      });
      $provide.value('Session', mockSession);

      mockUser = jasmine.createSpy('User');
      $provide.value('User', mockUser);
    });
  });

  beforeEach(
    inject(function(_$compile_, _$rootScope_, _$templateCache_, _$httpBackend_) {
      $compile = _$compile_;
      $rootScope = _$rootScope_;
      $templateCache = _$templateCache_;
      $httpBackend = _$httpBackend_;

      emailModel = "me@here.com";
      emailPrefix = "myPrefix";
      formSubmitted = false;
      emailNotFound = false;
      onEmailNotFound = function() {
        emailNotFound = true;
      };
      emailFound = false;
      onEmailFound = function() {
        emailFound = true;
      };
      validateUnique = true;
      checkUnique = true;
      focused = false;

      $templateCache.put('on-submit-messages', '<span ng-message="required">Required</span>');
      $templateCache.put('on-blur-messages',
        '<span ng-message="unique">Not a Unique Email</span>'
        + '<span ng-message="email">Invalid Email</span>');

      $httpBackend.whenGET(window.__env__['CRDS_API_ENDPOINT'] + 'api/lookup/0/find/?email=me@here.com').respond(404, "not found");

      scope = $rootScope.$new();
      element =
        '<email-field '
        + 'email="model.emailModel" '
        + 'prefix="model.emailPrefix" '
        + 'submitted="model.formSubmitted" '
        + 'on-email-not-found="model.onEmailNotFound" '
        + 'on-email-found="model.onEmailFound" '
        + 'validate-unique="model.validateUnique" '
        + 'check-unique="model.checkUnique" '
        + 'focused="model.focused"/>';

      scope.model = {
        emailModel: emailModel,
        emailPrefix: emailPrefix,
        formSubmitted: formSubmitted,
        onEmailNotFound: onEmailNotFound,
        onEmailFound: onEmailFound,
        validateUnique: validateUnique,
        checkUnique: checkUnique,
        focused: focused,
      };

      element = $compile(element)(scope);
      scope.$digest();
      isolateScope = element.isolateScope();
    })
  );

  describe("markup", function(){
    it("should have an id set with the prefix value", function(){
      expect(element.html()).toContain("id=\"" + emailPrefix + "-email\"");
    });

    it("should have the autofocus attribute set", function() {
      expect(element.html()).toContain("autofocus");
    });
  });

});
