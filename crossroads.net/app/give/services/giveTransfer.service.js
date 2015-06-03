(function () {
  angular.module('crossroads.give').factory('GiveTransferService',GiveTransferService);


    function GiveTransferService() {
        var transferObject = {
          amount : '',
          donor : '',
          email : '',
          program : '',
          view : '',
        }

        return transferObject;
    }

})()
