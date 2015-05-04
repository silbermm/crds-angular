'use strict()';
(function(){
	
	angular.module('crossroads.give').config(["$httpProvider", function ($httpProvider) {
        $httpProvider.defaults.useXDomain = true;
        $httpProvider.defaults.headers.common["X-Use-The-Force"] = true;
    }]);
})();