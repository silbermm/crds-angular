angular.module('crdsProfile').controller('crdsProfileCtrl', [
    '$scope', 'Profile',
    function ($scope, Profile) {
        $scope.genders = ["Male", "Female"];
        Profile.get().then(function (data) {
            $scope.person = data;
        });
    }
]);