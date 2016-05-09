
//初始化上传控件
function InitUploadify(importConfig) {
    //$("#divUploadFiles").empty();
    //$("#divUploadFiles").html('<input type="file" id="file_upload" name="file" />');
    $('#file_upload').attr("accept", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet, application/vnd.ms-excel");
    $('#file_upload').ace_file_input({
        no_file: 'No File ...',
        btn_choose: '浏览',
        btn_change: '浏览',
        droppable: false,
        onchange: null,
        thumbnail: false,
        before_change: function (files, dropped) {
            if (files instanceof Array || (!!window.FileList && files instanceof FileList)) {
                var result = [];
                var regex = /^(.*?)(.xls|.xlsx)$/g;
                for (var i = 0; i < files.length; i++) {
                    var file = files[i];
                    if (regex.test(file.name)) {
                        result.push(file);
                    } else {
                        alert("请上传Excel文件");
                    }
                    //if (file.size > 10240000) {alert("文件不能超过10M");continue;}
                }
                importConfig.FileCount = this.form["file_upload"].files.length;
                Check();
                return result;
            }
            importConfig.FileCount = this.form["file_upload"].files.length;
            Check();
            return true;
        },
        before_remove: function (files) {
            importConfig.FileCount = 0;
            return true;
        }
    });
}

//下载模板
function DownFile() {
    $.FormSubmit("downForm", window.ServiceAppPath + 'File/DownTemplate', { Type: window.importConfig.TypeId });
}

//下载失败数据
function DownFailData() {
    $.FormSubmit("downForm", window.ServiceAppPath + 'File/DownErrorFile', { Path: window.importConfig.ErrorFilePath });
}

//保存导入
function SaveDataImport(obj) {
    $("#trErrorFile").hide();
    var fileCheck = Check();
    if (!fileCheck) { return false; }
    $.ajaxFileUpload({
        url: window.ServiceAppPath + 'File/ImportSave?Type=' + window.importConfig.TypeId, //用于文件上传的服务器端请求地址
        secureuri: false,
        fileElementId: 'file_upload',
        dataType: 'json',
        success: function (data, status)  //服务器成功响应处理函数
        {
            c$.alert(data.ErrorMessage);
            //var file_input = $('#file_upload');
            //file_input.ace_file_input('update_settings', { 'before_change': before_change, 'btn_choose': btn_choose, 'no_icon': no_icon });
            if (data.Status) {
                $("#trErrorFile").hide();
                window.importConfig.ErrorFilePath = "";
                InitData(false);
                //关闭弹出框
                $(obj).Close();
            } else {
                if (!isNullOrEmpty(data.Data)) {
                    $("#trErrorFile").show();
                    window.importConfig.ErrorFilePath = data.Data;
                }
            }
            InitUploadify(window.importConfig);
        }
    });
}

//检查附件
function Check() {
    var fileCheck = CheckFile("#filesForm #error1");
    if (!fileCheck) { $("#filesForm #dtips").show(); }
    else {
        $("#filesForm #dtips").hide();
    }
    return fileCheck;
}

//检查附件
function CheckFile(errDiv) {
    var fileCheck = window.importConfig.FileCount > 0;
    var fileErrorObj = $(errDiv);
    if (!fileCheck) {
        fileErrorObj.html("<font color='red'>请上传附件！</font>");
        fileErrorObj.show();
    } else {
        fileErrorObj.empty();
        fileErrorObj.hide();
    }
    return fileCheck;
}
