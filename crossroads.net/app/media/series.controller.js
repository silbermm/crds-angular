'use strict';
(function () {
    module.exports = SeriesController;

    SeriesController.$inject = ['$routeParams','Series'];

    function SeriesController($routeParams, Series) {
        var vm = this;
        vm.msgisopen = true;
        vm.musicisopen = false;
        vm.selectedSeries = getSeriesByTitle($routeParams.title); // fix the references in the card(s)
        vm.series = Series.series;

        function getSeriesByTitle(seriesTitle) {
            vm.selectedSeries = _.find(vm.series, function(obj) {
                //return ((obj.title = seriesTitle)[0]); <-- is lodash building an array? check when debugging
                return (obj.title = seriesTitle); // <-- assume lodash returns only a single object
            })
        };
    }
})();

