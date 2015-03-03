(function(){

    
    
    module.exports = function ($http, Session) {

      var userid = Session.exists('userId') !== undefined ? Session.exists('userId') : 0;
      return {
        restrict: 'A',
        require: 'ngModel',
        link: function(scope, element, attrs, ngModel) {
            ngModel.$asyncValidators.unique = function (email) {
                return $http.get(__API_ENDPOINT__ + 'api/lookup/' + userid  + '/find/?email=' +  encodeURI(email));
              };
          }
        };
    }

})()
