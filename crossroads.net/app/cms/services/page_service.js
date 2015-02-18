angular.module('crdsCMS.services').factory('Page', function ($resource) {
    return $resource('http://content.crossroads.net/api/Page/?URLSegment=:url', { url: '@_url' }, { cache: true });

});