(function () {
  	'use strict';
  	module.exports = MyTripsController;
  	MyTripsController.$inject=['$log'];

  	function MyTripsController($log) {

  	var vm = this;
  	vm.myTrips = [{
        "tripStartDate": "7/25/2015",
        "tripEnd": "7/31/2015",
        "tripName": "(d) NKY Big Trip 2015",
        "fundraisingDays": 32,
        "fundraisingGoal": 1000,
        "totalRaised": 250,
        "tripGifts": [{
            "donorNickname": "TJ",
            "donorLastName": "Maddox",
            "donorEmail": "tmaddox33@gmail.com",
            "donationAmount": 250,
            "donationDate": "7/2/2015"
        	}]
    	},{
        "tripStartDate": "7/25/2015",
        "tripEnd": "7/31/2015",
        "tripName": "(d) GO Home",
        "fundraisingDays": 32,
        "fundraisingGoal": 1000,
        "totalRaised": 10000,
        "tripGifts": [{
            "donorNickname": "TJ",
            "donorLastName": "Maddox",
            "donorEmail": "tmaddox33@gmail.com",
            "donationAmount": 250,
            "donationDate": "7/2/2015"
        	}]
    	}]
	}
})()