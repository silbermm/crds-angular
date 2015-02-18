angular.module('crdsCMS.services',[]).factory('Message', function ($resource) {
    return $resource('https://content.crossroads.net/api/Message/:id', { id: '@_id' }, {cache: true});
});