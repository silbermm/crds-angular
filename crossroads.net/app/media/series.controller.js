'use strict';
(function () {
    module.exports = SeriesController;

    SeriesController.$inject = ['$stateParams', 'Series', 'Messages'];

    function SeriesController($stateParams, Series, Messages) {
        debugger;
        var vm = this;
        vm.msgisopen = true;
        vm.musicisopen = false;
        vm.series = Series.series;
        vm.selectedSeries = getSeriesByTitle($stateParams.title); // fix the references in the card(s)
        vm.messages = Messages.messages;

        function getSeriesByTitle(seriesTitle) {
            return _.find(vm.series, function(obj) {
                return (obj.title === seriesTitle);
            });
        };
    }
})();

