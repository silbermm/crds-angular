require('./bank_info.html');﻿
(function(){
  module.exports = function BankInfo($log, AUTH_EVENTS){
    console.log("in the directive now");
      return {
        restrict: 'EA',
        templateUrl: "give/bank_info.html",
        controller: "GiveCtrl",
        link: function (scope, elements, attrs) {
          var showForm = function () {
              $log.debug('here I am');
              scope.visible = true;
          };
        }
      };
  }
})();
