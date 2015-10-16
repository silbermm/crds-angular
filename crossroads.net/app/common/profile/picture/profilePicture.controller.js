(function() {
  'use strict';

  module.exports = ProfilePictureController;

  ProfilePictureController.$inject = ['ImageService','$modal'];

  /**
   * Controller for the ProfilePictureDirective
   * Variables passed into the directive and available to
   * this controller include:
   *    ...
   *    ...
   */
  function ProfilePictureController(ImageService, $modal) {
    var vm = this;
    vm.path = __API_ENDPOINT__ + 'api/image/profile/' + vm.contactId;
    vm.defaultImage = defaultImage;
    vm.openModal = openModal;

    function defaultImage(){
      return '//crossroads-media.imgix.net/images/avatar.svg';
    }

    function openModal() {
      var changeProfileImage = $modal.open({
        templateUrl: 'picture/profileImageUpload.html',
        controller: 'ChangeProfileImageCtrl as modal',
        backdrop: true
      });

      changeProfileImage.result.then(function (croppedImage) {
        vm.path = croppedImage;
        ImageService.ProfileImage.save(croppedImage);

      });

    }
  }

})();
