var cms_services_module = require('module');﻿

module.exports = function ($resource) {
    return $resource('https://content.crossroads.net/api/Page/?URLSegment=:url', { url: '@_url' }, { cache: true });
};
