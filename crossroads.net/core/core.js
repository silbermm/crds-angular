'use strict';

require('../styles/main.scss');

require('./templates/nav.html');
require('./templates/nav-mobile.html');
require('./templates/404.html');
require('./templates/500.html');
require('./templates/footer.html');
require('./templates/header.html');
require('./templates/brand-bar.html');

require('./app.core.module');
require('./login');
require('./home');
require('./register/register_directive');
require('./cms/services/cms_services_module');

// Common Components
require('./components/svgIcon.directive');
require('./components/preloader');
require('./components/volunteer_applications');
require('./components/loadingButton');

require('./content');
require('./email_field/email_field_directive');
require('./date_field/date_field_directive');
require('./password_field/password_field_directive');
require('./filters');

require('./core.config');
require('./core.controller');
require('./core.run');

require('./errors');

// require and export crds_utilities on global scope
require('expose?crds_utilities!./crds_utilities.js');
