(function () {
  'use strict';
  module.exports = function confirmPassword($modalInstance, $scope, $timeout) {
    var _this = this;

    _this.ok = ok;
    _this.cancel = cancel;

    function ok() {
      $modalInstance.close();
    };

    function cancel() {
      $modalInstance.dismiss('cancel');
    };
  };
})();