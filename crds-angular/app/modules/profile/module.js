'use strict';
(function(){
    angular.module('crdsProfile', ['ngResource', 'ui.bootstrap', 'ui.router']).config(['$httpProvider', '$stateProvider', '$urlRouterProvider', function($httpProvider, $stateProvider, $urlRouterProvider) {
        $httpProvider.defaults.timeout = 15000;

        $stateProvider
        .state('profile', {
            url: '/profile',
            templateUrl: 'app/modules/profile/templates/profile.html',
            controller: 'crdsProfileCtrl as profile',
            data: {
                require_login: true
            }
        })
        .state('profile.account', {

        })
        ;

        $urlRouterProvider.otherwise("/profile");

    }]);
})()
