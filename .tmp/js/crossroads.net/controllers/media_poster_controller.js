'use strict';

angular.module('crossroads')

.controller('mediaPosterCtrl', function($element) {

    // showPoster will hide the child img & svg tags, 
    // then find any YouTube embeds (iframes) that are in the parent element and show it.
    // Based on logic from here: https://gist.github.com/zigotica/4438876

    this.showPoster = function() {
      var elm = $element,
          conts   = elm.contents(),
          le      = conts.length,
          ifr     = null;

      for(var i = 0; i<le; i++){
        if(conts[i].nodeType === 8) ifr = conts[i].textContent;
      }

      $element.addClass('player').html(ifr);
      $element.off('click');
    };
});