'use strict()';
(function () {
    module.exports = function ($scope, $location, $anchorScroll, growl) {
        var _this = this;

        _this.scrollTo = function (id) {
            $location.hash(id);
            console.log($location.hash());
            $anchorScroll();
        }

        _this.Growl = function (type, message) {
            growl.info("GROWL");
            growl[type](message);
        }
    };
})();