'use strict()';

﻿(function(){
  
    module.exports = function SkillsService($resource){
        return $resource(__API_ENDPOINT__ + "api/skill/:userId");
    }

})()