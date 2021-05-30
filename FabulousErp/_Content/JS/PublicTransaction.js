function PureTransactionSave(companyID, headerObj, mainArr,CallBack) {

    var postingNumber = 0;

    var data = JSON.stringify({
        SaveTransaction: mainArr,
        SaveHeader: headerObj
    });

    $.ajax({
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        method: "post",
        url: "/api/TransactionApi/SaveSimpleTransaction?companyID=" + companyID,
        data: data,
        async: false,
        success: function (result) {
            postingNumber = result;
            if (CallBack != null) {
                CallBack(result);
            }
            window.open("/C_ReportsPrint/Done?searchNumber=" + postingNumber, "_blank");
        }
    });
    return postingNumber;
}