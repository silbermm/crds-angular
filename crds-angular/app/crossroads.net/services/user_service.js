(function () {
    angular.module('crossroads').factory('Users', ['$resource', UserService]);

    function UserService($resource) {

        return $resource('api/user');
    }
})()
