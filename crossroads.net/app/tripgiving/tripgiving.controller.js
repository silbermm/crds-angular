
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
	  		trips: [
	  			{
	  				tripParticipantId: '', 
	  				tripName: 'GO South Africa', 
	  				tripStart: '1437407769', 
	  				tripEnd: '1438214401'
	  			}]
  		},{
  			participantName: 'John Jones', 
	  		participantPhotoUrl: 'http://crossroads-media.s3.amazonaws.com/images/avatar.svg',
	  		trips: [
	  			{
	  				tripParticipantId: '', 
	  				tripName: 'GO South Africa', 
	  				tripStart: '1413504000', 
	  				tripEnd: '1414368000'
	  			}, {
	  				tripParticipantId: '', 
	  				tripName: 'GO South Dakota', 
	  				tripStart: '1413504000', 
	  				tripEnd: '1414368000'
	  			}]
  		}, {
  			participantName: 'Jane Jones', 
	  		participantPhotoUrl: 'http://crossroads-media.s3.amazonaws.com/images/avatar.svg',
	  		trips: [
	  			{
	  				tripParticipantId: '', 
	  				tripName: 'GO South Africa', 
	  				tripStart: '1413504000', 
	  				tripEnd: '1414368000'
	  			}, {
	  				tripParticipantId: '', 
	  				tripName: 'GO South Dakota', 
	  				tripStart: '1413504000', 
	  				tripEnd: '1414368000'
	  			}, {
	  				tripParticipantId: '', 
	  				tripName: 'GO South Carolina', 
	  				tripStart: '1413504000', 
	  				tripEnd: '1414368000'
	  			}]
  		}];
  }
})()
