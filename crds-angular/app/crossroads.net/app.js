"use strict";
(function () {
    angular.module("crossroads", ["ngResource","crdsProfile", "crdsOpportunity", "crdsCMS.services", "ui.router", "ngCookies", "angular-growl"])
    .run(["Session", "$rootScope", "MESSAGES", "$http", function (Session, $rootScope, MESSAGES, $http) {
        $rootScope.MESSAGES = MESSAGES;

        $http.get("api/authenticated").success(function (user) {
            // Authenticated                 
            $rootScope.userid = user.userId;
            $rootScope.username = user.username;
        }).error(function (data) {
            Session.clear();
            $rootScope.message = "You need to log in.";
            $rootScope.userid = null;
            $rootScope.username = null;
        });
    }])
    .constant("AUTH_EVENTS", {
            loginSuccess: "auth-login-success",
            loginFailed: "auth-login-failed",
            logoutSuccess: "auth-logout-success",
            sessionTimeout: "auth-session-timeout",
            notAuthenticated: "auth-not-authenticated",
            isAuthenticated : "auth-is-authenticated",
            notAuthorized: "auth-not-authorized"
    })
        //TODO I'm not sure if this is the best way, but I didn't want to hard code the IDs in the code...
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

    }).config(function(growlProvider) {
        growlProvider.globalPosition("top-center");
        growlProvider.globalTimeToLive(6000);
        growlProvider.globalDisableIcons(true);
        growlProvider.globalDisableCountDown(true);
    })
    .filter('html', ['$sce', function ($sce) {
        return function (val) {
            return $sce.trustAsHtml(val);
        };
    }])
    .controller("appCtrl", ["$scope", "$rootScope", "MESSAGES", "growl", "Session", "$http", "Message",
        function ($scope, $rootScope, MESSAGES, growl, Session, $http, Message) {

            var messagesRequest = Message.get("", function () {
                messagesRequest.messages.unshift(null);//Adding a null so the indexes match the DB
                //TODO Refactor to not use rootScope, should build an NgTemplate to use with NgMessages
                $rootScope.messages = messagesRequest.messages; 
            });

            $rootScope.$on("notify", function (event, id) {
                var message = Message.get({ id: id }, function () {
                    growl[message.message.type](message.message.message)
                });
            });
            $rootScope.$on("context", function (event, id) {
                var message = Message.get({ id: id }, function () {
                    return message.message.message;
                });
            });
        }
    ]);
})()