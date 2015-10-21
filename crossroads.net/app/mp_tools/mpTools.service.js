(function(){
  'use strict()'; 
  
  var _ = require('lodash');
  
  module.exports = MPTools; 

  MPTools.$inject = ['$location', '$resource', 'AuthService'];

  function MPTools($location, $resource, AuthService){
    var service = {
      setParams: setParams,
      getParams: getParams,
      allowAccess: allowAccess,
      getDonorId: getDonorId,
      Selection: $resource(__API_ENDPOINT__ + 'api/mptools/selection/:selectionId'),
      dto: {
        noSelection: undefined,
        selectionError: undefined,
        tooManySelections: undefined,
      },
    }
    var params =  {};

    function setParams(location) {
      console.log('saving location info to a service');
      params = {
        userGuid: location.search()['ug'],
        domainGuid: location.search()['dg'],
        pageId: location.search()['pageID'],
        recordId: location.search()['recordID'],
        recordDescription: location.search()['recordDescription'],
        selectedRecord: location.search()['s'],
        selectedCount: location.search()['sc']
      };
    }

    function getParams() {
      if(_.isEmpty(params) || params.userGuid === undefined){
        params = { 
          userGuid: $location.search()['ug'],
          domainGuid: $location.search()['dg'],
          pageId: $location.search()['pageID'],
          recordId: $location.search()['recordID'],
          recordDescription: $location.search()['recordDescription'],
          selectedRecord: $location.search()['s'],
          selectedCount: $location.search()['sc']
        };
      }
      return params;
    }

    function allowAccess(role) {
      return (AuthService.isAuthenticated() && AuthService.isAuthorized(role));
    }

    function getDonorId(goToFunction) {
      reset();

      var params = service.getParams();
      var donorId = getInt(params.recordId);
      if (donorId > 0) {
        goToFunction(donorId);
        return;
      }

      var selectionId = getInt(params.selectedRecord);
      var selectedCount = getInt(params.selectedCount);
      if (selectedCount == 1 && selectionId > 0) {
        service.Selection.get({selectionId: selectionId}, function(data) {
          goToFunction(data.RecordIds[0]);
        }, function(error) {
          service.dto.selectionError = true;
        });
      }

      service.dto.noSelection = donorId <= 0 && (selectedCount <= 0 || selectionId <= 0);
      service.dto.tooManySelections = selectedCount > 1;
    }

    function getInt(v) {
      var i = parseInt(v);
      return (isNaN(i) ? -1 : i);
    }

    function reset() {
      service.dto.noSelection = undefined;
      service.dto.selectionError = undefined;
      service.dto.tooManySelections = undefined;
    }

    return service;
  }

})();
