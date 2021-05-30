$("#InfoUpdateBtn").click(function () {

    var TitlePER = $("#TitlePER").val();
    var NationalORPassportIDPER = $("#NationalORPassportIDPER").val();
    var FirstNamePER = $("#FirstNamePER").val();
    var LastNamePER = $("#LastNamePER").val();
    var FamilyNamePER = $("#FamilyNamePER").val();
    var BuldingNoPER = $("#BuldingNoPER").val();
    var StreetPER = $("#StreetPER").val();
    var AvenuePER = $("#AvenuePER").val();
    var StatePER = $("#StatePER").val();
    var CountryPER = $("#CountryPER").val();
    var MobilePhonePER = $("#MobilePhonePER").val();
    var PositionFUN = $("#PositionFUN").val();
    var DepartmentFUN = $("#DepartmentFUN").val();
    var RoomNumFUN = $("#RoomNumFUN").val();
    var FloorFUN = $("#FloorFUN").val();
    var BuildingFUN = $("#BuildingFUN").val();
    var Email = $("#EmailFUN").val();
    var CityPER = $("#CityPER").val();

    var Test = true;

    if (TitlePER.length == 0) {
        $("#TitlePER").css("border-color", "red");
        Test = false;
    } else {
        $("#TitlePER").css("border-color", "");
    }
    if (NationalORPassportIDPER.length == 0) {
        $("#NationalORPassportIDPER").css("border-color", "red");
        Test = false;
    } else {
        $("#NationalORPassportIDPER").css("border-color", "");
    }
    if (FirstNamePER.length == 0) {
        $("#FirstNamePER").css("border-color", "red");
        Test = false;
    } else {
        $("#FirstNamePER").css("border-color", "");
    }

    if (LastNamePER.length == 0) {
        $("#LastNamePER").css("border-color", "red");
        Test = false;
    } else {
        $("#LastNamePER").css("border-color", "");
    }

    if (FamilyNamePER.length == 0) {
        $("#FamilyNamePER").css("border-color", "red");
        Test = false;
    } else {
        $("#FamilyNamePER").css("border-color", "");
    }

    if (BuldingNoPER.length == 0) {
        $("#BuldingNoPER").css("border-color", "red");
        Test = false;
    } else {
        $("#BuldingNoPER").css("border-color", "");
    }

    if (StreetPER.length == 0) {
        $("#StreetPER").css("border-color", "red");
        Test = false;
    } else {
        $("#StreetPER").css("border-color", "");
    }

    if (AvenuePER.length == 0) {
        $("#AvenuePER").css("border-color", "red");
        Test = false;
    } else {
        $("#AvenuePER").css("border-color", "");
    }

    if (StatePER.length == 0) {
        $("#StatePER").css("border-color", "red");
        Test = false;
    } else {
        $("#StatePER").css("border-color", "");
    }

    if (CountryPER.length == 0) {
        $("#CountryPER").css("border-color", "red");
        Test = false;
    } else {
        $("#CountryPER").css("border-color", "");
    }

    if (MobilePhonePER.length == 0) {
        $("#MobilePhonePER").css("border-color", "red");
        Test = false;
    } else {
        $("#MobilePhonePER").css("border-color", "");
    }

    if (PositionFUN.length == 0) {
        $("#PositionFUN").css("border-color", "red");
        Test = false;
    } else {
        $("#PositionFUN").css("border-color", "");
    }

    if (DepartmentFUN.length == 0) {
        $("#DepartmentFUN").css("border-color", "red");
        Test = false;
    } else {
        $("#DepartmentFUN").css("border-color", "");
    }

    if (RoomNumFUN.length == 0) {
        $("#RoomNumFUN").css("border-color", "red");
        Test = false;
    } else {
        $("#RoomNumFUN").css("border-color", "");
    }

    if (FloorFUN.length == 0) {
        $("#FloorFUN").css("border-color", "red");
        Test = false;
    } else {
        $("#FloorFUN").css("border-color", "");
    }

    if (BuildingFUN.length == 0) {
        $("#BuildingFUN").css("border-color", "red");
        Test = false;
    } else {
        $("#BuildingFUN").css("border-color", "");
    }

    if (CityPER.length == 0) {
        $("#CityPER").css("border-color", "red");
        Test = false;
    } else {
        $("#CityPER").css("border-color", "");
    }

    var pattern = /^([a-zA-Z0-9_.+-])+\@(([a-zA-Z0-9-])+\.)+([a-zA-Z0-9]{2,4})+$/;
    if (!pattern.test(Email) && Email.length > 0) {
        $("#EmailFUN").css("border-color", "red");
        Test = false;
    }
    else {
        $("#EmailFUN").css("border-color", "");
    }

    var allData = $("#Form1").serialize();

if (Test === true) {

    $.ajax({
        type: "POST",
        url: "/AccountProfileInfo/AccountInfos",
        data: allData,
        success: function (result) {
            window.location = "/Dashboard/dashboard";
        }
    });
}
});

$("#PUUpdateProfilePic").click(function () {

    $("#EditProfilePicturePU").modal();

});

$("#UpdatePP").click(function () {

    var InputLogo = $("#InputPersonalImg").get(0).files;

    var dataa = new FormData;
    dataa.append("InputPersonalImg", InputLogo[0]);

    $.ajax({
        type: "POST",
        url: "/AccountProfileInfo/UpdateProfilePicture",
        data: dataa,
        cache: false,
        contentType: false,
        processData: false,
        success: function () {
            location.reload();
        }
    });

});

function LetterOnly(e) {

    if ( e.ctrlKey || e.altKey) {
        e.preventDefault();
    } else {
        var key = e.keyCode;
        if (!((key == 8) || (key == 32) || (key == 46) || (key >= 35 && key <= 40) || (key >= 65 && key <= 90))) {
            e.preventDefault();
        }
    }
}