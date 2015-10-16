(function () {
  'use strict';
  module.exports = function changeProfileImageCtrl($modalInstance, $scope, $timeout) {
    var _this = this;

    _this.ok = ok;
    _this.cancel = cancel;
    _this.handleFileSelect = handleFileSelect;
    _this.myImage = '';
    _this.myCroppedImage = '';

    function handleFileSelect(evt) {
      var file = evt.currentTarget.files[0];
      var reader = new FileReader();
      reader.onload = function (evt) {
        $scope.$apply(function ($scope) {
          _this.myImage = evt.target.result;
        });
      };
      reader.readAsDataURL(file);
    }

    $timeout(function () {
      angular.element(document.querySelector('#fileInputModal')).on('change', handleFileSelect);
    });

    function ok() {
      $modalInstance.close(_this.myCroppedImage);
    }

    function cancel() {
      $modalInstance.dismiss('cancel');
    }
  };
})();
