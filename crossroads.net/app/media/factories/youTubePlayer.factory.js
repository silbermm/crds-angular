(function() {
  'use strict()';

  module.exports = YouTubePlayerFactory;

  function YouTubePlayerFactory($q, $window) {

    var deferred = $q.defer();
    var apiReady = deferred.promise;

    $window.onYouTubeIframeAPIReady = function() {
      deferred.resolve();
    }

    return {
      onReady: function(callback) {
        apiReady.then(callback);
      }
    }

  }

})();
