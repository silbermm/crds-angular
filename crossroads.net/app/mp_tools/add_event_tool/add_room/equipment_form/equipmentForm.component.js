(function() {
  'use strict';

  module.exports = EquipmentForm;

  EquipmentForm.$inject = ['Validation'];

  function EquipmentForm(Validation) {
    return {
      restrict: 'E',
      scope: {
        currentEquipment: '=',
        equipmentLookup: '='
      },
      controller: EquipmentController,
      controllerAs: 'equipment',
      bindToController: true,
      templateUrl: 'equipment_form/equipmentForm.html'
    };

    function EquipmentController() {
      var vm = this;
      vm.addEquipment = addEquipment;
      vm.fieldName = fieldName;
      vm.remove = remove;
      vm.showError = showError;
      vm.showFieldError = showFieldError;
      vm.validation = Validation;

      function addEquipment() {
        vm.currentEquipment.push({equipment: {name: null, quantity: 0 }});
      }

      function fieldName(name, idx) {
        return name + '-' + idx;
      }

      function remove(idx) {
        if(vm.currentEquipment[idx] !== undefined) {
          
        }
      }

      function showError(form) {
        return Validation.showErrors(form, 'equipmentChooser') ||
          Validation.showErrors(form, 'equip.quantity');
      }

      function showFieldError(form, name) {
        return Validation.showErrors(form, name);
      }
    }
  }
})();
