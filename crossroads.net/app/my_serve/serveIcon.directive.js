(function(){

  module.exports = ServeIcon;

  ServeIcon.$inject = [];

  function ServeIcon(){
    return {
      restrict: 'E',
      scope: {
        'member': '=member'
      },
      link: link
    }

    function link(scope, el, attr){
      console.log(scope.member.serveRsvp);
      if(scope.member.serveRsvp !== null && scope.member.serveRsvp !== undefined){
        if(scope.member.serveRsvp.attending !== null && scope.member.serveRsvp.attending !== undefined) {
          if(scope.member.serveRsvp.attending){
            el.append("<svg viewBox='0 0 32 32' class='icon icon-check-circle'><use xlink:href=\"#check-circle\" class='text-success'></use> </svg>");
          } else {
            el.append("<svg viewBox='0 0 32 32' class='icon icon-cancel-circle'><use xlink:href=\"#cancel-circle\" class='text-danger'></use> </svg>");
          }
        } else {
          el.addClass("hidden"); 
        }
      } else {
        el.addClass("hidden"); 
      }
    }
  }

})();
