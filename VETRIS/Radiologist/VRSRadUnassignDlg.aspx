<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VRSRadUnassignDlg.aspx.cs" Inherits="VETRIS.Radiologist.VRSRadUnassignDlg" %>
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
    <%--<link href="../css/style.css" rel="stylesheet" />--%>
    <link id="lnkSTYLE" runat="server" href = "../css/style.css" rel="stylesheet" type="text/css" />

    <script src="../scripts/jquery-1.7.1.js"></script>
    <script src="../scripts/jquery.soverlay.min.js"></script>
</head>
<body>
    <form id="form1" runat="server">
   <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
            <div class="sparklineHeader mt-b-10 marginTP10">
                <div class="sparkline10-hd">
                    <div class="row">
                        <div class="col-sm-5 col-xs-12">
                            <h2>Unassign Radiologist 
                                 <asp:Label ID="lblAsnType" runat="server"></asp:Label>
                            </h2>
                        </div>

                        <div class="col-sm-7 col-xs-12 text-right">


                            <button type="button" class="btn btn-custon-four btn-warning" id="btnReset1" runat="server">
                                <i class="fa fa-repeat edu-danger-error" aria-hidden="true"></i>&nbsp;Reset   
                            </button>

                            <button type="button" class="btn btn-custon-four btn-danger" id="btnClose1" runat="server">
                                <i class="fa fa-times edu-danger-error" aria-hidden="true"></i>&nbsp;Close   
                            </button>

                        </div>

                    </div>
                </div>
            </div>
            <div class="sparkline10-list mt-b-10" id="divStudyUID">
                <div class="sparkline10-hd">
                    <div class="row">
                        <div class="col-sm-12 col-xs-8">
                            Study UID : 
                            <asp:Label ID="lblSUID" runat="server"></asp:Label>
                        </div>


                    </div>
                </div>
            </div>
            <div class="sparkline10-list mt-b-10">
                <div class="row">
                    <div class="col-sm-12 col-xs-12">
                        <div class="searchSection">
                            <div class="row">
                                <div class="col-sm-12 col-xs-12">

                                    <div class="pull-left">
                                        <h3 class="h3Text">Study Details</h3>
                                    </div>

                                    <div class="borderSearch pull-left"></div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-12 col-xs-12">
                                    <div class="row marginTP10">
                                        <div class="col-sm-3 col-xs-12">
                                            Patient Name
                                        </div>
                                        <div class="col-sm-5 col-xs-12">
                                            <asp:Label ID="lblPatientName" runat="server"></asp:Label>
                                        </div>

                                    </div>
                                    <div class="row marginTP10">
                                        <div class="col-sm-3 col-xs-12">
                                            Modality
                                        </div>
                                        <div class="col-sm-5 col-xs-12">
                                            <asp:Label ID="lblModality" runat="server"></asp:Label>
                                        </div>
                                    </div>

                                    <div class="row marginTP10">
                                        <div class="col-sm-3 col-xs-12">
                                            Category
                                        </div>
                                        <div class="col-sm-5 col-xs-12">
                                            <asp:Label ID="lblCategory" runat="server"></asp:Label>
                                        </div>

                                    </div>
                                    <div class="row marginTP10">
                                        <div class="col-sm-3 col-xs-12">
                                            Institution
                                        </div>
                                        <div class="col-sm-5 col-xs-12">
                                            <asp:Label ID="lblInstitution" runat="server"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="row marginTP10">
                                        <div class="col-sm-3 col-xs-12">
                                            Priority
                                        </div>
                                        <div class="col-sm-6 col-xs-12">
                                            <asp:Label ID="lblPriority" runat="server"></asp:Label>
                                        </div>

                                    </div>
                                    <div class="row marginTP10">
                                        <div class="col-sm-3 col-xs-12">
                                            Service(s)
                                        </div>
                                        <div class="col-sm-5 col-xs-12">
                                            <asp:Label ID="lblServices" runat="server"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="row marginTP10">
                                        <div class="col-sm-3 col-xs-12">
                                            Current Status
                                        </div>
                                        <div class="col-sm-6 col-xs-12">
                                            <asp:Label ID="lblCurrStat" runat="server"></asp:Label>
                                        </div>


                                    </div>
                                    <div class="row marginTP10">
                                        <div class="col-sm-3 col-xs-12">
                                            Preliminary Radiologist Assigned
                                        </div>
                                        <div class="col-sm-5 col-xs-12">
                                            <asp:Label ID="lblPrelimRad" runat="server"></asp:Label>
                                        </div>
                                        <div class="col-sm-4 col-xs-12">
                                            <button type="button" class="btn btn-custon-four btn-danger" id="btnUnassignPrelim" runat="server" title="click to unassign the radiologist" style="display:none;">
                                                <i class="fa fa-pencil-square-o edu-danger-error" aria-hidden="true"></i>&nbsp;Unassign Prelim. Radiologist   
                                            </button>
                                        </div>
                                    </div>
                                    <div class="row marginTP10">
                                        <div class="col-sm-3 col-xs-12">
                                            Final Radiologist Assigned
                                        </div>
                                        <div class="col-sm-5 col-xs-12">
                                            <asp:Label ID="lblFinalRad" runat="server"></asp:Label>
                                        </div>
                                        <div class="col-sm-4 col-xs-12">
                                            <button type="button" class="btn btn-custon-four btn-danger" id="btnUnassignFinal" runat="server" title="click to unassign the radiologist" style="display:none;">
                                                <i class="fa fa-pencil-square-o edu-danger-error" aria-hidden="true"></i>&nbsp;Unassign Final Radiologist  
                                            </button>
                                        </div>
                                    </div>
                                    <div class="row marginTP10">
                                        <div class="col-sm-12 col-xs-12">
                                            &nbsp;
                                        </div>

                                    </div>
                                   
                                </div>
                            </div>
                        </div>
                    </div>

                </div>
            </div>
            <div class="sparklineHeader marginTP10">
                <div class="sparkline10-hd">
                    <div class="row">
                        <div class="col-sm-7 col-xs-12">
                            <div id="divMsg">
                                <asp:Label ID="lblMsg" runat="server"></asp:Label>
                            </div>
                        </div>

                        <div class="col-sm-5 col-xs-12 text-right">
                            <button type="button" class="btn btn-custon-four btn-warning" id="btnReset2" runat="server">
                                <i class="fa fa-repeat edu-danger-error" aria-hidden="true"></i>&nbsp;Reset 
                            </button>

                            <button type="button" class="btn btn-custon-four btn-danger" id="btnClose2" runat="server">
                                <i class="fa fa-times edu-danger-error" aria-hidden="true"></i>&nbsp;Close     
                            </button>

                        </div>

                    </div>
                </div>
            </div>
        </div>
        <input type="hidden" id="hdnError" runat="server" value="" />
        <input type="hidden" id="hdnID" runat="server" value="00000000-0000-0000-0000-000000000000" />
        <input type="hidden" id="hdnPrelimRadID" runat="server" value="00000000-0000-0000-0000-000000000000" />
        <input type="hidden" id="hdnFinalRadID" runat="server" value="00000000-0000-0000-0000-000000000000" />
        <input type="hidden" id="hdnType" runat="server" value="" />
        <input type="hidden" id="hdnSUID" runat="server" value="" />
        <input type="hidden" id="hdnStatusID" runat="server" value="0" />
        <input type="hidden" id="hdnUserID" runat="server" value="00000000-0000-0000-0000-000000000000" />
        <input type="hidden" id="hdnMenuID" runat="server" value="0" />
        <input type="hidden" id="hdnSessionID" runat="server" value="00000000-0000-0000-0000-000000000000" />
    </form>
</body>
<script type="text/javascript">
    var objhdnID = document.getElementById('<%=hdnID.ClientID %>');
    var objhdnError = document.getElementById('<%=hdnError.ClientID %>');
    var objhdnType = document.getElementById('<%=hdnType.ClientID %>');
    var objhdnSUID = document.getElementById('<%=hdnSUID.ClientID %>');
    var objhdnPrelimRadID = document.getElementById('<%=hdnPrelimRadID.ClientID %>');
    var objhdnFinalRadID = document.getElementById('<%=hdnFinalRadID.ClientID %>');
    var objhdnStatusID = document.getElementById('<%=hdnStatusID.ClientID %>');
    var objhdnUserID = document.getElementById('<%=hdnUserID.ClientID %>');
    var objhdnMenuID = document.getElementById('<%=hdnMenuID.ClientID %>');
    var objbtnUnassignPrelim = document.getElementById('<%=btnUnassignPrelim.ClientID %>');
    var objbtnUnassignFinal = document.getElementById('<%=btnUnassignFinal.ClientID %>');
    var objhdnSessionID = document.getElementById('<%=hdnSessionID.ClientID %>');
    var objlblMsg = document.getElementById('<%=lblMsg.ClientID %>');
    var strForm = "VRSRadAssignDlg";

</script>
<script src="../scripts/custome-javascript.js"></script>
<script src="scripts/RadUnassignDlg.js?v=<%=DateTime.Now.Ticks%>"></script>
</html>
