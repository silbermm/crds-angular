'use strict';
(function () {
    module.exports = SingleMediaController;

    SingleMediaController.$inject = ['SingleMedia', 'ItemProperty'];

    function SingleMediaController(SingleMedia, ItemProperty) {
        var vm = this;
        vm.msgisopen = true;
        vm.musicisopen = false;

        vm.media = SingleMedia[ItemProperty][0];
    }
})();

