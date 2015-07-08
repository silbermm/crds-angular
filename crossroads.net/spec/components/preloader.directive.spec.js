describe('preloader', function() {

  var $compile, $rootScope, element, scope, isolateScope;

  beforeEach(function() {
    module('crossroads.core');
  });

  beforeEach(inject(function(_$compile_, _$rootScope_) {
    $compile = _$compile_;
    $rootScope = _$rootScope_;
  }));

  it('should not have a fullscreen class', function(){
    scope = $rootScope.$new();
    element = '<preloader full-screen=\'false\'></preloader>';
    element = $compile(element)(scope);
    scope.$digest();
    isolateScope = element.isolateScope();
    expect(isolateScope.fullScreen).toBe(false);
    expect(isolateScope.isFullScreen()).toBe(false);
  });

  it('should have a fullscreen class', function(){
    scope = $rootScope.$new();
    element = '<preloader full-screen=\'true\'></preloader>';
    element = $compile(element)(scope);
    scope.$digest();
    isolateScope = element.isolateScope();
    expect(isolateScope.fullScreen).toBe(true);
    expect(isolateScope.isFullScreen()).toBe(true);
  });

});
