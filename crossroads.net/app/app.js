'use strict';

var angular = require('angular');

require('./templates/nav.html');
require('./templates/nav-mobile.html');

require('../node_modules/angular-toggle-switch/angular-toggle-switch-bootstrap.css');
require('../node_modules/angular-toggle-switch/angular-toggle-switch.css');

require('../styles/main.scss');
require('./profile');
require('./filters');
require('./events');
require('./cms/services/cms_services_module');

require('angular-aside');
require('angular-match-media');

require('./third-party/angular/angular-aside.min.css');
require('./third-party/angular/angular-growl.css');
require('./give');


require('./app.core.module');
require('./components/components.module');
require('./mp_tools');

var _ = require('lodash');
"use strict";
(function () {

   angular.module("crossroads", [
     'crossroads.core',
     "crossroads.profile", 
     "crossroads.filters", 
     'crossroads.mptools',
     'crossroads.components',
     'crossroads.give',
     "crdsCMS.services",
     'ngAside', 
     'matchMedia'
     ])
    .constant("AUTH_EVENTS", {
      loginSuccess: "auth-login-success",
      loginFailed: "auth-login-failed",
      logoutSuccess: "auth-logout-success",
      sessionTimeout: "auth-session-timeout",
      notAuthenticated: "auth-not-authenticated",
      isAuthenticated: "auth-is-authenticated",
      notAuthorized: "auth-not-authorized"
    })
    //TODO Pull out to service and/or config file
    .constant("MESSAGES", {
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
      creditCardDiscouraged:36,
      selectSignUpAndFrequency: 31,
      selectFrequency: 32,
      invalidDateRange: 35,
      noMembers: 33,
      noServingOpportunities: 34
    }).config(function (growlProvider) {
      growlProvider.globalPosition("top-center");
      growlProvider.globalTimeToLive(6000);
      growlProvider.globalDisableIcons(true);
      growlProvider.globalDisableCountDown(true);
    })
    .directive("emptyToNull", require('./shared/emptyToNull.directive.js'))
    .directive("stopEvent", require('./shared/stopevent.directive.js'))

    require('./app.controller');
    require('./apprun');
    require('./app.config');
    require('./routes');
    require('./register/register_directive');
    require('./login');
})()
