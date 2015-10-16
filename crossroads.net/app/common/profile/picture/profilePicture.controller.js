(function() {
  'use strict';

  module.exports = ProfilePictureController;

  ProfilePictureController.$inject = ['ImageService'];

  /**
   * Controller for the ProfilePictureDirective
   * Variables passed into the directive and available to
   * this controller include:
   *    ...
   *    ...
   */
  function ProfilePictureController(ImageService) {
    var vm = this;

    ImageService.ProfileImage.get({contact: vm.contactId}, function(data) {
      vm.image = data;
    });
  }

})();
