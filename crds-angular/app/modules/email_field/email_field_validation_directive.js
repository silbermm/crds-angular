(function () {

    angular.module("email_field").directive('uniqueEmail', ['$http','$cookieStore', 'User', UniqueEmail]);

    function UniqueEmail($http,$cookieStore, User) {
        return {
            restrict: 'A',
            require: 'ngModel',
            link: function (scope, element, attrs, ngModel) {
                ngModel.$asyncValidators.unique = function (email) {
                    User.setEmail(email);
                    return $http.get('api/lookup?email=' + encodeURI(email));
                };
            }
        };
    }

})()