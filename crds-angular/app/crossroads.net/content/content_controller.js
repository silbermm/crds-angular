/// <reference path="../content/content_controller.js" />
angular.module('crossroads').controller('ContentCtrl', ['$scope', '$state', '$stateParams','$log','Page', function ($scope, $state, $stateParams, $log, Page) {
    $scope.main = "ContentCtrl";
    $scope.params = $stateParams;
    var pageRequest = Page.get({ url: $stateParams.urlsegment }, function () {
        if (pageRequest.pages.length > 0) {
            $scope.content = pageRequest.pages[0].content;
        } else {
            var notFoundRequest = Page.get({ url: "page-not-found" }, function () {
                //Leave the comment below.  Once we have a true 404 page hosted in the same domain, this is how we 
                //will handle it.  Then the if/else below this can be removed
                // $state.go('404');
                if (notFoundRequest.pages.length > 0) {
                    $scope.content = notFoundRequest.pages[0].content;
                } else {
                    $scope.content = "404 Content not found";
                }
            });
        }
    });
}]);