(function () {
  'use strict';
  angular.module('crossroads.core').service('Session',SessionService);

  SessionService.$inject = ['$log','$cookies', '$http'];

  function SessionService($log, $cookies, $http) {
    var self = this;
    this.create = function (sessionId, userTokenExp, userId, username) {
      console.log('creating cookies!');
      var expDate = new Date();
      expDate.setTime(expDate.getTime() + (userTokenExp * 1000));
      $cookies.put('sessionId', sessionId, {
         'expires': expDate
      });
      $cookies.put('userId', userId);
      $cookies.put('username', username);

      // Set the defaults for $http in case the current page needs to
      // authenticate to API without a new $httpProvider being injected
      $http.defaults.headers.common['Authorization']= sessionId;
    };

    /*
     * This formats the family as a comma seperated string before storing in the
     * cookie called 'family'
     *
     * @param family - an array of participant ids
     */
    this.addFamilyMembers = function (family) {
      $log.debug('Adding ' + family + ' to family cookie');
      $cookies.put('family', family.join(','));
    };

    /*
     * @returns an array of participant ids
     */
    this.getFamilyMembers = function () {
      if(this.exists('family')){
        return _.map($cookies.get('family').split(','), function(strFam){
          return Number(strFam);
        });
      }
      return [];
    };

    this.isActive = function () {
      var ex = this.exists('sessionId');
      if (ex === undefined || ex === null ) {
          return false;
      }
      return true;
    };

    this.exists = function (cookieId) {
      return $cookies.get(cookieId);
    };

    this.clear = function () {
      $cookies.remove('sessionId');
      $cookies.remove('userId');
      $cookies.remove('username');
      $cookies.remove('family');
      $http.defaults.headers.common['Authorization']= undefined;
      return true;
    };

    this.getUserRole = function () {
        return '';
    };

    //TODO: Get this working to DRY up login_controller and register_controller
    this.redirectIfNeeded = function($state){

      if (self.hasRedirectionInfo()) {
        var url = self.exists('redirectUrl');
        var params = self.exists('params');
        self.removeRedirectRoute();
        if(params === undefined){
          $state.go(url);
        } else {
          $state.go(url, JSON.parse(params));
        }
      }
    };

    this.addRedirectRoute = function(redirectUrl, params) {
        $cookies.put('redirectUrl', redirectUrl);
		    $cookies.put('params', JSON.stringify(params));
    };

    this.removeRedirectRoute = function() {
        $cookies.remove('redirectUrl');
        $cookies.remove('params');
    };

    this.hasRedirectionInfo = function() {
        if (this.exists('redirectUrl') !== undefined) {
            return true;
        }
        return false;
    };

    return this;
  }

})();
