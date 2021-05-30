$(".wraploginn").mouseover(function () {
    $(".wraplogin").css("border", "2px solid #d06a6a");
    $(".wraplogin").css("animation-play-state", "paused");
});
$(".wraploginn").mouseout(function () {
    $(".wraplogin").css("border", "0 none");
    $(".wraplogin").css("animation-play-state", "running");

});

