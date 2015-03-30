'use strict';
(function() {
    module.exports = factory;
    factory.$inject = ['Lookup', 'Profile', '$resolve'];

    // Return a map of data needed by the profile pages.  The map is
    // actually a promise, which will be resolved by the Angular UI Router
    // resolver ($resolve).  This is similar to the behavior implemented by the
    // resolve property on a UI Router state.
    function factory(Lookup, Profile, $resolve) {
        var data = {
            genders: function() {
                return Lookup.query({
                    table: "genders"
                }).$promise;
            },

            maritalStatuses: function() {
                return Lookup.query({
                    table: "maritalstatus"
                }).$promise;
            },

            serviceProviders: function() {
                return Lookup.query({
                    table: "serviceproviders"
                }).$promise;
            },

            states: function() {
                return Lookup.query({
                    table: "states"
                }).$promise;
            },

            countries: function() {
                return Lookup.query({
                    table: "countries"
                }).$promise;
            },

            crossroadsLocations: function() {
                return Lookup.query({
                    table: "crossroadslocations"
                }).$promise;
            },

            person: function() {
                return Profile.Personal.get().$promise;
            },
        };

        return($resolve.resolve(data));
    }
})()
