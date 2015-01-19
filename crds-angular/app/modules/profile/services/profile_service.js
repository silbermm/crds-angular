(function(){
    angular.module('crdsProfile').factory('Profile', ['$resource', ProfileService]);

    function ProfileService($resource) {

        return {
            Personal: $resource('api/profile'),
            Account: $resource('api/account', null, { 'update': { method: 'PUT' } }),
            Password: $resource('api/account/password'),
            //Household: $resource('api/household')
            MySkills: $resource('api/myskills') 

        }

    }
})()
