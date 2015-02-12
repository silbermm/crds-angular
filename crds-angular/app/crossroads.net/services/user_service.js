(function () {
    angular.
        module('crossroads').
        factory('User', ['$resource','$log', UserService]);

    //By declaring api/user a resource, angular provides us with helpful verbs to perform CRUD operations. (save/update)
 

    function UserService($resource, $log) {
        $log.debug("Inside Users factory");

        var User = $resource('api/user');

        var newuser = new User();

        newuser.setEmail = function (data) { newuser.email = data }
        newuser.setPassword = function (data){newuser.password = data}

        newuser.getEmail = function () { return newuser.email }
        newuser.getPassword = function () { return  newuser.password }

        return newuser;
    }

})()
