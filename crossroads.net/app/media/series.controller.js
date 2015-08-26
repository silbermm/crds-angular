'use strict';
(function () {
    module.exports = SeriesController;

    SeriesController.$inject = ['$state', '$stateParams', 'Series', 'Messages'];

    function SeriesController($state, $stateParams, Series, Messages) {
        var vm = this;
        vm.msgisopen = true;
        vm.musicisopen = false;
        vm.series = Series.series;

        vm.selected = getSeriesByTitle($stateParams.title);

        if (!vm.selected) {
            $state.go('errors/404');
            return;
        }

        vm.messages = Messages.messages;

        function getSeriesByTitle(seriesTitle) {
            return _.find(vm.series, function(obj) {
                return (obj.title === seriesTitle);
            });
        };
    }
})();

