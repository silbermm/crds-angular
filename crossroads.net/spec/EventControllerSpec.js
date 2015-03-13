//  require('angular');

// // testing controller
// describe('EventsController', function() {
//    var $httpBackend, $rootScope, eventsController;

//    var responseArray = [
// 		{'time':'12:00','meridian':'pm','name':'Ewhiz Kids India','location':'B105 War Room'},
// 		{'time':'12:00','meridian':'pm','name':'Mason - Turner Office Space Use','location':'KC Room 215 4th grade'},
// 		{'time':'12:00','meridian':'pm','name':'Justice Small Group','location':'>Atrium Conference Room - 2<'},
// 		{'time':'12:10','meridian':'pm','name':'HS Huddle','location':'WAR ROOM'},
// 		{'time':'12:10','meridian':'pm','name':'Oakley Prayer','location':'Meeting Center Room D'}	   
//    ];
   
//    var endpoint = 'http://localhost:49380/';
//    //var endpoint = JSON.stringify(process.env.CRDS_API_ENDPOINT || "http://localhost:49380/");

//    // Set up the module
//    beforeEach(module('atrium-events'));

//    beforeEach(
//     inject(
//         function($injector) {
//        // Set up the mock http service responses
//        $httpBackend = $injector.get('$httpBackend');
//        // Get hold of a scope (i.e. the root scope)
//        $rootScope = $injector.get('$rootScope');
//        // The $controller service is used to create instances of controllers
//        var $controller = $injector.get('$controller');

//        eventsController = function(site) {
// 		 $rootScope.site = site;
//          return $controller('EventsController', {'$scope' : $rootScope });
//        };
//    }));


//    afterEach(function() {
//      $httpBackend.verifyNoOutstandingExpectation();
//      $httpBackend.verifyNoOutstandingRequest();
//    });

//       it('should place a get call', function(){
//        $httpBackend.when('GET', endpoint + 'api/events/oakley')
//                               .respond(responseArray);

//         var controller = eventsController("oakley");
//         $httpBackend.expectGET(endpoint + 'api/events/oakley');
//         $httpBackend.flush();
// 		var events = $rootScope.data.events;
// 		console.log("Events: " + jasmine.pp(events));
//         expect(events).toBeDefined();
// 		expect(events.length).toEqual(5);
// 		expect(events[0].time).toBeDefined();
// 		expect(events[1].time).toBeDefined();
// 		expect(events[2].time).toBeDefined();
// 		expect(events[3].time).toBeDefined();
// 		expect(events[4].time).toBeDefined();
//       });
// });