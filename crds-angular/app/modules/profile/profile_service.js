(function(){
    angular.module('crdsProfile').factory('Profile', ['$resource', ProfileService]);

    function ProfileService($resource) {
        return $resource('/api/profile/:id', { id: '@_id' }, {
            update: {
                method: 'PUT'
            }
        });
    }
})()