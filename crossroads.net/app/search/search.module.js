'use strict';
var constants = require('../constants');

require('./search-results.html');

angular.module(constants.MODULES.SEARCH, ['crossroads.core', 'crossroads.common'])
  .config(require('./search.routes'))
  .factory('Search', require('./search.service'))
  .controller('SearchCtrl', require('./search.controller'))
  .filter('limitToEllip', function () {
    return function (input, limit) {
       if (input && input.length > limit) {
         return input.slice(0, limit) + '&hellip;';
       }
       return input;
    };
  })
  .filter('htmlToPlaintext', function() {
    return function(text) {
      return  text ? String(text).replace(/<[^>]+>/gm, '') : '';
    };
  })
  ;
