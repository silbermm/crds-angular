"use strict";
(function () {
    angular.module('crossroads').service('Session', ['$cookies', '$cookieStore', SessionService]);

    function SessionService($cookies, $cookieStore) {
        this.create = function (sessionId, userId) {
            $cookies.sessionId = sessionId;
            $cookies.userId = userId;
        };

        this.authenticated = function() {
            return $cookies.sessionId;
        }
        
        this.clear = function () {
            $cookieStore.remove("sessionId");            
            $cookieStore.remove("userId");
        }

        this.getUserRole = function () {
            return "";
        }

        this.destroy = function () {
            $cookies.sessionId = null;
            $cookies.userId = null;
        };
        return this;
    }
})()