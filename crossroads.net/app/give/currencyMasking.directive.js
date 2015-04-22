angular.module('currencyMask', []).directive('currencyMask', function () {
  return {
    restrict: 'A',
    require: 'ngModel',
    link: function (scope, element, attrs, ngModelController) {
      // Run formatting on keyup
      var numberWithCommas = function(value) {
        value = value.replace(/[^0-9]/g, "");
        value = value.replace(/\d{1,3}(?=(\d{3})+(?!\d))/g, "$&,");
        if (value < 1){
        	value = '';
        }    
     	return value;
      };
      var applyFormatting = function() {
        var value = element.val();
        var original = value;
        if (!value || value.length == 0) { return }
        value = numberWithCommas(value);
        if (value != original) {
          element.val(value);
          element.triggerHandler('input')
        }
      };
      element.bind('keyup', function(e) {
        var keycode = e.keyCode;
        var isTextInputKey = 
          (keycode > 47 && keycode < 58)   || // number keys
          keycode == 32 || keycode == 8    || // spacebar or backspace
          (keycode > 64 && keycode < 91)   || // letter keys
          (keycode > 95 && keycode < 112)  || // numpad keys
          (keycode > 185 && keycode < 193) || // ;=,-./` (in order)
          (keycode > 218 && keycode < 223);   // [\]' (in order)
        console.log(keycode);
        if (isTextInputKey) {
        	console.log('here');
          applyFormatting();
        }
      });
      ngModelController.$parsers.push(function(value) {
        if (!value || value.length == 0) {
          return value;
        }
        value = value.toString();
        value = value.replace(/[^0-9\.]/g, "");
        return value;
      });
      ngModelController.$formatters.push(function(value) {
        if (!value || value.length == 0) {
          return value;
        }
        value = numberWithCommas(value, true);
        return value;
      });
    }
  };
});