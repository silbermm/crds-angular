using System;
using System.Collections.Generic;
using System.Linq;
using Crossroads.Utilities.Interfaces;
using log4net;
using MinistryPlatform.Translation.Extensions;
using MinistryPlatform.Translation.Models;
using MinistryPlatform.Translation.Models.EventReservations;
using MinistryPlatform.Translation.Services.Interfaces;

namespace MinistryPlatform.Translation.Services
{
    public class EquipmentService : BaseService, IEquipmentService
    {
        private readonly IMinistryPlatformService _ministryPlatformService;
        private readonly ILog _logger = LogManager.GetLogger(typeof(RoomService));

        public EquipmentService(IMinistryPlatformService ministryPlatformService, IAuthenticationService authenticationService, IConfigurationWrapper configuration)
            : base(authenticationService, configuration)
        {
            _ministryPlatformService = ministryPlatformService;
        }

        public List<EquipmentReservationDto> GetEquipmentReservations(int eventId, int roomId)
        {
            var token = ApiLogin();
            var search = string.Format(",{0},{1}", eventId, roomId);
            var records = _ministryPlatformService.GetPageViewRecords("GetEquipmentReservations", token, search);

            return records.Select(record => new EquipmentReservationDto
            {
                Cancelled = record.ToBool("Cancelled"),
                EquipmentId = record.ToInt("Equipment_ID"),
                EventEquipmentId = record.ToInt("Event_Equipment_ID"),
                EventId = record.ToInt("Event_ID"),
                Notes = record.ToString("Notes"),
                QuantityRequested = record.ToInt("Quantity_Requested"),
                RoomId = record.ToInt("Room_ID")
            }).ToList();
        }

        public int CreateEquipmentReservation(EquipmentReservationDto equipmentReservation, string token)
        {
            var equipmentReservationPageId = _configurationWrapper.GetConfigIntValue("EquipmentReservationPageId");
            var equipmentDictionary = new Dictionary<string, object>
            {
                {"Event_ID", equipmentReservation.EventId},
                {"Room_ID", equipmentReservation.RoomId},
                {"Equipment_ID", equipmentReservation.EquipmentId},
                {"Notes", equipmentReservation.Notes},
                {"Quantity_Requested", equipmentReservation.QuantityRequested},
                {"Cancelled", equipmentReservation.Cancelled}
            };

            try
            {
                return (_ministryPlatformService.CreateRecord(equipmentReservationPageId, equipmentDictionary, token, true));
            }
            catch (Exception e)
            {
                var msg = string.Format("Error creating Equipment Reservation, equipmentReservation: {0}", equipmentReservation);
                _logger.Error(msg, e);
                throw (new ApplicationException(msg, e));
            }
        }

        public void UpdateEquipmentReservation(EquipmentReservationDto equipmentReservation)
        {
            var token = ApiLogin();
            var equipmentReservationPageId = _configurationWrapper.GetConfigIntValue("EquipmentReservationPageId");
            var equipmentDictionary = new Dictionary<string, object>
            {
                {"Event_Equipment_ID", equipmentReservation.EventEquipmentId},
                {"Event_ID", equipmentReservation.EventId},
                {"Room_ID", equipmentReservation.RoomId},
                {"Equipment_ID", equipmentReservation.EquipmentId},
                {"Notes", equipmentReservation.Notes},
                {"Quantity_Requested", equipmentReservation.QuantityRequested},
                {"Approved", equipmentReservation.Approved},
                {"Cancelled", equipmentReservation.Cancelled}
            };

            try
            {
                _ministryPlatformService.UpdateRecord(equipmentReservationPageId, equipmentDictionary, token);
            }
            catch (Exception e)
            {
                var msg = string.Format("Error updating Equipment Reservation, equipmentReservation: {0}", equipmentReservation);
                _logger.Error(msg, e);
                throw (new ApplicationException(msg, e));
            }
        }

        public List<Equipment> GetEquipmentByLocationId(int locationId)
        {
            var t = ApiLogin();
            var search = string.Format(",,,,{0}", locationId);
            var records = _ministryPlatformService.GetPageViewRecords("EquipmentByLocationId", t, search);

            return records.Select(record => new Equipment
            {
                EquipmentId = record.ToInt("Equipment_ID"),
                EquipmentName = record.ToString("Equipment_Name"),
                QuantityOnHand = record.ToInt("Quantity_On_Hand")
            }).ToList();
        }
    }
}