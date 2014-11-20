'use strict';

angular.module('crossroads')

.controller('AppCtrl', function($rootScope, $log, growl) {

  $rootScope.$on('notify.success', function(event, message) {
    growl.success(lookup(message));
  });

  $rootScope.$on('notify.info', function(event, message) {
    growl.info(lookup(message));
  });

  $rootScope.$on('notify.warning', function(event, message) {
    growl.warning(lookup(message));
  });

  $rootScope.$on('notify.error', function(event, message) {
    growl.error(lookup(message));
  });

  var lookup = function(messageKey) {
    switch (messageKey) {
      case 'form.success':
        return 'Your request has been submitted successfully';
      case 'form.validation.error':
        return 'Your request has errors';
      case 'form.server.error':
        return 'An error has occurred';
      default:
        return messageKey;
    }
  };
});
