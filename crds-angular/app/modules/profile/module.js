'use strict';
(function(){
    angular.module('crdsProfile', ['ngResource', 'ui.bootstrap', 'ui.router']).config(['$httpProvider', '$stateProvider', '$urlRouterProvider', function ($httpProvider, $stateProvider, $urlRouterProvider) {
        $httpProvider.defaults.timeout = 15000;
        

    }]);
})()
