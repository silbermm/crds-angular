describe('GiveController', function() {

  beforeEach(module('crossroads'));
  var $controller, $rootScope, $scope, $state, $timeout, $httpProvider, Session, Profile;
 
    
  beforeEach(inject(function($injector){
    $controller = $injector.get('$controller');
    $rootScope = $injector.get('$rootScope');  
    $scope = $rootScope.$new();
    $state = $injector.get('$state');
    $timeout = $injector.get('$timeout'); 
    $httpProvider = $injector.get('$http'); 
    Session = $injector.get('Session');
    Profile = $injector.get('Profile');
  	$controller = $controller('GiveCtrl', { '$rootScope': $rootScope, '$scope': $scope, '$state': $state, '$timeout': $timeout, 
   		'$httpProvider': $httpProvider, 'Session': Session, 'Profile': Profile });
  }));
  
  it('should have the visa credit card class', function(){ 
    $controller.ccNumber = '4242424242424242';
    $controller.ccCardType();
    expect($controller.ccNumberClass).toBe("cc-visa"); 
  });

  it('should have the mastercard credit card class', function(){ 
    $controller.ccNumber = '5105105105105100';
    $controller.ccCardType();
    expect($controller.ccNumberClass).toBe("cc-mastercard"); 
  });
  
  it('should have the discover credit card class', function(){ 
    $controller.ccNumber = '6011111111111117';
    $controller.ccCardType();
    expect($controller.ccNumberClass).toBe("cc-discover"); 
  });

  it('should have the amex credit card class', function(){ 
    $controller.ccNumber = '378282246310005';
    $controller.ccCardType();
    expect($controller.ccNumberClass).toBe("cc-american-express"); 
  }); 
 
  it('should not a credit card class', function(){ 
    $controller.ccNumber = '';
    $controller.ccCardType();
    expect($controller.ccNumberClass).toBe(""); 
  }); 

});

  