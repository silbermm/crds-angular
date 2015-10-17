(function() {
  'use strict';

  module.exports = ProfilePictureController;

  ProfilePictureController.$inject = ['$rootScope', 'ImageService', '$modal'];

  /**
   * Controller for the ProfilePictureDirective
   * Variables passed into the directive and available to
   * this controller include:
   *    ...
   *    ...
   */
  function ProfilePictureController($rootScope, ImageService, $modal) {
    var vm = this;
    vm.path = ImageService.ProfileImageBaseURL + vm.contactId;
    vm.defaultImage = ImageService.DefaultProfileImage;
    vm.openModal = openModal;

    function openModal() {
      var changeProfileImage = $modal.open({
        templateUrl: 'picture/profileImageUpload.html',
        controller: 'ChangeProfileImageController as modal',
        backdrop: true,
        show: false,
      });

      changeProfileImage.result.then(function(croppedImage) {
        vm.path = croppedImage;
        ImageService.ProfileImage.save(croppedImage);
        $rootScope.$emit('profilePhotoChanged');
      });

    }
  }

})();
