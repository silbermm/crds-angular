"use strict";
(function () {

    function SessionService($cookies, $cookieStore) {
        this.create = function (sessionId, userId, username) {
            console.log("creating cookies!");
            $cookies.sessionId = sessionId;
            $cookies.userId = userId;
            $cookies.username = username;

        };

        this.isActive = function () {
            var ex = this.exists("sessionId");
            if (ex === undefined || ex === null ) {
                return false;
            }
            return true;
        };

        this.exists = function (cookieId) {
            return $cookies[cookieId];
        };
        
        this.clear = function () {
            $cookieStore.remove("sessionId");
            $cookieStore.remove("userId");
            $cookieStore.remove("username");
            return true;
        };

        this.getUserRole = function () {
            return "";
        };

        this.addRedirectRoute = function(redirectUrl, redirectParams) {
            $cookies.redirectUrl = redirectUrl;
            $cookies.redirectParams = redirectParams;
        };

        this.removeRedirectRoute = function() {
            $cookieStore.remove("redirectUrl");
            $cookieStore.remove("redirectParams");
        };

        this.hasRedirectionInfo = function() {
            if (this.exists("redirectUrl") !== undefined) {
                return true;
            }
            return false;
        }

        return this;
    }

    angular.module("crossroads").service("Session", ["$cookies", "$cookieStore", SessionService]);

})()
