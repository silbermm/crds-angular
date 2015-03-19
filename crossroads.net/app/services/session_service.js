"use strict";
(function () {

    function SessionService($cookies, $cookieStore) {
        var self = this;
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

        //TODO: Get this working to DRY up login_controller and register_controller
        this.redirectIfNeeded = function($state){
            if (self.hasRedirectionInfo()) {
                var url = self.exists("redirectUrl");
                var link = self.exists("link");
                self.removeRedirectRoute();
                if(link === undefined){
                    $state.go(url);
                }
                else
                {
                    $state.go(url,{link:link});
                }
            }
        };

        this.addRedirectRoute = function(redirectUrl, urlSegment) {
            $cookies.redirectUrl = redirectUrl;
            $cookies.link = link;
        };

        this.removeRedirectRoute = function() {
            $cookieStore.remove("redirectUrl");
            $cookieStore.remove("link");
        };

        this.hasRedirectionInfo = function() {
            if (this.exists("redirectUrl") !== undefined) {
                return true;
            }
            return false;
        };

        return this;
    }

    angular.module("crossroads").service("Session", ["$cookies", "$cookieStore", SessionService]);

})()
