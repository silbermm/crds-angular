(function() {
  module.exports = ValidateYoungestAge;

  ValidateYoungestAge.$inject = [];

  function ValidateYoungestAge() {
    return {
      restrict: 'A',
      require: 'ngModel',
      link: function(scope, el, attr, ngModel) {
        var ageToCheckAgainst = attr.youngestAge || null;
        ngModel.$validators.youngestAge = function(value) {
          if (!value || !ageToCheckAgainst) {
            return true;
          }

          var input = moment(value, 'MM/DD/YYYY');
          console.log(input);
          return input.years >= ageToCheckAgainst;
        };
      }

    };

  }
})();
