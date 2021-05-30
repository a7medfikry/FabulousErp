$(".Deletecb").each(function () {
    var Test = $(this);
    $(Test).change(function () {

        if ($(Test).prop('checked')) {
            if (confirm("Do You want to Delete This Group..") == true) {
                $(Test).prop("checked", true);
            } else {
                $(Test).prop("checked", false);
            }
        }

    });
});
