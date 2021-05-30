
$(document).ready(function () {

    $(".Search").each(function () {
        $(this).hide();
    });

    $("#text").change(function () {
        filter = $("#text").val();
        if (filter.length > 0) {
            $("#myTable tbody tr").filter(function () {
                $(this).each(function () {
                    found = false;
                    $(this).children().each(function () {
                        content = $(this).html();
                        if (content.match(filter)) {
                            found = true;
                        }
                    });
                    if (!found) {
                        $(this).hide();
                    }
                    else {
                        $(this).show("slow");
                        $("#myTable tfoot tr").hide();
                    }
                });
            });
        } else {
            $(".Search").hide();
            $("#myTable tfoot tr").show();
        }
    });

});