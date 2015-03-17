'use strict';

require('angular-module-resource');
require('angular-bootstrap-npm');
require('angular-ui-router');
require('angular-messages');

require('../password_field/password_field_directive');
require('../email_field/email_field_directive');

var app = require('angular');

app.module('crossroads.profile', ['ngResource', 'ngMessages', 'ui.bootstrap', 'ui.router', 'password_field','email_field'])
 
