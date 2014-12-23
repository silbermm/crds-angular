'use strict';
(function () {
    angular.module("crdsProfile").controller('crdsProfileCtrl', ['Profile', 'Lookup','$log',  ProfileController]);

    function ProfileController(Profile, Lookup, $log) {
        this.genders = Lookup.Genders.query();

        this.person = Profile.Personal.get();

        this.maritalStatuses = Lookup.MaritalStatus.query();
        this.serviceProviders = Lookup.ServiceProviders.query();
        this.states = Lookup.States.query();
        this.countries = Lookup.Countries.query();
        this.crossroadsLocations = Lookup.CrossroadsLocations.query();
        this.account = Profile.Account.get();
       
        this.savePersonal = function () {

            $log.debug("profile controller");
            this.person.$save(function () {
                $log.debug("person save successful");
            }, function () {
                $log.debug("person save unsuccessful");
            });
        }

        this.saveAccount = function () {
            $log.debug(this.account.NewPassword);
            $log.debug(this.account.emailPrefs);
            account.$save(function () {
                $log.debug("save successful");
            }, function () {
                $log.error("save unsuccessful");
            });
        }
    }

})()