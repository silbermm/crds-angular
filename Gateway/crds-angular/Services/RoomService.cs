using System;
using System.Collections.Generic;
using System.Linq;
using crds_angular.Models.Crossroads.Events;
using crds_angular.Services.Interfaces;
using MinistryPlatform.Translation.Services;

namespace crds_angular.Services
{
    public class RoomService : IRoomService
    {
        private readonly MinistryPlatform.Translation.Services.Interfaces.IRoomService _roomService;
        private readonly MinistryPlatform.Translation.Services.Interfaces.IEquipmentService _equipmentService;
        private readonly MinistryPlatform.Translation.Services.Interfaces.IEventService _eventService;

        public RoomService(MinistryPlatform.Translation.Services.Interfaces.IRoomService roomService,MinistryPlatform.Translation.Services.Interfaces.IEquipmentService equipmentService,MinistryPlatform.Translation.Services.Interfaces.IEventService eventService)
        {
            _roomService = roomService;
            _equipmentService = equipmentService;
            _eventService = eventService;
        }

        //public void CreateRoom()
        //{
            
        //}

        public List<Room> GetRoomsByLocationId(int id)
        {
            var eventDto = new EventReservationDto
            {
                CongregationId = 7,
                ContactId = 768379,
                Description = "description: testing api",
                DonationBatchTool = false,
                EndDateTime = DateTime.Today,
                EventTypeId = 4,
                MeetingInstructions = "meeting instructions: testing THE api",
                MinutesSetup = 8,
                MinutesTeardown = 9,
                ProgramId = 156,
                ReminderDaysId = 4,
                SendReminder = false,
                StartDateTime = DateTime.Today,
                Title = "title: testing api"

            };

            var eventId = _eventService.CreateEvent(eventDto);
            
           // var eventId = 3184060;
           // var roomId = 208;
           // var tmpResr = new RoomReservationDto();
           // tmpResr.Approved = false;
           // tmpResr.Cancelled = false;
           // tmpResr.EventId = eventId;
           // tmpResr.Hidden = true;
           // tmpResr.Notes = "testing api";
           // tmpResr.RoomId = roomId;
           // tmpResr.RoomLayoutId = 2;
           //var roomReservationId = _roomService.CreateRoomReservation(tmpResr);

           // var tmpEqpRs = new EquipmentReservationDto();
           // tmpEqpRs.EquipmentId = 79;
           // tmpEqpRs.Approved = false;
           // tmpEqpRs.Cancelled = false;
           // tmpEqpRs.EventId = eventId;
           // tmpEqpRs.Notes = "testing api";
           // tmpEqpRs.QuantityRequested = 5;
           // tmpEqpRs.RoomId = roomId;
           // var equipmentReservationId = _equipmentService.CreateEquipmentReservation(tmpEqpRs);


            var records = _roomService.GetRoomsByLocationId(id);

            return records.Select(record => new Room
            {
                BuildingId = record.BuildingId,
                Id = record.RoomId,
                LocationId = record.LocationId,
                Name = record.RoomName,
                BanquetCapacity = record.BanquetCapacity,
                Description = record.Description,
                TheaterCapacity = record.TheaterCapacity
            }).ToList();
        }

        public List<RoomLayout> GetRoomLayouts()
        {
            var records = _roomService.GetRoomLayouts();

            return records.Select(record => new RoomLayout
            {
                Id = record.LayoutId,
                LayoutName = record.LayoutName
            }).ToList();
        }
    }
}