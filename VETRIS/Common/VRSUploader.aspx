<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VRSUploader.aspx.cs" Inherits="VETRIS.Common.VRSUploader" %>

<%@ OutputCache Location="None" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title> 
    <link rel="stylesheet" href="../css/bootstrap.min.css" />
    <link rel="stylesheet" href="../css/font-awesome.min.css" />
    <link rel="stylesheet" href="../css/custom.css" />
  
     <link id="lnkCUSTOM" runat="server" href = "../css/custome-css-style.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../scripts/jquery.min.js"></script>
    <script type="text/javascript">
        function ReturnUploadData(strFileName, strDocName, strMode) {
            var arrArgs = new Array();

            arrArgs[0] = strFileName;
            arrArgs[1] = strDocName;

            if (parent.objiframePage.contentWindow) {
                if (typeof (parent.objiframePage.contentWindow.GetData) == "function")
                    parent.objiframePage.contentWindow.GetData(arrArgs);
            }
            else {
                if (typeof (parent.objiframePage.contentDocument.parentWindow.GetData) == "function")
                    parent.objiframePage.contentDocument.parentWindow.GetData(arrArgs);
            }
            parent.HideUpload(arrArgs);
        }
    </script>
</head>
<body class="nav-sm" style="background: transparent;">
    <form id="form1" runat="server">
        <div class="container">
            <div class="main_container singleTask_page" style="margin: 10px;">
                <div class="row">
                    <div class="col-sm-12 col-xs-12 margin-bottom-10">
                        <div class="x_panel">
                            <div class="x_title">
                                <div class="row">
                                    <div class="col-sm-9 col-xs-9">
                                        <h4>
                                            <asp:Label ID="lblUploadHdr" runat="server"></asp:Label>

                                        </h4>
                                    </div>
                                    <div class="col-sm-3 col-xs-3 text-right">
                                        <ul class="nav navbar-right panel_toolbox">
                                            <li class="pull-right">
                                                <button type="button" class="btn btn-danger btn_hdr" onclick="javascript: parent.HideUpload();"><i class="fa fa-close" aria-hidden="true"></i></button>
                                            </li>
                                        </ul>
                                    </div>
                                     <div class="borderHdr pull-left"></div>
                                    
                                </div>

                            </div>
                            <div class="x_content">
                                <div class="row">
                                    <div class="col-sm-12 col-xs-12 margin-bottom-10 margin-top-10">
                                        <asp:Label ID="lblUploadHelpStart" runat="server" Font-Italic="True" Text="[Only" Font-Size="Smaller"></asp:Label>
                                        <asp:Label ID="lblUploadHelpDoc" runat="server" Font-Italic="True" Text=" .DOC/.DOCX,.PDF,.XLS/.XLSX,.PPT,.TXT" Font-Size="Smaller"></asp:Label>
                                        <asp:Label ID="lblUploadHelpImg" runat="server" Font-Italic="True" Text=" .JPG/.JPEG,.GIF,.PNG,.BMP" Font-Size="Smaller"></asp:Label>
                                        <asp:Label ID="lblUploadHelpSize" runat="server" Font-Italic="True" Text="&nbsp;format supported. Maximum 5MB " Font-Size="Smaller"></asp:Label>
                                        <asp:Label ID="lblUploadHelpEnd" runat="server" Font-Italic="True" Text="of File Size Allowed]" Font-Size="Smaller"></asp:Label>
                                    </div>
                                </div>
                                <div class="row" style="padding-top: 5px; padding-bottom: 5px;">
                                    <div class="col-sm-9 col-xs-9">
                                        <asp:FileUpload ID="flUpload" runat="server" Height="22px" BorderStyle="Solid" BorderWidth="1" Width="80%" ValidateRequestMode="Disabled" />

                                    </div>
                                    <div class="col-sm-3 col-xs-3 text-right">
                                        <asp:Button ID="btnUpload" runat="server" OnClick="btnUpload_Click" CssClass="btn btn-warning" Text="Upload" />
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-xs-12 margin-bottom-10">
                                        <div class="form-group">
                                            <label class="control-label" for="username">Document Name</label>
                                            <asp:TextBox ID="txtDocName" runat="server" CssClass="form-control" MaxLength="100"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-sm-12 col-xs-12" style="height: 40px; overflow: auto; vertical-align: top; text-align: center;">
                                        <asp:Label ID="lblError" runat="server" ForeColor="Red"></asp:Label>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <asp:HiddenField ID="hdnDirName" runat="server" />
        <asp:HiddenField ID="hdnUploadType" runat="server" Value="DOC" />
        <asp:HiddenField ID="hdnFileID" runat="server" Value="" />
        <input type="hidden" id="hdnReturn" runat="server" value="" />
    </form>
</body>
<script type="text/javascript">
    var objhdnReturn = document.getElementById('<%=hdnReturn.ClientID %>');
    var objtxtDocName = document.getElementById('<%=txtDocName.ClientID %>');
    $(document).ready($(function () {
        parent.adjustUploadFrameHeight();
    }))

</script>
</html>
