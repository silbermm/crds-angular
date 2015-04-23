'use strict()';
(function(){
  
  require("angular-resource");
  require("angular-sanitize");
  require('angular-messages');
  require('angular-cookies');
  require('angular-growl');
  require('angular-toggle-switch');
  require('angular-ui-utils');
  require('angular-payments');

  require('angular').module('crossroads.core', [
    'ngResource',
    'ngSanitize',
    'ngPayments',
    'ui.router',
    'ui.utils',
    'ngCookies',
    'ngMessages',
    'angular-growl',
    'toggle-switch'
    ]);

})();
