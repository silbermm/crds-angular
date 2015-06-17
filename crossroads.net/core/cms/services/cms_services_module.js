var cms_services_module = angular.module('crossroads.core');

cms_services_module.factory('Message', function ($resource) {
    return $resource(__CMS_ENDPOINT__ + '/api/Message/:id', { id: '@_id' }, {cache: true});
});

cms_services_module.factory('Page', function ($resource) {
    return $resource(__CMS_ENDPOINT__ + '/api/Page/?link=:url', { url: '@_url' }, { cache: true });
});
