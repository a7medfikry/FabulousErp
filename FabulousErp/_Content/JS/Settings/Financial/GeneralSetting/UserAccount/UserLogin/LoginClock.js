function Clock() {
    var today = new Date();
    var h = today.getHours();
    var hour = today.getHours();
    var m = today.getMinutes();
    var s = today.getSeconds();
    var timeOfDay = (h < 12) ? " AM" + " ,Cairo Time" : " PM" + " ,Cairo Time";
    hour = (h > 12) ? h - 12 : h;
    m = checkTime(m);
    s = checkTime(s);
    document.getElementById('clock').innerHTML = hour + ":" + m + ":" + s + timeOfDay;
    //if (h < 12) {
    //    $("#text").text("Good Morning");
    //}
    //else {
    //    $("#text").text("Good Afternoon");
    //}
    var t = setTimeout(Clock, 500);
}
function checkTime(i) {
    if (i < 10) { i = "0" + i };  // add zero in front of numbers < 10
    return i;
}