'use strict()';
(function() {
  module.exports = function($resource) {
    return {
      Personal: $resource(__API_ENDPOINT__ + 'api/profile'),
      Person: $resource(__API_ENDPOINT__ +  'api/profile/:contactId'),
      Account: $resource(__API_ENDPOINT__ + 'api/account'),
      Password: $resource(__API_ENDPOINT__ + 'api/account/password'),
      Household: $resource(__API_ENDPOINT__ + 'api/profile/household/:householdId'),
    };
  };
})();
