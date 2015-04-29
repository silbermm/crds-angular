"use strict()";
(function() {

  module.exports = GetPrograms;

  function GetPrograms($resource) {
        return { Programs: $resource( __API_ENDPOINT__ +  'api/programs/:programType', {programType : '@programType'}, {'get':   {method:'GET', isArray:true}}) }
    }

})();