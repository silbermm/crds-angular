'use strict';

var angular = require('angular');

require("angular-resource");
require("angular-sanitize");
require('angular-messages');
require('angular-cookies');
require('angular-growl');
require('angular-toggle-switch');
require('angular-ui-utils');
require('./templates/nav.html');
require('./templates/nav-mobile.html');

require('../node_modules/angular-toggle-switch/angular-toggle-switch-bootstrap.css');
require('../node_modules/angular-toggle-switch/angular-toggle-switch.css');

require('../styles/main.scss');
require('./profile');
require('./filters');
require('./events');
require('./cms/services/cms_services_module');
require('./give/bankInfo_directive.js');
require('./give/donationConfirmation_directive.js')

require('angular-aside');

require('./third-party/angular/angular-aside.min.css');
require('./third-party/angular/angular-growl.css');
require('./give');



var _ = require('lodash');
"use strict";
(function () {

   angular.module("crossroads", ['ngResource', "crossroads.profile", "crossroads.filters", "crdsCMS.services", "ui.router", 'ui.utils', "ngCookies", "ngMessages", 'angular-growl', 'toggle-switch', 'ngAside','give', 'bank-info', 'donation-confirmation'])
  
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
        fullGroupError:19
    }).config(function (growlProvider) {
        growlProvider.globalPosition("top-center");
        growlProvider.globalTimeToLive(6000);
        growlProvider.globalDisableIcons(true);
        growlProvider.globalDisableCountDown(true);
    })
    .filter('html', ['$sce', function ($sce) {
        return function (val) {
            return $sce.trustAsHtml(val);
        };
    }])
        .controller("appCtrl", ["$scope", "$rootScope", "MESSAGES", "$http", "Message", "growl", "$aside",
        function ($scope, $rootScope, MESSAGES, $http, Message, growl, $aside) {

                console.log(__API_ENDPOINT__);

                $scope.prevent = function (evt) {
                    evt.stopPropagation();
                };

                var messagesRequest = Message.get("", function () {
                    messagesRequest.messages.unshift(null); //Adding a null so the indexes match the DB
                    //TODO Refactor to not use rootScope, now using ngTemplate w/ ngMessages but also need to pull this out into a service
                    $rootScope.messages = messagesRequest.messages;
                });

                $rootScope.error_messages = '<div ng-message="required">This field is required</div><div ng-message="minlength">This field is too short</div>';

                $rootScope.$on("notify", function (event, id) {
                    growl[$rootScope.messages[id].type]($rootScope.messages[id].message);
                });

                $rootScope.$on("context", function (event, id) {
                    var message = Message.get({
                        id: id
                    }, function () {
                        return message.message.message;
                    });
                });

                //Offcanvas menu
                $scope.asideState = {
                  open: false
                };

                $scope.openAside = function(position, backdrop) {
                  $scope.asideState = {
                    open: true,
                    position: position
                  };

                  function postClose() {
                    $scope.asideState.open = false;
                  }

                  $aside.open({
                    templateUrl: 'templates/nav-mobile.html',
                    placement: position,
                    size: 'sm',
                    controller: function($scope, $modalInstance) {
                      $scope.ok = function(e) {
                        $modalInstance.close();
                        e.stopPropagation();
                      };
                      $scope.cancel = function(e) {
                        $modalInstance.dismiss();
                        e.stopPropagation();
                      };
                    }
                  }).result.then(postClose, postClose);
                }
        }
    ])
    .directive("emptyToNull", require('./shared/emptyToNull.directive.js'));

    require('./apprun');
    require('./app.config');
    require('./routes');
    require('./register/register_directive');
    require('./login');
})()
