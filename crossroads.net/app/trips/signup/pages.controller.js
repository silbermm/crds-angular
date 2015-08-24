(function() {
  'use strict';

  module.exports = PagesController;

  PagesController.$inject = ['$location', '$anchorScroll', 'Trip'];

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
    vm.nolaRequired = nolaRequired;
    vm.whyPlaceholder = '';

    activate();

    function activate() {
      if (vm.pageTitle === 'Go India Application') {
        vm.whyPlaceholder = 'Please be specific. ' +
          'In instances where we have a limited number of spots, we strongly consider responses to this question.';
      }

    }

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
      vm.currentPage = 'thanks';
      toTop();
    }

    function nolaRequired() {
      if (vm.pageTitle === 'GO NOLA Application') {
        return 'required';
      }

      return '';
    }

    function toTop() {
      $location.hash('form-top');
      $anchorScroll();
    }

  }
})();
