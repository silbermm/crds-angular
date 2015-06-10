'use strict';

require('./profile');
require('./events');
require('./give');
require('./mp_tools');

"use strict";
(function () {

   angular.module("crossroads", [
     'crossroads.core',
     "crossroads.profile",
     'crossroads.mptools',
     'crossroads.give'
     ])
    require('./routes');

    require('./community_groups_signup')
    require('./mytrips');
    require('./profile/profile.html');
    require('./profile/personal/profile_personal.html');
    require('./profile/profile_account.html');
    require('./profile/skills/profile_skills.html');
    require('./styleguide');
    require('./give');
    require('./media');
    require('./myprofile');
    require('./community_groups_signup/group_signup_form.html');
    require('./my_serve');
    require('./go_trip_giving');
    require('./corkboard');
    require('./volunteer_signup');
    require('./volunteer_application');

})()
