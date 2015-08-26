'use strict';
(function () {
    module.exports = SeriesController;

    SeriesController.$inject = ['$stateParams', 'Series', 'Messages'];

    function SeriesController($stateParams, Series, Messages) {
        var vm = this;
        vm.msgisopen = true;
        vm.musicisopen = false;
        vm.series = Series.series;
        vm.selected = getSeriesByTitle($stateParams.title);
        vm.messages = Messages.messages;

        function getSeriesByTitle(seriesTitle) {
            return _.find(vm.series, function(obj) {
                return (obj.title === seriesTitle);
            });
        };
    }
})();

