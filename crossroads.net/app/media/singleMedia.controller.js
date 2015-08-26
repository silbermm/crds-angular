(function () {
  'use strict';
  module.exports = SingleMediaController;

  SingleMediaController.$inject = ['$state', 'SingleMedia', 'ItemProperty', 'ParentMedia', 'ParentItemProperty'];

  function SingleMediaController($state, SingleMedia, ItemProperty, ParentMedia, ParentItemProperty) {
    var vm = this;
    vm.msgisopen = true;
    vm.musicisopen = false;

    vm.media = SingleMedia[ItemProperty][0];

    if (!vm.media) {
      $state.go('errors/404');
      return;
    }

    vm.parentMedia = ParentMedia[ParentItemProperty][0];
  }
})();