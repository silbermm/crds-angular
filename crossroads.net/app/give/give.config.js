'use strict()';
(function(){
	var getCookie = require('../utilities/cookies');
	angular.module('crossroads.give').config(["$httpProvider", function ($httpProvider) {
        $httpProvider.defaults.useXDomain = true;
        //$httpProvider.defaults.headers.common['Authorization'] = getCookie('sessionId');
        // This is a dummy header that will always be returned in any 'Allow-Header' from any CORS request. This needs to be here because of IE.
        $httpProvider.defaults.headers.common["X-Use-The-Force"] = true;
    }]);
})();