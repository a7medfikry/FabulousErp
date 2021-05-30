$(document).ready(function () {

    $('#Inquiry-checkbookID').change(function () {
        var checkbookID = $(this).val();
        $.ajax({
            type: 'GET',
            url: '/Inquiry_CheckbookTransactions/CheckbookData?checkbookID=' + checkbookID,
            success: function (result) {
                $('#currency').val(result.CurrencyName);
                $('#checkbookName').val(result.CheckbookName);
                $('#checkbookBalance').val(result.Balance);
                $('#cashAccountBalance').val(result.CashAccountBalance);
            }
        }).done(function () {
            $('#allDate').click();
        });
    });

    // When Choose-Date Is From - To
    $('#fromDate').click(function () {
        $('#startDate').prop('disabled', false);
        $('#endDate').prop('disabled', false);
        $('#search').prop('disabled', false);
        var tableBody = $('#appendData');
        tableBody.empty();
    });

    // When Choose-Date Is All
    $('#allDate').click(function () {
        $('#startDate').prop('disabled', true);
        $('#endDate').prop('disabled', true);
        $('#search').prop('disabled', true);

        var tableBody = $('#appendData'),
            checkbookID = $('#Inquiry-checkbookID').val(),
            type,
            voided;
        $.ajax({
            type: 'GET',
            url: '/Inquiry_CheckbookTransactions/AllDate?checkbookID=' + checkbookID,
            success: function (result) {
                tableBody.empty();
                if (result.length > 0) {
                    $.each(result, function (key, item) {
                        if (item.C_CBTVoid != null) {
                            voided = "<span style='color:red; font-weight:600'>*</span>";
                        } else {
                            voided = "";
                        }
                        type = GetCheckBookTypeName(item.DocumentType)
                        
                        tableBody.append("<tr><td hidden>" + item.PostingNumber + "</td>" +
                            "<td>" + voided + "</td>" +
                            "<td>" + item.Date + "</td>" +
                            "<td>" + item.DocumentNumber + "</td>" +
                            "<td class='type' data-type='" + item.DocumentType +"'>" + type + "</td>" +
                            "<td>" + item.RecievedFrom + "</td>" +
                            "<td style='max-width: 185px;white-space: initial;'>" + item.Refrence + "</td>" +
                            "<td>" + item.Payment + "</td>" +
                            "<td>" + item.Deposit + "</td>" +
                            "<td>" + item.Balance + "</td>" +
                            "<td>" + "<button class='btn btn-secondary btn-sm Trans' id='ICBT-viewDetails'>View Details</button>" + "</td></tr>");
                    });
                    ReTranslate();

                } else {
                    tableBody.append("<tr><td colspan='7'>" + "There's No Any Transactions" + "</td></tr>");
                }
            }
        });
    });

    $('#search').click(function () {
        var startDate = $('#startDate').val(),
            endDate = $('#endDate').val(),
            tableBody = $('#appendData'),
            checkbookID = $('#Inquiry-checkbookID').val(),
            type,
            voided;

        $.ajax({
            type: 'GET',
            url: '/Inquiry_CheckbookTransactions/AllDate?checkbookID=' + checkbookID,
            success: function (result) {
                tableBody.empty();
                if (result.length > 0) {
                    for (var i = 0; i < result.length; i++) {
                        if (result[i].C_CBTVoid != null) {
                            voided = "<span style='color:red; font-weight:600'>*</span>";
                        } else {
                            voided = "";
                        }
                        type = GetCheckBookTypeName(result[i].DocumentType)
                        if (HandleDate(result[i].Date) >= startDate && HandleDate(result[i].Date) <= endDate) {
                            tableBody.append("<tr><td hidden>" + result[i].PostingNumber + "</td>" +
                                "<td>" + voided + "</td>" +
                                "<td>" + HandleDate(result[i].Date) + "</td>" +
                                "<td>" + result[i].DocumentNumber + "</td>" +
                                "<td class='type' data-type='" + result[i].DocumentType +"'>" + type + "</td>" +
                                "<td>" + result[i].RecievedFrom + "</td>" +
                                "<td>" + result[i].Refrence + "</td>" +
                                "<td>" + result[i].Payment + "</td>" +
                                "<td>" + result[i].Deposit + "</td>" +
                                "<td>" + result[i].Balance + "</td>" +
                                "<td>" + "<button class='btn btn-secondary btn-sm Trans' id='ICBT-viewDetails'>View Details</button>" + "</td></tr>");
                        }
                    }
                    ReTranslate();
                } else {
                    tableBody.append("<tr><td colspan='7'>" + "There's No Any Transactions" + "</td></tr>");
                }
            }
        });
    });


    $('#CBT-checkbookID').change(function () {
        if ($(this).val().length === 0) {
            $('#CBI-docType').val('');
            $('#CBI-docNumber').prop('disabled', true);
        } else {
            $('#CBI-docType').val('');
            $('#CBI-docNumber').empty();
        }
    });
    $('#CBI-docType').change(function () {
        if ($(this).val().length === 0) {
            $('#CBI-docNumber').prop('disabled', true);
        } else {
            $('#CBI-docNumber').empty();
            $('#CBI-docNumber').append($('<option>',
                {
                    text: '-Choose-'
                }));
            var documentType = $(this).val(),
                checkbookID = $('#CBT-checkbookID').val();
            $.ajax({
                type: 'GET',
                url: '/Inquiry_CheckbookTransactions/GetDocumentNumbers?checkbookID=' + checkbookID + '&documentType=' + documentType,
                success: function (result) {
                    $('#CBI-docNumber').prop('disabled', false);
                    $.each(result, function (key, res) {
                        $('#CBI-docNumber').append($('<option>',
                            {
                                value: res.PostingNumber,
                                text: res.DocumentNumber
                            }));
                    });
                    $('#CBI-docNumber').val(getParameterByName("postingNumber"))
                }
            });
        }
    });
    $('#CBI-docNumber').change(function () {
        var documentNumber = $(this).val();
        $('#TCGV-JENum').val(documentNumber);
        $('#TCGV-JENum').trigger('change');
    });

    //----------------------------------------------------------------------------------------
    if (window.location.pathname == '/Inquiry_CheckbookTransactions/Cash') {
        $(document).on('click', '#ICBT-viewDetails', function () {
            var checkbookID = $('#Inquiry-checkbookID option:selected').val(),
                rowID = $(this).closest('tr'),
                postingNumber = rowID.find('td:eq(0)').text(),
                documentTypeCheck = rowID.find('.type').attr("data-type"),
                documentType;
            if (documentTypeCheck == "TCCR") {
                documentType = "TCCR";
                window.location.href = '/Inquiry_CheckbookTransactions/CashInquiry?checkbookID='
                    + checkbookID + '&postingNumber=' + postingNumber + '&documentType=' + documentType;
            } else if (documentTypeCheck == "TCCW") {
                documentType = "TCCW";
                window.location.href = '/Inquiry_CheckbookTransactions/CashInquiry?checkbookID='
                    + checkbookID + '&postingNumber=' + postingNumber + '&documentType=' + documentType;
            } else if (documentTypeCheck == "TCBR") {
                documentType = "TCBR";
                window.location.href = '/Inquiry_CheckbookTransactions/BankInquiry?checkbookID='
                    + checkbookID + '&postingNumber=' + postingNumber + '&documentType=' + documentType;
            } else if (documentTypeCheck == "TCBC") {
                documentType = "TCBC";
                window.location.href = '/Inquiry_CheckbookTransactions/BankInquiry?checkbookID='
                    + checkbookID + '&postingNumber=' + postingNumber + '&documentType=' + documentType;
            } else {
                documentType = "TCBT";
                window.location.href = '/Inquiry_CheckbookTransactions/Transfer?postingNumber='
                    + postingNumber;
            }
        });
    } else if (window.location.pathname == '/Inquiry_CheckbookTransactions/CashInquiry' || window.location.pathname == '/Inquiry_CheckbookTransactions/BankInquiry') {
        var checkbookID = getParameterByName("checkbookID"),
            documentType = getParameterByName("documentType"),
            postingNumber = getParameterByName("postingNumber"),
            journalEntryNumber = getParameterByName("journalEntryNumber");
        if (checkbookID && postingNumber && documentType) {
            $('#CBT-checkbookID').val(checkbookID);
            $('#CBT-checkbookID').trigger('change');
            $('#CBI-docType').val(documentType);
            $('#CBI-docType').trigger('change');
            $('#CBI-docNumber').val(postingNumber);
            $('#CBI-docNumber').trigger('change');
            $('#TCGV-JENum').val(postingNumber);
            $('#TCGV-JENum').trigger('change');
        } else if (journalEntryNumber) {
            $('#TCGV-JENum').val(journalEntryNumber);
            $('#TCGV-JENum').trigger('change');
        }
    } else if (window.location.pathname == '/Inquiry_CheckbookTransactions/Transfer') {
        var postingNumber = getParameterByName("postingNumber"),
            journalEntryNumber = getParameterByName("journalEntryNumber");
        if (postingNumber) {
            $('#CBI-docNumber').val(postingNumber);
            $('#CBI-docNumber').trigger('change');
        } else if (journalEntryNumber) {
            $('#TCGV-JENum').val(journalEntryNumber);
            $('#TCGV-JENum').trigger('change');
        }
    } else if (window.location.pathname == '/C_ReportsPrint/Done') { // ******* Print Report *********
        var searchNumber = getParameterByName("searchNumber");
        if (searchNumber) {
            $('#TCGE-print').trigger('click');
        }
    }



});

function HandleDate(date) {
    var d = new Date(date),
        month = '' + (d.getMonth() + 1),
        day = '' + d.getDate(),
        year = d.getFullYear();
    if (month.length < 2) month = '0' + month;
    if (day.length < 2) day = '0' + day;
    return [year, month, day].join('-');
}
function getParameterByName(name, url) {
    if (!url) url = window.location.href;
    name = name.replace(/[\[\]]/g, '\\$&');
    var regex = new RegExp('[?&]' + name + '(=([^&#]*)|&|#|$)'),
        results = regex.exec(url);
    if (!results) return null;
    if (!results[2]) return '';
    return decodeURIComponent(results[2].replace(/\+/g, ' '));
}
