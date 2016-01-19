(function() {
  'use strict';

  module.exports = AddRoom;

  AddRoom.$inject = ['$log', '$rootScope', '$modal', 'AddEvent', 'Lookup', 'Room'];

  function AddRoom($log, $rootScope, $modal, AddEvent, Lookup, Room) {
    return {
      restrict: 'E',
      scope: {
        roomData: '='
      },
      templateUrl: 'add_room/add_room.html',
      controller: AddRoomController,
      controllerAs: 'addRoom',
      bindToController: true
    };

    function AddRoomController() {
      var vm = this;
      vm.choosenSite = choosenSite;
      vm.equipmentList = [];
      vm.layouts = Room.Layouts.query();
      vm.onAdd = onAdd;
      vm.removeRoom = removeRoom;
      vm.roomError = false;
      vm.showNoRoomsMessage = showNoRoomsMessage;
      vm.viewReady = false;

      activate();

      //////////////////

      function activate() {
        if (AddEvent.editMode) {
          Lookup.query({ table: 'crossroadslocations' }, function(locations) {
            AddEvent.eventData.event.congregation = _.find(locations, function(l) {
              return l.dp_RecordID === AddEvent.eventData.event.congregation.dp_RecordID;
            });
          });
        }
     
        if (AddEvent.eventData.event.congregation !== undefined) {
          Room.ByCongregation.query({
            congregationId: AddEvent.eventData.event.congregation.dp_RecordID
          }, function(data) {
            vm.rooms = data;
            vm.roomData = _.map(vm.roomData, function(r) {
              if(r.name === undefined) {
                r.name = _.find(data, function(roo){
                  return roo.id === r.id;
                }).name;
              }
              return r;
            });
            Room.Equipment.query({congregationId: AddEvent.eventData.event.congregation.dp_RecordID}, function(data){
              vm.equipmentList = data;                    
              _.forEach(vm.roomData, function(roomD) {
                roomD.equipment = mapEquipment(data, roomD.equipment);     
              });
              vm.viewReady = true;     
            });
          });

          return;
        }

        $log.error('The congregation was not passed in so we can\'t get the list of rooms or equipment');
        return;
      }

      function choosenSite() {
        return AddEvent.eventData.event.congregation.dp_RecordName;
      }

	    function mapEquipment(equipmentLookup, currentEquipmentList) {
        return _.map(currentEquipmentList, function(current) {
          if (current.equipment.name.quantity === undefined) {
            var found = _.find(equipmentLookup, function(e) {
              return e.id === current.equipment.name.id;
            });
            if (found) {
              current.equipment.name.quantity = found.quantity;
            }
            return current;
          }
        });
      }

      function removeRoomModal(room) {
				 var modalInstance = $modal.open({
					controller: 'RemoveRoomController as removeRoom',
					templateUrl: 'remove_room/remove_room.html',
					resolve: {
						items: function () {
							return room;
						}
					}
				});
				return modalInstance;
			}

      function onAdd() {
        if (vm.choosenRoom) {
          // is this room already added?
          var alreadyAdded = _.find(vm.roomData, function(r) {
            return r.id === vm.choosenRoom.id;
          });

          if (alreadyAdded) {
            $rootScope.$emit('notify', $rootScope.MESSAGES.allReadyAdded);
            return;
          }

          vm.roomData.push(vm.choosenRoom);
          return;
        }

        $rootScope.$emit('notify', $rootScope.MESSAGES.chooseARoom);
      }

      function removeRoom(currentRoom) {
        $log.debug("remove room: " + currentRoom);
        // show a modal????
      	var modalInstance = removeRoomModal(currentRoom);

        modalInstance.result.then(function() {
          vm.roomData = _.filter(vm.roomData, function(r) {
            // only return elements that aren't currentRoom
            return r.id !== currentRoom.id;
          });
        }, function() {
          $log.info('user doesn\'t want to delete this room...');
        });
			}

      function showNoRoomsMessage() {
        return (!vm.viewReady || vm.rooms === undefined || vm.rooms.length < 1);
      }
    }
  }

})();
