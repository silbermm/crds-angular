describe('random block directive', function() {
  beforeEach(function() {
    module('crossroads');
  });

  beforeEach(inject(function($compile) {
    this.element = $compile('<div class="random-block"><div></div><div></div></div>')({});
  }));

  it('hides one random child element', function() {
    expect(this.element.find("div.hidden").length).toEqual(1);
  });
});
