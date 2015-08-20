'use strict';
(function () {
  module.exports = MediaCtrl;

  MediaCtrl.$inject = ['Series'];

  function MediaCtrl(Series) {
    var vm = this;
    vm.msgisopen = true;
    vm.musicisopen = false;
    vm.series = Series.series;
  }
})();
