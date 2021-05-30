$(document).ready(function () {
    $("#IBT-branchID").change(function () {

        // ajax to get Branch-Name
        var branchID = $(this).val();
        $.ajax({
            type: "GET",
            url: "/Inquiry_BranchTax/GetBranchName?branchID=" + branchID,
            success: function (result) {
                $("#IBT-branchname").val(result);
            }
        });

        // ajax to get Data
        var setData = $("#IBT-tbody-append");
        setData.html("");
        $.ajax({
            type: "GET",
            url: "/Inquiry_BranchTax/GetData?branchID=" + branchID,
            contentType: "html",
            success: function (result) {
                if (result.length === 0) {
                    setData.append('<tr style="color:red"><td colspan="4"> No Result Found !!! </td></tr>')
                }
                else {
                    for (var i = 0; i < result.length; i++) {
                        var Data = "<tr>" +
                            "<td>" + result[i].TaxCode + "</td>" +
                            "<td>" + result[i].AccountID + "</td>" +
                            "<td>" + result[i].TaxDescribtion + "</td>" +
                            "<td>" + result[i].TaxPercentage + "</td>" +
                            "</tr>";
                        setData.append(Data);
                    }
                }
            },
        });



    });








});