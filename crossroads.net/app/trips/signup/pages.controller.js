(function() {
  'use strict';

  module.exports = PagesController;

  PagesController.$inject = ['$location', '$anchorScroll'];

  /**
   * Controller for all of the pages directives
   * Variobles passed into the directives:
   *    - currentPage
   *    - pageTitle
   *    - numberOfPages
   */
  function PagesController($location, $anchorScroll) {
    var vm = this;

    vm.handleNext = handleNext;
    vm.handlePrevious = handlePrevious;
    vm.handleSubmit = handleSubmit;

    function handleNext(nextPage) {
      vm.currentPage = nextPage;
      toTop();
    }

    function handlePrevious(prevPage) {
      vm.currentPage = prevPage;
      toTop();
    }

    function handleSubmit() {
      // submit info and then show the thankyou page directive
      vm.currentPage = vm.numberOfPages + 1;
      toTop();
    }

    function toTop() {
      $location.hash('form-top');
      $anchorScroll();
    }

  }
})();
