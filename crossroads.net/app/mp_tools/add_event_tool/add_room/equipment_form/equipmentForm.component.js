(function() {
  'use strict';

  module.exports = EquipmentForm;

  EquipmentForm.$inject = ['AddEvent', 'Validation'];

  function EquipmentForm(AddEvent, Validation) {
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
      vm.existing = existing;
      vm.fieldName = fieldName;
      vm.isCancelled = isCancelled;
      vm.remove = remove;
      vm.showError = showError;
      vm.showFieldError = showFieldError;
      vm.undo = undo;
      vm.validation = Validation;

      function addEquipment() {
        vm.currentEquipment.push({equipment: {name: null, quantity: 0 }});
      }

      function existing(equipment) {
        return _.has(equipment, 'cancelled');
      }

      function fieldName(name, idx) {
        return name + '-' + idx;
      }

      function isCancelled(equipment) {
        return existing(equipment) && equipment.cancelled;
      }

      function remove(idx) {
        if (vm.currentEquipment[idx] !== undefined) {
          if (existing(vm.currentEquipment[idx].equipment)) {
            vm.currentEquipment[idx].equipment.cancelled = true;
          } else {
            vm.currentEquipment.splice(idx, 1);
          }
        }
      }

      function showError(form) {
        return Validation.showErrors(form, 'equipmentChooser') ||
          Validation.showErrors(form, 'equip.quantity');
      }

      function showFieldError(form, name) {
        return Validation.showErrors(form, name);
      }

      function undo(idx) {
        if (vm.currentEquipment[idx] !== undefined) {
          if (existing(vm.currentEquipment[idx].equipment)) {
            vm.currentEquipment[idx].equipment.cancelled = false;
          }
        }
      }
    }
  }
})();
