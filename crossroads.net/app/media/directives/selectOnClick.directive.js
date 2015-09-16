(function () {
  'use strict';

  module.exports = SelectOnClick;

  SelectOnClick.$inject = ['$window'];

  function SelectOnClick($window) {
    return {
      restrict: 'A',
      link: function (scope, element, attrs) {
        element.on('click', function () {
          if (!$window.getSelection().toString()) {
            // Required for mobile Safari
            this.setSelectionRange(0, this.value.length);
          }
        });
      }
    };
  }
})();
