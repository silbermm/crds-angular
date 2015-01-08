'use strict';
(function () {
    angular.module("crdsProfile").controller("ProfilePersonalController", ['$rootScope', 'Profile', 'Lookup', '$log','MESSAGES', ProfilePersonalController]);

    function ProfilePersonalController($rootScope, Profile, Lookup, $log, MESSAGES) {
        var _this = this;

        _this.phoneFormat = /^\(?(\d{3})\)?[\s.-]?(\d{3})[\s.-]?(\d{4})$/;
        _this.zipFormat = /^(\d{5}([\-]\d{4})?)$/;

      


        _this.loading = true;

        _this.initProfile = function (form) {
            _this.form = form;
            _this.genders = Lookup.query({ table: "genders" });
            _this.maritalStatuses = Lookup.query({table: "maritalstatus"});
            _this.serviceProviders = Lookup.query({ table: "serviceproviders" });
            _this.states = Lookup.query({lookup: "states"});
            _this.countries = Lookup.query({table: "countries"});
            _this.crossroadsLocations = Lookup.query({table: "crossroadslocations"});
            _this.person = Profile.Personal.get(function () {
                _this.loading = false;                
            });
            
        }

        _this.savePersonal = function () {
            if (_this.form.personal.$invalid) {
                $log.debug("The form is invalid!");
                $rootScope.$emit('notify.error', MESSAGES.generalError);
                return 
            }
            _this.person.$save(function () {
                $log.debug("person save successful");
            }, function () {
                $log.debug("person save unsuccessful");
            });
        }

        _this.convertHomePhone = function () {
            if (_this.person.Home_Phone) {
                if (_this.form.personal.homephone.$valid) {
                    _this.person.Home_Phone = _this.person.Home_Phone.replace(_this.phoneFormat, '$1-$2-$3');
                }
            }
        }

        _this.convertPhone = function () {
            if(_this.form.personal.mobile.$valid) {                
                _this.person.Mobile_Phone = _this.person.Mobile_Phone.replace(_this.phoneFormat, '$1-$2-$3');
            }
        }

        _this.serviceProviderRequired = function () {
            if (_this.person.Mobile_Phone === 'undefined' || _this.person.Mobile_Phone === null || _this.person.Mobile_Phone === "") {
                return false;
            }
            return true;
        }

    }

})()