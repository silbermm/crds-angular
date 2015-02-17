'use strict';
(function(){
    angular.module('crdsProfile', ['ngResource', 'ngMessages', 'ui.bootstrap', 'ui.router', 'password_field','email_field']).config(['$httpProvider', '$stateProvider', '$urlRouterProvider', function ($httpProvider, $stateProvider, $urlRouterProvider) {
        $httpProvider.defaults.timeout = 15000;
    }]);
})()
