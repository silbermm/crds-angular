(function() {
  'use strict';

  module.exports = AddEventComponent;

  AddEventComponent.$inject = [];

  function AddEventComponent() {
    return {
      restrict: 'E',
      scope: {

      },
      templateUrl: 'add_event/add_event.html',
      controller: AddEventController,
      controllerAs: 'addEvent',
      bindToController: true
    };
  }

  function AddEventController() {
    var vm = this;


  }

})();
