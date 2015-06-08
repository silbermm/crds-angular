(function () {
  angular.module('crossroads.give').factory('GiveTransferService',GiveTransferService);


    function GiveTransferService() {
        var transferObject = {
          account : '',
          amount : '',
          donor : '',
          email : '',
          program : '',
          routing : '',
          view : '',
        }

        return transferObject;
    }

})()
