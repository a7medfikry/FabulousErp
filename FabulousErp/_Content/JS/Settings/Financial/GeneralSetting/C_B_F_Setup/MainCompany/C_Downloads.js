function DCommerical() {
    var CompanyID = $("#CompanyID").val();

    $.ajax({
        type: "GET",
        url: "/MainCompany/DownloadComm?CompanyID=" + CompanyID,
        success: function (data) {
            if (data == "False") {
                Talert("Document Deleted From Server..!");
            } else {
                window.location = "/MainCompany/DownloadComm?CompanyID=" + CompanyID;
            }
        },
        error: function (req, status, error) {
            Talert("Document not Found..! Attach one If You Need..");
        }
    });
}


function DTax() {
    var CompanyID = $("#CompanyID").val();

    $.ajax({
        type: "GET",
        url: "/MainCompany/DownloadTax?CompanyID=" + CompanyID,
        success: function (data) {
            if (data == "False") {
                Talert("Document Deleted From Server..!");
            } else {
                window.location = "/MainCompany/DownloadTax?CompanyID=" + CompanyID;
            }
        },
        error: function (req, status, error) {
            Talert("Document not Found..! Attach one If You Need..:)");
        }
    });
}


function DVat() {
    var CompanyID = $("#CompanyID").val();

    $.ajax({
        type: "GET",
        url: "/MainCompany/DownloadVat?CompanyID=" + CompanyID,
        success: function (data) {
            if (data == "False") {
                Talert("Document Deleted From Server..!");
            } else {
                window.location = "/MainCompany/DownloadVat?CompanyID=" + CompanyID;
            }
        },
        error: function (req, status, error) {
            Talert("Document not Found..! Attach one If You Need..:)");
        }
    });
}


function DImport() {
    var CompanyID = $("#CompanyID").val();

    $.ajax({
        type: "GET",
        url: "/MainCompany/DownloadImport?CompanyID=" + CompanyID,
        success: function (data) {
            if (data == "False") {
                Talert("Document Deleted From Server..!");
            } else {
                window.location = "/MainCompany/DownloadImport?CompanyID=" + CompanyID;
            }
        },
        error: function (req, status, error) {
            Talert("Document not Found..! Attach one If You Need..:)");
        }
    });
}


function DExport() {
    var CompanyID = $("#CompanyID").val();

    $.ajax({
        type: "GET",
        url: "/MainCompany/DownloadExport?CompanyID=" + CompanyID,
        success: function (data) {
            if (data == "False") {
                Talert("Document Deleted From Server..!");
            } else {
                window.location = "/MainCompany/DownloadExport?CompanyID=" + CompanyID;
            }
        },
        error: function (req, status, error) {
            Talert("Document not Found..! Attach one If You Need..:)");
        }
    });
}



function DInsurance() {
    var CompanyID = $("#CompanyID").val();

    $.ajax({
        type: "GET",
        url: "/MainCompany/DownloadInsurance?CompanyID=" + CompanyID,
        success: function (data) {
            if (data == "False") {
                Talert("Document Deleted From Server..!");
            } else {
                window.location = "/MainCompany/DownloadInsurance?CompanyID=" + CompanyID;
            }
        },
        error: function (req, status, error) {
            Talert("Document not Found..! Attach one If You Need..:)");
        }
    });
}