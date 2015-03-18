(function () {
    'use strict';
    module.exports = function MyProfileCtrl($scope, $log, $location, $anchorScroll) {
      var _this = this;

      _this.isCollapsed = true;
      $scope.phoneToggle = true;

      _this.householdPhoneFocus = function () {
        _this.isCollapsed = false;
        
        $location.hash('homephonecont');
        

        setTimeout(function () {
            $anchorScroll();
        }, 500);

        //$('#homephone').focus();
      };
    };
})();