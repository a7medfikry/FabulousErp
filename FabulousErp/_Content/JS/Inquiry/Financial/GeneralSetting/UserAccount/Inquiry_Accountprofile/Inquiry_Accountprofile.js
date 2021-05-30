$("#InquiryuserID").change(function () {
    var InquiryuserID = $(this).val();
    $.ajax({
        type: "GET",
        url: "/Inquiry_UserProfile/UserIDSearch?InquiryuserID=" + InquiryuserID,
        success: function (result) {
            for (var i = 0; i < result.length; i++) {
                $("#user-date").text(result[i].Date);
                $("#user-username").text(result[i].UserName);
                $("#user-passchange").text(result[i].LastPasswordChangedDate);
                $("#user-group").text(result[i].GroupID);
                $("#user-title").text(result[i].TitlePER);
                $("#user-national").val(result[i].NationalORPassportIDPER);
                $("#user-firstname").val(result[i].FirstNamePER);
                $("#user-lastname").val(result[i].LastNamePER);
                $("#user-familyname").val(result[i].FamilyNamePER);
                $("#user-building").val(result[i].BuldingNoPER);
                $("#user-street").val(result[i].StreetPER);
                $("#user-avenue").val(result[i].AvenuePER);
                $("#user-state").val(result[i].StatePER);
                $("#user-country").val(result[i].CountryPER);
                $("#user-homephone").val(result[i].HomePhonePER);
                $("#user-mobilephone").val(result[i].MobilePhonePER);
                $("#user-othermobile").val(result[i].OthMobilePhonePER);
                $("#user-position").val(result[i].PositionFUN);
                $("#user-department").val(result[i].DepartmentFUN);
                $("#user-room").val(result[i].RoomNumFUN);
                $("#user-floor").val(result[i].FloorFUN);
                $("#user-building2").val(result[i].BuildingFUN);
                $("#user-tele").val(result[i].TelephoneNumFUN);
                $("#user-teleext").val(result[i].TEXtentionFUN);
                $("#user-fax").val(result[i].FaxNumFUN);
                $("#user-faxext").val(result[i].FExtentionFUN);
                $("#user-mobilephone").val(result[i].MobilePhoneFUN);
                $("#user-email").val(result[i].EmailFUN);
            }
        },
        error: function (req, status, error) {
        }
    });
});