'use strict';
(function () {
    angular.module("crdsProfile").controller('crdsProfileCtrl', ['Profile', 'Lookup','$log',  ProfileController]);

    function ProfileController(Profile, Lookup, $log) {
        this.genders = Lookup.Genders.query();
<<<<<<< HEAD
        this.profile = Profile.get();
=======
        this.person = Profile.Personal.get();
>>>>>>> 696d5cae9fd7f4e5a4965d6d1b9a00df9d775468
        this.maritalStatuses = Lookup.MaritalStatus.query();
        this.serviceProviders = Lookup.ServiceProviders.query();
        this.states = Lookup.States.query();
        this.countries = Lookup.Countries.query();
        this.crossroadsLocations = Lookup.CrossroadsLocations.query();
        this.account = Profile.Account.get();

        this.savePersonal = function (profile) {
<<<<<<< HEAD
            profile.$save(function () {
=======
            
            profile.person.$save(function () {
>>>>>>> 696d5cae9fd7f4e5a4965d6d1b9a00df9d775468
                //on success give message
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