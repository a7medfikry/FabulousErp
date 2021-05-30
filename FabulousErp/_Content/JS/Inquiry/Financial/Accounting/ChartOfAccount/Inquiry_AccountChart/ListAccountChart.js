$("#ChartID").change(function () {
    var ChartID = $(this).val();
    var table = $("#example tbody");
    if (ChartID.length > 0) {
        $.ajax({
            method: "GET",
            url: "/Inquiry_AccountChart/GetSearchList?ChartID=" + ChartID,
            success: function (result) {
                table.empty();
                $.each(result, function (a, b) {
                    table.append("<tr><td>" + b.AccountChartID + "</td>" +
                        "<td>" + b.AccountChartName + "</td>" +
                        "<td>" + b.EstablishDate + "</td></tr>");
                });
                $("#example").DataTable();
            }
        });
    }
});

//on document Ready
$(() => {
    var table = $("#example tbody");
    $.ajax({
        url: '/Inquiry_AccountChart/GetList',
        method: "GET",
        xhrFields: {
            withCredentials: true
        },
        success: function (result) {
            table.empty();
            $.each(result, function (a, b) {
                table.append("<tr><td>" + b.AccountChartID + "</td>" +
                    "<td>" + b.AccountChartName + "</td>" +
                    "<td>" + b.EstablishDate + "</td></tr>");
            });
            $("#example").DataTable({
                responsive: true,
                ordering: false
            });
        }
    });


});