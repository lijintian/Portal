$.extend({
    createUploadIframe: function (frameId, uri) {
        //create frame
        var iframeHtml = '<iframe id="' + frameId + '" name="' + frameId + '" style="position:absolute; top:-9999px; left:-9999px"';
        if (window.ActiveXObject) {
            if (typeof uri == 'boolean') {
                iframeHtml += ' src="' + 'javascript:false' + '"';
            }
            else if (typeof uri == 'string') {
                iframeHtml += ' src="' + uri + '"';
            }
        }
        iframeHtml += ' />';
        $(iframeHtml).appendTo(document.body);
        return $('#' + frameId).get(0);
    },
    createUploadForm: function (formId, fileElementId, data) {
        //create form	
        var form = $('<form  action="" method="POST" name="' + formId + '" id="' + formId + '" enctype="multipart/form-data"></form>');
        if (data) {
            for (var i in data) {
                var item = $('<input type="hidden" name="' + i + '"/>');
                item.val(data[i]);
                item.appendTo(form);
            }
        }
        var oldElement = $('#' + fileElementId);
        //var newElement = $(oldElement).clone();
        //$(oldElement).attr('id', fileId);
        //$(oldElement).before(newElement);
        $(oldElement).appendTo(form);
        //set attributes
        $(form).css('position', 'absolute');
        $(form).css('top', '-1200px');
        $(form).css('left', '-1200px');
        $(form).appendTo('body');
        return form;
    },

    ajaxFileUpload: function (s) {
        // TODO introduce global settings, allowing the client to modify them for all requests, not only timeout		
        s = $.extend({}, $.ajaxSettings, s);
        var id = new Date().getTime();
        var formId = 'jUploadForm' + id;
        var frameId = 'jUploadFrame' + id;
        var parentElement = $('#' + s.fileElementId).parent();
        var form = $.createUploadForm(formId, s.fileElementId, (typeof (s.data) == 'undefined' ? false : s.data));
        var io = $.createUploadIframe(frameId, s.secureuri);
        // Watch for a new set of requests
        if (s.global && !$.active++) {
            $.event.trigger("ajaxStart");
        }
        var requestDone = false;
        var isloading = typeof s.isloading == 'undefined' ? true : s.isloading;
        if (isloading) {
            try {
                $.blockUI({
                    fadeIn: 0,
                    fadeOut: 0,
                    showOverlay: false,
                    message: s.msg,
                    centerY: 0,
                    css: {
                        border: '#6198dd 1px solid',
                        padding: '30px 0px 0px 0px',
                        width: '240px',
                        height: '72px',
                        position: 'fixed',
                        color: '#6a6868',
                        top: '50%', left: '50%',
                        margin: '-31px 0 0 -120px',
                        background: "url(" + window.ServiceAppPath + "Content/images/loading.gif) no-repeat 50% 28% #f4f9fe"
                    }
                });
            }
            catch (e) { }
        }
        // Create the request object
        var xml = {};
        if (s.global)
            $.event.trigger("ajaxSend", [xml, s]);
        // Wait for a response to come back
        var uploadCallback = function (isTimeout) {
            //var io = document.getElementById(frameId);
            try {
                if (io.contentWindow) {
                    xml.responseText = io.contentWindow.document.body ? io.contentWindow.document.body.innerHTML : null;
                    xml.responseXML = io.contentWindow.document.XMLDocument ? io.contentWindow.document.XMLDocument : io.contentWindow.document;

                } else if (io.contentDocument) {
                    xml.responseText = io.contentDocument.document.body ? io.contentDocument.document.body.innerHTML : null;
                    xml.responseXML = io.contentDocument.document.XMLDocument ? io.contentDocument.document.XMLDocument : io.contentDocument.document;
                }
            } catch (e) {
                $.handleError(s, xml, null, e);
            }
            if (xml || isTimeout == "timeout") {
                requestDone = true;
                var status;
                try {
                    //debugger;
                    status = isTimeout != "timeout" ? "success" : "error";
                    // Make sure that the request was successful or notmodified
                    if (status != "error") {
                        // process the data (runs the xml through httpData regardless of callback)
                        var data = $.uploadHttpData(xml, s.dataType);
                        // If a local callback was specified, fire it and pass it the data
                        if (s.success)
                            s.success(data, status);

                        // Fire the global callback
                        if (s.global)
                            $.event.trigger("ajaxSuccess", [xml, s]);
                    } else {
                        //$.handleError(s, xml, status);
                        var data = $.uploadHttpData(xml, s.dataType);
                        var html = $(data);
                        var error = "系统维护中，请稍后再试!";
                        if (html.find("#errorMsg").length > 0) {
                            error = html.find("#errorMsg").eq(0).html();
                        }
                        $(this).showMsgDailog("异常提示", error, 500, 50);
                    }
                } catch (e) {
                    status = "error";
                    //$.handleError(s, xml, status, e);
                    var data = $.uploadHttpData(xml, s.dataType);
                    var html = $(data);
                    var error = "系统维护中，请稍后再试!";
                    if (html.find("#errorMsg").length > 0) {
                        error = html.find("#errorMsg").eq(0).html();
                    }
                    $(this).showMsgDailog("异常提示", error, 500, 50);
                }
                // The request was completed
                if (s.global)
                    $.event.trigger("ajaxComplete", [xml, s]);
                // Handle the global AJAX counter
                if (s.global && !--$.active)
                    $.event.trigger("ajaxStop");
                // Process result
                if (isloading) {
                    try { $.unblockUI(); } catch (e) { }
                }
                if (s.complete) {
                    s.complete(xml, status);
                }
                //debugger;
                $(io).unbind();
                //setTimeout(function () {
                try {
                    $(io).remove();
                    var fileObj = $("#" + s.fileElementId);
                    fileObj.appendTo(parentElement);
                    $(form).remove();

                } catch (e) {
                    $.handleError(s, xml, null, e);
                }
                //}, 100);
                xml = null;
            }
        };
        // Timeout checker
        if (s.timeout > 0) {
            setTimeout(function () {
                // Check to see if the request is still happening
                if (!requestDone) uploadCallback("timeout");
            }, s.timeout);
        }
        try {
            var form = $('#' + formId);
            $(form).attr('action', s.url);
            $(form).attr('method', 'POST');
            $(form).attr('target', frameId);
            if (form.encoding) {
                $(form).attr('encoding', 'multipart/form-data');
            }
            else {
                $(form).attr('enctype', 'multipart/form-data');
            }
            $(form).submit();
        } catch (e) {
            $.handleError(s, xml, null, e);
        }
        $('#' + frameId).load(uploadCallback);
        return { abort: function () { } };
    },
    uploadHttpData: function (r, type) {
        var data = !type;
        data = type == "xml" || data ? r.responseXML : r.responseText;
        // If the type is "script", eval it in global context
        if (type == "script")
            $.globalEval(data);
        // Get the JavaScript object, if JSON is used.
        if (type == "json")
            eval("data = " + data);
        // evaluate scripts within html
        if (type == "html")
            $("<div>").html(data).evalScripts();
        return data;
    }
})

