(function () {
    
    var getCookie = require('./utilities/cookies');
    
    angular.module("email_field").directive('uniqueEmail', ['$http', 'User', 'Session', UniqueEmail]);
    
    function UniqueEmail($http, User, Session) {
        return {
            restrict: 'A',
            require: 'ngModel',
            link: function (scope, element, attrs, ngModel) {
                ngModel.$asyncValidators.unique = function (email) {
                    var userid = Session.exists('userId') !== undefined ? Session.exists('userId') : 0;
                    return $http({
                      method:"GET", 
                      url: __API_ENDPOINT__ + 'api/lookup/' + Session.exists('userId')  + '/find/?email=' + encodeURI(email),
                      withCredentials: true,
                      headers: {
                        'Authorization': getCookie('sessionId')
                      }
                    }).success(function(succ) {
                        User.email = email;
                    }).error(function(err) {
                        User.email = email;
                    });

                };
            }
        };
    }

})()
