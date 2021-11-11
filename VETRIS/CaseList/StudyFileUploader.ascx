<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="StudyFileUploader.ascx.cs" Inherits="VETRIS.CaseList.StudyFileUploader" %>
<meta charset="utf-8" />
<meta http-equiv="x-ua-compatible" content="ie=edge" />
<meta name="description" content="" />
<meta name="viewport" content="width=device-width, initial-scale=1" />

<link href="https://fonts.googleapis.com/css?family=Roboto:100,300,400,700,900" rel="stylesheet" />
<link rel="stylesheet" href="../css/bootstrap.min.css" />
<link rel="stylesheet" href="../css/font-awesome.min.css" />
<link href="../css/responsive.css" rel="stylesheet" />
<link id="lnkSTYLE" runat="server" href = "../css/style.css" rel="stylesheet" type="text/css" />
<link href="../scripts/jquery-ui-1.12.1/jquery-ui.css" rel="stylesheet" />

<script type="text/javascript" src="../scripts/jquery.min.js"></script>
<script src="../scripts/jquery-ui-1.12.1/jquery-ui.min.js"></script>


<style>
    .progressbar
    {
        width: 100%;
        height: 21px;
    }

    .progressbarlabel
    {
        width: 98%;
        height: 21px;
        position: absolute;
        text-align: center;
        font-size: small;
    }
</style>

<div class="col-lg-12 col-md-12 col-sm-12 col-xs-12 userControl" >
    <div class="row">
        <div class="col-sm-6 col-xs-12">
            <span class="pull-left" style="padding-left: 0; padding-top: 5px; padding-right: 2px;">Select &nbsp;</span>
            <div class="pull-left grid_option customRadio">
                <asp:RadioButton ID="rdoFile" runat="server" GroupName="grpUpldType" Checked="true" />
                <label for="rdoFile" class="label-default" style="width: auto; margin-top: 10px;"></label>
            </div>
            <span class="pull-left" style="padding-left: 0; padding-top: 5px; padding-right: 2px;">File(s)</span>
            <div class="pull-left grid_option customRadio marginLFT10">
                <asp:RadioButton ID="rdoFolder" runat="server" GroupName="grpUpldType" />
                <label for="rdoFolder" class="label-default" style="width: auto; margin-top: 10px;"></label>
            </div>
            <span class="pull-left" style="padding-left: 0; padding-top: 5px; padding-right: 2px;">Folder(s)</span>
            <span class="pull-left" style="padding-left: 0; padding-top: 5px; padding-right: 2px;">&nbsp;to upload:</span>
        </div>
        <div class="row">
            <div class="col-sm-12 col-xs-12">
                <div class="col-sm-12 col-xs-12 margin-bottom-10 margin-top-10">
                    <asp:Label ID="lblHelpStart" runat="server" Font-Italic="True" Text="[Only" Font-Size="Smaller"></asp:Label>
                    <asp:Label ID="lblHelpStartDCM" runat="server" Font-Italic="True" Text="DICOM,.JPG/.JPEG,.PNG,.BMP,.PDF" Font-Size="Smaller"></asp:Label>
                    <asp:Label ID="lblUploadHelpSize" runat="server" Font-Italic="True" Text="&nbsp;format supported. Maximum 100 MB of File Size Allowed per Upload]" Font-Size="Smaller"></asp:Label>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-sm-12 col-xs-12 marginMobileTP5">
                <input type="file" id="fUpload" multiple="multiple" class="fUploader" style="width:98%;" />
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-sm-12 col-xs-12" style="margin-top: 5px;">
            <div id="divMsg" runat="server" style="color: red; height: 50px; overflow: auto;">
            </div>

        </div>
    </div>
    <div class="row">
        <div class="col-sm-12 col-xs-12" style="margin-top: 5px;">
            <div id="progressbar" class="progressbar">
                <div id="progresslabel" class="progressbarlabel"></div>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-sm-12 col-xs-12" style="margin-top: 5px;">
           &nbsp;
        </div>
    </div>
</div>
<input type="hidden" id="hdnPathToSave" value="" runat="server" />
<input type="hidden" id="hdnUserID" value="" runat="server" />
<input type="hidden" id="hdnRootDirectory" value="" runat="server" />
<script type="text/javascript">
    var objrdoFile = document.getElementById("<%=rdoFile.ClientID%>");
    var objrdoFolder = document.getElementById("<%=rdoFolder.ClientID%>");
    var objfUpload = document.getElementById("fUpload");
    var objdivMsg = document.getElementById("<%=divMsg.ClientID%>");
    var objhdnPathToSave = document.getElementById("<%=hdnPathToSave.ClientID%>");
    var objhdnUserID = document.getElementById("<%=hdnUserID.ClientID%>");
    var objhdnRootDirectory = document.getElementById("<%=hdnRootDirectory.ClientID%>");

    var xhr = new XMLHttpRequest();


    $(document).ready($(function () {

        $("#fUpload").on('change', function (evt) {
            var data = new FormData();
            var size = 0;
            var files = $("#fUpload").get(0).files;
            for (var i = 0; i < files.length; i++) {
                size = size + files[i].size;
                data.append(files[i].name, files[i]);
            }
            //debugger;
            data.append("tmpPath", objhdnPathToSave.value);
            data.append("uid", objhdnUserID.value);
            //if (size <= 104251492)
            if (size <= (100 * 1024 * 1024)) {
                
                xhr.upload.addEventListener("progress", function (evt) {
                    if (evt.lengthComputable) {
                        var progress = Math.round(evt.loaded * 100 / evt.total);
                        $("#progressbar").progressbar("value", progress);
                    }
                }, false);

                xhr.open("POST", "StudyFilesUploadHandler.ashx");
                xhr.onreadystatechange = ClientSideUpdate;
                xhr.send(data);

                $("#progressbar").progressbar({
                    max: 100,
                    change: function (evt, ui) {
                        $("#progresslabel").text($("#progressbar").progressbar("value") + "%");
                        if (parent.objbtnSubmit1 != null) parent.objbtnSubmit1.disabled = true;
                        if (parent.objbtnSubmit2 != null) parent.objbtnSubmit2.disabled = true;
                        if (parent.objbtnClose1 != null) parent.objbtnClose1.disabled = true;
                        if (parent.objbtnClose2 != null) parent.objbtnClose2.disabled = true;
                    },
                    complete: function (evt, ui) {
                        if (parent.objbtnSubmit1 != null) parent.objbtnSubmit1.disabled = false;
                        if (parent.objbtnSubmit2 != null) parent.objbtnSubmit2.disabled = false;
                        if (parent.objbtnClose1 != null) parent.objbtnClose1.disabled = false;
                        if (parent.objbtnClose2 != null) parent.objbtnClose2.disabled = false;
                        // $("#progresslabel").text("File upload successful!");
                    }
                });
            }
            else {
                var size_upld = parseFloat((size / 1024) / 1024).toFixed(2);
                parent.parent.PopupMessage(objhdnRootDirectory.value, "StudyFileUploader", "Study File Upload", "332", "true", size_upld.toString());
                $("#fUpload").val('');
            }
            evt.preventDefault();
        });
    }))
    function ClientSideUpdate() {
        var result;
        var arrArgs = new Array();

        if (xhr.readyState == 4) {
            result = xhr.responseText;
            arrArgs[0] = result;
            parent.ProcessStudyFileUpload(arrArgs);
            // objdivMsg.innerHTML = result;
        }
    }
    function SetUploaderAttribute() {
        if (objrdoFile.checked) {
            objfUpload.removeAttribute("webkitdirectory");
            objfUpload.removeAttribute("directory");
        }
        else if (objrdoFolder.checked) {
            objfUpload.setAttribute("webkitdirectory", "");
            objfUpload.setAttribute("directory", "");
        }
    }
</script>

