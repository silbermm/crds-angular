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
require('./register/register_directive');
require('./cms/services/cms_services_module');

