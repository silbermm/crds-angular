(function(){

  module.exports = ProfileModalController;

  ProfileModalController.$inject = ["$modalInstance", "person"];

  function ProfileModalController($modalInstance, person){
      var vm = this;

      vm.cancel = cancel;
      vm.modalInstance = $modalInstance;
      vm.ok = ok;
      vm.person = person;

      //////////////////////////

      function ok() {
        $modalInstance.close(vm.person);
      }

      function cancel() {
        $modalInstance.dismiss("cancel");
      }


  }

})()
