"use strict";
(function () {
    angular.module('crossroads').service('Session', ['$cookies', SessionService]);

    function SessionService($cookies) {
        this.create = function (sessionId, userId) {
            $cookies.sessionId = sessionId;
            $cookies.userId = userId;
        };

        this.authenticated = function() {
            return $cookies.sessionId;
        }
        
        this.clear = function () {
            $cookies.sessionId = null;
            $cookies.userId = null;
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