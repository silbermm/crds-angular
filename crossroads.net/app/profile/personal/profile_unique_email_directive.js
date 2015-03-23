'use strict()';
(function(){

    module.exports = function ($http, Session, User) {

      return {
        restrict: 'A',
        require: 'ngModel',
        link: function(scope, element, attrs, ngModel) {
          var userid = Session.exists('userId') !== undefined ? Session.exists('userId') : 0;
          ngModel.$asyncValidators.unique = function (email) {
            console.log('personal profile unique email');
            return $http.get(__API_ENDPOINT__ + 'api/lookup/' + userid  + '/find/?email=' +  encodeURI(email))
            .success(function(succ) {
                console.log("success function");
                User.email = email;
            }).error(function(err) {
                console.log("error callback!");
                User.email = email;
            });
          };
        }
      };
  }

})()
