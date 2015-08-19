"use strict()";
(function() {

    module.exports = GetMedia;

    function GetMedia($resource) {
        return { Programs: $resource( __CRDS_CMS_ENDPOINT__ +  'api/series/', {'get':   {method:'GET', isArray:true}}) }
    }

})();