(function() {
  'use strict';

  module.exports = PagesController;

  PagesController.$inject = ['$location', '$anchorScroll'];

  function PagesController($location, $anchorScroll) {
    var vm = this;

    vm.handleNext = handleNext;
    vm.handlePrevious = handlePrevious;
    vm.handleSubmit = handleSubmit;
    vm.showSubmit = showSubmit;

    function handleNext(nextPage) {
      vm.currentPage = nextPage;
      $location.hash('form-top');
      $anchorScroll();
    }

    function handlePrevious(prevPage) {
      vm.currentPage = prevPage;
      $location.hash('form-top');
      $anchorScroll();
    }

    function handleSubmit() {

    }

    function showSubmit() {
      
    }

  }
})();
