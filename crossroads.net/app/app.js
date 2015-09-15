require('./events');
require('./mp_tools');
require('../lib/select.css');
(function() {
  'use strict()';

  var MODULE = 'crossroads';
  var constants = require('./constants');

  angular.module(constants.MODULES.CROSSROADS, [
      constants.MODULES.CORE,
      constants.MODULES.COMMON,
      constants.MODULES.PROFILE,
      constants.MODULES.MPTOOLS,
      constants.MODULES.GIVE,
      constants.MODULES.TRIPS,
      constants.MODULES.MEDIA
   ]);

  angular.module(constants.MODULES.CROSSROADS).config(require('./routes'));

  require('./community_groups_signup');
  require('./styleguide');
  require('./thedaily');
  require('./gotrips');
  require('./community_groups_signup/group_signup_form.html');
  require('./my_serve');
  require('./volunteer_signup');
  require('./volunteer_application');
  require('./search');
  require('./blog');
  require('./adbox');
  require('./giving_history');

})();
