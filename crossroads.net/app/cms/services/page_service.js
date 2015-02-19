var cms_services_module = require('module');﻿

cms_service_module.factory('Page', function ($resource) {
    return $resource('http://content.crossroads.net/api/Page/?URLSegment=:url', { url: '@_url' }, { cache: true });

});