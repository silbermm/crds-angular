(function () {
    angular.module('atrium-events').
        factory('Events', ['$resource','$log', EventsService]);

    //By declaring api/events a resource, angular provides us with helpful verbs to perform CRUD operations. (save/update)
 

    function EventsService($resource, $log) {
        console.log("Inside Events factory");
		//var Event = $resource('http://silbervm:49380/api/Publicevents/oakley');
        var Event = $resource(__API_ENDPOINT__ + 'api/Publicevents/:location', { location: 'oakley' }, { cache: false });
        var newevent = new Event();
        //newevent.array =[];
        return newevent;
    }

})()
