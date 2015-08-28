'use strict';
//var stripe = require ('stripe');
require('angular-stripe');
angular
    .module('crossroads.give', ['angular-stripe','crossroads.core'])
    .constant('GIVE_PROGRAM_TYPES', { Fuel: 1, Events: 2, Trips: 3, NonFinancial: 4 })
    .constant('GIVE_ROLES', { StewardshipDonationProcessor: 7 });

require('./donation_details');
require('./directives/bankInfo.directive');
require('./directives/creditCardInfo.directive');
require('./directives/currencyMask.directive');
require('./directives/bankCreditCardDetails.directive');
