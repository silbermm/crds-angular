'use strict()';
(function () {
    module.exports = function ($scope, $location, $anchorScroll) {
        var _this = this;

        _this.scrollTo = function (id) {
            $location.hash(id);
            console.log($location.hash());
            $anchorScroll();
        };
    };
})();