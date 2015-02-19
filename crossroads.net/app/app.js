var angular = require('angular');
require('../styles/main.scss');﻿

require('./profile/module');
require('./cms/services/cms_services_module');
require('./opportunity/module');
require('angular-cookies');

//require('./services/session_service');

"use strict";
(function () {

    angular.module("crossroads", ["crdsProfile", "crdsCMS.services", "crdsOpportunity", "ui.router", "ngCookies"])

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

    // }).config(function (growlProvider) {
    //     growlProvider.globalPosition("top-center");
    //     growlProvider.globalTimeToLive(6000);
    //     growlProvider.globalDisableIcons(true);
    //     growlProvider.globalDisableCountDown(true);
    })
    .filter('html', ['$sce', function ($sce) {
        return function (val) {
            return $sce.trustAsHtml(val);
        };
    }])
    .controller("appCtrl", ["$scope", "$rootScope", "MESSAGES", "$http", "Message",
        function ($scope, $rootScope, MESSAGES, $http, Message) {

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
    require('./register/register_directive');
})()
