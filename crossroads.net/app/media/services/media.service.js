"use strict()";
(function () {

  module.exports = Media;

  function Media($resource) {
    return {
      Series: function () {
        return $resource(__CMS_ENDPOINT__ + 'api/series/');
      },
      Messages: function () {
        return $resource(__CMS_ENDPOINT__ + 'api/message/:seriesid', {seriesid : '@seriesid'});
      }
    };
  }

})();
