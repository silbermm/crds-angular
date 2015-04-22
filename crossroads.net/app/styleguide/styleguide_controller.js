'use strict()';
(function () {
  module.exports = function ($scope, $location, $anchorScroll, growl, $modal) {
    var _this = this;

    _this.scrollTo = function (id) {
      $location.hash(id);
      console.log($location.hash());
      $anchorScroll();
    }

    _this.Growl = function (type, message) {
      growl[type](message);
    }


    _this.open = function (size) {

      var modalInstance = $modal.open({
        templateUrl: 'styleModalContent.html',
        backdrop: true,
        size: size,
      })
    }
  }
})();