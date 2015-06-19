describe('Volunteer Application Factory', function() {

  var mockPageInfo = setupPageInfo();

  var $rootScope, scope, Page, Opportunity, VolunteerApplication, $httpBackend;

  beforeEach(module('crossroads'));

  beforeEach(inject(function(_Page_, _Opportunity_, _VolunteerApplication_, $injector) {
    Page = _Page_;
    Opportunity = _Opportunity_;
    VolunteerApplication = _VolunteerApplication_;
    $httpBackend = $injector.get('$httpBackend');
  }));

  it('should fetch the page object', function(){
    VolunteerApplication.getPageInfo();
    $httpBackend.expectGET( window.__env__['CRDS_CMS_ENDPOINT'] + 
        '/api/Page/?link=%2Fvolunteer-application%2Fkids-club%2F').respond(200, mockPageInfo );
    $httpBackend.flush();
  });

  it('should show the adult application', function(){

  });

  it('should show the student application', function(){

  });

  it('should show an error is the person is under 10', function(){

  });

  function setupPageInfo() {
    return {
      pages : [
      {
      accessDenied: '<p>Oops! Looks like you are not authorized to access this information.' +
        'If you think this is a mistake, please contact the system administrator.</p>',
      canEditType: 'Inherit',
      canViewType: 'Inherit',
      content: '<p>Please complete this application.</p>',
      extraMeta: null,
      group: '27705',
      hasBrokenFile: '0',
      hasBrokenLink: '0',
      id: 83,
      link: '/volunteer-application/kids-club/',
      menuTitle: null,
      metaDescription: null,
      noExistingResponse: '<p>Oops! Please contact the group leader of the team you are looking to serve.' +
        'Looks like we don\'t have a request from you to join this team.</p>',
      opportunity: '115',
      pageType: 'VolunteerApplicationPage',
      parent: 82,
      renderedContent: '<p>Please complete this application.</p>',
      reportClass: null,
      showInMenus: '1',
      showInSearch: '1',
      sort: '1',
      success: '<p>Default SUCCESS text for this page, see ApplicationPage.php to change</p>',
      title: 'Kids Club',
      uRLSegment: 'kids-club',
      version: '15'
      }]
    };
  }
 

});
