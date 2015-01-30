(function () {

    function opportunityService($resource) {
        return $resource("api/opportunity/:opportunityId");
    }
    angular.module("crdsOpportunity").factory("Opportunity", ["$resource", opportunityService]);

})();
