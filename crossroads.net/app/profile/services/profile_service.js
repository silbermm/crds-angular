'use strict()';
﻿(function(){
  module.exports = function($resource) {
      return {
          Personal: $resource(__API_ENDPOINT__ + 'api/profile'),
          Account: $resource(__API_ENDPOINT__ + 'api/account'),
          Password: $resource(__API_ENDPOINT__ + 'api/account/password'),
          //Household: $resource('api/household')
          MySkills: $resource(__API_ENDPOINT__ + 'api/myskills') 
      };
  };
})()
