'use strict()';
(function(){

  angular.module('crossroads.core', [
    'ngResource',
    'ngSanitize',
    'ngPayments',
    'ui.router',
    'ui.utils',
    'ngCookies',
    'ngMessages',
    'angular-growl',
    'toggle-switch',
    'sn.addthis',
    'ngAside',
    'mailchimp',
    'matchMedia',
    'ui.bootstrap'
    ])
    .constant('AUTH_EVENTS', {
      loginSuccess: 'auth-login-success',
      loginFailed: 'auth-login-failed',
      logoutSuccess: 'auth-logout-success',
      sessionTimeout: 'auth-session-timeout',
      notAuthenticated: 'auth-not-authenticated',
      isAuthenticated: 'auth-is-authenticated',
      notAuthorized: 'auth-not-authorized'
    })
    //TODO Pull out to service and/or config file
    .constant('MESSAGES', {
      generalError: 1,
      emailInUse: 2,
      fieldCanNotBeBlank: 3,
      invalidEmail: 4,
      invalidPhone: 5,
      invalidData: 6,
      profileUpdated: 7,
      photoTooSmall: 8,
      credentialsBlank: 9,
      loginFailed: 10,
      invalidZip: 11,
      invalidPassword: 12,
      successfullRegistration: 13,
      succesfulResponse: 14,
      failedResponse: 15,
      fromDateToLarge: 37,
      successfullWaitlistSignup:17,
      noPeopleSelectedError:18,
      fullGroupError:19,
      invalidDonationAmount:22,
      invalidAccountNumber:23,
      invalidRoutingTransit:24,
      invalidCard:25,
      invalidCvv:26,
      donorEmailAlreadyRegistered:28,
      serveSignupSuccess:29,
      serveSignupMoreError:30,
      creditCardDiscouraged:36,
      selectSignUpAndFrequency: 31,
      selectFrequency: 32,
      invalidDateRange: 35,
      noMembers: 33,
      noServingOpportunities: 34,
      toDateToSmall: 38,
      invalidPaymentMethodInformation: 39,
      noInitiativeSelected: 16,
      toolsError: 40,
      paymentMethodDeclined: 44,
      paymentMethodProcessingError: 47,
      noResponse: 49,
      ageError: 54,
      mailchimpSuccess: 55,
      tripSearchNotFound: 59
    })
    .config(function (growlProvider) {
      growlProvider.globalPosition('top-center');
      growlProvider.globalTimeToLive(6000);
      growlProvider.globalDisableIcons(true);
      growlProvider.globalDisableCountDown(true);
    })
    .directive('emptyToNull', require('./shared/emptyToNull.directive.js'))
    .directive('stopEvent', require('./shared/stopevent.directive.js'))
    .directive('requireMultiple', require('./shared/requireMultiple.directive.js'))
    .directive('autofocus', require('./shared/autofocus.directive.js'))
    .factory('ContentPageService', require('./cms/services/content_page.service'));

})();
