<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VRSDownloadRouter.aspx.cs" Inherits="VETRIS.DownloadRouter.VRSDownloadRouter" %>

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
    <link href="../css/bootstrap.min.css" rel="stylesheet" />
    <link href="../css/font-awesome.min.css" rel="stylesheet" />
    <link href="../css/responsive.css" rel="stylesheet" />
    
    <link id="lnkSTYLE" runat="server" href = "../css/style.css" rel="stylesheet" type="text/css" />
    <script src="../scripts/jquery-1.7.1.js"></script>
</head>
<body>
    <form id="form1" runat="server">

        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
            <div class="sparklineHeader mt-b-10 marginTP10">
                <div class="sparkline10-hd">
                    <div class="row">
                        <div class="col-sm-5 col-xs-12">
                            <h2>Download Dicom Router For The Institution <span class="mandatory">*</span></h2>
                            
                        </div>
                        <div class="col-sm-5 col-xs-12 text-right">
                            <asp:DropDownList ID="ddlInstitution" runat="server" CssClass="form-control custom-select-value"></asp:DropDownList>
                        </div>
                        <div class="col-sm-2 col-xs-12 text-right">


                            <button type="button" class="btn btn-custon-four btn-danger" id="btnClose" runat="server">
                                <i class="fa fa-times edu-danger-error" aria-hidden="true"></i>&nbsp;Close
                                       
                            </button>
                        </div>
                    </div>
                </div>
            </div>


            <div class="sparkline10-list mt-b-10">
               
                <div class="sparkline10-graph text-center" style="margin-top: 50px; margin-bottom: 50px;">
                    <button type="button" class="btn btn-custon-four btn-success" id="btndownload" runat="server" style="padding: 10px;">
                        <i class="fa fa-download" aria-hidden="true"></i>&nbsp;Click here to download the DICOM Router - Version   
                    </button>

                </div>
            </div>
        </div>

        <input type="hidden" id="hdnError" runat="server" value="" />
        <input type="hidden" id="hdnID" runat="server" value="00000000-0000-0000-0000-000000000000" />
        <input type="hidden" id="hdnVer" runat="server" value="" />
    </form>
</body>
<script type="text/javascript">
    var objddlInstitution = document.getElementById('<%=ddlInstitution.ClientID %>');
    var objhdnID = document.getElementById('<%=hdnID.ClientID %>');
    var objhdnError = document.getElementById('<%=hdnError.ClientID %>');
    var objhdnVer = document.getElementById('<%=hdnVer.ClientID %>');
    var objddlInstitution = document.getElementById('<%=ddlInstitution.ClientID %>');
    var strForm = "VRSDownloadRouter";

</script>
<script src="../scripts/custome-javascript.js"></script>
<script src="../scripts/AppPages.js"></script>
<script src="scripts/DownloadRouter.js?10012020"></script>
</html>
