'use strict';

angular.module('crdsProfile')

.service('Countries', function(thinkMinistry) {
  return {
    all: function() {
      return thinkMinistry.get('GetPageRecords?pageId=442').then(function(data) {
        return data;
      });
    }
  };
});
