'use strict()';
(function () {
  module.exports = function ($scope, $location, messages, $modal) {
    var _this = this;

    _this.open = function (size) {

      var modalInstance = $modal.open({
        templateUrl: 'corkboardModalContent.html',
        backdrop: true,
        size: size,
      })
    }
  }
})();
