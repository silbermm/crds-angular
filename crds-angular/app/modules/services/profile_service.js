(function(){
    angular.module('crdsServices').factory('Profile', ['$resource', ProfileService]);

    function ProfileService($resource) {
        //TODO This file seems like a near duplicate, should this be removed?
        return {
            Personal: $resource('api/profile'),
            Account: $resource('api/account'),
            Password: $resource('api/account/password')
        }
    }
})()
