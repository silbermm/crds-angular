
(function() {
  'use strict';

  module.exports = TripGivingController;

  TripGivingController.$inject = ['$scope', '$log', 'CmsInfo'];

  function TripGivingController($scope, $log, CmsInfo) {
  	var vm = this;
  	vm.pageHeader = CmsInfo.pages[0].renderedContent;
  }
})()
