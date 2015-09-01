(function () {
  'use strict';
  module.exports = SingleMediaController;

  SingleMediaController.$inject = ['SingleMedia', 'ItemProperty', 'ParentMedia', 'ParentItemProperty', 'ImageURL'];

  function SingleMediaController(SingleMedia, ItemProperty, ParentMedia, ParentItemProperty, ImageURL) {
    var vm = this;
    vm.msgisopen = true;
    vm.musicisopen = false;

    vm.media = SingleMedia[ItemProperty][0];
    vm.imageurl = ImageURL;

    if (ParentMedia){
      vm.parentMedia = ParentMedia[ParentItemProperty][0];
    }
    else {
      vm.parentMedia = false;
    }

  }
})();