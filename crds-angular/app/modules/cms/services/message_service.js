angular.module('crdsCMS.services',[]).factory('Message', function ($resource) {
    return $resource('http://content.crossroads.net/api/Message/:id', { id: '@_id' });
});