(function () {
  'use strict';
  module.exports = SingleMediaController;

  SingleMediaController.$inject = ['$rootScope', '$scope', '$sce', 'SingleMedia', 'ItemProperty', 'ParentMedia', 'ParentItemProperty', 'ImageURL', 'YT_EVENT'];

  function SingleMediaController($rootScope, $scope, $sce, SingleMedia, ItemProperty, ParentMedia, ParentItemProperty, ImageURL, YT_EVENT) {
    var vm = this;
    vm.msgisopen = true;
    vm.musicisopen = false;
    vm.showVideo = ShowVideo;
    vm.stopVideo = StopVideo;
    vm.sendControlEvent = SendControlEvent;
    vm.stillIsVisible = true;
    vm.videoIsVisible = false;

    vm.media = SingleMedia[ItemProperty][0];

    vm.imageUrl = ImageURL;

    // if the video url is bound directly in the iframe at some point, it will need to be marked as
    // trusted for Strict Contextual Escaping, such as --
    // $sce.trustAsResourceUrl("https://www.youtube.com/embed/" + _.get(vm.media, 'serviceId'));
    vm.videoUrl = _.get(vm.media, 'serviceId');
    $sce.trustAsResourceUrl(vm.videoUrl);

    $scope.YT_EVENT = YT_EVENT;

    function SendControlEvent(ctrlEvent) {
      $rootScope.$broadcast(ctrlEvent);
    };

    $scope.$on(YT_EVENT.STATUS_CHANGE, function(event, data) {
      $scope.yt.playerStatus = data;
    });

    if (ParentMedia){
      vm.parentMedia = ParentMedia[ParentItemProperty][0];
    }
    else {
      vm.parentMedia = false;
    }

    function ShowVideo() {
      vm.stillIsVisible = false;
      vm.videoIsVisible = true;
    };

    function StopVideo() {
      vm.sendControlEvent(YT_EVENT.STOP);
      vm.stillIsVisible = true;
      vm.videoIsVisible = false;
    };
  }

})();