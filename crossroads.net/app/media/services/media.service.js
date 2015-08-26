"use strict()";
(function () {

  module.exports = Media;

  function Media($resource) {
    return {
      Series: function (params) {
        return $resource(__CMS_ENDPOINT__ + 'api/series/',
          params,
          {
            'get': { method:'GET', cache: true}
          });
      },
      Musics: function(params) {
        return $resource(__CMS_ENDPOINT__ + 'api/musics/',
          params,
          {
            'get': { method:'GET', cache: true}
          });
      },
      Videos: function(params) {
        return $resource(__CMS_ENDPOINT__ + 'api/videos/',
          params,
          {
            'get': { method:'GET', cache: true}
          });
      },
      Messages: function (params) {
        return $resource(__CMS_ENDPOINT__ + 'api/message/',
          params,
          {
            'get': { method:'GET', cache: true}
          });

      },
    };
  }
})();
