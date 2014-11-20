'use strict';

angular.element(window).bind('load', function() {
    var addThisScript = document.createElement('script');
    addThisScript.setAttribute('type', 'text/javascript');
    addThisScript.setAttribute('src', '//s7.addthis.com/js/300/addthis_widget.js#domready=1');
    document.body.appendChild(addThisScript);
    // jshint ignore:start
    var addthis_config = addthis_config||{};
    addthis_config.pubid = 'ra-5391d6a6145291c4';
    // jshint ignore:end
});
