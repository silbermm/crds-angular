'use strict';

var moment = require('moment');

var getCookie =  function(cname) {
  var name = cname + '=';
  var ca = document.cookie.split(';');
  for (var i = 0; i < ca.length; i++) {
    var c = ca[i];
    while (c.charAt(0) === ' ') { c = c.substring(1); }
    if (c.indexOf(name) === 0) { return c.substring(name.length, c.length); }
  }
  return '';
};

// This custom type is needed to allow us to NOT URLEncode slashes when using ui-sref
// See this post for details: https://github.com/angular-ui/ui-router/issues/1119
var preventRouteTypeUrlEncoding = function(urlMatcherFactory, routeType, urlPattern) {
  return (urlMatcherFactory.type(routeType, {
    encode: function (val) {
      return val != null ? val.toString() : val;
    },
    decode: function (val) {
      return val != null ? val.toString() : val;
    },
    is: function (val) {
      return this.pattern.test(val);
    },
    pattern: urlPattern
  }));
};

//================================================
// Check if the user is connected
//================================================
var checkLoggedin = function ($q, $timeout, $http, $location, $rootScope, $cookies) {
  // TODO Added to debug/research US1403 - should remove after issue is resolved
  console.log('US1403: checkLoggedIn');
  var deferred = $q.defer();
  $http.defaults.headers.common['Authorization'] = $cookies.get('sessionId');
  $http({
    method: 'GET',
    url: __API_ENDPOINT__ + 'api/authenticated',
    headers: {
      'Authorization': $cookies.get('sessionId')
    }
  }).success(function (user) {
    // TODO Added to debug/research US1403 - should remove after issue is resolved
    console.log('US1403: checkLoggedIn success');
    // Authenticated
    if (user.userId !== undefined) {
      // TODO Added to debug/research US1403 - should remove after issue is resolved
      console.log('US1403: checkLoggedIn success with user');
      $timeout(deferred.resolve, 0);
      $rootScope.userid = user.userId;
      $rootScope.username = user.username;
    } else {
      // TODO Added to debug/research US1403 - should remove after issue is resolved
      console.log('US1403: checkLoggedIn success, undefined user');
      Session.clear();
      $rootScope.message = 'You need to log in.';
      $timeout(function () {
        deferred.reject();
      }, 0);
      $location.url('/');
    }
  }).error(function (e) {
    console.log(e);
    console.log('ERROR: trying to authenticate');
  });
  return deferred.promise;
};

/**
 * Takes a javascript date and returns a
 * string formated MM/DD/YYYY
 * @param date - Javascript Date
 * @param days to add - How many days to add to the original date passed in
 * @return string formatted in the way we want to display
 */
function formatDate(date, days){
  if(days === undefined){
    days = 0; 
  }
  var d = moment(date);
  d.add(days, 'd');
  return d.format('MM/DD/YY');
}

module.exports = {
  getCookie: getCookie,
  preventRouteTypeUrlEncoding: preventRouteTypeUrlEncoding,
  checkLoggedin: checkLoggedin,
  formatDate: formatDate
};
