'use strict()';
(function(){
  var _ = require('lodash');

  require('./myserve.html');
  var app = require("angular").module('crossroads');

  require("./capacity.service");

  app.factory("filterState", require('./filterState.service.js'));
  app.controller("MyServeController", require('./myserve.controller'));

  require('./serveTabs.html');
  app.directive("serveTabs", require('./serveTabs.directive'));

  require('./serveTeam.html');
  app.directive("serveTeam", require('./serveTeam.directive'));

  app.factory("ServeOpportunities", require('../services/serveOpportunities.service'));

  require('./refine/refineList.html');
  app.directive("refineList", require('./refine/refineList.directive'));


  require('./refine/multipleDates.validation');
})()
