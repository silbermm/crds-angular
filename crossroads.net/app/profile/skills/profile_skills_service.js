'use strict()';

﻿(function(){
  
    module.exports = function SkillsService($resource){
        return $resource("api/skill");
    }

})()