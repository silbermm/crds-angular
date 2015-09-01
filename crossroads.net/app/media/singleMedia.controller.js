(function () {
  'use strict';
  module.exports = SingleMediaController;

  SingleMediaController.$inject = ['$rootScope', '$scope', 'SingleMedia', 'ItemProperty', 'ParentMedia', 'ParentItemProperty', 'ImageURL', 'YT_event'];

  function SingleMediaController($rootScope, $scope, SingleMedia, ItemProperty, ParentMedia, ParentItemProperty, ImageURL, YT_event) {
    var vm = this;
    vm.msgisopen = true;
    vm.musicisopen = false;
    vm.showvideo = ShowVideo;
    vm.stopVideo = StopVideo;
    vm.sendControlEvent = SendControlEvent;
    vm.stillisvisible = true;
    vm.videoisvisible = false;

    vm.media = SingleMedia[ItemProperty][0];

    vm.imageurl = ImageURL;

    // if the video url is bound directly in the iframe at some point, it will need to be marked as
    // trusted for Strict Contextual Escaping, such as --
    // $sce.trustAsResourceUrl("https://www.youtube.com/embed/" + _.get(vm.media, 'serviceId'));
    vm.videourl = _.get(vm.media, 'serviceId');
    debugger;

    $scope.yt = {
      width: 600,
      height: 480,
      videoid: vm.videourl,
      playerStatus: "NOT PLAYING"
    };

    $scope.YT_event = YT_event;

    function SendControlEvent(ctrlEvent) {
      $rootScope.$broadcast(ctrlEvent);
    };

    $scope.$on(YT_event.STATUS_CHANGE, function(event, data) {
      $scope.yt.playerStatus = data;
    });

    if (ParentMedia){
      vm.parentMedia = ParentMedia[ParentItemProperty][0];
    }
    else {
      vm.parentMedia = false;
    }

    function ShowVideo() {
      vm.stillisvisible = false;
      vm.videoisvisible = true;
    };

    function StopVideo() {
      vm.sendControlEvent(YT_event.STOP);
      vm.stillisvisible = true;
      vm.videoisvisible = false;
    };
  }

})();