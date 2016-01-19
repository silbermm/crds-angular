require('crds-core');
require('../../app/ang');

require('../../app/app');

describe('MP Tools Service', function() {

  var $rootScope, scope, MPTools, $location;

  beforeEach(angular.mock.module('crossroads.mptools'));

  beforeEach(inject(function(_$location_){
    $location = _$location_
    spyOn($location, 'search').and.returnValue({
      dg:'8b6242c9-ea32-40f7-97a2-e2bb3524ced2',
      'ug':'c29e64a5-820b-461f-a57c-5831d070d578', 
      pageID:'292',
      recordID:'-1',
      recordDescription: null,
      s:'11467', 
      sc:'1', 
      p:0, 
      v:387
    });
  }));

  beforeEach(inject(function(_$location_, _MPTools_) {
    MPTools = _MPTools_;
  }));

  it("should have the correct params from the session service", function(){
    var params = MPTools.getParams();
    expect(params.userGuid).toBe("c29e64a5-820b-461f-a57c-5831d070d578");
    expect(params.domainGuid).toBe('8b6242c9-ea32-40f7-97a2-e2bb3524ced2');
    expect(params.pageId).toBe('292');
    expect(params.recordId).toBe('-1');
    expect(params.selectedRecord).toBe('11467');
    expect(params.selectedCount).toBe('1');
  });

  it("should have the correct params when passed in", function(){
    MPTools.setParams($location);
    var params = MPTools.getParams();
    expect(params.userGuid).toBe("c29e64a5-820b-461f-a57c-5831d070d578");
    expect(params.domainGuid).toBe('8b6242c9-ea32-40f7-97a2-e2bb3524ced2');
    expect(params.pageId).toBe('292');
    expect(params.recordId).toBe('-1');
    expect(params.selectedRecord).toBe('11467');
    expect(params.selectedCount).toBe('1');
  });

});
