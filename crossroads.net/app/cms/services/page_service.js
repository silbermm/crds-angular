var cms_services_module = require('module');﻿

module.exports = function ($resource) {
    return $resource(__CMS_ENDPOINT__ + '/api/Page/?link=:url', { url: '@_url' }, { cache: true });
};
