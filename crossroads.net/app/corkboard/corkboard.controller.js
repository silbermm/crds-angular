'use strict';
(function () {
  module.exports = function CorkboardCtrl($scope, $log, messages, $event) {

    var vm = this;

    vm.hstep = 1;
    vm.mstep = 15;
    vm.isMeridian = true;
    vm.myTime = Date.now();
    vm.view = "corkboard-listings.html";

    vm.openDatePicker = openDatePicker;

    function openDatePicker($event) {
      $event.preventDefault();
      $event.stopPropagation();

      vm.opened = true;
    }

  }
})()