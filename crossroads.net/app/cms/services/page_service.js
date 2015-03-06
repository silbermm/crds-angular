var cms_services_module = require('module');﻿

module.exports = function ($resource) {
    return $resource(__CMS_ENDPOINT__ + '/api/Page/?URLSegment=:url', { url: '@_url' }, { cache: true });
};
