'use strict';
(function () {
    angular.module("crdsProfile").controller('crdsProfileCtrl', ['Profile', 'Lookup', '$q', '$log',  ProfileController]);

    function ProfileController(Profile, Lookup, $q, $log) {
        
	    var _this = this;
      
	    
        
        _this.initAccount = function () {
            _this.account = Profile.Account.get();
            _this.password = new Profile.Password();
            $log.debug(_this.password);
        }

        _this.saveAccount = function () {
            $log.debug(_this.password.password);
            $log.debug(_this.account.EmailNotifications);

            if (_this.password.password) {
                $log.debug("Save the password first!");
                _this.password.$save(function () {
                    $log.debug("password saved succesfully!");
                    _this.password.password = null;
                }, function () {
                    _this.password.password = null;
                    $log.error("did not save the password successfully. You need to have a Mobile Phone number set.");
                });
            }

            _this.account.$save(function () {
                $log.debug("save successful");
                
            }, function () {
                $log.error("save unsuccessful");
            });
        }
    }

})()
