(function(){
    angular.module('crdsServices').factory('Profile', ['$resource', ProfileService]);

    function ProfileService($resource) {

        return {
            Personal: $resource('api/profile'),
            Account: $resource('api/account'),
            Password: $resource('api/account/password')
        }
    }
})()
