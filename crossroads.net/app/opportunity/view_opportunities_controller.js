"use strict";
module.exports = function viewOpportunitiesController($log, messages, opportunity) {
    var _this = this;
    _this.submitRequest = function(opportunityId) {
        _this.opportunity = opportunity.save({ "opportunityId": opportunityId }, null).$promise.then(function(opportunity) {
            _this.created = true;
        }, function() {
            _this.rejected = true;
        });
    }
}