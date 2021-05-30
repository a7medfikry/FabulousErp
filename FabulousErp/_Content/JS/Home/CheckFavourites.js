//var formType = {
//    "G.S": 1,
//    "Setting": 1,
//    "Transaction": 2,
//    "Inquiry": 3,
//    "Report": 4
//}

$(document).ready(function () {

    var pageURL = '<li id="{{id}}"> <a href="{{link}}"> {{name}} </a></li>'

    //$.ajax({
    //    type: "GET",
    //    url: "/api/FavouriteAPI/GetFavourits",
    //    success: function (result) {
    //        $(result).each(function (i, res) {
    //            var linkResult = pageURL.replace("{{id}}", res.FormCode + "F").replace("{{link}}", res.FormURL).replace("{{name}}", res.FormName);

    //            var type = res.Type;

    //            if (type === 1) {

    //                $('#settingFavTemplate').append(linkResult);

    //            } else if (type === 2) {

    //                $('#transactionFavTemplate').append(linkResult);

    //            } else if (type === 3) {

    //                $('#inquiryFavTemplate').append(linkResult);

    //            } else if (type === 4) {

    //                $('#reportFavTemplate').append(linkResult);
    //            }

    //            if (res.FormURL == window.location.pathname) {
    //                $('.favouriteCheckBox').prop('checked', true);
    //            }

    //        });
    //    }
    //});

    $('.favouriteCheckBox').click(function () {

        var checkVal = $(this).val();
        var checkURL = window.location.pathname;
        var checkName = $(this).attr("data-FormName");

        if ($(this).is(':checked')) {

            var getType = $('.MainHeaderNav').find('a[href="' + checkURL + '"]').parents('.nav-item').attr('data-class');

            var type;

            switch (getType) {
                case "MainHeaderSetting":
                    type = 1
                    break;
                case "MainHeaderTransaction":
                    type = 2
                    break;
                case "MainHeaderInquiry":
                    type = 3
                    break;
                case "MainHeaderReport":
                    type = 4
                    break;
            }

            $.ajax({
                url: "/api/FavouriteAPI/AddFavourite?formCode=" + checkVal + "&checkURL=" + checkURL + "&type=" + type + "&formName=" + checkName,
                type: "POST"
            });

        } else {

            $.ajax({
                url: "/api/FavouriteAPI/RemoveFavourite?formCode=" + checkVal,
                type: "DELETE"
            });

        }

    });
    
});