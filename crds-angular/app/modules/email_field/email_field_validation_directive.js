(function () {

    angular.module("email_field").directive('uniqueEmail', ['$http', UniqueEmail]);

    function UniqueEmail($http) {
        return {
            restrict: 'A',
            require: 'ngModel',
            link: function (scope, element, attrs, ngModel) {
                ngModel.$asyncValidators.unique = function (email) {
                    return $http.get('api/lookup?email=' + encodeURI(email));
                };
            }
        };
    }

})()