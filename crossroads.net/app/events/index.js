'use strict';

// Define atrium-events module
var app = angular.module('atrium-events', ['ngResource'], function($locationProvider) {
	// This is needed in order to be able to use the $location.search() to get the query parameters
	$locationProvider.html5Mode(true);
});

// Events Controller
// TODO Enable this if we eventually use the controller & service on the Atrium Events page
//app.controller('EventsController',['$scope','$log', '$http', '$location', 'Events', require('./events_controller')]);

// Events Service
// TODO Enable this if we eventually use the controller & service on the Atrium Events page
//app.factory('Events', ['$resource','$log',require('./service/events_service')]);

// Atrium events directive to build the contents of the div
// TODO Figure out a better way to build this page.  The jQuery Cycle2 plugin wasn't playing well with angular,
// and the scrolling marquee wasn't working.  As a workaround, we are building the entire contents of the atrium
// events here, and then manually invoking the Cycle2 plugin to initialize.  We should be able to do something with
// an ng-repeat, and maybe pure CSS3 animation, then this directive could be removed.
app.directive("addEventsData", ['$log', '$location', '$resource', function($log, $location, $resource) {
		return{
				restrict: 'A',
				priority: 1001,
				link: function(scope, element, attrs) {
					$log.debug("In addEventsData directive");
					var evts = $resource(__API_ENDPOINT__ + 'api/events/:site').query({site:$location.search().site}, function(response) {
						$log.debug("Response: " + response);
						evts = response;
						
						var tbody = $('<tbody>');
						for(var i = 0; i < evts.length; i++) {
							$log.debug("Event: " + evts[i]);
							var row = $('<tr>');
							var evtTime = $('<td class="first-cell">');
							var evtMeridian = $('<span>');
							var evtName = $('<td class="second-cell">');
							var evtLocation = $('<td class="third-cell">');
							
							evtTime.append(evts[i].time);
							evtMeridian.append(evts[i].meridian)
							evtTime.append(evtMeridian);
							evtName.append(evts[i].name);
							evtLocation.append(evts[i].location);
							
							row.append(evtTime);
							row.append(evtName);
							row.append(evtLocation);
							
							tbody.append(row);
						}
						var atriumCycleCell = $('<div class="atrium-cycle-cell">');
						var atriumTable = $('<table class="table atrium-table">');
						atriumTable.append(tbody);
						atriumCycleCell.append(atriumTable);
						atriumCycleCell.append($('<hr class="atrium-border" />'));
						element.append(atriumCycleCell);
						
						var marqueeElement = $(".atrium-body");
						var marqueeHeight = $(marqueeElement).height();
						var windowHeight = $(window).height();
						$log.debug("Marquee Height: " + marqueeHeight);
						$log.debug("Window Height: " + windowHeight);
						if (marqueeHeight >= windowHeight) {
							$log.debug("Appending clone of div to force scroll");
							element.append(atriumCycleCell.clone());
							$('hr.atrium-border').show();
						};						
						element.cycle();
					});
				}
	
}}]);
