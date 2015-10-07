(function() {
  module.exports =  Validation;

  function Validation() {
    return {
      showErrors: function(form, field) {
        if (form[field] === undefined) {
          return false;
        }

        if (form.$submitted || form[field].$dirty) {
          return form[field].$invalid;
        }

        return false;
      },
    };
  }
})();
