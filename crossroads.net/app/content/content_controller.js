'use strict';
module.exports = function($rootScope, $scope, $state, $stateParams, $log, Page) {
  $scope.main = "ContentCtrl";
  $scope.params = $stateParams;
  // TODO Remove or rework this with US1044
  var urlSegment = $stateParams.urlsegment.replace(/^.*\//, "");
  var pageRequest = Page.get({ url: urlSegment }, function() {
      if (pageRequest.pages.length > 0) {
		  $scope.content = pageRequest.pages[0].renderedContent;
      } else {
          var notFoundRequest = Page.get({ url: "page-not-found" }, function() {
              if (notFoundRequest.pages.length > 0) {
                  $scope.content = notFoundRequest.pages[0].renderedContent;
              } else {
                  $scope.content = "404 Content not found";
              }
          });
      }
  });
}
