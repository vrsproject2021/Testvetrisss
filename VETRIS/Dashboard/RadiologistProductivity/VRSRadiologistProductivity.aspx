<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VRSRadiologistProductivity.aspx.cs" Inherits="VETRIS.Dashboard.RadiologistProductivity.VRSRadiologistProductivity" %>

<%@ Register Assembly="ComponentArt.Web.UI" Namespace="ComponentArt.Web.UI" TagPrefix="ComponentArt" %>
<%@ OutputCache Location="None" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Periodic Radiologist Productivity</title>
    <!-- Google Fonts
		============================================ -->
    <link href="https://fonts.googleapis.com/css?family=Roboto:100,300,400,700,900" rel="stylesheet" />
    <!-- Bootstrap CSS
		============================================ -->
    <link rel="stylesheet" href="../../css/bootstrap.min.css" />
    <!-- Bootstrap CSS
		============================================ -->
    <link rel="stylesheet" href="../../css/font-awesome.min.css" />
    <!-- metisMenu CSS
		============================================ -->
    <link rel="stylesheet" href="../../css/metisMenu/metisMenu.min.css" />
    <link rel="stylesheet" href="../../css/metisMenu/metisMenu-vertical.css?1" />
    <!-- style CSS
		============================================ -->
    <link id="lnkSTYLE" runat="server" href="../css/style.css" rel="stylesheet" type="text/css" />
    <link id="dashboardSTYLE" runat="server" href="../css/dashboard.css" rel="stylesheet" type="text/css" />
    <link id="dashboardLightbox" runat="server" href="../../css/DARK/lightbox.css" rel="stylesheet" type="text/css" />
    <!-- responsive CSS
		============================================ -->
    <link rel="stylesheet" href="../../css/responsive.css" />
    <link rel="stylesheet" href="../../css/table.css?<%=string.Format("{0:yyyyMMddHHmmss}",DateTime.Now) %>" />
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/flatpickr/dist/flatpickr.min.css" />
    <link href="https://cdn.jsdelivr.net/npm/select2@4.1.0-beta.1/dist/css/select2.min.css" rel="stylesheet" />

</head>
<body>
    <form id="form1" runat="server">
        <header style="margin-top: 5px;">
            <div class="form-inline mt-2">

                <div class="form-group">
                    <label for="fromDate">Date From:</label>
                    <input type="text" id="fromDate" placeholder="From" name="from" style="padding: 2px 4px; width: 92px;" class="date form-control" />
                </div>

                <div class="form-group">
                    <label for="toDate">To:</label>
                    <input type="text" id="toDate" placeholder="To" name="to" style="padding: 2px 4px; width: 92px;" class="date form-control" />
                </div>

                <div class="form-group">
                    <label for="md">Modality:</label>
                    <asp:DropDownList ID="ddlModality" AppendDataBoundItems="true" Style="padding: 2px 4px; width: 220px;" runat="server" CssClass="form-control custom-select-value"></asp:DropDownList>
                </div>
                <div class="form-group">
                    <label class="pull-left custom-label"  style="margin-top: 4px;" for="ob">Order by:</label>
                    <div class="pull-left grid_option1 customRadio" style="margin-left: 2px;">
                        <asp:RadioButton ID="rdoNumberName" runat="server" GroupName="grpStat" />
                        <label for="rdoNumberName" class="label-default" style="width: auto; margin-top: 10px;"></label>
                    </div>
                    <span class="pull-left custom-label" style="padding-left: 0px; padding-top: 5px; padding-right: 5px;">Radiologist Name</span>
                    <div class="pull-left grid_option1 customRadio">
                        <asp:RadioButton ID="rdoNumberCases" runat="server" GroupName="grpStat" />
                        <label for="rdoNumberCases" class="label-default" style="width: auto; margin-top: 10px;"></label>
                    </div>
                    <span class="pull-left custom-label" style="padding-left: 0px; padding-top: 5px; padding-right: 5px;">Number of Cases</span>
                </div>
                <button id="btnApply" type="button" class="btn btn-custon-four btn-success btn-new-cases" onclick="callSlotTime()">
                    &nbsp;Apply
                </button>
            </div>

        </header>
        <div class="flex-new-cases">
            <div class="row" style="min-height: 540px; overflow-y: auto;">
                <table class="styled-table" id="data">
                    <thead>
                        <tr>
                            <th rowspan="2">Name</th>
                            <th id="dynamicColspan">Cases Completed</th>
                        </tr>
                        <tr id="dynamicColumn">

                        </tr>
                    </thead>
                    <tbody class="tbody-main">
                        
                    </tbody>
                </table>
            </div>

        </div>

        <input type="hidden" runat="server" id="hdnRefreshTime" value="0" />
        <input type="hidden" runat="server" id="hdnIsRefreshBtn" value="N" />
        <input type="hidden" runat="server" id="hdnDesc" value="" />
        <input type="hidden" runat="server" id="hdnreportTitle" value="" />
        <input type="hidden" runat="server" id="hdnTheme" value="" />
    </form>


</body>
<script src="../../scripts/windows-iana/dist/windows-iana.esm.js" crossorigin="anonymous"></script>
<script src="../../scripts/moment.min.js" crossorigin="anonymous"></script>
<script src="../../scripts/moment-timezone-with-data.min.js" crossorigin="anonymous"></script>
<script src="../../scripts/jquery-1.12.4.min.js" type="text/javascript"></script>
<script src="../../scripts/bootstrap.min.js" type="text/javascript"></script>
<script src="../../scripts/custome-javascript.js" type="text/javascript"></script>

<script src="https://cdn.jsdelivr.net/npm/select2@4.1.0-beta.1/dist/js/select2.min.js"></script>
<script src="https://cdn.jsdelivr.net/npm/flatpickr"></script>
<script src="https://cdn.jsdelivr.net/npm/flatpickr/dist/plugins/rangePlugin.js"></script>
<script src="https://cdnjs.cloudflare.com/ajax/libs/underscore.js/1.8.3/underscore.js"></script>
<script src="../scripts/Chart.2.9.4.js"></script>
<script src="../scripts/chartTools.js"></script>
<script type="text/javascript" src="https://www.gstatic.com/charts/loader.js"></script>
<script src="../scripts/radiologistProductivity.js?t=<%=DateTime.Now.Ticks%>"></script>
</html>

