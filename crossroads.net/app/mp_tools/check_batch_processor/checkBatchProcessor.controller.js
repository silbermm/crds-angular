(function() {
  'use strict()';

  module.exports = CheckBatchProcessor;

  CheckBatchProcessor.$inject = ['$rootScope', '$log', 'MPTools', 'CheckScannerBatches', 'getPrograms'];

  function CheckBatchProcessor($rootScope, $log, MPTools, CheckScannerBatches, getPrograms) {
    var _this = this;

    _this.batches = [];
    _this.params = MPTools.getParams();

    activate();

    function activate() {
      CheckScannerBatches.query(function(data) {
        _this.batches = data;
      });
    }
  }
})();
