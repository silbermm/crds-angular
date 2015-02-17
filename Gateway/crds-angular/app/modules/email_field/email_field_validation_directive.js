(function () {

    angular.module("email_field").directive('uniqueEmail', ['$http', 'User', UniqueEmail]);

    function UniqueEmail($http, User) {
        return {
            restrict: 'A',
            require: 'ngModel',
            link: function (scope, element, attrs, ngModel) {
                ngModel.$asyncValidators.unique = function (email) {

                    return $http.get('api/lookup?email=' + encodeURI(email)).success(function(succ) {
                        User.email = email;
                    }).error(function(err) {
                        User.email = email;
                    });

                };
            }
        };
    }

})()