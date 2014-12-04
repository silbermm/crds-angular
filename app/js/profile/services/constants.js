'use strict';
// This is a constant service where we can keep the values which we do not want
// to change and change based on the business requirements. Please ignore the
// weight names since these are subject to change based on the Ministry
// platform data.

angular.module('crdsProfile')
    .constant('Constants', {
        weightages: {
            crossroadsLocation:15,
            streetAddress:10,
            email:14,
            atleastOneSkillSelected:8,
            profileImage:20,
            birthDay:5,
            dateStartedAttending:4,
            firstName:2,
            lastName:2,
            gender:1,
            maritalStatus:1,
            mobilePhone:15,
            homePhone:2,
            employer:1
        }
    });
