'use strict()';
(function(){

  var app = angular.module("crossroads.core");
  app.config(AppConfig);

  AppConfig.$inject = ['$httpProvider', '$locationProvider', 'datepickerConfig', 'datepickerPopupConfig', '$cookiesProvider'];

  function AppConfig($httpProvider, $locationProvider, datepickerConfig, datepickerPopupConfig, $cookiesProvider){
    $locationProvider.html5Mode({
        enabled:true,
        requireBase:false
    });

    $httpProvider.defaults.useXDomain = true;
    $httpProvider.defaults.headers.common['Authorization']= crds_utilities.getCookie('sessionId');
    // This is a dummy header that will always be returned in any 'Allow-Header' from any CORS request. This needs to be here because of IE.
    $httpProvider.defaults.headers.common["X-Use-The-Force"] = true;

    configureDefaultCookieScope($cookiesProvider);
    configureDatePickersDefaults(datepickerConfig, datepickerPopupConfig);
  }

  var configureDefaultCookieScope =  function($cookiesProvider) {
    $cookiesProvider.defaults.path = "/";
  }

  var configureDatePickersDefaults = function (datepickerConfig, datepickerPopupConfig) {
    datepickerConfig.showWeeks = false;
    datepickerPopupConfig.showWeeks = false;
  }

})();
