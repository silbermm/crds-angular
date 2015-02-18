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
	        $log.debug("EmailNotifications:"+_this.account.EmailNotifications);
	        $log.debug("_this.account:"+_this.account);
	        $log.debug(_this.account);

	        if ($scope.required === "true" && (_this.password.password == null || _this.password.password == "")) {
	           $rootScope.$emit('notify', $rootScope.MESSAGES.generalError);
	           return;
	        }

            if (_this.password.password) {
                $log.debug("Save the password first!");
                _this.password.$save(function () {
                    $log.debug("password saved succesfully!");
                    $rootScope.$emit('notify', $rootScope.MESSAGES.profileUpdated);
                    _this.password.password = null;
                }, function () {
                    _this.password.password = null;
                    $log.error("did not save the password successfully. You need to have a Mobile Phone number set.");
                });
            }

            if ($scope.required === "true") {
                if (_this.form.account_form.password.$error.minlength) {
                    $log.debug("Password is too short!");
                    $rootScope.$emit('notify', $rootScope.MESSAGES.generalError);
                    throw "password length needs to be > 5"
                    return
                }
            }

            _this.account.$save(function () {
                $log.debug("save successful");
                $rootScope.$emit('notify', $rootScope.MESSAGES.profileUpdated);
            }, function () {
                $log.error("save unsuccessful");
            });
        }
    }

})()
