angular.module('crdsProfile').controller('crdsProfileCtrl', ['Profile', ProfileController]);

function ProfileController(Profile) {
    this.genders = ["Male", "Female"];
    this.person = Profile.get({ id: 5 });
}   