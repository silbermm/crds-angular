'use strict()';
(function(){

	angular.module('crossroads').config(["$httpProvider", function ($httpProvider) {
        $httpProvider.defaults.useXDomain = true;
        $httpProvider.defaults.headers.common["X-Use-The-Force"] = true;
    }]);
})();
