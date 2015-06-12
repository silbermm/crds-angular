(function(){
  module.exports = VolunteerService;

  VolunteerService.$inject = ['$resource'];

  function VolunteerService($resource){
    return {
      Family: $resource(__API_ENDPOINT__ + 'api/volunteer-application/family/:contactId')
    }
  }
})();
