require('./login_form.html');
(function(){ 
  module.exports = function LoginForm($log, AUTH_EVENTS){
      return {
        restrict: 'EA',
        templateUrl: "login/login_form.html",
        controller: "LoginCtrl",
        link: function (scope) {
          var showForm = function () {
              $log.debug('not logged in');
              scope.visible = true;
          };
          scope.visible = false;     
        }
      };
  }
})();
