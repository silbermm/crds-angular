'use strict';
(function () {
    angular.module("crdsProfile").controller("ProfilePersonalController", ['$rootScope', '$log','MESSAGES', 'genders', 'maritalStatuses', 'serviceProviders', 'states', 'countries', 'crossroadsLocations', 'person', ProfilePersonalController]);

    function ProfilePersonalController($rootScope, $log, MESSAGES, genders, maritalStatuses, serviceProviders, states, countries, crossroadsLocations, person) {
        var _this = this;

        _this.person = person;
        _this.genders = genders;
        _this.martialStatuses = maritalStatuses;
        _this.serviceProviders = serviceProviders;
        _this.states = states;
        _this.countries = countries;
        _this.crossroadsLocations = crossroadsLocations;

        _this.phoneFormat = /^\(?(\d{3})\)?[\s.-]?(\d{3})[\s.-]?(\d{4})$/;
        _this.zipFormat = /^(\d{5}([\-]\d{4})?)$/;
        _this.dateFormat = /^(0[1-9]|1[012])[- /.](0[1-9]|[12][0-9]|3[01])[- /.]((19|20)\d\d)$/;
      
        _this.loading = true;

        _this.initProfile = function (form) {
            _this.form = form;
            //configurePerson();
        }

        function configurePerson() {
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

        //_this.stateChanged = function () {
        //    if (_this.currentState.State_Abbreviation) {
        //         _this.person.State = _this.currentState.State_Abbreviation;
        //    }
        //    $log.debug(_this.person.State);
        //}

        //_this.countryChanged = function () {
        //    if (_this.currentCountry.dp_RecordName) {
        //        _this.person.Foreign_Country = _this.currentCountry.dp_RecordName;
        //    }
        //    $log.debug(_this.person.Foreign_Country);            
        //}

        _this.savePersonal = function () {

            $log.debug(_this.person);

            if (_this.form.personal.$invalid) {
                $log.debug("The form is invalid!");
                $rootScope.$emit('notify.error', MESSAGES.generalError);
                return 
            }
            _this.person["State/Region"] = _this.person.State
            _this.person.$save(function () {
                $log.debug("person save successful");
            }, function () {
                $log.debug("person save unsuccessful");
            });
        }

        _this.isDobError = function () {            
            return (_this.form.personal.birthdate.$touched || _this.form.personal.$submitted) && _this.form.personal.birthdate.$invalid
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