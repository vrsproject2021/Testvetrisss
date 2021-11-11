<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VRSCases.aspx.cs" Inherits="VETRIS.Dashboard.Cases.VRSCases" %>

<%@ Register Assembly="ComponentArt.Web.UI" Namespace="ComponentArt.Web.UI" TagPrefix="ComponentArt" %>
<%@ OutputCache Location="None" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Dashboard</title>
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
    <%--<link rel="stylesheet" href="../../css/style.css?7" />--%>
    <link id="lnkSTYLE" runat="server" href = "../css/style.css" rel="stylesheet" type="text/css" />
    <link id="dashboardSTYLE" runat="server" href = "../css/dashboard.css" rel="stylesheet" type="text/css" />

    <!-- responsive CSS
		============================================ -->
    <link rel="stylesheet" href="../../css/responsive.css" />

</head>
<body>
    <form id="caseForm" runat="server">
        <div class="open-case">
            OPEN CASES: NORMAL
        </div>

        <div class="flex-container-div">
            <div class="chart-container">
                <div class="chart-header ">Modality</div>
                <div id="pieCaseNormal" class="chart-body "></div>
            </div>
            <div class="chart-container">
                <div class="chart-header ">Status</div>
                <div id="pieCaseStatusNormal" class="chart-body "></div>
            </div>
            <div class="chart-container">
                <div class="chart-header ">Elapsed time (minutes)</div>
                <div id="pieCaseTimeNormal" class="chart-body "></div>
            </div>

            <%--<div id="pieCaseNormal"></div>
            <div id="pieCaseStatusNormal"></div>
            <div id="pieCaseTimeNormal"></div>--%>

            <%--<div>
                <canvas id="pieCaseStatusNormal" width="400" height="400"></canvas>
            </div>
            <div>
                <canvas id="pieCaseTimeNormal" width="400" height="400"></canvas>
            </div>--%>
        </div>


        <div class="open-case">
            OPEN CASES: STAT
        </div>
        <div class="flex-container-div">
            <div class="chart-container">
                <div class="chart-header ">Modality</div>
                <div id="pieCaseStat" class="chart-body "></div>
            </div>
            <div class="chart-container">
                <div class="chart-header ">Status</div>
                <div id="pieCaseStatusStat" class="chart-body "></div>
            </div>
            <div class="chart-container">
                <div class="chart-header ">Elapsed time (minutes)</div>
                <div id="pieCaseTimeStat" class="chart-body "></div>
            </div>

            <%--<div>
                <canvas id="pieCaseStat" width="400" height="400"></canvas>
            </div>
            <div>
                <canvas id="pieCaseStatusStat" width="400" height="400"></canvas>
            </div>
            <div>
                <canvas id="pieCaseTimeStat" width="400" height="400"></canvas>
            </div>--%>
        </div>


    </form>
    <input type="hidden" runat="server" id="hdnUserID" value="00000000-0000-0000-0000-000000000000" />
    <input type="hidden" runat="server" id="hdnDesc" value="" />
    <input type="hidden" runat="server" id="hdnMenuId" value="" />
    <input type="hidden" runat="server" id="hdnRefreshTime" value="0" />
    <input type="hidden" runat="server" id="hdnIsRefreshBtn" value="N" />
     <input type="hidden" runat="server" id="hdnreportTitle" value="" />
    <input type="hidden" runat="server" id="hdnTheme" value="" />
</body>
<script src="../../scripts/jquery-1.12.4.min.js" type="text/javascript"></script>
<script src="../../scripts/bootstrap.min.js" type="text/javascript"></script>
<script src="../../scripts/jquery-1.7.1.js" type="text/javascript"></script>
<script src="../../scripts/custome-javascript.js" type="text/javascript"></script>
<script src="../scripts/Chart.2.9.4.js"></script>
<script src="https://cdn.jsdelivr.net/gh/emn178/chartjs-plugin-labels/src/chartjs-plugin-labels.js"></script>
<script type="text/javascript" src="https://www.gstatic.com/charts/loader.js"></script>
<script src="../scripts/chartTools.js?t=<%=DateTime.Now.Ticks%>"></script>
<script src="../scripts/openCases.js?t=<%=DateTime.Now.Ticks%>"></script>

</html>
