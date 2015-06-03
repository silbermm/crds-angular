'use strict';

require('../password_field/password_field_directive');
require('../email_field/email_field_directive');

angular.module('crossroads.profile', ['ngResource', 'ngMessages', 'ui.bootstrap', 'ui.router', 'password_field','email_field'])
