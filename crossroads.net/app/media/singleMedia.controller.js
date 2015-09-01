(function () {
  'use strict';
  module.exports = SingleMediaController;

  SingleMediaController.$inject = ['$scope', 'SingleMedia', 'ItemProperty', 'ParentMedia', 'ParentItemProperty', 'ImageURL'];

  function SingleMediaController($scope, SingleMedia, ItemProperty, ParentMedia, ParentItemProperty, ImageURL) {
    var vm = this;

    vm.imageurl = ImageURL;
    vm.isMessage = (ItemProperty === 'messages');
    vm.isSubscribeOpen = false;
    vm.media = SingleMedia[ItemProperty][0];
    vm.setSoundCloudPlayer = setSoundCloudPlayer;
    vm.switchToAudio = switchToAudio;
    vm.switchToVideo = switchToVideo;
    vm.showVideo = showVideo;
    vm.showAudio = showAudio;
    vm.videoIsOpen = true;

    if (ParentMedia){
      vm.parentMedia = ParentMedia[ParentItemProperty][0];
    }
    else {
      vm.parentMedia = false;
    }

    function setSoundCloudPlayer(soundCloudPlayer) {
      vm.soundCloudPlayer = soundCloudPlayer;
    }

    function showAudio() {
      return !showVideo()
    }

    function showVideo() {
      return vm.videoIsOpen;
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

    function switchToAudio() {
      vm.videoIsOpen = false;

      //stopYouTubePlayer();
    }

    function switchToVideo() {
      vm.videoIsOpen = true;

      stopSoundCloudPlayer();
    }

    $scope.$on("$destroy", function() {
      stopSoundCloudPlayer();
    });
  }
})();