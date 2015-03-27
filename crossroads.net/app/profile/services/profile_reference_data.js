'use strict';
(function() {
    module.exports = factory;
    factory.$inject = ['Lookup', 'Profile', '$resolve'];

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

        // var resolvedData = {
        //     genders: [],
        //     maritalStatuses: [],
        //     serviceProviders: [],
        //     states: [],
        //     countries: [],
        //     crossroadsLocations: [],
        //     person: {},
        // };
        //
        // $resolve.resolve(data).then(function(result) {
        //     resolvedData = result;
        // });

        return($resolve.resolve(data));
    }
})()
