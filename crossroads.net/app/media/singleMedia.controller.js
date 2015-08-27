(function () {
  'use strict';
  module.exports = SingleMediaController;

  SingleMediaController.$inject = ['$state', 'SingleMedia', 'ItemProperty', 'ParentMedia', 'ParentItemProperty', 'ImageURL'];

  function SingleMediaController($state, SingleMedia, ItemProperty, ParentMedia, ParentItemProperty, ImageURL) {
    var vm = this;
    vm.msgisopen = true;
    vm.musicisopen = false;

    vm.media = SingleMedia[ItemProperty][0];
    vm.imageurl = ImageURL;

    if (!vm.media) {
      $state.go('errors/404');
      return;
    }

    if (ParentMedia){
      vm.parentMedia = ParentMedia[ParentItemProperty][0];
    }
    else {
      vm.parentMedia = "none";
    }

  }
})();