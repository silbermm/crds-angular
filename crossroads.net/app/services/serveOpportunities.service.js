(function(){
  module.exports = ServeOpportunities;
 
  ServeOpportunities.$inject = ['$resource'];

  function ServeOpportunities($resource){
    return {
    	ServeDays: $resource(__API_ENDPOINT__ + 'api/profile/servesignup');
    	LastOpportunityDate: $resource(__API_ENDPOINT__ + 'api/opportunity/getLastOpportunityDate/:id');
    }
  }  
})();
