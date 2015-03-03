'use strict';

var angular = require('angular');

require("angular-resource");
require("angular-sanitize");
require('angular-messages');
require('angular-cookies');
require('angular-growl');
require('angular-snap');
require('./templates/nav.html');
require('./templates/nav-mobile.html');

require('./third-party/snap/snap.min.js');
require('../node_modules/angular-snap/angular-snap.min.css');

require('../styles/main.scss');
require('./profile');
require('./cms/services/cms_services_module');

require('./third-party/angular/angular-growl.css');

"use strict";
(function () {

    angular.module("crossroads", ['ngResource', "crdsProfile", "crdsCMS.services", "ui.router", "ngCookies", "ngMessages", 'angular-growl', 'snap'])

    .constant("AUTH_EVENTS", {
            loginSuccess: "auth-login-success",
            loginFailed: "auth-login-failed",
            logoutSuccess: "auth-logout-success",
            sessionTimeout: "auth-session-timeout",
            notAuthenticated: "auth-not-authenticated",
            isAuthenticated : "auth-is-authenticated",
            notAuthorized: "auth-not-authorized"
    })
    //TODO Pull out to service and/or config file
    .constant("MESSAGES", {
        generalError: 1,
        emailInUse: 2,
        fieldCanNotBeBlank: 3,
        invalidEmail: 4,
        invalidPhone: 5,
        invalidData: 6,
        profileUpdated: 7,
        photoTooSmall: 8,
        credentialsBlank: 9,
        loginFailed: 10,
        invalidZip: 11,
        invalidPassword: 12,
        successfullRegistration: 13,
        succesfulResponse: 14,
        failedResponse: 15
    }).config(function (growlProvider) {
        growlProvider.globalPosition("top-center");
        growlProvider.globalTimeToLive(6000);
        growlProvider.globalDisableIcons(true);
        growlProvider.globalDisableCountDown(true);
    })
    .config(function(snapRemoteProvider) {
        snapRemoteProvider.globalOptions = {
            disable: 'right',
            touchToDrag: false
        };
    })
    .filter('html', ['$sce', function ($sce) {
        return function (val) {
            return $sce.trustAsHtml(val);
        };
    }])
    .controller("appCtrl", ["$scope", "$rootScope", "MESSAGES", "$http", "Message", "growl",
        function ($scope, $rootScope, MESSAGES, $http, Message, growl) {
  
          console.log(__API_ENDPOINT__);

            $scope.prevent = function(evt) {
                evt.stopPropagation();
            };
            
            var messagesRequest = Message.get("", function () {
                messagesRequest.messages.unshift(null);//Adding a null so the indexes match the DB
                //TODO Refactor to not use rootScope, now using ngTemplate w/ ngMessages but also need to pull this out into a service
                $rootScope.messages = messagesRequest.messages;
            });

            $rootScope.error_messages = '<div ng-message="required">This field is required</div><div ng-message="minlength">This field is too short</div>';

            $rootScope.$on("notify", function (event, id) {
                growl[$rootScope.messages[id].type]($rootScope.messages[id].message);
           });

            $rootScope.$on("context", function (event, id) {
                var message = Message.get({ id: id }, function () {
                    return message.message.message;
                });
            });
        }
    ]);
    require('./apprun');
    require('./routes');
    require('./register/register_directive');
    
    require('./login');
})()
