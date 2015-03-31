// Common configuration files with defaults plus overrides from environment vars
var webServerDefaultPort = 8080;

module.exports = {

  // A base URL for your application under test.
  baseUrl:
    'http://' + (process.env.CRDS_CLIENT_HOST || 'localhost') +
          ':' + (process.env.HTTP_CLIENT_PORT || webServerDefaultPort)

};