var cms_services_module = require('module');﻿

cms_service_module.factory('Message', function ($resource) {
    return $resource(__CMS_ENDPOINT__ + '/api/Message/:id', { id: '@_id' }, {cache: true});
});