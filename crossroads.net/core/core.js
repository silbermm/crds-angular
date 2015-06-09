'use strict';

require('../styles/main.scss');

require('./templates/nav.html');
require('./templates/nav-mobile.html');
require('./templates/404.html');
require('./templates/500.html');
require('./templates/footer.html');
require('./templates/header.html');
require('./templates/brand-bar.html');
require('./templates/favicons.html');
require('./templates/head.html');

require('./app.core.module')
require('./login');
require('./home');
require('./register/register_directive');
require('./cms/services/cms_services_module');

require('./components/svgIcon.directive');
require('./components/preloader');
require('./content');
require('./email_field/email_field_directive');
require('./password_field/password_field_directive');
require('./filters');

require('./app.config');
require('./app.controller');
require('./app.run');
require('./routes');

require('./errors');

// require and export getCookie on global scope
require('expose?getCookie!./utilities/cookies.js');
