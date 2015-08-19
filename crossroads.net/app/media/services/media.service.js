"use strict()";
(function() {

    module.exports = Media;

    function Media($resource) {
        return {
            Series: function() {
                //get : function(get) {
                //    //return $resource( __CRDS_CMS_ENDPOINT__ +  'api/series/', {'get':   {method:'GET', isArray:true}})
                //    //return $resource( __CRDS_CMS_ENDPOINT__ +  'api/series/', get {'get':   {method:'GET', isArray:true}})
                //}
                //return $resource( __CMS_ENDPOINT__ +  'api/series/', {'get':   {method:'GET', isArray:true}})
                // don' need to specify get, post, etc
                return $resource( __CMS_ENDPOINT__ +  'api/series/');
            }
        };
    }

})();