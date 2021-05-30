$(document).ready(function () {

    $.ajax({

        type: "Get",
        url: "/AccountProfileInfo/GetMainInfo",
        success: function(data){

            $("#UserName").text(data.UserName);

            $("#DateOfCreation").text(data.Date);

            $("#PassLastChangeDate").text(data.LastPasswordChangedDate);

            $("#GroupNumber").text(data.MembOfGroup);

            $("#TitlePER").val(data.TitlePER);

            $("#NationalORPassportIDPER").val(data.NationalORPassportIDPER);

            $("#FirstNamePER").val(data.FirstNamePER);

            $("#LastNamePER").val(data.LastNamePER);

            $("#FamilyNamePER").val(data.FamilyNamePER);

            $("#BuldingNoPER").val(data.BuldingNoPER);

            $("#StreetPER").val(data.StreetPER);

            $("#AvenuePER").val(data.AvenuePER);

            $("#StatePER").val(data.StatePER);

            $("#CountryPER").val(data.CountryPER);

            $("#HomePhonePER").val(data.HomePhonePER);

            $("#MobilePhonePER").val(data.MobilePhonePER);

            $("#OthMobilePhonePER").val(data.OthMobilePhonePER);

            $("#CityPER").val(data.CityPER);

            $("#PositionFUN").val(data.PositionFUN);

            $("#DepartmentFUN").val(data.DepartmentFUN);

            $("#RoomNumFUN").val(data.RoomNumFUN);

            $("#FloorFUN").val(data.FloorFUN);

            $("#BuildingFUN").val(data.BuildingFUN);

            $("#TelephoneNumFUN").val(data.TelephoneNumFUN);

            $("#TEXtentionFUN").val(data.TEXtentionFUN);

            $("#FaxNumFUN").val(data.FaxNumFUN);

            $("#FExtentionFUN").val(data.FExtentionFUN);

            $("#MobilePhoneFUN").val(data.MobilePhoneFUN);

            $("#EmailFUN").val(data.EmailFUN);

            $("#ImagePp").attr('src', data.InputPersonalImg);
        }
    });

    $("#InputPersonalImg").change(function () {

        var File = this.files;

        if (File && File[0]) {
            ReadImage(File[0]);
        }
    });

    function ReadImage(file) {
        var reader = new FileReader;
        var image = new Image;

        reader.readAsDataURL(file);
        reader.onload = function (_file) {
            image.src = _file.target.result;
            image.onload = function () {

                $("#UpdateImagePp").attr('src', _file.target.result);
            }
        }
    }

});