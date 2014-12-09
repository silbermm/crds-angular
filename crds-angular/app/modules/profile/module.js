'use strict';
(function(){
    angular.module('crdsProfile', ['ngResource', 'ui.bootstrap']).controller('crdsProfileCtrl', ['Profile', 'Lookup', ProfileController]);

      function ProfileController(Profile, Lookup) {
          this.genders = Lookup.Genders.query();
          this.person = Profile.get({ id: 5 });
          this.maritalStatuses = Lookup.MaritalStatus.query();
          this.serviceProviders = Lookup.ServiceProviders.query();          
      }

})()
