(function() {
  'use strict';

  module.exports = AddEventToolService;

  AddEventToolService.$inject = [];

  function AddEventToolService() {
    var obj = {
      currentPage: 1,
      editMode: false,
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
      },

      fromEventDto: function(event) {
        return {
          event: {
            congregation: {
              dp_RecordID: event.congregationId
            },
            primaryContact: {
              contactId: event.contactId
            },
            eventType: {
              dp_RecordID: event.eventTypeId
            },
            description: event.description,
            donationBatchTool: event.donationBatchTool,
            endDate: new Date(event.endDateTime),
            startDate: new Date(event.startDateTime),
            meetingInstructions: event.meetingInstructions,
            minutesSetup: event.minutesSetup,
            minutesCleanup: event.minutesTeardown,
            program: {
              ProgramId: event.programId
            },
            reminderDays: event.reminderDays,
            sendReminder: event.sendReminder,
            startTime: new Date(event.startDateTime),
            endTime: new Date(event.endDateTime),
            eventTitle: event.title
          },
          rooms: _.map(event.rooms, function(r) { return fromRoomDto(r); })
        };
      }
    };

    function getRoomDto(room) {
      var roomDto = {
        hidden: room.hidden,
        roomId: room.id,
        notes: room.description,
        layoutId: room.layout.id,
        equipment: _.map(room.equipment, function(e) { return getEquipmentDto(e.equipment); })
      };
      if (_.has(room, 'cancelled')) {
        roomDto.cancelled = room.cancelled;
      }

      if (_.has(room, 'roomReservationId')) {
        roomDto.roomReservationId = room.roomReservationId;
      }

      return roomDto;
    }

    function getEquipmentDto(equipment) {
      var equipmentDto = {
        equipmentId: equipment.name.id,
        quantityRequested: equipment.choosenQuantity
      };
      if (_.has(equipment, 'cancelled')) {
        equipmentDto.cancelled = equipment.cancelled;
      }

      if (_.has(equipment, 'equipmentReservationId')) {
        equipmentDto.equipmentReservationId = equipment.equipmentReservationId;
      }

      return equipmentDto;
    }

    function fromRoomDto(roomDto) {
      return {
        hidden: roomDto.hidden,
        id: roomDto.roomId,
        layout: {
          id: roomDto.layoutId
        },
        notes: roomDto.notes,
        roomReservationId: roomDto.roomReservationId,
        cancelled: roomDto.cancelled,
        equipment: _.map(roomDto.equipment, function(e) {
          return fromEquipmentDto(e);
        })
      };
    }

    function fromEquipmentDto(equipmentDto) {
      return {
        equipment: {
          name: {
            id: equipmentDto.equipmentId
          },
          choosenQuantity: equipmentDto.quantityRequested,
          equipmentReservationId: equipmentDto.equipmentReservationId,
          cancelled: equipmentDto.cancelled
        }
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
