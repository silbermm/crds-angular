"use strict";
(function () {

    angular.module("crdsProfile").directive("validateDate", ["$log", ValidDateDirective]);

    function ValidDateDirective($log) {

        return {
            restrict: "A",
            require: 'ngModel',
            link: function(scope, element, attrs, ngModel){
                ngModel.$validators.invalidDate = function (value) {
                    if (value === undefined) {
                        return true;
                    }
                    // Date coming in is formatted mm/dd/yyyy, not valid for Moment.js
                    var dateFormat = /^(0[1-9]|1[012])[- /.](0[1-9]|[12][0-9]|3[01])[- /.]((19|20)\d\d)$/;
                    if (!value.match(dateFormat)) {
                        return false;
                    } else {
                        var newDate = value.replace(dateFormat, "$3 $1 $2");
                        var m = moment(newDate, "YYYY MM DD");
                        return m.isValid();
                    }
                };
            }

        };

    }

})()