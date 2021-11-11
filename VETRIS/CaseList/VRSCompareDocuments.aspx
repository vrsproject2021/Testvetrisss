<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VRSCompareDocuments.aspx.cs" Inherits="VETRIS.CaseList.VRSCompareDocuments" %>

<%@ OutputCache Location="None" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta charset="utf-8" />
    <meta http-equiv="x-ua-compatible" content="ie=edge" />
    <meta name="description" content="" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link rel="shortcut icon" type="image/x-icon" href="../images/favicon.ico" />

    <link href="https://fonts.googleapis.com/css?family=Roboto:100,300,400,700,900" rel="stylesheet" />
    <link href="../css/bootstrap.min.css" rel="stylesheet" />
    <link href="../css/font-awesome.min.css" rel="stylesheet" />
    <link href="../css/responsive.css" rel="stylesheet" />
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
                        <div class="col-sm-6 col-xs-12">
                            <h2>Compare Reports</h2>
                        </div>
                        <div class="col-sm-6 col-xs-12 text-right">

                            <button type="button" class="btn btn-custon-four btn-danger" id="btnClose1" runat="server">
                                <i class="fa fa-times edu-danger-error" aria-hidden="true"></i>&nbsp;Close
                                       
                            </button>
                        </div>
                    </div>
                </div>
            </div>
            <div class="sparkline10-list mt-b-10">
                <div class="row">
                    <div class="col-sm-6 col-xs-6 col-sm-12 col-xs-12">
                        <div class="searchSection marginTP10">
                            <div class="row">
                                <div class="col-sm-12 col-xs-12">
                                    <div class="pull-left">
                                        <h3 class="h3Text">Prepared by :
                                            <asp:Label ID="lblPrepBy1" runat="server"></asp:Label>
                                        </h3>
                                    </div>
                                    <div class="borderSearch pull-left"></div>
                                </div>
                            </div>
                            <div class="row">
                                <iframe id="iframeDoc1" style="padding: 0px; margin: 10px; width: 97%; height: 475px; background-color: transparent; border: none;"></iframe>
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-6 col-xs-6 col-sm-12 col-xs-12">
                        <div class="searchSection marginTP10">
                            <div class="row">
                                <div class="col-sm-12 col-xs-12">
                                    <div class="pull-left">
                                        <h3 class="h3Text">Prepared by :
                                            <asp:Label ID="lblPrepBy2" runat="server"></asp:Label>
                                        </h3>
                                    </div>
                                    <div class="borderSearch pull-left"></div>
                                </div>
                            </div>
                            <div class="row">
                                <iframe id="iframeDoc2" style="padding: 0px; margin: 10px; width: 97%; height: 475px; background-color: transparent; border: none;"></iframe>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="sparkline10-list mt-b-10" id="divReason" style="display:none;">
                <div class="row">
                    <div class="col-sm-12 col-xs-12">
                        <div class="form-group">
                            <label class="control-label">Reason to mark the report as abnormal</label>
                            <asp:Label ID="lblAbRptReason" runat="server" CssClass="form-control"></asp:Label>

                        </div>
                    </div>
                </div>
            </div>
            <div class="sparkline10-list mt-b-10" id="div3rdframe" style="display: none;">
                <div class="row">
                    <div class="col-sm-6 col-xs-6 col-sm-12 col-xs-12">
                        <div class="searchSection marginTP10">
                            <div class="row">
                                <div class="col-sm-12 col-xs-12">
                                    <div class="pull-left">
                                        <h3 class="h3Text">Prepared by :
                                            <asp:Label ID="lblPrepBy3" runat="server"></asp:Label>
                                        </h3>
                                    </div>
                                    <div class="borderSearch pull-left"></div>
                                </div>
                            </div>
                            <div class="row">
                                <iframe id="iframeDoc3" style="padding: 0px; margin: 10px; width: 97%; height: 475px; background-color: transparent; border: none;"></iframe>
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-6 col-xs-6 col-sm-12 col-xs-12">
                        <div class="searchSection marginTP10">
                            <div class="row">
                                <div class="col-sm-12 col-xs-12">
                                    <div class="pull-left">
                                        <h3 class="h3Text">Prepared by :
                                            <asp:Label ID="lblPrepBy4" runat="server"></asp:Label>
                                        </h3>
                                    </div>
                                    <div class="borderSearch pull-left"></div>
                                </div>
                            </div>
                            <div class="row">
                                <iframe id="iframeDoc4" style="padding: 0px; margin: 10px; width: 97%; height: 475px; background-color: transparent; border: none;"></iframe>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="sparklineHeader marginTP10">
                <div class="sparkline10-hd">
                    <div class="row">
                        <div class="col-sm-6 col-xs-12">
                            &nbsp;
                        </div>
                        <div class="col-sm-6 col-xs-12 text-right">

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
        <input type="hidden" id="hdnUserID" runat="server" value="00000000-0000-0000-0000-000000000000" />
        <input type="hidden" id="hdnCustomRpt" runat="server" value="N" />
        <input type="hidden" id="hdnPName" runat="server" value="" />
        <input type="hidden" id="hdnFolder" runat="server" value="" />
        <input type="hidden" id="hdnFileName1" runat="server" value="" />
        <input type="hidden" id="hdnFileName2" runat="server" value="" />
        <input type="hidden" id="hdnFileName3" runat="server" value="" />
        <input type="hidden" id="hdnDivider" runat="server" value="" />
        <input type="hidden" id="hdnRootDirectory" runat="server" value="" />
        <input type="hidden" id="hdnServerPath" runat="server" value="" />
        <input type="hidden" id="hdnTrans" runat="server" value="N" />
    </form>
</body>
<script type="text/javascript">
    var objhdnID = document.getElementById('<%=hdnID.ClientID %>');
    var objhdnUserID = document.getElementById('<%=hdnUserID.ClientID %>');
    var objhdnError = document.getElementById('<%=hdnError.ClientID %>');
    var objhdnRootDirectory = document.getElementById('<%=hdnRootDirectory.ClientID %>');
    var objhdnServerPath = document.getElementById('<%=hdnServerPath.ClientID %>');
    var objhdnDivider = document.getElementById('<%=hdnDivider.ClientID %>');
    var objhdnPName = document.getElementById('<%=hdnPName.ClientID %>');
    var objhdnCustomRpt = document.getElementById('<%=hdnCustomRpt.ClientID %>');
    var objhdnFolder = document.getElementById('<%=hdnFolder.ClientID %>');
    var objhdnFileName1 = document.getElementById('<%=hdnFileName1.ClientID %>');
    var objhdnFileName2 = document.getElementById('<%=hdnFileName2.ClientID %>');
    var objhdnFileName3 = document.getElementById('<%=hdnFileName3.ClientID %>');
    var objiframeDoc1 = document.getElementById('iframeDoc1');
    var objiframeDoc2 = document.getElementById('iframeDoc2');
    var objiframeDoc3 = document.getElementById('iframeDoc3');
    var objiframeDoc4 = document.getElementById('iframeDoc4');
    var objhdnTrans = document.getElementById('<%=hdnTrans.ClientID %>');
    var objlblAbRptReason = document.getElementById('<%=lblAbRptReason.ClientID %>');
    var objdivReason = document.getElementById('divReason');
    var strForm = "VRSCompareDocuments";
</script>
<script src="../scripts/custome-javascript.js"></script>
<script src="scripts/CompareDocuments.js?v=<%=DateTime.Now.Ticks%>"></script>
</html>
