(function() {
  'use strict';

  var MODULES = require('crds-constants').MODULES;

  angular.module(MODULES.COMMUNITY_GROUPS)
    .controller('CommunityGroupSignupController', require('./communityGroupSignup.controller'))
    ;
  require('./communityGroupSignupForm.html');
})();
