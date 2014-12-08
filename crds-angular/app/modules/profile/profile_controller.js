angular.module('crdsProfile').controller('crdsProfileCtrl', ['Profile', 'Lookup', ProfileController]);

function ProfileController(Profile, Lookup) {
    this.genders = Lookup.Genders.query();
    this.person = Profile.get({ id: 5 });
    this.maritalStatuses = Lookup.MaritalStatus.query();
    this.serviceProviders = Lookup.ServiceProviders.query();

    this.datePickerOpened = false;
    this.dt = null;

    this.today = function () {
        dt = new Date();
    }

    this.date = {
        open: function ($event) {
            $event.preventDefault();
            $event.stopPropagation();
            datePickerOpened = true;
        },
        dateOptions: {
            formatYear: 'yy',
            startingDay: 1,
            showWeeks : false
        },
        clear: function(){
            dt = null;
        }
    }
}