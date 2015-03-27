'use strict';
(function() {
    module.exports = ProfilePersonalDirective;

    ProfilePersonalDirective.$inject = ['$http', '$log', '$resolve', '$q', 'Session', 'User', 'ProfileReferenceData'];

    function ProfilePersonalDirective($http, $log, $resolve, $q, Session, User, ProfileReferenceData) {
        $log.debug("ProfilePersonalDirective");

        return {
                restrict: 'E',
                transclude: true,
                //bindToController: true,
                templateUrl: "personal/profile_personal.template.html",
                controller: "ProfilePersonalController as profile",
                link: link,
                // data: {
                //     isProtected: true
                // },
        };
    }

    function link(scope, el, attr, controller) {
        // debugger;
        // controller.ProfileReferenceData.then(function(response) {
        //     debugger;
        //     controller.person = response.person;
        //     controller.genders = response.genders;
        //     controller.maritalStatuses = response.maritalStatuses;
        //     controller.serviceProviders = response.serviceProviders;
        //     controller.states = response.states;
        //     controller.countries = response.countries;
        //     controller.crossroadsLocations = response.crossroadsLocations;
        // });
        // controller.genders().then(function(response) {
        //     controller.genders = response;
        // });
        //
        // controller.maritalStatuses().then(function(response) {
        //     controller.maritalStatuses = response
        // });
        //
        // controller.serviceProviders().then(function(response) {
        //     controller.serviceProviders = response;
        // });
        //
        // controller.states().then(function(response) {
        //     controller.states = response;
        // });
        //
        // controller.countries().then(function(response) {
        //     controller.countries = response;
        // });
        //
        // controller.crossroadsLocations().then(function(response) {
        //     controller.crossroadsLocations = response;
        // });
        //
        // controller.person().then(function(response) {
        //     controller.person = response;
        // });
        //
        // console.log('Linking: ' + controller);
    }
})()
