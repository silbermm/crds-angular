'use strict';

angular.module('crdsProfile')

.service('Gender', function(thinkMinistry) {
  return {
    all: function() {
      return thinkMinistry.get('GetPageLookupRecords?pageId=311').then(function(data) {
        return data;
      });
    }
  };
});
