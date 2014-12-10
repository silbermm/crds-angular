'use strict';
(function () {
    angular.module("crdsProfile").controller('crdsProfileCtrl', ['Profile', 'Lookup', ProfileController]);

    function ProfileController(Profile, Lookup) {
        this.genders = Lookup.Genders.query();
        this.person = Profile.get({ id: 5 });
        this.maritalStatuses = Lookup.MaritalStatus.query();
        this.serviceProviders = Lookup.ServiceProviders.query();
        this.countries = Lookup.Countries.query();
        this.crossroadsLocations = Lookup.CrossroadsLocations.query();
    }
})()