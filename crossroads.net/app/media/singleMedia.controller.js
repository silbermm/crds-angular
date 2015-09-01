(function () {
  'use strict';
  module.exports = SingleMediaController;

  SingleMediaController.$inject = ['$scope', 'SingleMedia', 'ItemProperty', 'ParentMedia', 'ParentItemProperty', 'ImageURL'];

  function SingleMediaController($scope, SingleMedia, ItemProperty, ParentMedia, ParentItemProperty, ImageURL) {
    var vm = this;
    vm.msgisopen = true;
    vm.musicisopen = false;
    vm.media = SingleMedia[ItemProperty][0];
    vm.imageurl = ImageURL;
    vm.setSoundCloudPlayer = setSoundCloudPlayer;
    vm.switchToVideo = switchToVideo;

    if (ParentMedia){
      vm.parentMedia = ParentMedia[ParentItemProperty][0];
    }
    else {
      vm.parentMedia = false;
    }

    function setSoundCloudPlayer(soundCloudPlayer) {
      vm.soundCloudPlayer = soundCloudPlayer;
    }

    function stopSoundCloudPlayer(){
      if (!vm.soundCloudPlayer) {
        return;
      }

      if (!vm.soundCloudPlayer.playing) {
        return;
      }

      vm.soundCloudPlayer.pause();
    }

    function switchToVideo() {
      vm.musicisopen = false;
      vm.msgisopen = true;

      stopSoundCloudPlayer();
    }

    $scope.$on("$destroy", function() {
      stopSoundCloudPlayer();
    });

  }
})();