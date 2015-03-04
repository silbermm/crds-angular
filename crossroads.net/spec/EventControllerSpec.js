//require('angular');

// testing controller
describe('EventsController', function() {
   var $httpBackend, $rootScope, eventsController;
   
   var endpoint = 'http://localhost:49380/';

   // Set up the module
   beforeEach(module('atrium-events'));

   beforeEach(
    inject(
        function($injector) {
       // Set up the mock http service responses
       $httpBackend = $injector.get('$httpBackend');
       // backend definition common for all tests
       $httpBackend.when('GET', endpoint + 'api/Publicevents/oakley')
                              .respond({events:[
                                  {'time':'12:00','meridian':'pm','name':'Ewhiz Kids India','location':'B105 War Room'},
                                  {'time':'12:00','meridian':'pm','name':'Mason - Turner Office Space Use','location':'KC Room 215 4th grade'},
                                  {'time':'12:00','meridian':'pm','name':'Justice Small Group','location':'>Atrium Conference Room - 2<'},
                                  {'time':'12:10','meridian':'pm','name':'HS Huddle','location':'WAR ROOM'},
                                  {'time':'12:10','meridian':'pm','name':'Oakley Prayer','location':'Meeting Center Room D'}]});

       // Get hold of a scope (i.e. the root scope)
       $rootScope = $injector.get('$rootScope');
       // The $controller service is used to create instances of controllers
       var $controller = $injector.get('$controller');

       eventsController = function() {
         return $controller('EventsController', {'$scope' : $rootScope });
       };
   }));


   afterEach(function() {
     $httpBackend.verifyNoOutstandingExpectation();
     $httpBackend.verifyNoOutstandingRequest();
   });

      it('should place a get call', function(){
        var controller = eventsController();
        $httpBackend.expectGET(endpoint + 'api/Publicevents/oakley');
        $httpBackend.flush();
		var events = $rootScope.events.$$state.value.events;
		console.log("Events: " + jasmine.pp(events));
        expect(events).toBeDefined();
		expect(events[0].time).toBeDefined();
		expect(events[1].time).toBeDefined();
		expect(events[2].time).toBeDefined();
		expect(events[3].time).toBeDefined();
		expect(events[4].time).toBeDefined();
      });


   // it('should fetch authentication token', function() {
   //   $httpBackend.expectGET('/auth.py');
   //   var controller = createController();
   //   $httpBackend.flush();
   // });


   // xit('should fail authentication', function() {

   //   // Notice how you can change the response even after it was set
   //   authRequestHandler.respond(401, '');

   //   $httpBackend.expectGET('/auth.py');
   //   var controller = createController();
   //   $httpBackend.flush();
   //   expect($rootScope.status).toBe('Failed...');
   // });


   // it('should send msg to server', function() {
   //   var controller = createController();
   //   $httpBackend.flush();

   //   // now you don’t care about the authentication, but
   //   // the controller will still send the request and
   //   // $httpBackend will respond without you having to
   //   // specify the expectation and response for this request

   //   $httpBackend.expectPOST('/add-msg.py', 'message content').respond(201, '');
   //   $rootScope.saveMessage('message content');
   //   expect($rootScope.status).toBe('Saving...');
   //   $httpBackend.flush();
   //   expect($rootScope.status).toBe('');
   // });


   // it('should send auth header', function() {
   //   var controller = createController();
   //   $httpBackend.flush();

   //   $httpBackend.expectPOST('/add-msg.py', undefined, function(headers) {
   //     // check if the header was send, if it wasn't the expectation won't
   //     // match the request and the test will fail
   //     return headers['Authorization'] == 'xxx';
   //   }).respond(201, '');

   //   $rootScope.saveMessage('whatever');
   //   $httpBackend.flush();
   // });
   
   function DumpObject(obj)
{
  var od = new Object;
  var result = "";
  var len = 0;

  for (var property in obj)
  {
    var value = obj[property];
    if (typeof value == 'string')
      value = "'" + value + "'";
    else if (typeof value == 'object')
    {
      if (value instanceof Array)
      {
        value = "[ " + value + " ]";
      }
      else
      {
        var ood = DumpObject(value);
        value = "{ " + ood.dump + " }";
      }
    }
    result += "'" + property + "' : " + value + ", ";
    len++;
  }
  od.dump = result.replace(/, $/, "");
  od.len = len;

  return od;
}
});