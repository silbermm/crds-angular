'use strict()';
(function(){

    module.exports = function ($http, Session, User) {

      return {
        restrict: 'A',
        require: 'ngModel',
        scope: {
            validateUnique: "@",
            onEmailFound: "&",
            onEmailNotFound: "&",
        },
        link: function(scope, element, attrs, ngModel) {
            var userid = Session.exists('userId') !== undefined ? Session.exists('userId') : 0;
            var shouldValidateUnique = true;
            element.bind('blur', function (e) {
                if(scope.validateUnique === undefined || scope.validateUnique === null) {
                    shouldValidateUnique = true;
                } else {
                    shouldValidateUnique = scope.validateUnique;
                }

                if(shouldValidateUnique) {
                    ngModel.$setValidity('unique', undefined);
                }

                $http.get(__API_ENDPOINT__ + 'api/lookup/' + userid  + '/find/?email=' +  encodeURI(element.val()), {
                    headers: {
                        "X-Use-The-Force": "true"
                    }
                })
                .success(function(data) {
                    // Successful response from this call means we did NOT find a matching email
                    if(shouldValidateUnique) {
                        ngModel.$setValidity('unique', true);
                        User.email = element.val();
                    }
                    scope.onEmailNotFound();

                })
                .error(function(err) {
                    // Error response from this call means we DID find a matching email
                    if(shouldValidateUnique) {
                        ngModel.$setValidity('unique', false);
                        User.email = element.val();
                    }
                    scope.onEmailFound();
                });
            });
        //   var userid = Session.exists('userId') !== undefined ? Session.exists('userId') : 0;
        //   ngModel.$asyncValidators.unique = function (email) {
        //     console.log('personal profile unique email');
        //         return $http.get(__API_ENDPOINT__ + 'api/lookup/' + userid  + '/find/?email=' +  encodeURI(email), {
        //             headers: {
        //                 "X-Use-The-Force": "true"
        //             }
        //         })
        //     .success(function(succ) {
        //         User.email = email;
        //     }).error(function(err) {
        //         User.email = email;
        //     });
        //   };
        }
      };
  }

})()
