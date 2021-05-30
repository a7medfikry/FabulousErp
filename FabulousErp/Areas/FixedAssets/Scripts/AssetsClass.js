$("#Deprecation_method").change(function () {
    if ($(this).val() === 3) {
        $(".DeprecationRateDiv").hide();
    } else {
        $(".DeprecationRateDiv").show();
    }
});
function SubmitAccounts(data) {
    $("#Assets_accountsDiv").find("#Class_id").val(data);
    $("#Assets_accountsDiv").find("form").submit();
}
$(document).on("click", "#Create",function () {
    $("#FirstForm").trigger("click")
})
function AccountsSuccess() {
    location.href = "/FixedAssets/Assets_class"
}
function ValidateAccountForm() {
    var Valid = $("#AccountForm").validate();
    Valid.focusInvalid();
    if (!$("#AccountForm").valid()) {
        return false;
    }
    return true;
}