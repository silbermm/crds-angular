'use strict';

angular.module('crdsProfile')

.service('States', function(thinkMinistry) {
  return {
    all: function() {
      return thinkMinistry.get('GetPageRecords?pageId=452').then(function(data) {
        return data;
      });
    }
  };
});
