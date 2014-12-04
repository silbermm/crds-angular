angular.module('crdsProfile').controller('crdsProfileCtrl', [
    '$scope', 'Profile',
    function ($scope, Profile) {

        Profile.get().then(function (data) {
            $scope.person = data;
        });
    }
]);