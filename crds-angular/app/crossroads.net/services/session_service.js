"use strict";
(function () {
    angular.module('crossroads').service('Session', ['$cookies', '$cookieStore',SessionService]);

    function SessionService($cookies, $cookieStore) {
        this.create = function (sessionId, userId, username) {
            console.log("creating cookies!");
            $cookies.sessionId = sessionId;
            $cookies.userId = userId;
            $cookies.username = username;

        };

        this.exists = function (cookieId) {
            return $cookies[cookieId];
        };
        
        this.clear = function () {
            $cookieStore.remove("sessionId");
            $cookieStore.remove("userId");
            $cookieStore.remove("username");
        };

        this.getUserRole = function () {
            return "";
        };

        return this;
    }
})()