'use strict()';
(function(){

  angular.module('crossroads.filters').filter('html', HtmlFilter);

  HtmlFilter.$inject = ['$sce'];

  function HtmlFilter($sce){
    return function (val) {
      return $sce.trustAsHtml(val);
    };
  }

})();

