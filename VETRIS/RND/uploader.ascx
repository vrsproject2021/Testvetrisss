<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="uploader.ascx.cs" Inherits="VETRIS.RND.uploader" %>
<meta charset="utf-8" />
<meta http-equiv="x-ua-compatible" content="ie=edge" />
<meta name="description" content="" />
<meta name="viewport" content="width=device-width, initial-scale=1" />

<link href="https://fonts.googleapis.com/css?family=Roboto:100,300,400,700,900" rel="stylesheet" />
<link rel="stylesheet" href="../css/bootstrap.min.css" />
<link rel="stylesheet" href="../css/font-awesome.min.css" />
<link href="../css/responsive.css" rel="stylesheet" />
<link href="../css/style.css?1" rel="stylesheet" />
<link rel="stylesheet" href="../css/custom.css" />
<link href="../css/custome-css-style.css" rel="stylesheet" />
<link href="../scripts/jquery-ui-1.12.1/jquery-ui.css" rel="stylesheet" />

<script type="text/javascript" src="../scripts/jquery.min.js"></script>
<script src="../scripts/jquery-ui-1.12.1/jquery-ui.min.js"></script>

<script type="text/javascript">
    function ReturnUploadData(strFileName) {
        var arrArgs = new Array();

        arrArgs[0] = strFileName;
        parent.ProcessDCMUpload(arrArgs);
    }
   
</script>
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
<div class="col-lg-12 col-md-12 col-sm-12 col-xs-12" style="background-color:#fff;">
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
        <div class="col-sm-12 col-xs-12 marginMobileTP5">
            <%--<asp:FileUpload ID="fUpload" AllowMultiple="true" runat="server" Height="22px" BorderStyle="Solid" BorderWidth="1" Width="100%" ValidateRequestMode="Disabled" />--%>
            <input type="file" id="fUpload" multiple="multiple" style="height: 22px; border: solid 1px #000; width: 100%;" />
        </div>
    </div>
    <div class="row">
        <div class="col-sm-12 col-xs-12" style="margin-top: 5px;">
            <div id="divMsg" runat="server" style="color: red; height: 100px; overflow: auto;">
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
</div>

<script type="text/javascript">

    var objrdoFile = document.getElementById("<%=rdoFile.ClientID%>");
    var objrdoFolder = document.getElementById("<%=rdoFolder.ClientID%>");
    var objfUpload = document.getElementById("fUpload");
    var objdivMsg = document.getElementById("<%=divMsg.ClientID%>");
    
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
            data.append("tmpPath", "E:/VetChoice/VETRIS/RND/UploadedFiles");
            data.append("uid", "11111111-1111-1111-1111-111111111111");
            
            xhr.upload.addEventListener("progress", function (evt) {
                if (evt.lengthComputable) {
                    var progress = Math.round(evt.loaded * 100 / evt.total);
                    $("#progressbar").progressbar("value", progress);
                }
            }, false);

            if (size <= 104251492) {
                xhr.open("POST", "UploadHandler.ashx");
                xhr.onreadystatechange = ClientSideUpdate;
                xhr.send(data);

                $("#progressbar").progressbar({
                    max: 100,
                    change: function (evt, ui) {
                        $("#progresslabel").text($("#progressbar").progressbar("value") + "%");
                    },
                    complete: function (evt, ui) {
                        // $("#progresslabel").text("File upload successful!");
                    }
                });
            }
            else {
                alert("Too many files to upload or size of file(s) greater than 100 MB. Maximum size allowed per upload is 100MB.");
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
            parent.ProcessDCMUpload(arrArgs);
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
