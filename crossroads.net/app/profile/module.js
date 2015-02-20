'use strict';

require('angular-module-resource');
require('angular-bootstrap-npm');
require('angular-ui-router');
require('../password_field/password_field_directive');
require('../email_field/email_field_directive');

angular.module('crdsProfile', ['ngResource', 'ngMessages', 'ui.bootstrap', 'ui.router', 'password_field','email_field'])
  .config(['$httpProvider', '$stateProvider', '$urlRouterProvider', function ($httpProvider, $stateProvider, $urlRouterProvider) {
    $httpProvider.defaults.timeout = 15000;
}]);
