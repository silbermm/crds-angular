'use strict';
(function () {
    angular.module("crdsProfile").controller("ProfilePersonalController", ['$rootScope', '$filter', 'Profile', 'Lookup', '$log','MESSAGES', ProfilePersonalController]);

    function ProfilePersonalController($rootScope, $filter, Profile, Lookup, $log, MESSAGES) {
        var _this = this;

        _this.phoneFormat = /^\(?(\d{3})\)?[\s.-]?(\d{3})[\s.-]?(\d{4})$/;
        _this.zipFormat = /^(\d{5}([\-]\d{4})?)$/;
        _this.dateFormat = /^(0[1-9]|1[012])[- /.](0[1-9]|[12][0-9]|3[01])[- /.](19|20)\d\d$/;
      

        _this.loading = true;

        _this.initProfile = function (form) {
            _this.form = form;
            _this.genders = Lookup.query({ table: "genders" });
            _this.maritalStatuses = Lookup.query({table: "maritalstatus"});
            _this.serviceProviders = Lookup.query({ table: "serviceproviders" });
            _this.states = Lookup.query({ lookup: "states" }, function (s) {
                _this.statesSelect = _.map(s, function (val) {
                    return val.State_Abbreviation;
                });
            });
            _this.countries = Lookup.query({ table: "countries" });
            _this.crossroadsLocations = Lookup.query({table: "crossroadslocations"});
            _this.person = Profile.Personal.get(function () {
                _this.loading = false;
                _this.person.Anniversary_Date = $filter('date')(new Date(_this.person.Anniversary_Date), 'MM/dd/yyyy');
                _this.currentState = _this.person.State;
                _this.currentCountry = _this.person.Foreign_Country;
            });
            
        }

        _this.stateChanged = function () {
            if (_this.currentState.State_Abbreviation) {
                 _this.person.State = _this.currentState.State_Abbreviation;
            }
            $log.debug(_this.person.State);
        }

        _this.countryChanged = function () {
            if (_this.currentCountry.dp_RecordName) {
                _this.person.Foreign_Country = _this.currentCountry.dp_RecordName;
            }
            $log.debug(_this.person.Foreign_Country);            
        }

        _this.savePersonal = function () {

            $log.debug(_this.person);

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
                if (_this.form.personal.homephone.$valid) {
                    _this.person.Home_Phone = _this.person.Home_Phone.replace(_this.phoneFormat, '$1-$2-$3');
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