"use strict";
(function () {

    var moment = require("moment");
    module.exports = function($rootScope, $log, MESSAGES, ProfileReferenceData) {
        var _this = this;

        _this.ProfileReferenceData = ProfileReferenceData.getInstance();
        _this.person = {};

        _this.passwordPrefix = "account-page";
        _this.submitted = false;

        _this.phoneFormat = /^\(?(\d{3})\)?[\s.-]?(\d{3})[\s.-]?(\d{4})$/;
        _this.zipFormat = /^(\d{5}([\-]\d{4})?)$/;
        _this.dateFormat = /^(0[1-9]|1[012])[- /.](0[1-9]|[12][0-9]|3[01])[- /.]((19|20)\d\d)$/;

        _this.loading = true;
        _this.viewReady = false;

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

                _this.viewReady = true;
            });
        };

        function configurePerson(person) {
            _this.person = person;

            if (_this.person.dateOfBirth !== undefined) {
                var newBirthDate = _this.person.dateOfBirth.replace(_this.dateFormat, "$3 $1 $2");
                var mBdate = moment(newBirthDate, "YYYY MM DD");
                _this.person.dateOfBirth = mBdate.format("MM/DD/YYYY");
            }

            if (_this.person.anniversaryDate !== undefined) {
                var mAdate = moment(new Date(_this.person.anniversaryDate));
                _this.person.anniversaryDate = mAdate.format("MM/DD/YYYY");
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
            //debugger;
            _this.person.$save(function () {
                $rootScope.$emit('notify', $rootScope.MESSAGES.profileUpdated);
                $log.debug("person save successful");
                if(_this.modalInstance !== undefined) {
                    _this.closeModal(true);
                }
            }, function () {
                $log.debug("person save unsuccessful");
            });
        };
        _this.isDobError = function () {
            return (_this.form.personal.birthdate.$touched || _this.form.personal.$submitted) && _this.form.personal.birthdate.$invalid;
        };
        _this.convertHomePhone = function () {
            if (_this.form.personal["home-phone"].$valid) {
                    _this.person.homePhone = _this.person.homePhone.replace(_this.phoneFormat, "$1-$2-$3");
                }
        };
        _this.convertPhone = function() {
            if (_this.form.personal["mobile-phone"].$valid) {
                _this.person.mobilePhone = _this.person.mobilePhone.replace(_this.phoneFormat, "$1-$2-$3");
            }
        };
        _this.serviceProviderRequired = function () {
            if (_this.person.mobilePhone === "undefined" || _this.person.mobilePhone === null || _this.person.mobilePhone === "" || this.form.personal["mobile-phone"].$invalid) {
                return false;
            }
            return true;
        };
    }

})();
