<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VRSCheckArchiveFileCount.aspx.cs" Inherits="VETRIS.CaseList.VRSCheckArchiveFileCount" %>
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

    <script src="../scripts/jquery.min.js"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
            <div class="sparklineHeader mt-b-10 marginTP10">
                <div class="sparkline10-hd">
                    <div class="row">
                        <div class="col-sm-10 col-xs-12">
                            <h2>Checking Synced File(s) Of 
                               <asp:Label ID="lblPatientName" runat="server" Text=""></asp:Label>
                            </h2>
                        </div>
                        

                        <div class="col-sm-2 col-xs-12 text-right">
                            <button type="button" class="btn btn-custon-four btn-danger" id="btnClose1" runat="server">
                                <i class="fa fa-times edu-danger-error" aria-hidden="true"></i>&nbsp;Close   
                            </button>
                        </div>
                    </div>
                </div>
            </div>

            <div class="sparkline10-list mt-b-10" id="divDL" style="display:none;">
               
                <div class="row" id="divCount">
                    <div class="col-sm-12 col-xs-12">
                        <div class="pull-left" id="divCountLoad" style="display:none;"><img src="../images/loader1.gif" style="margin-top:2px;"/></div>
                        <div class="pull-left" id="divCountInfo" style="display:none;"><img src="../images/info.png" style="margin-top:2px;"/></div>
                        <div class="pull-left" id="divCountErr" style="display:none;"><img src="../images/error.png" style="margin-top:2px;"/></div>
                        <span id="spnCountMsg" class="pull-left" style="padding:5px;"></span>
                    </div>
                </div>
            </div>
            <div class="sparkline10-list mt-b-10" id="divMsg" style="display: none;">

                <asp:Label ID="lblMsg" runat="server" Text=""></asp:Label>
            </div>
        </div>
        <input type="hidden" id="hdnError" runat="server" value="" />
        <input type="hidden" id="hdnID" runat="server" value="00000000-0000-0000-0000-000000000000" />
       
        <input type="hidden" id="hdnArchFolderPath" runat="server" value="" />
        <input type="hidden" id="hdnArchAltFolderPath" runat="server" value="" />
        <input type="hidden" id="hdnInstCode" runat="server" value="" />
         <input type="hidden" id="hdnInstName" runat="server" value="" />
        <input type="hidden" id="hdnStudyUID" runat="server" value="" />
      
    </form>
</body>
<script type="text/javascript">
    var objhdnID = document.getElementById('<%=hdnID.ClientID %>');
    var objhdnError = document.getElementById('<%=hdnError.ClientID %>');
    var objhdnInstCode = document.getElementById('<%=hdnInstCode.ClientID %>');
    var objhdnInstName = document.getElementById('<%=hdnInstName.ClientID %>');
    var objhdnArchFolderPath = document.getElementById('<%=hdnArchFolderPath.ClientID %>');
    var objhdnArchAltFolderPath = document.getElementById('<%=hdnArchAltFolderPath.ClientID %>');
    var objhdnStudyUID = document.getElementById('<%=hdnStudyUID.ClientID %>');
    var strForm = "VRSCheckArchiveFileCount";
</script>
<script src="../scripts/custome-javascript.js"></script>
<script src="scripts/CheckArchiveFIleCount.js"></script>
</html>
