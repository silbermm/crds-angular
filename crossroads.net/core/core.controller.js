'use strict()';
(function(){

  angular.module('crossroads.core').controller('coreController', CoreController);

  CoreController.$inject = [
    '$scope',
    '$rootScope',
    'MESSAGES',
    'ContentBlock',
    'growl',
    '$aside',
    'screenSize',
    '$state'];

  function CoreController($scope, $rootScope, MESSAGES, ContentBlock, growl, $aside, screenSize, $state) {

    var vm = this;

    vm.asideState = { open: false };
    vm.openAside = openAside;
    vm.prevent = prevent;
    vm.resolving = true;
    vm.state = $state;
    vm.mapContentBlocks = mapContentBlocks;

    ////////////////////////////
    // State Change Listeners //
    ////////////////////////////
    $scope.$on('$stateChangeStart', function(event, toState, toParams, fromState, fromParams) {
      if (toState.resolve && !event.defaultPrevented) {
        vm.resolving = true;
      }
    });

    $scope.$on('$stateChangeSuccess', function(event, toState, toParams, fromState, fromParams) {
      vm.resolving = false;
    });

    $scope.$on('$stateChangeError', function(event,toState, toParams, fromState, fromParams, error){
      console.error('$stateChangeError: ' + error);
      //TODO: put the 'toState' in the session if we want to redirect to that page
      vm.resolving = false;
      $state.go('content', {link:'/server-error/'});
    });


    //////////////////////////
    /////// $ROOTSCOPE ///////
    //////////////////////////
    $rootScope.mobile = screenSize.on('xs, sm', function(match){ $rootScope.mobile = match; });

    $rootScope.$on('notify', function (event, msg, refId, ttl) {
      var parms = { };
      if(refId !== undefined && refId !== null) {
        parms.referenceId = refId;
      }
      if(ttl !== undefined && ttl !== null) {
        parms.ttl = ttl;
      }
      growl[msg.type](msg.content, parms);
    });

    $rootScope.$on('mailchimp-response', function (event, result, msg) {
      if (result === 'success') {
        $rootScope.$emit('notify', $rootScope.MESSAGES.mailchimpSuccess);
      } else if (result === 'error') {
        $rootScope.$emit('notify', $rootScope.MESSAGES.generalError);
      }
    });

    $rootScope.$on('context', function (event, id) {
     var contentBlocks = ContentBlock.get({
        id: id
      }, function () {
        return contentBlocks.ContentBlock.content;
      });
    });

    var contentBlockRequest = ContentBlock.get('', function () {
      mapContentBlocks(contentBlockRequest.contentBlocks);
    });

    function mapContentBlocks(contentBlocks) {
      _.reduce(contentBlocks, function(messages, cb) {
        messages[cb.title] = cb;
        return(messages);
      }, MESSAGES);
    }

    function openAside(position, backdrop) {
      vm.asideState = {
        open: true,
        position: position
      };

      function postClose() {
        vm.asideState.open = false;
      }
      $aside.open({
        templateUrl: 'templates/nav-mobile.html',
        placement: position,
        size: 'sm',
        controller: function($scope, $modalInstance) {
          $scope.ok = function(e) {
            $modalInstance.close();
            e.stopPropagation();
          };
          $scope.cancel = function(e) {
            $modalInstance.dismiss();
            e.stopPropagation();
          };
        }
      }).result.then(postClose, postClose);

    }

    function prevent(evt){
      evt.stopPropagation();
    }

  }

})();
