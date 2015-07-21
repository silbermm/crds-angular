
(function() {
  'use strict';

  module.exports = TripGivingController;

  TripGivingController.$inject = ['$scope', '$log', 'CmsInfo'];

  function TripGivingController($scope, $log, CmsInfo) {
  	var vm = this;
  	vm.pageHeader = CmsInfo.pages[0].renderedContent;

  	//This is mock data. Will be replaced with real search results after US224
  	vm.tripParticipants = [{
	  		participantName: 'James Jones', 
	  		participantPhotoUrl: 'http://crossroads-media.s3.amazonaws.com/images/avatar.svg',
	  		showShareButtons: true,
	  		showGiveButton: false,
	  		trips: [
	  			{
	  				tripParticipantId: '', 
	  				tripName: 'GO South Africa', 
	  				tripStart: '1437407769',
	  				tripStartDate: 'Jul 20, 2015', 
	  				tripEnd: 'Jul 30, 2015'
	  			}]
  		},{
  			participantName: 'John Jones', 
	  		participantPhotoUrl: 'http://crossroads-media.s3.amazonaws.com/images/avatar.svg',
	  		showShareButtons: false,
	  		showGiveButton: true,
	  		trips: [
	  			{
	  				tripParticipantId: '', 
	  				tripName: 'GO South Africa', 
	  				tripStart: '1413504000', 
	  				tripStartDate: 'Oct 17, 2014',
	  				tripEnd: 'Oct 27, 2014'
	  			}, {
	  				tripParticipantId: '', 
	  				tripName: 'GO South Dakota', 
	  				tripStart: '1413504000', 
	  				tripStartDate: 'Oct 17, 2014',
	  				tripEnd: 'Oct 27, 2014'
	  			}]
  		}, {
  			participantName: 'Jane Jones', 
	  		participantPhotoUrl: 'http://crossroads-media.s3.amazonaws.com/images/avatar.svg',
	  		showShareButtons: false,
	  		showGiveButton: true,
	  		trips: [
	  			{
	  				tripParticipantId: '', 
	  				tripName: 'GO South Africa', 
	  				tripStart: '1413504000', 
	  				tripStartDate: 'Oct 17, 2014',
	  				tripEnd: 'Oct 27, 2014'
	  			}, {
	  				tripParticipantId: '', 
	  				tripName: 'GO South Dakota', 
	  				tripStart: '1413504000', 
	  				tripStartDate: 'Oct 17, 2014',
	  				tripEnd: 'Oct 27, 2014'
	  			}, {
	  				tripParticipantId: '', 
	  				tripName: 'GO South Carolina', 
	  				tripStart: '1413504000', 
	  				tripStartDate: 'Oct 17, 2014',
	  				tripEnd: 'Oct 27, 2014'
	  			}]
  		}];
  }
})()
