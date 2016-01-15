(function() {
  'use strict';

  module.exports = AddEventToolService;

  AddEventToolService.$inject = [];

  function AddEventToolService() {
    var obj = {
      currentPage: 1,
      eventData: {
        event: {},
        rooms: []
      },
      getEventDto: function(eventData) {
        return {
          congregationId: eventData.event.congregation.dp_RecordID,
          contactId: eventData.event.primaryContact.contactId,
          description: eventData.event.description,
          donationBatchTool: (eventData.event.donationBatchTool) ? eventData.event.donationBatchTool : false,
          endDateTime: dateTime(eventData.event.endDate, eventData.event.endTime),
          startDateTime: dateTime(eventData.event.startDate, eventData.event.startTime),
          meetingInstructions: eventData.event.meetingInstructions,
          eventTypeId: eventData.event.eventType.dp_RecordID,
          minutesSetup: eventData.event.minutesSetup,
          minutesTeardown: eventData.event.minutesCleanup,
          programId: eventData.event.program.ProgramId,
          reminderDaysId: (eventData.event.reminderDays > 0) ? eventData.event.reminderDays : null,
          title: eventData.event.eventTitle,
          sendReminder: eventData.event.sendReminder,
          rooms: _.map(eventData.rooms, function(r) { return getRoomDto(r); })
        };
      }
    };

    function getRoomDto(room) {
      return {
        hidden: room.hidden,
        roomId: room.id,
        notes: room.description,
        layoutId: room.layout.id,
        equipment: _.map(room.equipment, function(e) { return getEquipmentDto(e.equipment); })
      };
    }

    function getEquipmentDto(equipment) {
      return {
        equipmentId: equipment.name.id,
        quantityReserved: equipment.choosenQuantity
      };
    }

    function dateTime(dateForDate, dateForTime) {
      return new Date(
          dateForDate.getFullYear(),
          dateForDate.getMonth(),
          dateForDate.getDate(),
          dateForTime.getHours(),
          dateForTime.getMinutes(),
          dateForTime.getSeconds(),
          dateForTime.getMilliseconds());
    }

    return obj;
  }

})();
