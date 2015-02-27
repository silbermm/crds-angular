(function() {
    angular.module('crossroads').controller('ContentCtrl', [
        '$scope', '$stateParams', '$log', 'Page', function($scope, $stateParams, $log, Page) {
            $scope.main = "ContentCtrl";
            $scope.params = $stateParams;
            var pageRequest = Page.get({ url: $stateParams.urlsegment }, function() {
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
    ]);

})();