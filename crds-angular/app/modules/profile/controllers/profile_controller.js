'use strict';
(function () {
    angular.module("crdsProfile").controller('crdsProfileCtrl', ['Profile', 'Lookup', ProfileController]);

    angular.module("crdsProfile").controller('crdsProfilePersonalCtrl', ['Profile', ProfilePersonalController]);
    angular.module("crdsProfile").controller('crdsProfileHouseholdCtrl', ['Profile', ProfileHouseholdController]);

    function ProfileController(Profile, Lookup) {
        this.genders = Lookup.Genders.query();
        this.profile = Profile.get();
        this.maritalStatuses = Lookup.MaritalStatus.query();
        this.serviceProviders = Lookup.ServiceProviders.query();
        this.states = Lookup.States.query();
        this.countries = Lookup.Countries.query();
        this.crossroadsLocations = Lookup.CrossroadsLocations.query();

        this.savePersonal = function (profile) {
            profile.person.$save(function () {
                //on success give message
            });
        }
    }

    function ProfilePersonalController(Profile) {
        this.personal = Profile.get();
    }

    function ProfileHouseholdController(Profile) {
        this.household = Profile.get();
    }

})()