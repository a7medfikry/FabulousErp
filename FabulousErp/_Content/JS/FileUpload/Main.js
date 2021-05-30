/*
 * jQuery File Upload Plugin JS Example
 * https://github.com/blueimp/jQuery-File-Upload
 *
 * Copyright 2010, Sebastian Tschan
 * https://blueimp.net
 *
 * Licensed under the MIT license:
 * https://opensource.org/licenses/MIT
 */

/* global $, window */

$(function () {
    'use strict';

    // Initialize the jQuery File Upload widget:
    $('#fileupload').fileupload({
        // Uncomment the following to send cross-domain cookies:
        //xhrFields: {withCredentials: true},
        //url: 'server/php/'
        url: '/UploadFile/Create',
        sequentialUploads: true

    });

    // Enable iframe cross-domain access via redirect option:
    $('#fileupload').fileupload(
        'option',
        'redirect',
        window.location.href.replace(
            /\/[^\/]*$/,
            '/cors/result.html?%s'
        )
    );


 
    

});

function GetFiles(PO, FileKey = null, Relation_id = null) {
    var segments = window.location.pathname.split('/');
    var Id = getParameterByName("OtherImageChar", window.location.href);
    var Remove = [];
    $.each(segments, function (k, i) {
        if (!isNaN(i) && i != "") {
            Remove.push({ k: k, i: i });
        }
        else if (i == "Create" || i == "Edit" || i == "Details" || i == "Index") {
            Remove.push({ k: k, i: i });
        }
    })
    $.each(Remove, function (k, i) {
        segments.splice(segments.indexOf(i.i), 1);
    })
    var Url = segments.join('/');
    // Load existing files:
    $('#fileupload').addClass('fileupload-processing');
    $('#fileupload').find(".template-download.in").remove();
    $.ajax({
        // Uncomment the following to send cross-domain cookies:
        //xhrFields: {withCredentials: true},
        url: "/UploadFile/GetAllImages/?Page=" + Url + "&PO=" + PO + "&FileKey=" + FileKey + "&Relation_id=" + Relation_id,//$('#fileupload').fileupload('option', 'url'),
        dataType: 'json',
        context: $('#fileupload')[0]
    }).always(function () {
        $(this).removeClass('fileupload-processing');
    }).done(function (result) {
        $(this).fileupload('option', 'done')
            .call(this, $.Event('done'), { result: result });
    });
}