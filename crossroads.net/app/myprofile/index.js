var app = angular.module("crossroads");
require('./myprofile.html');
require('./templates/profile_personal.html');
require('./templates/profile_account.html');
require('./templates/profile_giving.html');
require('./templates/profile_history.html');
require('./templates/profile_household.html');
require('./templates/profile_skills.html');
require('./profile_modal.js');
require('./templates/profile_giving_edit_modal.html');
require('./templates/profile_giving_remove_modal.html');
require('./templates/profile_image_upload.html');
require('./templates/confirmPassword.html');


app.controller("MyProfileCtrl", ['$scope', '$log', '$location', '$anchorScroll', '$modal', require("./myprofile_controller")]);
app.controller("ChangeProfileImageCtrl", ['$modalInstance', '$scope', '$timeout', require("./templates/changeProfileImage.controller")]);
app.controller("ConfirmPasswordCtrl", ['$modalInstance', '$scope', '$timeout', require("./templates/confirmPassword.controller")]);
