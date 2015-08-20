(function() {
  'use strict';

  module.exports = PagesController;

  PagesController.$inject = [];

  function PagesController() {
    var vm = this;

    vm.handleNext = handleNext;
    vm.handlePrevious = handlePrevious;
    vm.handleSubmit = handleSubmit;
    vm.showSubmit = showSubmit;

    function handleNext(nextPage) {
      vm.currentPage = nextPage;
    }

    function handlePrevious(prevPage) {
      vm.currentPage = prevPage;
    }

    function handleSubmit() {

    }

    function showSubmit() {
      
    }

  }
})();
