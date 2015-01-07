(function(){
    angular.module('crdsProfile').factory('Skills', ["$resource", SkillsService]);
    
    function SkillsService($resource){
        return $resource("api/skill");
    }

})()