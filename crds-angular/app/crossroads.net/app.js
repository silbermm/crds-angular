'use strict';
(function () {
    angular.module('crossroads', ['crdsProfile', 'ui.router', 'ngCookies', 'angular-growl'])
    .run(['Session', '$rootScope', 'MESSAGES', function(Session, $rootScope, MESSAGES){
        $rootScope.username = Session.exists('username');
        $rootScope.MESSAGES = MESSAGES;
    }])
    .constant('AUTH_EVENTS', {
            loginSuccess: 'auth-login-success',
            loginFailed: 'auth-login-failed',
            logoutSuccess: 'auth-logout-success',
            sessionTimeout: 'auth-session-timeout',
            notAuthenticated: 'auth-not-authenticated',
            isAuthenticated : 'auth-is-authenticated',
            notAuthorized: 'auth-not-authorized'
    })
    .constant('MESSAGES', {
        generalError: "Oh No! There was an error, please fix and try again.",
        emailInUse: "This email address is already in use by another account.",
        fieldCanNotBeBlank: "This field can not be blank.",
        invalidEmail: "Email address entered does not appear to be valid.",
        invalidPhone: "Phone number entered does not appear to be valid.",
        invalidData: "Date entered does not appear to be valid.",
        profileUpdated: "Great! You successfully updated your profile information",
        photoTooSmall: "The photo you attempted to upload was too small.  Please choose another photo.",
        credentialsBlank: "Hold up! Username and password can't be blank",
        loginFailed: "Oops! Login failed. Please try again or use <a>Forgot Password</a>",
        invalidZip: "Zip code entered does not appear to be valid.",
        invalidPassword: "New password is invalid.  It must be at least 6 characters in length."
    }).config(function(growlProvider) {
        growlProvider.globalPosition('top-center');
        growlProvider.globalTimeToLive(6000);
        growlProvider.globalDisableIcons(true);
    })
    .controller('appCtrl', ['$scope', '$rootScope', 'MESSAGES', 'growl', function ($scope, $rootScope, MESSAGES, growl) {

        $rootScope.$on('notify.success', function (event, message) {
            growl.success(message);
        });

        $rootScope.$on('notify.info', function (event, message) {
            growl.info(message);
        });

        $rootScope.$on('notify.warning', function (event, message) {
            growl.warning(message);
        });

        $rootScope.$on('notify.error', function (event, message) {
            growl.error(message);
        });
    }]);
})()