'use strict';

var app = angular.module('crossroads');

require('./adbox-index.html');
require('./ads/donate.html');
require('./ads/e-gift.html');
require('./ads/faq.html');
require('./ads/give.html');
require('./ads/grow.html');
require('./ads/kids.html');
require('./ads/previous_series.html');
require('./ads/sign_up_go.html');
require('./ads/sign_up.html');
require('./ads/spotlight_daniel.html');
require('./ads/spotlight_heather.html');
require('./ads/spotlight_karen.html');
require('./ads/spotlight_kristen.html');
require('./ads/spotlight_skip.html');
require('./ads/teen_jeremy.html');
require('./ads/teen_naomi.html');

app.controller("AdboxCtrl", ['$log', require("./adbox.controller")]);
