(function() {
  'use strict';

  module.exports = EquipmentForm;

  EquipmentForm.$inject = [];

  function EquipmentForm() {
    return {
      restrict: 'E',
      scope: {
        currentEquipment: '='
      },
      controller: EquipmentController,
      controllerAs: 'equipment',
      bindToController: true,
      templateUrl: 'equipment_form/equipmentForm.html'
    };

    function EquipmentController() {
      var vm = this;
      vm.addEquipment = addEquipment;

      function addEquipment() {
        vm.currentEquipment.push({ }); 
      }

    }
  }
})();
