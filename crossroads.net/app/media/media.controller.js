'use strict';
(function () {
  module.exports = MediaController;

  MediaController.$inject = ['Series'];

  function MediaController(Series) {
    var vm = this;
    vm.msgisopen = true;
    vm.musicisopen = false;
    vm.series = Series.series;
  }
})();
