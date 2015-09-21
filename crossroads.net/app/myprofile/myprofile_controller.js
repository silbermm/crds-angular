(function () {
  'use strict';
  module.exports = function MyProfileCtrl($scope, $log, $location, $anchorScroll, $modal) {
    var _this = this;

    //METHODS
    _this.openModal = openModal;

    //VARIABLES
    _this.profileImage = "//crossroads-media.imgix.net/images/avatar.svg";
    _this.isCollapsed = true;
    _this.phoneToggle = true;

    _this.householdPhoneFocus = function () {
      _this.isCollapsed = false;

      $location.hash('homephonecont');


      setTimeout(function () {
        $anchorScroll();
      }, 500);

      //$('#homephone').focus();
    };

    function openModal() {

      var changeProfileImage = $modal.open({
        templateUrl: 'templates/profile_image_upload.html',
        controller: 'ChangeProfileImageCtrl as modal',
        backdrop: true
      });

      changeProfileImage.result.then(function (croppedImage) {
        _this.profileImage = croppedImage;
      });

    };

  };
})();
