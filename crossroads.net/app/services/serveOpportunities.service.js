(function(){
  module.exports = ServeOpportunities;
 
  ServeOpportunities.$inject = ['$resource'];

  function ServeOpportunities($resource){
    return $resource(__API_ENDPOINT__ + 'api/profile/servesignup');
  }  
})();
