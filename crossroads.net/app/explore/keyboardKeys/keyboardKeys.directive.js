(function () {
  'use strict';

  module.exports = keyboardKeys;

  keyboardKeys.$inject = ['$window', '$document'];

  function keyboardKeys($window, $document) {
    return {
      restrict: 'A',
      link: function (scope) {
        var keydown = function (e) {
          if (e.keyCode === 38) {
            e.preventDefault();
            console.log('keydown');
            scope.$emit('arrow-up');
          }
          if (e.keyCode === 40) {
            e.preventDefault();
            console.log('keyup');
            scope.$emit('arrow-down');
          }
        };
        $document.on('keydown', keydown);
        scope.$on('$destroy', function () {
          $document.off('keydown', keydown);
        });
      }
    }
  }
})();
