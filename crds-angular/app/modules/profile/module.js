'use strict';
(function(){
    angular.module('crdsProfile', ['ngResource', 'ui.bootstrap']).config(['$httpProvider', function($httpProvider) {
        $httpProvider.defaults.timeout = 15000;
    }]);
})()
