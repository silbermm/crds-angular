(function () {
  'use strict';

  module.exports = MediaListCard;

  MediaListCard.$inject = [];

  function MediaListCard() {
    return {
      restrict: "EA",
      templateUrl: "templates/mediaListCard.html",
      scope: {
        items: '=',
        limit: "="
      }
    };
  }
})();
