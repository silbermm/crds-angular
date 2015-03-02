(function(){

    
    
    module.exports = function ($http, Session) {
        return {
            restrict: 'A',
            require: 'ngModel',
            link: function(scope, element, attrs, ngModel) {
                ngModel.$asyncValidators.unique = function (email) {
                    return $http.get(__API_ENDPOINT__ + 'api/lookup/' + Session.exists('userId')  + '/find/?email=' +  encodeURI(email));
                  };
            }
        };
    }

})()
