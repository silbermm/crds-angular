'use strict';
(function () {
    angular.module("crdsProfile").controller('crdsProfileCtrl', ['$rootScope','Profile', 'Lookup', '$q', '$log','$scope',  ProfileController]);

    function ProfileController($rootScope, Profile, Lookup, $q, $log, $scope) {
    	$log.debug("Inside the ProfileController");
        
        var _this = this;

        _this.passwordPrefix = "account-page";

        _this.initAccount = function () {
            $log.debug("Account is initialized");
            _this.account = new Profile.Account();
            _this.account.$get();
            _this.password = new Profile.Password();
        }

	    _this.saveAccount = function (form) {
	        _this.form = form;
	        if ($scope.required === "true" && (form.account_form.password_field.password == null || form.account_form.password_field.password == "")) {
	           $rootScope.$emit('notify', $rootScope.MESSAGES.generalError);
	           return;
	        }

	        if (form.account_form.password_field.password && !form.account_form.password_field.password.$error.minlength) {
                $log.debug("Save the password first!");
                _this.password.$save(function () {
                	if (form.account_form.password_field.password.$error.minlength) {
                    $log.debug("Password is too short!");
                    $rootScope.$emit('notify', $rootScope.MESSAGES.generalError);
                    throw "password length needs to be > 5";
                    return;
                }
                    $log.debug("password saved succesfully!");
                    $rootScope.$emit('notify', $rootScope.MESSAGES.profileUpdated);
                    _this.password.password = null;
                });
            }

                

            _this.account.$save(function () {
            	if (form.account_form.password_field.password.$error.minlength) {
                    $log.debug("Password is too short!");
                    $rootScope.$emit('notify', $rootScope.MESSAGES.generalError);
                    throw "password length needs to be > 5";
                    return;
                }
                $log.debug("save successful");
                _this.password.password = null;
                $rootScope.$emit('notify', $rootScope.MESSAGES.profileUpdated);
            }, function () {
                $log.error("save unsuccessful");
                _this.password.password = null;
            });
        }
    }

})()
