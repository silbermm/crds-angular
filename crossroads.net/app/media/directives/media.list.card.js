(function () {
  'use strict';

  module.exports = MediaListCard;

  MediaListCard.$inject = [];

  function MediaListCard() {
    return {
      restrict: "EA",
      templateUrl: "templates/media.list.card.html",
      scope: {
        items: '=',
        limit: "="
      }
    };
  }
})();
