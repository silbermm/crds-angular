(function() {
  'use strict';

  module.exports = Media;

  function Media($resource) {
    return {
      Series: function(params) {
        return $resource(__CMS_ENDPOINT__ + 'api/series/:id',
          params,
          {
            get: { method:'GET', cache: true}
          });
      },

      SingleMedia: function(params) {
        return $resource(__CMS_ENDPOINT__ + 'api/singleMedia/:id',
          params,
          {
            get: { method:'GET', cache: true}
          });
      },

      Musics: function(params) {
        return $resource(__CMS_ENDPOINT__ + 'api/musics/:id',
          params,
          {
            get: { method:'GET', cache: true}
          });
      },

      Videos: function(params) {
        return $resource(__CMS_ENDPOINT__ + 'api/videos/:id',
          params,
          {
            get: { method:'GET', cache: true}
          });
      },

      Messages: function(params) {
        return $resource(__CMS_ENDPOINT__ + 'api/message/:id',
          params,
          {
            get: { method:'GET', cache: true}
          });

      }
    };
  }
})();
