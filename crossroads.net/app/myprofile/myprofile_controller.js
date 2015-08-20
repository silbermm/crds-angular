(function () {
  'use strict';
  module.exports = function MyProfileCtrl($scope, $log, $location, $anchorScroll, $modal) {
    var _this = this;
    _this.changeProfileImage = changeProfileImage;

    _this.isCollapsed = true;
    _this.phoneToggle = true;
    _this.myImage = '';
    _this.myCroppedImage = '';

    _this.householdPhoneFocus = function () {
      _this.isCollapsed = false;

      $location.hash('homephonecont');


      setTimeout(function () {
        $anchorScroll();
      }, 500);

      //$('#homephone').focus();
    };

    function changeProfileImage() {
      $modal.open({
        templateUrl: 'templates/profile_image_upload.html',
        controller: 'ChangeProfileImageCtrl as modal',
        backdrop: true
      });
    };

    var handleFileSelect = function (evt) {
      var file = evt.currentTarget.files[0];
      var reader = new FileReader();
      reader.onload = function (evt) {
        $scope.$apply(function ($scope) {
          _this.myImage = evt.target.result;
        });
      };
      reader.readAsDataURL(file);
    };
    angular.element(document.querySelector('#fileInput')).on('change', handleFileSelect);


  };
})();