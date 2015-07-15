require('../../dependencies/dependencies');
require('../../core/core');
require('../../app/app');

describe('SVG Icon Directive', function() {

  var $compile, $rootScope, element, scope, isolateScope;

  beforeEach(function(){
    angular.mock.module('crossroads');
  });
 
  beforeEach(inject(function(_$compile_, _$rootScope_){
    $compile = _$compile_;
    $rootScope = _$rootScope_; 
    scope = $rootScope.$new();
  })); 
  

  it('should show the checkmark icon', function(){ 
    element = '<svg-icon icon=\'check-circle\'></svg-icon>';

    element = $compile(element)(scope);
    scope.$digest();
    isolateScope = element.isolateScope();
  
    expect(element.html()).toContain('icon-check-circle'); 
    expect(element.html()).toContain('#check-circle');
  });

  it('should show the cancel icon', function(){ 
    element = '<svg-icon icon=\'cancel-circle\'></svg-icon>';
    element = $compile(element)(scope);
    scope.$digest();
    isolateScope = element.isolateScope();
  
    expect(element.html()).toContain('icon-cancel-circle'); 
    expect(element.html()).toContain('#cancel-circle');
  });

});
