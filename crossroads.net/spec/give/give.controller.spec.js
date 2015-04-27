describe('GiveCtrl', function() {

  beforeEach(module('crossroads'));
  
  var $controller;
 
    
  beforeEach(inject(function(_$controller_){
    $controller = _$controller_;
          
  });
  
       
  it('should produce an amount error', function(){ 
   	var controller = $controller('GiveCtrl', { $scope: $scope });
   	// give.goToAccount() with null
    //check state, should still be give/amoumt
    //form should be invalid     
  });
  
  it('should change state because the amount was valid and prompt for login', function(){ 
   	var controller = $controller('GiveCtrl', { $scope: $scope });
   	// give.goToAccount() with  a valid amount
    //check state, should still be give/login
    //form should be valid  
  });

  
  });

});