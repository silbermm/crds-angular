require('angular-module-resource');
require('angular-ui-router');
require('angular-bootstrap-npm');

var opportunity_services_module = angular.module("crdsOpportunity", ["ngResource", "ngMessages", "ui.bootstrap", "ui.router"]);

(function () {

    function opportunityService($resource) {
        return $resource(__API_ENDPOINT__ + "api/opportunity/:opportunityId");
    }
    opportunity_service_module.factory("Opportunity", ["$resource", opportunityService]);

})();
