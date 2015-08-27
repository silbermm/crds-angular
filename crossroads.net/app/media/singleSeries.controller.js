(function () {
    'use strict';
    module.exports = SingleSeriesController;

    SingleSeriesController.$inject = ['$state', '$stateParams', 'Selected', 'Messages'];

    function SingleSeriesController($state, $stateParams, Selected, Messages) {
        var vm = this;
        vm.msgisopen = true;
        vm.musicisopen = false;
        vm.selected = Selected;

        if (!vm.selected) {
            $state.go('errors/404');
            return;
        }

        vm.messages = Messages.messages;
    }
})();

