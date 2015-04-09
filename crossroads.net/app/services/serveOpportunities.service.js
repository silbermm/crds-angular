(function(){
  module.exports = ServeOpportunities;
 
  ServeOpportunities.$inject = ['$resource'];

  function ServeOpportunities($resource){
    return {
    	LastOpportunityDate: $resource(__API_ENDPOINT__ + 'api/opportunity/getLastOpportunityDate/:id'),
    	ServeDays: $resource(__API_ENDPOINT__ + 'api/serve/family-serve-days')
    }
  }  
})();
