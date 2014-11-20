'use strict';

angular.module('crdsProfile')

.controller('crdsProfileHousehold',
  function($scope, $filter, Profile, Congregations, States, Countries) {

  Congregations.web().then(function(congregations) {
    $scope.congregations = congregations;
  });

  States.all().then(function(states) {
    $scope.states = states;
  });

  Countries.all().then(function(countries) {
    $scope.countries = countries;
  });

  $scope.saveHousehold = function() {
    Profile.saveHousehold($scope.household);
    Profile.saveAddress($scope.address);
  };

  $scope.setCountry = function() {
    $scope.address.foreignCountry = $scope.countryObj.country;
    $scope.address.countryCode = $scope.countryObj.code3;
  };
});
