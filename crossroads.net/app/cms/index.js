'use strict';
var app = require('angular').module('crdsCMS');
var cms_services = require('angular').module('crdsCMS.services', ['ngResource']);

cms_services.factory('Message',['$resouce', require('./message_factory')]);
cms_services.factory('Page', ['$resource', require('./page_factory')]);

