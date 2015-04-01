var app = require("angular").module("crossroads");
require('./give.html');

/****** DEMO PAGES ******/
require('./demo/guest_giver/give.html');
require('./demo/guest_giver/give-login.html');
require('./demo/guest_giver/give-login-guest.html');
require('./demo/guest_giver/give-confirmation.html');
require('./demo/guest_giver/give-register.html');
require('./demo/guest_giver/give-logged-in-bank-info.html');
require('./demo/guest_giver/give-confirm-amount.html');
require('./demo/guest_giver/give-change-information.html');

app.controller("GiveCtrl", ['$scope', require("./give_controller")]);