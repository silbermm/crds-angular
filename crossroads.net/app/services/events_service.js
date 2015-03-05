(function () {
    angular.module('atrium-events').
        factory('Events', ['$resource','$log', EventsService]);

    function EventsService($resource, $log) {
        console.log("Inside Events factory");
        return($resource(__API_ENDPOINT__ + 'api/events/:site'));
    }
})()
