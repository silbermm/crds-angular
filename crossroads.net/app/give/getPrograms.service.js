"use strict()";
(function() {

  module.exports = GetPrograms;

  function GetPrograms() {

    var getPrograms = {

      fetchPrograms: function() {

        var hardcodedList = [
          "Ministry Fund",
          "Game Change Fund",
          "Old St. George"
        ];

        return hardcodedList;
      }

    };

    return getPrograms;
  }

})();