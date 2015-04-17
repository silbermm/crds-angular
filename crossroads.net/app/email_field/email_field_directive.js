
require('./email_field.html');
(function () {
    angular.module("email_field",[]).directive("emailField", ['$log', '$http', 'Session', 'User', EmailField]);

    function EmailField($log, $http, Session, User) {
        return {
            restrict: 'EA',
            replace: true,
            scope: {
                email: "=",
                submitted: "=submitted",
                prefix: "=prefix",
                validateUnique: "=",
                checkUnique: "=",
                onEmailFound: "&",
                onEmailNotFound: "&",
            },
            templateUrl: 'email_field/email_field.html',
            link: function (scope, element, attrs) {
                if(document.location.hash === "#/register"){
                    scope.redirectTo = "#/login"
                }else{
                    scope.redirectTo = "#"
                }

                // TODO For some reason, the template still says "{{prefix}}-email",
                // instead of being interpolated with the real prefix value when the
                // link function runs.  So for now, we can get to the nested email input
                // field by looking for the single input field within this element.
                var emailElement = element.find("input");
                // var emailElement = document.querySelector("#" + scope.prefix + "-email");

                var ngModel = scope.email_field.email;

                attachUniqueEmailFunctions(scope, emailElement, ngModel, $http, Session, User);

                scope.checkEmail = function () {
                    //TODO Put this logic in a method that is globally accessible
                    return (ngModel.$error.required && scope.submitted && ngModel.$dirty ||
                        ngModel.$error.required && scope.submitted && !ngModel.$touched ||
                        ngModel.$error.required && scope.submitted && ngModel.$touched ||
                        ngModel.$error.unique && ngModel.$dirty ||
                        !ngModel.$error.required && ngModel.$dirty && !ngModel.$valid);
                };
            }
        }
    }

    // Convenience method to get a true/false value, accounting for nulls and undefined
    function isTrue(b, defaultValue) {
        var ret = defaultValue;
        if(b === undefined || b === null) {
            ret = defaultValue;
        } else {
            ret = b;
        }
        return(ret);
    }

    function attachUniqueEmailFunctions(scope, element, ngModel, $http, Session, User) {
        if(isTrue(scope.validateUnique, true)) {
            ngModel.$asyncValidators.unique = function(email) {
                return(checkUniqueEmail(scope, email, $http, Session, User));
            };
        } else if(isTrue(scope.checkUnique, false)) {
            element.bind('blur', function() {
                //scope.$apply(checkUniqueEmail(scope, element.val(), $http, Session, User));
                checkUniqueEmail(scope, element.val(), $http, Session, User)
            });
        }
    }

    function checkUniqueEmail(scope, email, $http, Session, User) {
          var userid = Session.exists('userId') !== undefined ? Session.exists('userId') : 0;
          return $http.get(__API_ENDPOINT__ + 'api/lookup/' + userid  + '/find/?email=' +  encodeURI(email), {
              headers: {
                  "X-Use-The-Force": "true"
              }
          })
          .success(function(data) {
              // Successful response from this call means we did NOT find a matching email
              // which means that the unique email field is valid
              User.email = email;
              var onEmailNotFound = scope.onEmailNotFound();
              if(angular.isDefined(onEmailNotFound) && angular.isFunction(onEmailNotFound)) {
                  onEmailNotFound();
              }
          })
          .error(function(err) {
              // Error response from this call means we DID find a matching email,
              // which means that the unique email field is not valid
              User.email = email;
              var onEmailFound = scope.onEmailFound();
              if(angular.isDefined(onEmailFound) && angular.isFunction(onEmailFound)) {
                  onEmailFound();
              }
          });
    };

})()
