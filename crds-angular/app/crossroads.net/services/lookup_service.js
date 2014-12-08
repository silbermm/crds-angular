angular.module('crossroads').factory('LookupService', function ($http, Session) {
    this.gender = function (thinkMinistry) {
        return {
            all: function () {
                return thinkMinistry.get('GetPageLookupRecords?pageId=311').then(function (data) {
                    return data;
                });
            }
        };
    }

    this.maritalStatus = function () {
        return {
            all: function (thinkMinistry) {
                return thinkMinistry.get('GetPageLookupRecords?pageId=339').then(function (data) {
                    return data;
                });
            }
        };
    }
});