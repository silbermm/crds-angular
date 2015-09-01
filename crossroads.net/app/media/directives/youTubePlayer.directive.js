(function() {
  'use strict';

  module.exports = YouTubePlayer;

  YouTubePlayer.$inject = ['$window', 'YT_EVENT'];

  function YouTubePlayer($window, YT_EVENT) {
    return {
      restrict: 'E',

      scope: {
        height: '@',
        width: '@',
        videoid: '@'
      },

      template: '<div></div>',

      link: function(scope, element) {
        var tag = document.createElement('script');
        tag.src = 'https://www.youtube.com/iframe_api';
        var firstScriptTag = document.getElementsByTagName('script')[0];
        firstScriptTag.parentNode.insertBefore(tag, firstScriptTag);

        var player;

        $window.onYouTubeIframeAPIReady = function() {

          player = new YT.Player(element.children()[0], {
            playerVars: {
              autoplay: 0,
              html5: 1,
              theme: 'light',
              modesbranding: 0,
              color: 'white',
              iv_load_policy: 3,
              showinfo: 1,
              controls: 1
            },

            height: scope.height,
            width: scope.width,
            videoId: scope.videoid
          });
        }

        scope.$watch('videoid', function(newValue, oldValue) {

          if (newValue == oldValue) {
            return;
          }

          player.cueVideoById(scope.videoid);

        });

        scope.$watch('height + width', function(newValue, oldValue) {
          if (newValue == oldValue) {
            return;
          }

          player.setSize(scope.width, scope.height);

        });

        scope.$on(YT_EVENT.STOP, function() {
          player.seekTo(0);
          player.stopVideo();
        });

        scope.$on(YT_EVENT.PLAY, function() {
          player.playVideo();
        });

        scope.$on(YT_EVENT.PAUSE, function() {
          player.pauseVideo();
        });

      }
    };
  }
})();
