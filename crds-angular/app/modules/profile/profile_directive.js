
angular.module('crdsProfile').directive('crdsProfile', [
    function() {
        return {
            restrict: 'EA',
            //template: 'Name: {{customer.name}}<br /> Street: {{customer.street}}<br />data: {{data}}'
            contoller: 'crdsProfileCtrl',
            controllerAs: 'profile',
            templateUrl: '/app/modules/profile/profile_personal.html'
        };
    }
]);

