(function(){
  module.exports = function TimeFilter(){
    return manipulateTime;
  }

  function manipulateTime(input){
    // Split the time into hours, minutes and seconds
    var split = input.split(":");
    var period = "AM"; 
    if(split[0] > 11){
      period = "PM";
    }
    if(split[0] > 12){
      split[0] = Number(split[0]) - 12
    }
    return split[0] + ":" + split[1] + period;
  }
})()
