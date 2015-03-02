(function(){

    
    
    module.exports = function ($http) {
        return {
            restrict: 'A',
            require: 'ngModel',
            link: function(scope, element, attrs, ngModel) {
                ngModel.$asyncValidators.unique = function (email) {
                    return $http.get(__API_ENDPOINT__ + 'api/lookup?email=' + encodeURI(email));
                };
            }
        };
    }

})()
