(function() {
  'use strict';

  module.exports = OnetimeEvent;

  OnetimeEvent.$inject = [];

  function OnetimeEvent() {
    return {
      restrict: 'E',
      templateUrl: 'onetime_event/onetimeEvent.html',
      scope: {
        cmsInfo: '=',
        group: '=',
        family: '='
      },
      controller: OnetimeEventController,
      controllerAs: 'onetimeEvent',
      bindToController: true
    };

    function OnetimeEventController() {
      var vm = this;
      vm.pageInfo = vm.cmsInfo.pages[0];
      console.log('family: ' + vm.family);
      vm.family = _.filter(vm.family, function(f) {
        console.log('Looking at ' + f);
        return f.age >= vm.group.minAge;
      });

      vm.events = _.filter(vm.group.events, function(event) {
        var start = moment(event.startDate);
        var now = moment();
        return start.isAfter(now);
      });
      console.log('family after: ' + vm.family);
    }
  }

})();
