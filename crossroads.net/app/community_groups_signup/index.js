(function() {
  'use strict';

  var MODULES = require('crds-constants').MODULES;

  angular.module(MODULES.COMMUNITY_GROUPS, [MODULES.COMMON])
    .config(require('./communityGroups.routes'))

    //.directive('communityGroupSignup', require('./communityGroups.directive'))
    .controller('CommunityGroupSignupController', require('./communityGroupSignup.controller'))
    ;
  require('./communityGroupSignupForm.html');
})();
