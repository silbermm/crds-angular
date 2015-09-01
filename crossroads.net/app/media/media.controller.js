(function () {
  'use strict';
  module.exports = MediaController;

  MediaController.$inject = ['Series', 'Musics', 'Videos'];

  function MediaController(Series, Musics, Videos) {
    var vm = this;
    vm.series = Series.series;
    vm.musics = Musics.musics;
    vm.videos = Videos.videos;
  }
})();
