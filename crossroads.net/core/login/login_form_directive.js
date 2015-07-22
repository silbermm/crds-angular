require('./login_form.html');
(function(){ 
  module.exports = function LoginForm($log, AUTH_EVENTS){
      return {
        restrict: 'EA',
        templateUrl: "login/login_form.html",
        controller: "LoginCtrl",
        link: function (scope, elements, attrs) {
          var showForm = function () {
              $log.debug('not logged in');
              scope.visible = true;
          };
          if(attrs.prefix){
            scope.passwordPrefix = attrs.prefix;
          }
          
          if (attrs.registerUrl) {
            scope.registerUrl = attrs.registerUrl;
            scope.showRegister = true;
          }
          
          $log.debug(scope.passwordPrefix);
          scope.visible = false;     
        }
      };
  }
})();
