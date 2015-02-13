(function () {
    angular.
        module('crossroads').
        factory('User', ['$resource','$log', UserService]);

    //By declaring api/user a resource, angular provides us with helpful verbs to perform CRUD operations. (save/update)
 

    function UserService($resource, $log) {
        $log.debug("Inside Users factory");
        var User = $resource('api/user');
        var newuser = new User();
        return newuser;
    }

})()
