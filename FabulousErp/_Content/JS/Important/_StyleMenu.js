// generate Cookies 
function createCookie(name, value, days) {
    if (days) {
        var date = new Date();
        date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
        var expires = "; expires=" + date.toGMTString();
    }
    else var expires = "";
    document.cookie = name + "=" + value + expires + "; path=/";
}

function getCookie(cname) {
    var name = cname + "=";
    var decodedCookie = decodeURIComponent(document.cookie);
    var ca = decodedCookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') {
            c = c.substring(1);
        }
        if (c.indexOf(name) == 0) {
            return c.substring(name.length, c.length);
        }
    }
    return "";
}

function eraseCookie(name) {
    createCookie(name, "", -1);
}

function checkCookie() {
    var x = getCookie("color");
    if (x === "dark-style") {
        colordark();
    }
}
// generate Cookies 



$('.menu-right').click(function () {
    $('.menu-right').css({
        width: '150px',
        height: '200px'
    });
    $('.overlay').addClass('active');
});

$("#dark-style").click(function () {
    colordark();
});





function colordark() {
    //date navbar
    $(".navbar-expand-lg").css({
        background: 'black'
    });

    //footer
    $(".footer").css({
        background: '#25252e'
    });

    //favourites leftbar
    $(".nav-left").css({
        background: 'black'
    });

    createCookie('color', 'dark-style', 2);
}
