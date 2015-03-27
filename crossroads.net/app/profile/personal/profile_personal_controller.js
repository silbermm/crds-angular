"use strict";
(function () {

    var moment = require("moment");
    module.exports = function($rootScope, $log, MESSAGES, ProfileReferenceData) {
        var _this = this;

        _this.ProfileReferenceData = ProfileReferenceData;
        // _this.ProfileReferenceData.then(function(response) {
        //     _this.person = response.person;
        //     _this.genders = response.genders;
        //     _this.maritalStatuses = response.maritalStatuses;
        //     _this.serviceProviders = response.serviceProviders;
        //     _this.states = response.states;
        //     _this.countries = response.countries;
        //     _this.crossroadsLocations = response.crossroadsLocations;
        // });
        _this.person = {};
        // _this.genders = [];
        // _this.maritalStatuses = [];
        // _this.serviceProviders = [];
        // _this.states = [];
        // _this.countries = [];
        // _this.crossroadsLocations = [];

        _this.passwordPrefix = "account-page";
        _this.submitted = false;

        _this.phoneFormat = /^\(?(\d{3})\)?[\s.-]?(\d{3})[\s.-]?(\d{4})$/;
        _this.zipFormat = /^(\d{5}([\-]\d{4})?)$/;
        _this.dateFormat = /^(0[1-9]|1[012])[- /.](0[1-9]|[12][0-9]|3[01])[- /.]((19|20)\d\d)$/;

        _this.loading = true;

        _this.initProfile = function (form) {
            _this.form = form;
            _this.ProfileReferenceData.then(function(response) {
                _this.genders = response.genders;
                _this.maritalStatuses = response.maritalStatuses;
                _this.serviceProviders = response.serviceProviders;
                _this.states = response.states;
                _this.countries = response.countries;
                _this.crossroadsLocations = response.crossroadsLocations;
                configurePerson(response.person);
            });
        };

        function configurePerson(person) {
            _this.person = person;

            if (_this.person.Date_of_Birth !== undefined) {
                var newBirthDate = _this.person.Date_of_Birth.replace(_this.dateFormat, "$3 $1 $2");
                var mBdate = moment(newBirthDate, "YYYY MM DD");
                _this.person.Date_of_Birth = mBdate.format("MM/DD/YYYY");
            }

            if (_this.person.Anniversary_Date !== undefined) {
                var mAdate = moment(new Date(_this.person.Anniversary_Date));
                _this.person.Anniversary_Date = mAdate.format("MM/DD/YYYY");
            }
        }

        _this.savePersonal = function () {
            _this.submitted = true;
            $log.debug(_this.form.personal);
            if (_this.form.personal.$invalid) {
                $rootScope.$emit('notify', $rootScope.MESSAGES.generalError);
                _this.submitted = false;
                return;
            }
            _this.person["State/Region"] = _this.person.State;
            _this.person.$save(function () {
                $log.debug("person save successful");
            }, function () {
                $log.debug("person save unsuccessful");
            });
        };
        _this.isDobError = function () {
            return (_this.form.personal.birthdate.$touched || _this.form.personal.$submitted) && _this.form.personal.birthdate.$invalid;
        };
        _this.convertHomePhone = function () {
            if (_this.form.personal["home-phone"].$valid) {
                    _this.person.Home_Phone = _this.person.Home_Phone.replace(_this.phoneFormat, "$1-$2-$3");
                }
        };
        _this.convertPhone = function() {
            if (_this.form.personal["mobile-phone"].$valid) {
                _this.person.Mobile_Phone = _this.person.Mobile_Phone.replace(_this.phoneFormat, "$1-$2-$3");
            }
        };
        _this.serviceProviderRequired = function () {
            if (_this.person.Mobile_Phone === "undefined" || _this.person.Mobile_Phone === null || _this.person.Mobile_Phone === "" || this.form.personal["mobile-phone"].$invalid) {
                return false;
            }
            return true;
        };
    }

})();
