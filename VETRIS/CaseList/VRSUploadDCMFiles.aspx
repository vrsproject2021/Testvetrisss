<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VRSUploadDCMFiles.aspx.cs" Inherits="VETRIS.CaseList.VRSUploadDCMFiles" %>
<%@ Register TagPrefix="Bulk" TagName="Uploader" Src="DICOMUploader.ascx" %>
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
</head>
<body style="background-color:#fff;">
    <form id="attachme" method="post" enctype="multipart/form-data" runat="server">
        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
            <Bulk:Uploader runat="server" ID="Uploader1"></Bulk:Uploader>
        </div>
    </form>
</body>
</html>
