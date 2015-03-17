'use strict';
module.exports = function($rootScope, $scope, $state, $stateParams, $log, Page) {
  $scope.main = "ContentCtrl";
  $scope.params = $stateParams;
  var pageRequest = Page.get({ url: $stateParams.urlsegment }, function() {
      if (pageRequest.pages.length > 0) {
		  if(pageRequest.pages[0].pageType === "SignupPage") {
			  $rootScope.signupPage = pageRequest.pages[0];
			  $state.go('community-groups-signup', {groupId: $rootScope.signupPage.group});
		  } else {
			  $scope.content = pageRequest.pages[0].renderedContent;
		  }
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
