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
    vm.path = __API_ENDPOINT__ + 'api/image/profile/' + vm.contactId;
    vm.defaultImage = defaultImage;
    //vm.contactId need to wait for this...
    ImageService.ProfileImage.get({contact: 6}, function(data) {
      vm.image = data;
    });

    function defaultImage(){
      return '//crossroads-media.imgix.net/images/avatar.svg';
    }
  }

})();
