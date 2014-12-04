'use strict';

angular.module('crdsProfile')

.service('Congregations', function(thinkMinistry) {
  return {
    web: function() {
      var path = 'GetPageRecords?pageId=466';
      return thinkMinistry.get(path).then(function(data) {
        return data;
      });
    },
  };
});
