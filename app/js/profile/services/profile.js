'use strict';

angular.module('crdsProfile')

.factory('Profile', function($http, $q, thinkMinistry) {

  return {
    saveContact: function(contact) {
      return thinkMinistry.post('UpdatePageRecord?pageId=292', contact);
    },

    getContact: function(contactId) {

      var deferred;
      deferred = $q.defer();
      $http.get(
        'http://my.crossroads.net/gateway/api/person/' + contactId)
        .then(function(response) {
          if (typeof response.data === 'object') {
            deferred.resolve(response.data);
          } else {
            deferred.resolve(null);
          }
        }, function(error) {
          return deferred.reject(error);
        });

      return deferred.promise;
    },

    saveHousehold: function(household) {
      return thinkMinistry.post('UpdatePageRecord?pageId=465', household);
    },

    getHousehold: function(householdId) {
      var path = 'GetPageRecord?pageId=465&recordId=' + householdId;

      return thinkMinistry.get(path).then(function(data) {
        return data[0];
      });
    },

    saveAddress: function(address) {
      return thinkMinistry.post('UpdatePageRecord?pageId=468', address);
    },

    getAddress: function(addressId) {
      var path = 'GetPageRecord?pageId=468&recordId=' + addressId;

      return thinkMinistry.get(path).then(function(data) {
        return data[0];
      });
    },

    getFamily: function(contactId) {
      var path = 'GetSubpageRecords?subpageId=417&parentRecordId=' + contactId;

      return thinkMinistry.get(path).then(function(data) {
        return data;
      });
    },

    getGivingHistory: function() {
      var path = 'GetPageRecords?pageId=467';

      return thinkMinistry.get(path).then(function(data) {
        return data;
      });
    },
  };
});
