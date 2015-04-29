'use strict()';
(function(){
  var moment = require('moment');

  angular.module('crossroads.mptools').controller('SignupToServeController', SignupToServeController);

  SignupToServeController.$inject = ['$log', '$location', '$window', 'MPTools', 'Su2sData', 'ServeOpportunities' ];

  function SignupToServeController($log, $location, $window, MPTools, Su2sData, ServeOpportunities){
    var vm = this; 
  
    vm.allParticipants = [];
    vm.cancel = cancel;
    vm.eventDates = [];
    vm.frequency = [{
        value: 0,
        text: "Once"
      }, {
        value: 1,
        text: "Every Week"
      }, {
        value: 2,
        text: "Every Other Week"
      }];
    vm.group = {};
    vm.open = open;
    vm.params = MPTools.getParams();
    vm.populateDates = populateDates;
    vm.saveRsvp = saveRsvp;
    vm.showError = showError;
    vm.ready = false;

    activate();

    ////////////////////////////////////////////
    
    function activate(){
      Su2sData.get({"oppId": vm.params.recordId}, function(g){
        vm.group = g;
        vm.allParticipants = g.Participants;
        vm.ready = true;
      });
    }

    function cancel(){
      $window.close();
    }

    function open($event, opened) {
      $event.preventDefault();
      $event.stopPropagation();
      vm[opened] = true;
    }

    function parseDate(stringDate) {
      var m = moment(stringDate);

      if (!m.isValid()) {
        var dateArr = stringDate.split("/");
        var dateStr = dateArr[2] + " " + dateArr[0] + " " + dateArr[1];
        // https://github.com/moment/moment/issues/1407
        // moment("2014 04 25", "YYYY MM DD"); // string with format
        m = moment(dateStr, "YYYY MM DD");

        if (!m.isValid()) {
          //throw error
          throw new Error("Parse Date Failed Moment Validation");
        }
      }
      $log.debug('date: ' + m.format('X'));
      return m.format('X');
    }

    function populateDates(){
      if (vm.selectedFrequency.value === 0){
        ServeOpportunities.AllOpportunityDates.query({"id": vm.params.recordId}, function(retVal){
          _.each(retVal, function(d){
            var dateNum = Number(d * 1000);
            var dateObj = new Date(dateNum);
            vm.eventDates.push((dateObj.getMonth() + 1) + "/" + dateObj.getDate() + "/" + dateObj.getFullYear());
          })
        });
      }
    }

    function saveRsvp(isValid){
      if(!isValid){
        return ;
      }

      _.each(vm.participants, function(participant){
        var saveRsvp = new ServeOpportunities.SaveRsvp();
        saveRsvp.contactId = participant.contactId;
        saveRsvp.opportunityId = vm.params.recordId;
        saveRsvp.eventTypeId = vm.group.eventTypeId;
        if (vm.selectedFrequency.value === 0){
          saveRsvp.endDate = parseDate(vm.selectedEvent);
          saveRsvp.startDate = parseDate(vm.selectedEvent);
          saveRsvp.alternateWeeks = false;
        }
        saveRsvp.signUp = vm.attending;
        //saveRsvp.alternateWeeks = (scope.currentMember.currentOpportunity.frequency.value === 2);
        saveRsvp.$save(function(saved){
          $log.debug("saved!");
          //$window.close(); uncomment this before done
        }, function(err){
          $rootScope.$emit('notify', $rootScope.MESSAGES.generalError);
        });
      });
    }

    function showError(){
      return vm.params.selectedCount > 1 || vm.params.recordDescription === undefined;
    }
  }

})();
