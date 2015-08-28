(function () {
    'use strict';
    module.exports = SingleSeriesController;

    SingleSeriesController.$inject = ['Selected', 'Messages'];

    function SingleSeriesController(Selected, Messages) {
        var vm = this;
        vm.msgisopen = true;
        vm.musicisopen = false;
        vm.selected = Selected;
        vm.messages = Messages.messages;
    }
})();

