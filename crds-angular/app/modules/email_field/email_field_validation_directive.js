(function () {

    angular.module("email_field").directive('uniqueEmail', ['$http', 'User', UniqueEmail]);

    function UniqueEmail($http, User) {
        return {
            restrict: 'A',
            require: 'ngModel',
            link: function (scope, element, attrs, ngModel) {
                ngModel.$asyncValidators.unique = function (email) {
                    User.setEmail(email); // This sets the email in our User model because angular prevents binding after the email_field is set to invalid - which we don't want to happen
                    return $http.get('api/lookup?email=' + encodeURI(email));
                };
            }
        };
    }

})()