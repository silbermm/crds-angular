'use strict';

angular.module('crdsProfile')

.service('MaritalStatus', function(thinkMinistry) {
  return {
    all: function() {
      return thinkMinistry.get('GetPageLookupRecords?pageId=339').then(function(data) {
        return data;
      });
    }
  };
});
