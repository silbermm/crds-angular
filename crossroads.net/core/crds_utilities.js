'use strict()';

module.exports = {
  getCookie: function(cname) {
    var name = cname + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
      var c = ca[i];
      while (c.charAt(0) == ' ') c = c.substring(1);
      if (c.indexOf(name) == 0) return c.substring(name.length, c.length);
    }
    return "";
  },

  // This custom type is needed to allow us to NOT URLEncode slashes when using ui-sref
  // See this post for details: https://github.com/angular-ui/ui-router/issues/1119
  preventRouteTypeUrlEncoding: function(urlMatcherFactory, routeType, urlPattern) {
    return(urlMatcherFactory.type(routeType, {
      encode: function(val) { return val != null ? val.toString() : val; },
      decode: function(val) { return val != null ? val.toString() : val; },
      is: function(val) { return this.pattern.test(val); },
      pattern: urlPattern
    }));
  }
};

