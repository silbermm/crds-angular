(function () {
    'use strict';
    module.exports = function MyProfileCtrl($scope, $log, $location, $anchorScroll, messages, opportunity, Profile) {
      var _this = this;

      _this.isCollapsed = true;
      $scope.phoneToggle = true;

      //$location.hash('homephone');
      //console.log($location.hash());

      _this.householdPhoneFocus = function () {
        console.log('hello');
        _this.isCollapsed = false;

        //$location.hash('homephone');
        //console.log($location.hash());

        //$anchorScroll();
        //$('#homephone').focus();
      };
    };
})();