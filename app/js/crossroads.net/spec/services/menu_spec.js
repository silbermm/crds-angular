describe("Menu", function() {
  beforeEach(module("crossroads"));

  beforeEach(inject(function(Menu) {
    this.menu = Menu;
  }));

  it("toggles mobile display", function() {
    expect(this.menu.isMobileShowing).toBeFalsy();
    this.menu.toggleMobileDisplay();
    expect(this.menu.isMobileShowing).toBeTruthy();
  });

  describe("toggleSelecteMenuItem", function() {
    it("selects index when nothing selected", function() {
      expect(this.menu.selectedMenuItem).toBeFalsy();
      this.menu.toggleMenuItem(2);
      expect(this.menu.selectedMenuItem).toEqual(2);
    });

    it("unselects if same index is toggled", function() {
      this.menu.toggleMenuItem(2);
      this.menu.toggleMenuItem(2);
      expect(this.menu.selectedMenuItem).toBeFalsy();
    });
  });
});
