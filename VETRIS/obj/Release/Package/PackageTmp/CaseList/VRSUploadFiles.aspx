<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VRSUploadFiles.aspx.cs" Inherits="VETRIS.CaseList.VRSUploadFiles" %>

<%@ OutputCache Location="None" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
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

    <script type="text/javascript" src="../scripts/jquery.min.js"></script>
    <script type="text/javascript">
        function ReturnUploadData(strFileName, strDocName, strMode) {
            var arrArgs = new Array();

            arrArgs[0] = strFileName;
            arrArgs[1] = strDocName;
            parent.ProcessUpload(arrArgs);
        }
    </script>
</head>
<body class="nav-sm" style="background: transparent;">
    <form id="form1" runat="server">
        <div style="margin: 10px;">
            <div class="row">
                    <div class="col-sm-12 col-xs-12 margin-bottom-10">
                                <div class="row">
                                    <div class="col-sm-12 col-xs-12">
                                        <div class="pull-left">
                                            <asp:Label ID="lblUploadHdr" runat="server" ForeColor="#1e77bb" Font-Size="16px"></asp:Label>
                                        </div>

                                    </div>
                                    <div class="col-sm-12 col-xs-12">
                                        <div class="borderSearch"></div>
                                   </div>
                                </div>

                                <div class="row">
                                    <div class="col-sm-6 col-xs-12">
                                        <div class="row">
                                            <div class="col-sm-12 col-xs-12 margin-bottom-10 margin-top-10">
                                                <asp:Label ID="lblUploadHelpStart" runat="server" Font-Italic="True" Text="[Only" Font-Size="Smaller"></asp:Label>
                                                <%--<asp:Label ID="lblUploadHelpDoc" runat="server" Font-Italic="True" Text=" .DOC/.DOCX,.PDF,.XLS/.XLSX,.PPT,.TXT" Font-Size="Smaller"></asp:Label>--%>
                                                <asp:Label ID="lblUploadHelpDoc" runat="server" Font-Italic="True" Text=" .PDF" Font-Size="Smaller"></asp:Label>
                                              <%--  <asp:Label ID="lblUploadHelpImg" runat="server" Font-Italic="True" Text=" .JPG/.JPEG,.GIF,.PNG,.BMP" Font-Size="Smaller"></asp:Label>--%>
                                                  <asp:Label ID="lblUploadHelpImg" runat="server" Font-Italic="True" Text=" .JPG/.JPEG,.PNG" Font-Size="Smaller"></asp:Label>
                                                <asp:Label ID="lblUploadHelpSize" runat="server" Font-Italic="True" Text="&nbsp;format supported. Maximum 5MB " Font-Size="Smaller"></asp:Label>
                                                <asp:Label ID="lblUploadHelpEnd" runat="server" Font-Italic="True" Text="of File Size Allowed]" Font-Size="Smaller"></asp:Label>
                                            </div>

                                            <div class="col-sm-12 col-xs-12">
                                                <asp:FileUpload ID="flUpload" runat="server" Height="22px" BorderStyle="Solid" BorderWidth="1" Width="80%" ValidateRequestMode="Disabled" />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-sm-6 col-xs-12 text-right marginMobileTP10">
                                        <asp:Button ID="btnUpload" runat="server" OnClick="btnUpload_Click" CssClass="btn btn-warning" Text="Upload" />
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-12 col-xs-12 margin-bottom-10">
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
        <asp:HiddenField ID="hdnDirName" runat="server" />
        <asp:HiddenField ID="hdnUploadType" runat="server" Value="DOC" />
        <asp:HiddenField ID="hdnFileID" runat="server" Value="" />
        <input type="hidden" id="hdnReturn" runat="server" value="" />
    </form>
</body>

</html>
