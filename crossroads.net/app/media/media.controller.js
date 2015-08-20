'use strict';
(function () {
  module.exports = MediaCtrl;

  MediaCtrl.$inject = ['Series', 'Musics', 'Videos'];

  function MediaCtrl(Series, Musics, Videos) {
    var vm = this;
    vm.msgisopen = true;
    vm.musicisopen = false;
    vm.series = Series.series;
    vm.musics = Musics.musics;
    vm.videos = Videos.videos;
  }
})();
