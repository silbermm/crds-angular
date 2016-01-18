(function() {
  'use strict';

  module.exports = AddRoom;

  AddRoom.$inject = ['$log', '$rootScope', '$modal', 'AddEvent', 'Room'];

  function AddRoom($log, $rootScope, $modal, AddEvent, Room) {
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
        if (AddEvent.eventData.event.congregation !== undefined) {
          Room.ByCongregation.query({
            congregationId: AddEvent.eventData.event.congregation.dp_RecordID
          }, function(data) {
            vm.rooms = data;
            _.forEach(vm.roomData, function(r) {
              if(r.name === undefined) {
                var tempRoom = _.find(data, function(roo){
                  return roo.id === r.id;
                });
                r.name = tempRoom.name;
              }
            });
            vm.viewReady = true;
          });

          vm.equipmentList = Room.Equipment.query({congregationId: AddEvent.eventData.event.congregation.dp_RecordID});
          return;
        }

        $log.error('The congregation was not passed in so we can\'t get the list of rooms or equipment');
        return;
      }

      function choosenSite() {
        // make sure it doesn't already exist first...
        return AddEvent.eventData.event.congregation.dp_RecordName;
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
