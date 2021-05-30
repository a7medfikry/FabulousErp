$("#Number_of_parts").on("keyup", function () {
    if ($("#Number_of_parts").val() < $("#PartsSerialDiv").find(".Parts").length) {
        while ($("#Number_of_parts").val() < $("#PartsSerialDiv").find(".Parts").length) {
            $("#PartsSerialDiv").find(".Parts").last().remove();
        }
    } else {
        for (i = $("#PartsSerialDiv").find(".Parts").length; i < $(this).val(); i++) {

            $("#PartsSerialDiv").append("<div class='col-sm-4 Parts'><label>Assets Part Number " + (i + 1) + "</label><input type='hidden' name='PartsSerial[" + i + "].Part_number' value='" + (i + 1) + "' /><input placeholder='Part " + (i + 1) + "' class='form-control col-sm-12' name='PartsSerial[" + i + "].Serial' required='required'/></div>")
        }
    }

})
