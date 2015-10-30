(function() {
  'use strict';

  require('blueimp-load-image');

  module.exports = changeProfileImageCtrl;

  changeProfileImageCtrl.$inject = ['$modalInstance', '$scope', '$timeout'];
    
  function changeProfileImageCtrl($modalInstance, $scope, $timeout) {

    var vm = this;

    vm.ok = ok;
    vm.cancel = cancel;
    vm.focus = focus;
    vm.handleFileSelect = handleFileSelect;
    vm.myImage = '';
    vm.myCroppedImage = '';
    vm.init = false;

    function handleFileSelect(evt) {
      loadImage(
        evt.currentTarget.files[0],
        function (file) {
          var reader = new FileReader();
          reader.onload = function(evt) {
            $scope.$apply(function($scope) {
              vm.myImage = evt.target.result;
            });
          };

          reader.readAsDataURL(file);
        }
      );
    }

    $timeout(function() {
      angular.element(document.querySelector('#fileInputModal')).on('change', handleFileSelect);
    });

    function ok() {
      $modalInstance.close(vm.myCroppedImage);
      vm.init = false;
    }

    function cancel() {
      $modalInstance.dismiss('cancel');
      vm.init = false;
    }

    function focus() {
      $timeout(function() {
        if (vm.init && angular.element(document.querySelector('#fileInputModal'))[0].files.length === 0) {
          cancel();
        }

        vm.init = true;
      }, 200);
    }
  }
})();
