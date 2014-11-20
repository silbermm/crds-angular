var __bind = function(fn, me){ return function(){ return fn.apply(me, arguments); }; };

angular.module("crdsProfile").directive("crdsProfileLogin", function($animate) {
  return {
    require: ['^crdsProfile', 'crdsProfileLogin'],
  restrict: 'EA',
  scope: true,
  terminal: true,
  priority: 1000,
  transclude: 'element',
  link: function($scope, $element, $attrs, controllers, $transclude) {
    var crdsProfileCtrl, crdsProfileLoginCtrl;
    crdsProfileCtrl = controllers[0];
    crdsProfileLoginCtrl = controllers[1];
    crdsProfileCtrl.registerLogin(crdsProfileLoginCtrl);
  },
  controller: function($scope, $element, $attrs, $transclude) {
    return new ((function() {
      function _Class() {
        this.hide = __bind(this.hide, this);
        this.show = __bind(this.show, this);
      }

      _Class.prototype.show = function(parent, after) {
        if (!this.loginElement) {
          return $transclude((function(_this) {
            return function(clone, newScope) {
              _this.loginElement = clone;
              return $animate.enter(clone, parent, after);
            };
          })(this));
        }
      };

      _Class.prototype.hide = function() {
        if (this.loginElement) {
          $animate.leave(this.loginElement);
          return this.loginElement = null;
        }
      };

      return _Class;

    })());
  }
  };
});

