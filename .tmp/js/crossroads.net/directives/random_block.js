'use strict';

angular.module('crossroads')

.directive('randomBlock', function() {
  return {
    restrict: 'C',
    compile: function(telement) {

      var children = _.map(telement.children(), function(child) {
        return angular.element(child);
      });

      _.each(children, function(child) {
        child.addClass('hidden');
      });

      var visible = _.sample(children);
      visible.removeClass('hidden');
    }
  };
});
