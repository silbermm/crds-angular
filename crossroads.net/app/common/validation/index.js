var constants = require('../../constants');

angular.module(constants.MODULES.COMMON)
    .factory('Validation', require('./validation.service'))
    .directive('invalidZip', require('./invalidZip.validation.directive'))
    ;

