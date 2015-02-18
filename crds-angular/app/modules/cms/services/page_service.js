angular.module('crdsCMS.services').factory('Page', function ($resource) {
    return $resource('https://content.crossroads.net/api/Page/?URLSegment=:url', { url: '@_url' }, { cache: true });

});