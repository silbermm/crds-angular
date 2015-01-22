(function () {
    angular.module('crossroads').factory('Users', ['$resource', UserService]);

    //By declaring api/user a resource, angular provides us with helpful verbs to perform CRUD operations. (save/update)
    function UserService($resource) {

        return $resource('api/user');
    }
})()
