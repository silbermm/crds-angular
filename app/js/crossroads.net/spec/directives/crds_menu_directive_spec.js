describe("crds menu directive", function() {
  beforeEach(function() {
    module("crossroads");
  });

  beforeEach(inject(function($compile, $rootScope, $httpBackend) {
    $httpBackend.whenGET("https://my.crossroads.net/ministryplatform/oauth/me").respond("Success");
    $httpBackend.whenGET("/api/ministryplatformapi/PlatformService.svc/GetCurrentUserInfo").respond({
      "DateOfBirth": null,
      "DisplayName": "Doe, John",
      "EmailAddress": "jdoe@example.net",
      "ExternalIdentities": [],
      "FirstName": "John",
      "GenderId": 1,
      "LastName": "Doe",
      "Locale": "",
      "MaritalStatusId": 2,
      "MiddleName": null,
      "MobilePhone": "513-555-1234",
      "NewPassword": null,
      "Nickname": "John",
      "PrefixId": null,
      "SuffixId": null,
      "Theme": "mpdark",
      "TimeZoneId": ""
    });

    this.scope = $rootScope;
    this.scope.menu = {
      headings: [
      {
        title: "hi"
      }, {
        link: "foo",
        items: [
        {
          title: "thing",
          link: "#",
          items: [
          {
            title: "thing",
            link: "#"
          }
          ]
        }
        ]
      }
      ]
    };
    this.compile = $compile;
    this.element = this.compile("<div crds-menu='menu'></div>")(this.scope);
    this.scope.$digest();
  }));

  it("should have an element", function() {
    expect(this.element.html().length).toBeGreaterThan(0);
  });

  it("should render a menu", function() {
    expect(this.element.find(".navbar--heading__title").html()).toMatch(/hi/);
  });

  it("should render items", function() {
    expect(this.element.find(".heading__item").html()).toMatch(/thing/);
  });
});
