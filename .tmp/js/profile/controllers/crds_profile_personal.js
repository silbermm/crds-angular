'use strict';

angular.module('crdsProfile')

.controller('crdsProfilePersonal', function($scope, Profile, MaritalStatus, Gender) {
  MaritalStatus.all().then(function(statuses) {
    $scope.maritalStatuses = statuses;
  });

  Gender.all().then(function(genders) {
    $scope.genders = genders;
  });

  $scope.savePersonal = function(data) {
    Profile.saveContact(data);
  };
});
