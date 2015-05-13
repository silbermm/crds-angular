"use strict";
(function () {

  angular.module("crossroads").service("Session",SessionService);
  
  SessionService.$inject = ['$log','$cookies', '$cookieStore'];

  function SessionService($log, $cookies, $cookieStore) {
    var self = this;
    this.create = function (sessionId, userId, username) {
      console.log("creating cookies!");
      $cookies.sessionId = sessionId;
      $cookies.userId = userId;
      $cookies.username = username;
    };

    /*
     * This formats the family as a comma seperated string before storing in the 
     * cookie called 'family'
     *
     * @param family - an array of participant ids
     */
    this.addFamilyMembers = function (family) {
      $log.debug("Adding " + family + " to family cookie");
      $cookies.family = family.join(",");
    };

    /*
     * @returns an array of participant ids
     */
    this.getFamilyMembers = function () {
      if(this.exists('family')){
        return _.map($cookies.family.split(","), function(strFam){
          return Number(strFam);  
        });
      }
      return [];
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
      $cookieStore.remove('family');
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
        } else {
          $state.go(url,{link:link});
        }
      }
    };

    this.addRedirectRoute = function(redirectUrl, link) {
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

})()
