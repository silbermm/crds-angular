require('../../dependencies/dependencies');
require('../../core/core');

describe('CoreController', function() {

  beforeEach(angular.mock.module('crossroads'));

  var $controller, $rootScope, Message, MESSAGES, $aside, $scope, controller, growl, screenSize, $state;

  beforeEach(inject(function(_$controller_, _$rootScope_, _ContentBlock_, _MESSAGES_, _$aside_, _growl_, _screenSize_, _$state_, _$log_){
    $controller = _$controller_;
    $rootScope = _$rootScope_;
    ContentBlock = _ContentBlock_;
    MESSAGES = _MESSAGES_;
    $aside = _$aside_;
    growl = _growl_;
    screenSize = _screenSize_;
    $state = _$state_;
    $scope = $rootScope.$new();
    controller = $controller('coreController', {
      '$scope': $scope,
      '$rootScope': $rootScope,
      'MESSAGES': MESSAGES,
      'ContentBlock': ContentBlock,
      'growl': growl,
      '$aside': $aside,
      'screenSize': screenSize,
      '$state': $state
    });
  }));

  describe('function mapContentBlocks', function() {
    it('should set MESSAGES with retrieved contentBlocks', function() {
      var contentBlocks = [
        null,
        {id: 1, title: 'firstMessage'},
        {id: 2, title: 'secondMessage'},
        {id: 3, title: 'thirdMessage'},
        null,
        {id: 4, title: 'fourthMessage'},
      ];

      controller.mapContentBlocks(contentBlocks);
      expect(_.size(MESSAGES)).toBe(4);
      expect(MESSAGES.firstMessage).toBe(1);
      expect(MESSAGES.secondMessage).toBe(2);
      expect(MESSAGES.thirdMessage).toBe(3);
      expect(MESSAGES.fourthMessage).toBe(4);
    });
  });
});
