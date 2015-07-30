
require('./profile');
require('./events');
require('./give');
require('./mp_tools');

(function () {
  'use strict()';

  var MODULE = 'crossroads';

  angular.module(MODULE, [
   'crossroads.core',
   'crossroads.profile',
   'crossroads.mptools',
   'crossroads.give'
   ]);

  angular.module(MODULE).config(require('./routes'));

  require('./community_groups_signup');
  require('./profile/profile.html');
  require('./profile/personal/profile_personal.html');
  require('./profile/profile_account.html');
  require('./profile/skills/profile_skills.html');
  require('./styleguide');
  require('./thedaily');
  require('./give');
  require('./media');
  require('./myprofile');
  require('./community_groups_signup/group_signup_form.html');
  require('./my_serve');
  require('./trips/tripgiving');
  require('./trips/mytrips')
  require('./volunteer_signup');
  require('./volunteer_application');
  require('./search');
  require('./blog');
  require('./adbox');
})();
