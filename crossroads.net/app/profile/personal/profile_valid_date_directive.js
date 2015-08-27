"use strict";
(function () {


    module.exports = function($log) {

        return {
            restrict: "A",
            require: 'ngModel',
            link: function(scope, element, attrs, ngModel){
                console.log(attrs);
                ngModel.$validators.invalidDate = function (value) {
                    var format = attrs.validateDate;
                    format = format.replace(/[- /.]/, ' ');
                    if (value === undefined) {
                        return true;
                    }
                    if (format === undefined || format === '') {
                        format = 'MM DD YYYY';
                    }
                    var dateFormat = format.replace(/[- /.]/g, '[- /.]');
                    dateFormat = dateFormat.replace(/YYYY/, '((19|20)\\d\\d)');
                    dateFormat = dateFormat.replace(/MM/, '(0[1-9]|1[012])');
                    dateFormat = dateFormat.replace(/DD/, '(0[1-9]|[12][0-9]|3[01])');
                    dateFormat = new RegExp(dateFormat);
                    console.log('Format: ' + dateFormat);
                    // Date coming in is formatted mm/dd/yyyy, not valid for Moment.js
                    //var dateFormat = /^(0[1-9]|1[012])[- /.](0[1-9]|[12][0-9]|3[01])[- /.]((19|20)\d\d)$/;
                    if (!value.match(dateFormat)) {
                        return false;
                    } else {
                        var m = moment(value, format);
                        return m.isValid();
                    }
                };
            }

        };

    }

})()