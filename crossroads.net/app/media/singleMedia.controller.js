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

    vm.setSoundCloudPlayer = function(soundCloudPlayer) {
      vm.soundCloudPlayer = soundCloudPlayer;
    }

    vm.switchToVideo = function() {
      vm.musicisopen = false;
      vm.msgisopen = true;

      stopSoundCloudPlayer();
    }

    var stopSoundCloudPlayer = function(){
      if (!vm.soundCloudPlayer) {
        return;
      }

      if (!vm.soundCloudPlayer.playing) {
        return;
      }

      vm.soundCloudPlayer.pause();
    }

    if (ParentMedia){
      vm.parentMedia = ParentMedia[ParentItemProperty][0];
    }
    else {
      vm.parentMedia = false;
    }

    $scope.$on("$destroy", function() {
      stopSoundCloudPlayer();
    });

  }
})();