(function() {
  'use strict';

  module.exports = OnetimeEvent;

  OnetimeEvent.$inject = [];

  function OnetimeEvent() {
    return {
      restrict: 'E',
      templateUrl: 'onetime_event/onetimeEvent.html',
      scope: {
        cmsInfo: '='
      },
      controller: OnetimeEventController,
      controllerAs: 'onetimeEvent',
      bindToController: true
    };

    function OnetimeEventController() {
      var vm = this;
      vm.pageInfo = vm.cmsInfo.pages[0];
    }
  }

})();
