'use strict';

angular.module('crdsAuth', [])

.factory('Auth', function($http, $rootScope, $q) {
  return {
    getCurrentUser: function() {
      var deferred;
      deferred = $q.defer();
      $http.get(
        '/api/ministryplatformapi/PlatformService.svc/GetCurrentUserInfo')
        .then(function(response) {
          if (typeof response.data === 'object') {
            deferred.resolve(response.data);
          } else {
            deferred.resolve(null);
          }
        }, function(error) {
          return deferred.reject(error);
        });
      return deferred.promise;
    },

    logout: function() {
      return $http['delete']('/logout').then(function() {
        $rootScope.currentUser = null;
      });
    },

    login: function(username, password) {
      var data, deferred;
      deferred = $q.defer();
      data = {
        username: username,
        password: password
      };
      $http({
        url: '/login',
        method: 'POST',
        data: $.param(data),
        headers: {
          'Content-Type': 'application/x-www-form-urlencoded',
        Authorization: null
        }
      }).success(function(data) {
        $rootScope.$emit('login:hide');
        return deferred.resolve(data);
      }).error(function(data, status) {
        if (status === 0) {
          console.log('Could not reach API');
          return deferred.reject('Could not reach API');
        } else {
          console.log('Invalid username & password');
          return deferred.reject('Invalid username & password');
        }
      });
      return deferred.promise;
    }
  };
});
