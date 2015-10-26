(function() {
  'use strict()';

  var MODULE = 'crossroads.mptools';

  require('./adminTool.html');

  var app = angular.module(MODULE);
  app.controller('AdminToolController', require('./adminTool.controller'));
  app.config(require('./adminTool.routes'));

})();
