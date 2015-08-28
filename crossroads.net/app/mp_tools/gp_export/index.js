(function() {
  'use strict()';

  var module = 'crossroads.mptools';

  require('./gpeEport.service');
  require('./gpeEport.html');

  angular.module(module).controller('GPExportController', require('./gpExport.controller'));

})();
