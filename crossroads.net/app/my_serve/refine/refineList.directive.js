'use strict()';
(function(){

  module.exports = RefineDirective;

  function RefineDirective(){
    return {
      restrict: "E",
      replace: true,
      templateUrl: "refine/refineList.html",
      scope: {
        "servingDays": "=servingDays"
      },
      link : link
    }

    function link(scope, el, attr){

      scope.filterFamily = filterFamily; 
      
      //////////////////////////////////
      
      function activtate(){}

      function filterFamily(){
        
      }

    }
  }


})()
