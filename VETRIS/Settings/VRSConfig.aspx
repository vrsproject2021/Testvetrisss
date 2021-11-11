<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VRSConfig.aspx.cs" Inherits="VETRIS.Settings.VRSConfig" %>

<%@ Register Assembly="ComponentArt.Web.UI" Namespace="ComponentArt.Web.UI" TagPrefix="ComponentArt" %>
<%@ OutputCache Location="None" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <meta charset="utf-8" />
    <meta http-equiv="x-ua-compatible" content="ie=edge" />
    <meta name="description" content="" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />

    <link href="https://fonts.googleapis.com/css?family=Roboto:100,300,400,700,900" rel="stylesheet" />
    <link href="https://fonts.googleapis.com/css?family=Roboto:100,300,400,700,900" rel="stylesheet" />
    <link href="../css/bootstrap.min.css" rel="stylesheet" />
    <link href="../css/font-awesome.min.css" rel="stylesheet" />
    <link href="../css/responsive.css" rel="stylesheet" />
    <%--<link href="../css/style.css" rel="stylesheet" />--%>
    <link id="lnkSTYLE" runat="server" href = "../css/style.css" rel="stylesheet" type="text/css" />

    <%--<link href="../css/theme.css" rel="stylesheet" />
    <link href="../css/grid_style.css?1" rel="stylesheet" />
        <link href="../css/tabStyle1.css" rel="stylesheet" />--%>

    <link id="lnkTHEME" runat="server" href="../css/theme.css" rel="stylesheet"  type="text/css"/>
    <link id="lnkGRID" runat="server" href = "../css/grid_style.css" rel="stylesheet" type="text/css" />
    <link id="lnkTAB" runat="server" href = "../css/tabStyle1.css" rel="stylesheet" type="text/css" />
    

    <script src="../scripts/jquery.min.js"></script>
    <script src="scripts/ConfigHdr.js?v=<%=DateTime.Now.Ticks%>"></script>
    <style>
        .cell-row-update div {
            width: 100% !important;
        }
    </style>
</head>
<body>
    <form id="form2" runat="server">
        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
            <div class="sparklineHeader mt-b-10 marginTP10">
                <div class="sparkline10-hd">

                    <div class="row">
                        <div class="col-sm-6 col-xs-12">
                            <h2>Configuration</h2>
                        </div>
                        <div class="col-sm-6 col-xs-12 text-right">

                            <button type="button" id="btnClose1" runat="server" class="btn btn-custon-four btn-danger">
                                <i class="fa fa-times" aria-hidden="true"></i>&nbsp;Close

                            </button>
                        </div>
                    </div>
                </div>
            </div>

            <div class=" row mt-b-10 marginTP10">
                <div class="col-sm-12 col-xs-12">
                    <div class="sparkline10-list mt-b-10">
                        <div class=" row mt-b-10 marginTP10">
                            <div class="col-sm-12 col-xs-12">
                                <ComponentArt:TabStrip ID="tsConfig"
                                    CssClass="TopGroup"
                                    SiteMapXmlFile="ConfigTabData.xml"
                                    EnableTheming="false" DefaultGroupSeparatorWidth="5px"
                                    DefaultGroupShowSeparators="true"
                                    DefaultItemLookId="DefaultTabLook"
                                    DefaultSelectedItemLookId="SelectedTabLook"
                                    DefaultGroupTabSpacing="1"
                                    ImagesBaseUrl="../images/"
                                    MultiPageId="ImageManagerPages"
                                    Width="100%"
                                    runat="server">
                                    <ItemLooks>
                                        <ComponentArt:ItemLook LookId="DefaultTabLook" CssClass="DefaultTab" HoverCssClass="DefaultTabHover" LabelPaddingLeft="0" LabelPaddingRight="1" LabelPaddingTop="5" LabelPaddingBottom="4" LeftIconWidth="10" />
                                        <ComponentArt:ItemLook LookId="SelectedTabLook" CssClass="SelectedTab" LabelPaddingLeft="2" LabelPaddingRight="1" LabelPaddingTop="5" LabelPaddingBottom="4" LeftIconWidth="10" />
                                    </ItemLooks>
                                    <%--<ClientEvents>
                                                <TabSelect EventHandler="tsConfig_OnSelect" />
                                            </ClientEvents>--%>
                                </ComponentArt:TabStrip>
                            </div>
                        </div>
                        <div class=" row mt-b-10 marginTP10">
                            <div class=" row mt-b-10 marginTP10">
                                <div class="col-sm-12 col-xs-12">
                                    <ComponentArt:MultiPage ID="ImageManagerPages" runat="server" Width="100%">
                                        <ComponentArt:PageView ID="GS" runat="server" Width="100%">
                                            <div class="col-sm-12 col-xs-12">

                                                <div class="row">

                                                    <div class="col-sm-12 col-xs-12 text-right" style="margin-left: -5px;">
                                                        <button type="button" id="btnSave1" runat="server" class="btn btn-custon-four btn-success">
                                                            <i class="fa fa-floppy-o" aria-hidden="true"></i>&nbsp;Save
                                                        </button>
                                                    </div>
                                                </div>


                                                <div class="sparkline10-list mt-b-10">
                                                    <div class="searchSection">

                                                        <div class="sparkline10-graph">
                                                            <div class="static-table-list">
                                                                <div class="table-responsive">
                                                                    <ComponentArt:CallBack ID="CallBackBrw" runat="server" OnCallback="CallBackBrw_Callback">
                                                                        <Content>
                                                                            <ComponentArt:Grid
                                                                                ID="grdBrw"
                                                                                CssClass="Grid"
                                                                                AutoTheming="true"
                                                                                DataAreaCssClass=""
                                                                                SearchOnKeyPress="true"
                                                                                EnableViewState="true"
                                                                                RunningMode="Client"
                                                                                ShowSearchBox="false"
                                                                                SearchBoxPosition="TopLeft"
                                                                                SearchTextCssClass="GridHeaderText" PageSize="200"
                                                                                ShowHeader="false"
                                                                                FooterCssClass="GridFooter"
                                                                                GroupingNotificationText=""
                                                                                ScrollBar="Off"
                                                                                ScrollTopBottomImagesEnabled="true"
                                                                                ScrollTopBottomImageHeight="2"
                                                                                ScrollTopBottomImageWidth="16"
                                                                                ScrollImagesFolderUrl="../images/scroller/"
                                                                                ScrollButtonWidth="16"
                                                                                ScrollButtonHeight="17"
                                                                                ShowFooter="false"
                                                                                ScrollBarCssClass="ScrollBar"
                                                                                ScrollGripCssClass="ScrollGrip"
                                                                                ScrollBarWidth="16"
                                                                                PagerTextCssClass="GridFooterText"
                                                                                PagerButtonWidth="24"
                                                                                PagerButtonHeight="24"
                                                                                PagerButtonHoverEnabled="true"
                                                                                ImagesBaseUrl="../images/"
                                                                                LoadingPanelFadeDuration="1000"
                                                                                LoadingPanelFadeMaximumOpacity="80"
                                                                                LoadingPanelClientTemplateId="LoadingFeedbackTemplate"
                                                                                LoadingPanelPosition="MiddleCenter"
                                                                                Width="99%"
                                                                                runat="server"
                                                                                HeaderCssClass="GridHeader"
                                                                                GroupingNotificationPosition="TopRight"
                                                                                SearchBoxCssClass="EditTextBoxStyle">
                                                                                <Levels>
                                                                                    <ComponentArt:GridLevel AllowGrouping="false"
                                                                                        DataMember="sys_group"
                                                                                        DataKeyField="group_id"
                                                                                        ShowTableHeading="false"
                                                                                        TableHeadingCssClass="GridHeader"
                                                                                        RowCssClass="Row"
                                                                                        HoverRowCssClass="HoverRow"
                                                                                        ColumnReorderIndicatorImageUrl="reorder.gif"
                                                                                        DataCellCssClass="DataCell"
                                                                                        HeadingCellCssClass="HeadingCell"
                                                                                        HeadingRowCssClass="HeadingRow"
                                                                                        HeadingTextCssClass="HeadingCellText"
                                                                                        EditCellCssClass="active"
                                                                                        SortedDataCellCssClass="SortedDataCell"
                                                                                        SelectedRowCssClass="SelectedRow"
                                                                                        SortAscendingImageUrl="col-asc.png"
                                                                                        SortDescendingImageUrl="col-desc.png"
                                                                                        SortImageWidth="10"
                                                                                        SortImageHeight="19">
                                                                                        <ConditionalFormats>
                                                                                            <ComponentArt:GridConditionalFormat ClientFilter="((DataItem.get_index() + grdBrw.get_recordOffset()) % 2) > 0" RowCssClass="AltRow" SelectedRowCssClass="SelectedRow" />
                                                                                        </ConditionalFormats>
                                                                                        <Columns>
                                                                                            <ComponentArt:GridColumn DataField="group_id" Align="left" HeadingText="group_id" AllowGrouping="false" Visible="false" />
                                                                                            <ComponentArt:GridColumn DataField="group_name" Align="left" HeadingText="Group Name" AllowGrouping="false" Width="950" />
                                                                                        </Columns>
                                                                                    </ComponentArt:GridLevel>
                                                                                    <ComponentArt:GridLevel
                                                                                        AllowGrouping="false"
                                                                                        DataMember="sys_settings"
                                                                                        DataKeyField="control_code"
                                                                                        ShowTableHeading="false"
                                                                                        TableHeadingCssClass="GridHeader"
                                                                                        RowCssClass="Row"
                                                                                        HoverRowCssClass="HoverRow"
                                                                                        ColumnReorderIndicatorImageUrl="reorder.gif"
                                                                                        DataCellCssClass="DataCell"
                                                                                        HeadingCellCssClass="HeadingCell"
                                                                                        HeadingRowCssClass="HeadingRow"
                                                                                        HeadingTextCssClass="HeadingCellText"
                                                                                        SortedDataCellCssClass="SortedDataCell"
                                                                                        SelectedRowCssClass="SelectedRow"
                                                                                        SortAscendingImageUrl="col-asc.png"
                                                                                        SortDescendingImageUrl="col-desc.png"
                                                                                        SortImageWidth="10"
                                                                                        SortImageHeight="19">
                                                                                        <ConditionalFormats>
                                                                                            <ComponentArt:GridConditionalFormat ClientFilter="((DataItem.get_index() + grdBrw.get_recordOffset()) % 2) > 0" RowCssClass="AltRow" SelectedRowCssClass="SelectedRow" />
                                                                                        </ConditionalFormats>
                                                                                        <Columns>
                                                                                            <ComponentArt:GridColumn DataField="control_code" Align="left" HeadingText="control_code" AllowGrouping="false" Visible="false" DataCellCssClass="cell-row-update" />
                                                                                            <ComponentArt:GridColumn DataField="group_id" Align="left" HeadingText="group_id" AllowGrouping="false" Visible="false" />
                                                                                            <ComponentArt:GridColumn DataField="control_desc" Align="left" HeadingText="Description" AllowGrouping="false" Width="350" DataCellClientTemplateId="CONTROLDESC" FixedWidth="true" />
                                                                                            <ComponentArt:GridColumn DataField="data_type" Align="left" HeadingText="data_type" AllowGrouping="false" Visible="false" />
                                                                                            <ComponentArt:GridColumn DataField="data_type_string" Align="left" HeadingText="Value (string)" DataCellCssClass="cell-row-update" DataCellClientTemplateId="data_type_str" AllowGrouping="false" Width="250" />
                                                                                            <ComponentArt:GridColumn DataField="data_type_number" Align="right" HeadingText="Value (number)" DataCellClientTemplateId="data_type_number" AllowGrouping="false" Width="150" FixedWidth="true" AllowSorting="False" />
                                                                                            <ComponentArt:GridColumn DataField="data_type_decimal" Align="right" HeadingText="Value (decimal)" DataCellClientTemplateId="data_type_decimal" AllowGrouping="false" Width="150" FormatString="#0.00" FixedWidth="true" AllowSorting="False" />
                                                                                            <ComponentArt:GridColumn DataField="is_password" Align="left" HeadingText="is_password" AllowGrouping="false" Visible="false" />
                                                                                            <ComponentArt:GridColumn DataField="ui_control" Align="left" HeadingText="ui_control" AllowGrouping="false" Visible="false" />
                                                                                            <ComponentArt:GridColumn DataField="ui_value_list" Align="left" HeadingText="ui_value_list" AllowGrouping="false" Visible="false" />
                                                                                        </Columns>
                                                                                    </ComponentArt:GridLevel>
                                                                                </Levels>
                                                                                <ClientEvents>
                                                                                    <RenderComplete EventHandler="grdBrw_onRenderComplete" />
                                                                                    <ItemExpand EventHandler="grdBrw_onItemExpand" />
                                                                                    <ItemCollapse EventHandler="grdBrw_onItemCollapse" />
                                                                                </ClientEvents>
                                                                                <ClientTemplates>
                                                                                    <ComponentArt:ClientTemplate ID="CONTROLDESC">
                                                                                        <span id="spnDesc_## DataItem.GetMember('control_code').Value ##" title="## DataItem.GetMember('control_desc').Value ##">## DataItem.GetMember('control_desc').Value ##</span>
                                                                                    </ComponentArt:ClientTemplate>
                                                                                    <ComponentArt:ClientTemplate ID="data_type_str">
                                                                                        <input type="text" id="txtDataTypeStr_## DataItem.GetMember('control_code').Value ##" class="GridTextBox"
                                                                                            value="## DataItem.GetMember('data_type_string').Value ##" style="width: 100%; padding-left: 5px;"
                                                                                            onchange="javascript:txtDataTypeStr_OnChange('## DataItem.GetMember('control_code').Value ##','## DataItem.GetMember('group_id').Value ##','## DataItem.GetMember('data_type').Value ##');" />

                                                                                        <select id="ddlDataTypeStr_## DataItem.GetMember('control_code').Value ##" class="form-control custom-select-value" style="width: 99%; display: none;" onchange="javascript:ddlDataTypeStr_OnChange('## DataItem.GetMember('control_code').Value ##','## DataItem.GetMember('group_id').Value ##','## DataItem.GetMember('data_type').Value ##');">
                                                                                        </select>
                                                                                    </ComponentArt:ClientTemplate>
                                                                                    <ComponentArt:ClientTemplate ID="data_type_number">
                                                                                        <input type="text" id="txtDataTypeNumber_## DataItem.GetMember('control_code').Value ##" class="GridTextBox"
                                                                                            value="## DataItem.GetMember('data_type_number').Value ##" style="width: 100%; text-align: right;"
                                                                                            onchange="javascript:txtDataTypeNumber_OnChange('## DataItem.GetMember('control_code').Value ##','## DataItem.GetMember('group_id').Value ##','## DataItem.GetMember('data_type').Value ##');"
                                                                                            onkeypress="javascript:return parent.CheckInteger(event);"
                                                                                            onblur="javascript:ResetValueInteger(this,'0');"
                                                                                            onfocus="jvascript:this.select();" />
                                                                                        <select id="ddlDataTypeNumber_## DataItem.GetMember('control_code').Value ##" class="form-control custom-select-value" style="width: 99%; display: none;" onchange="javascript:ddlDataTypeNumber_OnChange('## DataItem.GetMember('control_code').Value ##','## DataItem.GetMember('group_id').Value ##','## DataItem.GetMember('data_type').Value ##');">
                                                                                    </ComponentArt:ClientTemplate>
                                                                                    <ComponentArt:ClientTemplate ID="data_type_decimal">
                                                                                        <input type="text" id="txtDataTypeDecimal_## DataItem.GetMember('control_code').Value ##" class="GridTextBox"
                                                                                            value="## DataItem.GetMember('data_type_decimal').Value ##" style="width: 100%; text-align: right;"
                                                                                            onchange="javascript:txtDataTypeDecimal_OnChange('## DataItem.GetMember('control_code').Value ##','## DataItem.GetMember('group_id').Value ##','## DataItem.GetMember('data_type').Value ##');"
                                                                                            onkeypress="javascript:return parent.CheckDecimal(event);"
                                                                                            onblur="javascript:ResetValueDecimal(this);"
                                                                                            onfocus="jvascript:this.select();" />
                                                                                    </ComponentArt:ClientTemplate>

                                                                                </ClientTemplates>
                                                                            </ComponentArt:Grid>
                                                                            <span id="spnERR" runat="server"></span>
                                                                        </Content>
                                                                        <LoadingPanelClientTemplate>
                                                                            <table style="height: 400px; width: 100%;" border="0">
                                                                                <tr>
                                                                                    <td style="text-align: center;">
                                                                                        <table border="0" style="width: 70px; display: inline-block;">
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <img src="../images/Searching.gif" border="0" alt="" />
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </LoadingPanelClientTemplate>
                                                                        <ClientEvents>
                                                                            <CallbackComplete EventHandler="grdBrw_onCallbackComplete" />
                                                                        </ClientEvents>
                                                                    </ComponentArt:CallBack>


                                                                </div>
                                                            </div>
                                                        </div>

                                                    </div>
                                                </div>

                                                <div class="row">

                                                    <div class="col-sm-12 col-xs-12 text-right" style="margin-left: -5px;">

                                                        <button type="button" id="btnSave2" runat="server" class="btn btn-custon-four btn-success">
                                                            <i class="fa fa-floppy-o" aria-hidden="true"></i>&nbsp;Save</button>
                                                    </div>
                                                </div>

                                            </div>
                                        </ComponentArt:PageView>
                                        <ComponentArt:PageView ID="OT" runat="server" Width="100%">
                                            <div class="col-sm-12 col-xs-12">
                                                <div class="row">

                                                    <div class="col-sm-12 col-xs-12 text-right" style="margin-left: -5px;">
                                                        <button type="button" id="btnSaveOT1" runat="server" class="btn btn-custon-four btn-success">
                                                            <i class="fa fa-floppy-o" aria-hidden="true"></i>&nbsp;Save
                                                        </button>
                                                    </div>
                                                </div>

                                                <div class="sparkline10-list mt-b-10">
                                                    <div class="searchSection">

                                                        <div class="sparkline10-graph">
                                                            <div class="static-table-list">
                                                                <div class="table-responsive">
                                                                    <ComponentArt:CallBack ID="CallBackOT" runat="server" OnCallback="CallBackOT_Callback">
                                                                        <Content>
                                                                            <ComponentArt:Grid
                                                                                ID="grdOT"
                                                                                CssClass="Grid"
                                                                                AutoTheming="true"
                                                                                DataAreaCssClass=""
                                                                                SearchOnKeyPress="true"
                                                                                EnableViewState="true"
                                                                                RunningMode="Client"
                                                                                ShowSearchBox="false"
                                                                                SearchBoxPosition="TopLeft"
                                                                                SearchTextCssClass="GridHeaderText" PageSize="200"
                                                                                ShowHeader="false"
                                                                                FooterCssClass="GridFooter"
                                                                                GroupingNotificationText=""
                                                                                ScrollBar="Off"
                                                                                ScrollTopBottomImagesEnabled="true"
                                                                                ScrollTopBottomImageHeight="2"
                                                                                ScrollTopBottomImageWidth="16"
                                                                                ScrollImagesFolderUrl="../images/scroller/"
                                                                                ScrollButtonWidth="16"
                                                                                ScrollButtonHeight="17"
                                                                                ShowFooter="false"
                                                                                ScrollBarCssClass="ScrollBar"
                                                                                ScrollGripCssClass="ScrollGrip"
                                                                                ScrollBarWidth="16"
                                                                                PagerTextCssClass="GridFooterText"
                                                                                PagerButtonWidth="24"
                                                                                PagerButtonHeight="24"
                                                                                PagerButtonHoverEnabled="true"
                                                                                ImagesBaseUrl="../images/"
                                                                                LoadingPanelFadeDuration="1000"
                                                                                LoadingPanelFadeMaximumOpacity="80"
                                                                                LoadingPanelClientTemplateId="LoadingFeedbackTemplate"
                                                                                LoadingPanelPosition="MiddleCenter"
                                                                                Width="99%"
                                                                                runat="server"
                                                                                HeaderCssClass="GridHeader"
                                                                                GroupingNotificationPosition="TopRight"
                                                                                SearchBoxCssClass="EditTextBoxStyle">
                                                                                <Levels>

                                                                                    <ComponentArt:GridLevel
                                                                                        AllowGrouping="false"
                                                                                        DataKeyField="day_no"
                                                                                        ShowTableHeading="false"
                                                                                        TableHeadingCssClass="GridHeader"
                                                                                        RowCssClass="Row"
                                                                                        HoverRowCssClass="HoverRow"
                                                                                        ColumnReorderIndicatorImageUrl="reorder.gif"
                                                                                        DataCellCssClass="DataCell"
                                                                                        HeadingCellCssClass="HeadingCell"
                                                                                        HeadingRowCssClass="HeadingRow"
                                                                                        HeadingTextCssClass="HeadingCellText"
                                                                                        SortedDataCellCssClass="SortedDataCell"
                                                                                        SelectedRowCssClass=""
                                                                                        SortAscendingImageUrl="col-asc.png"
                                                                                        SortDescendingImageUrl="col-desc.png"
                                                                                        SortImageWidth="10"
                                                                                        SortImageHeight="19">
                                                                                        <ConditionalFormats>
                                                                                            <ComponentArt:GridConditionalFormat ClientFilter="((DataItem.get_index() + grdOT.get_recordOffset()) % 2) > 0" RowCssClass="AltRow" SelectedRowCssClass="AltRow" />
                                                                                            <ComponentArt:GridConditionalFormat ClientFilter="((DataItem.get_index() + grdOT.get_recordOffset()) % 2) == 0" RowCssClass="Row" SelectedRowCssClass="Row" />
                                                                                        </ConditionalFormats>
                                                                                        <Columns>
                                                                                            <ComponentArt:GridColumn DataField="day_no" Align="left" HeadingText="Day #" AllowGrouping="false" Width="50" AllowSorting="False" />
                                                                                            <ComponentArt:GridColumn DataField="day_name" Align="left" HeadingText="Day" AllowGrouping="false" Width="150" AllowSorting="False" />
                                                                                            <ComponentArt:GridColumn DataField="from_time" Align="left" HeadingText="From (HH:mm)" DataCellClientTemplateId="FROM" AllowGrouping="false" Width="100" AllowSorting="False" />
                                                                                            <ComponentArt:GridColumn DataField="till_time" Align="left" HeadingText="Till (HH:mm)" DataCellClientTemplateId="TILL" AllowGrouping="false" Width="100" FixedWidth="true" AllowSorting="False" />
                                                                                            <ComponentArt:GridColumn DataField="time_zone_id" Align="left" HeadingText="Time Zone" DataCellClientTemplateId="TZONE" AllowGrouping="false" Width="180" FixedWidth="true" AllowSorting="False" />
                                                                                            <ComponentArt:GridColumn DataField="message_display" Align="center" HeadingText="Message To Display" DataCellClientTemplateId="MSG" AllowGrouping="false" Width="130" FixedWidth="true" AllowSorting="False" />
                                                                                        </Columns>
                                                                                    </ComponentArt:GridLevel>
                                                                                </Levels>
                                                                                <ClientEvents>
                                                                                    <RenderComplete EventHandler="grdOT_onRenderComplete" />
                                                                                </ClientEvents>
                                                                                <ClientTemplates>
                                                                                    <ComponentArt:ClientTemplate ID="FROM">
                                                                                        <input type="text" id="txtFromTime_## DataItem.GetMember('day_no').Value ##" class="GridTextBox"
                                                                                            value="## DataItem.GetMember('from_time').Value ##" style="width: 60%; text-align: center;" maxlength="5"
                                                                                            onkeypress="javascript:return parent.CheckTime(event);" onchange="javascript:parent.GsRetStatus = 'true';" onblur="javascript:txtFromTime_OnBlur('## DataItem.GetMember('day_no').Value ##');" onfocus="javascript:this.select();" />
                                                                                    </ComponentArt:ClientTemplate>
                                                                                    <ComponentArt:ClientTemplate ID="TILL">
                                                                                        <input type="text" id="txtTillTime_## DataItem.GetMember('day_no').Value ##" class="GridTextBox"
                                                                                            value="## DataItem.GetMember('till_time').Value ##" style="width: 60%; text-align: center;" maxlength="5"
                                                                                            onkeypress="javascript:return parent.CheckTime(event);" onchange="javascript:parent.GsRetStatus = 'true';" onblur="javascript:txtTillTime_OnBlur('## DataItem.GetMember('day_no').Value ##');" onfocus="javascript:this.select();" />
                                                                                    </ComponentArt:ClientTemplate>
                                                                                    <ComponentArt:ClientTemplate ID="TZONE">
                                                                                        <select id="ddlTZ_## DataItem.GetMember('day_no').Value ##" class="form-control custom-select-value" style="width: 99%;" onchange="javascript:ddlTZ_OnChange('## DataItem.GetMember('day_no').Value ##');">
                                                                                        </select>
                                                                                    </ComponentArt:ClientTemplate>
                                                                                    <ComponentArt:ClientTemplate ID="MSG">
                                                                                        <button type="button" id="btnMsg_## DataItem.GetMember('day_no').Value ##" class="btn btn-warning btn_grd" title="click to view/update the message to display" onclick="javascript:btnMsg_OnClick('## DataItem.GetMember('day_no').Value ##');">
                                                                                            <i class="fa fa-exclamation-triangle" aria-hidden="true"></i>
                                                                                        </button>
                                                                                    </ComponentArt:ClientTemplate>
                                                                                </ClientTemplates>
                                                                            </ComponentArt:Grid>
                                                                            <span id="spnOTERR" runat="server"></span>
                                                                        </Content>
                                                                        <LoadingPanelClientTemplate>
                                                                            <table style="height: 400px; width: 100%;" border="0">
                                                                                <tr>
                                                                                    <td style="text-align: center;">
                                                                                        <table border="0" style="width: 70px; display: inline-block;">
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <img src="../images/Searching.gif" border="0" alt="" />
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </LoadingPanelClientTemplate>
                                                                        <ClientEvents>
                                                                            <CallbackComplete EventHandler="grdOT_onCallbackComplete" />
                                                                        </ClientEvents>
                                                                    </ComponentArt:CallBack>


                                                                </div>
                                                            </div>
                                                        </div>

                                                    </div>
                                                </div>

                                                <div class="row">

                                                    <div class="col-sm-12 col-xs-12 text-right" style="margin-left: -5px;">

                                                        <button type="button" id="btnSaveOT2" runat="server" class="btn btn-custon-four btn-success">
                                                            <i class="fa fa-floppy-o" aria-hidden="true"></i>&nbsp;Save</button>
                                                    </div>
                                                </div>
                                            </div>
                                        </ComponentArt:PageView>
                                        <ComponentArt:PageView ID="SANH" runat="server" Width="100%">
                                            <div class="col-sm-12 col-xs-12">

                                                <div class="row">

                                                    <div class="col-sm-12 col-xs-12 text-right" style="margin-left: -5px;">
                                                        <button type="button" id="btnSaveSMA1" runat="server" class="btn btn-custon-four btn-success">
                                                            <i class="fa fa-floppy-o" aria-hidden="true"></i>&nbsp;Save
                                                        </button>
                                                    </div>
                                                </div>


                                                <div class="sparkline10-list mt-b-10">
                                                    <div class="row">
                                                        <div class="col-sm-6 col-xs-12">
                                                            <div class="searchSection">
                                                                <div class="row">
                                                                    <div class="col-sm-12 col-xs-12">
                                                                        <div class="pull-left">
                                                                            <h3 class="h3Text">
                                                                                Species Wise
                                                                                <span style="font-size: 10px;">(Validation priority level 1)</span>
                                                                            </h3>
                                                                        </div>
                                                                        <div class="borderSearch pull-left"></div>
                                                                    </div>

                                                                </div>
                                                                <div class="sparkline10-graph">
                                                                    <div class="static-table-list">
                                                                        <div class="table-responsive">
                                                                            <ComponentArt:CallBack ID="CallBackSSA" runat="server" OnCallback="CallBackSSA_Callback">
                                                                                <Content>
                                                                                    <ComponentArt:Grid
                                                                                        ID="grdServiceSpc"
                                                                                        CssClass="Grid"
                                                                                        AutoTheming="true"
                                                                                        DataAreaCssClass=""
                                                                                        SearchOnKeyPress="true"
                                                                                        EnableViewState="true"
                                                                                        RunningMode="Client"
                                                                                        ShowSearchBox="false"
                                                                                        SearchBoxPosition="TopLeft"
                                                                                        SearchTextCssClass="GridHeaderText" PageSize="200"
                                                                                        ShowHeader="false"
                                                                                        FooterCssClass="GridFooter"
                                                                                        GroupingNotificationText=""
                                                                                        ScrollBar="Off"
                                                                                        ScrollTopBottomImagesEnabled="true"
                                                                                        ScrollTopBottomImageHeight="2"
                                                                                        ScrollTopBottomImageWidth="16"
                                                                                        ScrollImagesFolderUrl="../images/scroller/"
                                                                                        ScrollButtonWidth="16"
                                                                                        ScrollButtonHeight="17"
                                                                                        ShowFooter="false"
                                                                                        ScrollBarCssClass="ScrollBar"
                                                                                        ScrollGripCssClass="ScrollGrip"
                                                                                        ScrollBarWidth="16"
                                                                                        PagerTextCssClass="GridFooterText"
                                                                                        PagerButtonWidth="24"
                                                                                        PagerButtonHeight="24"
                                                                                        PagerButtonHoverEnabled="true"
                                                                                        ImagesBaseUrl="../images/"
                                                                                        LoadingPanelFadeDuration="1000"
                                                                                        LoadingPanelFadeMaximumOpacity="80"
                                                                                        LoadingPanelClientTemplateId="LoadingFeedbackTemplate"
                                                                                        LoadingPanelPosition="MiddleCenter"
                                                                                        Width="99%"
                                                                                        runat="server"
                                                                                        HeaderCssClass="GridHeader"
                                                                                        GroupingNotificationPosition="TopRight"
                                                                                        SearchBoxCssClass="EditTextBoxStyle">
                                                                                        <Levels>
                                                                                            <ComponentArt:GridLevel AllowGrouping="false"
                                                                                                DataMember="Service"
                                                                                                DataKeyField="id"
                                                                                                ShowTableHeading="false"
                                                                                                TableHeadingCssClass="GridHeader"
                                                                                                RowCssClass="Row"
                                                                                                HoverRowCssClass="HoverRow"
                                                                                                ColumnReorderIndicatorImageUrl="reorder.gif"
                                                                                                DataCellCssClass="DataCell"
                                                                                                HeadingCellCssClass="HeadingCell"
                                                                                                HeadingRowCssClass="HeadingRow"
                                                                                                HeadingTextCssClass="HeadingCellText"
                                                                                                EditCellCssClass="active"
                                                                                                SortedDataCellCssClass="SortedDataCell"
                                                                                                SelectedRowCssClass="SelectedRow"
                                                                                                SortAscendingImageUrl="col-asc.png"
                                                                                                SortDescendingImageUrl="col-desc.png"
                                                                                                SortImageWidth="10"
                                                                                                SortImageHeight="19">
                                                                                                <ConditionalFormats>
                                                                                                    <ComponentArt:GridConditionalFormat ClientFilter="((DataItem.get_index() + grdServiceSpc.get_recordOffset()) % 2) > 0" RowCssClass="AltRow" SelectedRowCssClass="SelectedRow" />
                                                                                                </ConditionalFormats>
                                                                                                <Columns>
                                                                                                    <ComponentArt:GridColumn DataField="id" Align="left" HeadingText="id" AllowGrouping="false" Visible="false" />
                                                                                                    <ComponentArt:GridColumn DataField="name" Align="left" HeadingText="Service" AllowGrouping="false" Width="500" />
                                                                                                </Columns>
                                                                                            </ComponentArt:GridLevel>
                                                                                            <ComponentArt:GridLevel
                                                                                                AllowGrouping="false"
                                                                                                DataMember="Species"
                                                                                                DataKeyField="rec_id"
                                                                                                ShowTableHeading="false"
                                                                                                TableHeadingCssClass="GridHeader"
                                                                                                RowCssClass="Row"
                                                                                                HoverRowCssClass="HoverRow"
                                                                                                ColumnReorderIndicatorImageUrl="reorder.gif"
                                                                                                DataCellCssClass="DataCell"
                                                                                                HeadingCellCssClass="HeadingCell"
                                                                                                HeadingRowCssClass="HeadingRow"
                                                                                                HeadingTextCssClass="HeadingCellText"
                                                                                                SortedDataCellCssClass="SortedDataCell"
                                                                                                SelectedRowCssClass="SelectedRow"
                                                                                                SortAscendingImageUrl="col-asc.png"
                                                                                                SortDescendingImageUrl="col-desc.png"
                                                                                                SortImageWidth="10"
                                                                                                SortImageHeight="19">
                                                                                                <ConditionalFormats>
                                                                                                    <ComponentArt:GridConditionalFormat ClientFilter="((DataItem.get_index() + grdServiceSpc.get_recordOffset()) % 2) > 0" RowCssClass="AltRow" SelectedRowCssClass="SelectedRow" />
                                                                                                </ConditionalFormats>
                                                                                                <Columns>
                                                                                                    <ComponentArt:GridColumn DataField="rec_id" Align="left" HeadingText="rec_id" AllowGrouping="false" Visible="false" DataCellCssClass="cell-row-update" />
                                                                                                    <ComponentArt:GridColumn DataField="service_id" Align="left" HeadingText="service_id" AllowGrouping="false" Visible="false" />
                                                                                                    <ComponentArt:GridColumn DataField="species_id" Align="left" HeadingText="species_id" AllowGrouping="false" Visible="false" />
                                                                                                    <ComponentArt:GridColumn DataField="species_name" Align="left" HeadingText="Species" AllowGrouping="false" Width="200" />
                                                                                                    <ComponentArt:GridColumn DataField="available" Align="left" HeadingText="Available ?" DataCellClientTemplateId="SVCSPCAVAILABLE" AllowGrouping="false" Width="70" />
                                                                                                    <ComponentArt:GridColumn DataField="message_display" Align="center" HeadingText="Message To Display" DataCellClientTemplateId="MSGSVCSPC" AllowGrouping="false" Width="130" FixedWidth="true" AllowSorting="False" />
                                                                                                    <ComponentArt:GridColumn Align="center" HeadingText="Except Institution" DataCellClientTemplateId="EXINSTSPC" AllowGrouping="false" Width="100" FixedWidth="true" AllowSorting="False" />
                                                                                                </Columns>
                                                                                            </ComponentArt:GridLevel>
                                                                                        </Levels>
                                                                                        <ClientEvents>
                                                                                            <RenderComplete EventHandler="grdServiceSpc_onRenderComplete" />
                                                                                            <ItemExpand EventHandler="grdServiceSpc_onItemExpand" />
                                                                                            <ItemCollapse EventHandler="grdServiceSpc_onItemCollapse" />
                                                                                        </ClientEvents>
                                                                                        <ClientTemplates>
                                                                                            <ComponentArt:ClientTemplate ID="SVCSPCAVAILABLE">
                                                                                                <div class="grid_option optSwitch pull-left">
                                                                                                    <input type="checkbox" id="chkSvcSpcAvailable_## DataItem.GetMember('rec_id').Value ##" onclick="javascript: UpdateSpeciesAvailability('## DataItem.GetMember('service_id').Value ##','## DataItem.GetMember('rec_id').Value ##');" />
                                                                                                    <label for="chkSvcSpcAvailable_## DataItem.GetMember('rec_id').Value ##" class="label-default"></label>
                                                                                                </div>
                                                                                            </ComponentArt:ClientTemplate>
                                                                                            <ComponentArt:ClientTemplate ID="EXINSTSPC">
                                                                                                <button type="button" id="btnSASPCExInst_## DataItem.GetMember('rec_id').Value ##" class="btn btn-primary btn_grd" title="click to view/update the exception institution" onclick="javascript:btnSASPCExInst_OnClick('## DataItem.GetMember('service_id').Value ##','## DataItem.GetMember('species_id').Value ##','## DataItem.GetMember('rec_id').Value ##');">
                                                                                                    <i class="fa fa-hospital-o" aria-hidden="true"></i>
                                                                                                </button>
                                                                                            </ComponentArt:ClientTemplate>
                                                                                            <ComponentArt:ClientTemplate ID="MSGSVCSPC">
                                                                                                <button type="button" id="btnSASPCMsg_## DataItem.GetMember('rec_id').Value ##" class="btn btn-warning btn_grd" title="click to view/update the message to display" onclick="javascript:btnSASPCMsg_OnClick('## DataItem.GetMember('service_id').Value ##','## DataItem.GetMember('rec_id').Value ##');">
                                                                                                    <i class="fa fa-exclamation-triangle" aria-hidden="true"></i>
                                                                                                </button>
                                                                                            </ComponentArt:ClientTemplate>
                                                                                        </ClientTemplates>
                                                                                    </ComponentArt:Grid>
                                                                                    <span id="spnSASPCERR" runat="server"></span>
                                                                                </Content>
                                                                                <LoadingPanelClientTemplate>
                                                                                    <table style="height: 400px; width: 100%;" border="0">
                                                                                        <tr>
                                                                                            <td style="text-align: center;">
                                                                                                <table border="0" style="width: 70px; display: inline-block;">
                                                                                                    <tr>
                                                                                                        <td>
                                                                                                            <img src="../images/Searching.gif" border="0" alt="" />
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                </table>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </LoadingPanelClientTemplate>
                                                                                <ClientEvents>
                                                                                    <CallbackComplete EventHandler="grdServiceSpc_onCallbackComplete" />
                                                                                </ClientEvents>
                                                                            </ComponentArt:CallBack>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="col-sm-6 col-xs-12">
                                                            <div class="searchSection">
                                                                <div class="row">
                                                                    <div class="col-sm-12 col-xs-12">
                                                                        <div class="pull-left">
                                                                            <h3 class="h3Text">
                                                                                Modality Wise
                                                                                <span style="font-size: 10px;">(Validation priority level 2)</span>
                                                                            </h3>
                                                                        </div>
                                                                        <div class="borderSearch pull-left"></div>
                                                                    </div>

                                                                </div>
                                                                <div class="sparkline10-graph">
                                                                    <div class="static-table-list">
                                                                        <div class="table-responsive">
                                                                            <ComponentArt:CallBack ID="CallBackSMA" runat="server" OnCallback="CallBackSMA_Callback">
                                                                                <Content>
                                                                                    <ComponentArt:Grid
                                                                                        ID="grdService"
                                                                                        CssClass="Grid"
                                                                                        AutoTheming="true"
                                                                                        DataAreaCssClass=""
                                                                                        SearchOnKeyPress="true"
                                                                                        EnableViewState="true"
                                                                                        RunningMode="Client"
                                                                                        ShowSearchBox="false"
                                                                                        SearchBoxPosition="TopLeft"
                                                                                        SearchTextCssClass="GridHeaderText" PageSize="200"
                                                                                        ShowHeader="false"
                                                                                        FooterCssClass="GridFooter"
                                                                                        GroupingNotificationText=""
                                                                                        ScrollBar="Off"
                                                                                        ScrollTopBottomImagesEnabled="true"
                                                                                        ScrollTopBottomImageHeight="2"
                                                                                        ScrollTopBottomImageWidth="16"
                                                                                        ScrollImagesFolderUrl="../images/scroller/"
                                                                                        ScrollButtonWidth="16"
                                                                                        ScrollButtonHeight="17"
                                                                                        ShowFooter="false"
                                                                                        ScrollBarCssClass="ScrollBar"
                                                                                        ScrollGripCssClass="ScrollGrip"
                                                                                        ScrollBarWidth="16"
                                                                                        PagerTextCssClass="GridFooterText"
                                                                                        PagerButtonWidth="24"
                                                                                        PagerButtonHeight="24"
                                                                                        PagerButtonHoverEnabled="true"
                                                                                        ImagesBaseUrl="../images/"
                                                                                        LoadingPanelFadeDuration="1000"
                                                                                        LoadingPanelFadeMaximumOpacity="80"
                                                                                        LoadingPanelClientTemplateId="LoadingFeedbackTemplate"
                                                                                        LoadingPanelPosition="MiddleCenter"
                                                                                        Width="99%"
                                                                                        runat="server"
                                                                                        HeaderCssClass="GridHeader"
                                                                                        GroupingNotificationPosition="TopRight"
                                                                                        SearchBoxCssClass="EditTextBoxStyle">
                                                                                        <Levels>
                                                                                            <ComponentArt:GridLevel AllowGrouping="false"
                                                                                                DataMember="Service"
                                                                                                DataKeyField="id"
                                                                                                ShowTableHeading="false"
                                                                                                TableHeadingCssClass="GridHeader"
                                                                                                RowCssClass="Row"
                                                                                                HoverRowCssClass="HoverRow"
                                                                                                ColumnReorderIndicatorImageUrl="reorder.gif"
                                                                                                DataCellCssClass="DataCell"
                                                                                                HeadingCellCssClass="HeadingCell"
                                                                                                HeadingRowCssClass="HeadingRow"
                                                                                                HeadingTextCssClass="HeadingCellText"
                                                                                                EditCellCssClass="active"
                                                                                                SortedDataCellCssClass="SortedDataCell"
                                                                                                SelectedRowCssClass="SelectedRow"
                                                                                                SortAscendingImageUrl="col-asc.png"
                                                                                                SortDescendingImageUrl="col-desc.png"
                                                                                                SortImageWidth="10"
                                                                                                SortImageHeight="19">
                                                                                                <ConditionalFormats>
                                                                                                    <ComponentArt:GridConditionalFormat ClientFilter="((DataItem.get_index() + grdService.get_recordOffset()) % 2) > 0" RowCssClass="AltRow" SelectedRowCssClass="SelectedRow" />
                                                                                                </ConditionalFormats>
                                                                                                <Columns>
                                                                                                    <ComponentArt:GridColumn DataField="id" Align="left" HeadingText="id" AllowGrouping="false" Visible="false" />
                                                                                                    <ComponentArt:GridColumn DataField="name" Align="left" HeadingText="Service" AllowGrouping="false" Width="500" />
                                                                                                </Columns>
                                                                                            </ComponentArt:GridLevel>
                                                                                            <ComponentArt:GridLevel
                                                                                                AllowGrouping="false"
                                                                                                DataMember="Modality"
                                                                                                DataKeyField="rec_id"
                                                                                                ShowTableHeading="false"
                                                                                                TableHeadingCssClass="GridHeader"
                                                                                                RowCssClass="Row"
                                                                                                HoverRowCssClass="HoverRow"
                                                                                                ColumnReorderIndicatorImageUrl="reorder.gif"
                                                                                                DataCellCssClass="DataCell"
                                                                                                HeadingCellCssClass="HeadingCell"
                                                                                                HeadingRowCssClass="HeadingRow"
                                                                                                HeadingTextCssClass="HeadingCellText"
                                                                                                SortedDataCellCssClass="SortedDataCell"
                                                                                                SelectedRowCssClass="SelectedRow"
                                                                                                SortAscendingImageUrl="col-asc.png"
                                                                                                SortDescendingImageUrl="col-desc.png"
                                                                                                SortImageWidth="10"
                                                                                                SortImageHeight="19">
                                                                                                <ConditionalFormats>
                                                                                                    <ComponentArt:GridConditionalFormat ClientFilter="((DataItem.get_index() + grdBrw.get_recordOffset()) % 2) > 0" RowCssClass="AltRow" SelectedRowCssClass="SelectedRow" />
                                                                                                </ConditionalFormats>
                                                                                                <Columns>
                                                                                                    <ComponentArt:GridColumn DataField="rec_id" Align="left" HeadingText="rec_id" AllowGrouping="false" Visible="false" DataCellCssClass="cell-row-update" />
                                                                                                    <ComponentArt:GridColumn DataField="service_id" Align="left" HeadingText="service_id" AllowGrouping="false" Visible="false" />
                                                                                                    <ComponentArt:GridColumn DataField="modality_id" Align="left" HeadingText="modality_id" AllowGrouping="false" Visible="false" />
                                                                                                    <ComponentArt:GridColumn DataField="modality_name" Align="left" HeadingText="Modality" AllowGrouping="false" Width="200" />
                                                                                                    <ComponentArt:GridColumn DataField="available" Align="left" HeadingText="Available ?" DataCellClientTemplateId="SVCAVAILABLE" AllowGrouping="false" Width="70" />
                                                                                                    <ComponentArt:GridColumn DataField="message_display" Align="center" HeadingText="Message To Display" DataCellClientTemplateId="MSGSVC" AllowGrouping="false" Width="130" FixedWidth="true" AllowSorting="False" />
                                                                                                    <ComponentArt:GridColumn Align="center" HeadingText="Except Institution" DataCellClientTemplateId="EXINST" AllowGrouping="false" Width="100" FixedWidth="true" AllowSorting="False" />
                                                                                                </Columns>
                                                                                            </ComponentArt:GridLevel>
                                                                                        </Levels>
                                                                                        <ClientEvents>
                                                                                            <RenderComplete EventHandler="grdService_onRenderComplete" />
                                                                                            <ItemExpand EventHandler="grdService_onItemExpand" />
                                                                                            <ItemCollapse EventHandler="grdService_onItemCollapse" />
                                                                                        </ClientEvents>
                                                                                        <ClientTemplates>
                                                                                            <ComponentArt:ClientTemplate ID="SVCAVAILABLE">
                                                                                                <div class="grid_option optSwitch pull-left">
                                                                                                    <input type="checkbox" id="chkSvcAvailable_## DataItem.GetMember('rec_id').Value ##" onclick="javascript: UpdateAvailability('## DataItem.GetMember('service_id').Value ##','## DataItem.GetMember('rec_id').Value ##');" />
                                                                                                    <label for="chkSvcAvailable_## DataItem.GetMember('rec_id').Value ##" class="label-default"></label>
                                                                                                </div>
                                                                                            </ComponentArt:ClientTemplate>
                                                                                            <ComponentArt:ClientTemplate ID="EXINST">
                                                                                                <button type="button" id="btnSAExInst_## DataItem.GetMember('rec_id').Value ##" class="btn btn-primary btn_grd" title="click to view/update the exception institution" onclick="javascript:btnSAExInst_OnClick('## DataItem.GetMember('service_id').Value ##','## DataItem.GetMember('modality_id').Value ##','## DataItem.GetMember('rec_id').Value ##');">
                                                                                                    <i class="fa fa-hospital-o" aria-hidden="true"></i>
                                                                                                </button>
                                                                                            </ComponentArt:ClientTemplate>
                                                                                            <ComponentArt:ClientTemplate ID="MSGSVC">
                                                                                                <button type="button" id="btnSAMsg_## DataItem.GetMember('rec_id').Value ##" class="btn btn-warning btn_grd" title="click to view/update the message to display" onclick="javascript:btnSAMsg_OnClick('## DataItem.GetMember('service_id').Value ##','## DataItem.GetMember('rec_id').Value ##');">
                                                                                                    <i class="fa fa-exclamation-triangle" aria-hidden="true"></i>
                                                                                                </button>
                                                                                            </ComponentArt:ClientTemplate>
                                                                                        </ClientTemplates>
                                                                                    </ComponentArt:Grid>
                                                                                    <span id="spnSAERR" runat="server"></span>
                                                                                </Content>
                                                                                <LoadingPanelClientTemplate>
                                                                                    <table style="height: 400px; width: 100%;" border="0">
                                                                                        <tr>
                                                                                            <td style="text-align: center;">
                                                                                                <table border="0" style="width: 70px; display: inline-block;">
                                                                                                    <tr>
                                                                                                        <td>
                                                                                                            <img src="../images/Searching.gif" border="0" alt="" />
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                </table>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </LoadingPanelClientTemplate>
                                                                                <ClientEvents>
                                                                                    <CallbackComplete EventHandler="grdService_onCallbackComplete" />
                                                                                </ClientEvents>
                                                                            </ComponentArt:CallBack>
                                                                        </div>
                                                                    </div>
                                                                </div>

                                                            </div>
                                                        </div>


                                                        <div class="row">

                                                            <div class="col-sm-12 col-xs-12 text-right" style="margin-left: -5px;">

                                                                <button type="button" id="btnSaveSMA2" runat="server" class="btn btn-custon-four btn-success">
                                                                    <i class="fa fa-floppy-o" aria-hidden="true"></i>&nbsp;Save</button>
                                                            </div>
                                                        </div>

                                                    </div>
                                                </div>
                                            </div>
                                        </ComponentArt:PageView>
                                        <ComponentArt:PageView ID="SAAH" runat="server" Width="100%">
                                            <div class="col-sm-12 col-xs-12">

                                                <div class="row">

                                                    <div class="col-sm-12 col-xs-12 text-right" style="margin-left: -5px;">
                                                        <button type="button" id="btnSaveSAAH1" runat="server" class="btn btn-custon-four btn-success">
                                                            <i class="fa fa-floppy-o" aria-hidden="true"></i>&nbsp;Save
                                                        </button>
                                                    </div>
                                                </div>


                                                <div class="sparkline10-list mt-b-10">
                                                    <div class="row">
                                                        <div class="col-sm-6 col-xs-12">
                                                            <div class="searchSection">
                                                                <div class="row">
                                                                    <div class="col-sm-12 col-xs-12">
                                                                        <div class="pull-left">
                                                                            <h3 class="h3Text">
                                                                                Species Wise
                                                                                <span style="font-size: 10px;">(Validation priority level 1)</span>
                                                                            </h3>
                                                                        </div>
                                                                        <div class="borderSearch pull-left"></div>
                                                                    </div>

                                                                </div>
                                                                <div class="sparkline10-graph">
                                                                    <div class="static-table-list">
                                                                        <div class="table-responsive">
                                                                            <ComponentArt:CallBack ID="CallBackSASPAH" runat="server" OnCallback="CallBackSASPAH_Callback">
                                                                                <Content>
                                                                                    <ComponentArt:Grid
                                                                                        ID="grdSASPAH"
                                                                                        CssClass="Grid"
                                                                                        AutoTheming="true"
                                                                                        DataAreaCssClass=""
                                                                                        SearchOnKeyPress="true"
                                                                                        EnableViewState="true"
                                                                                        RunningMode="Client"
                                                                                        ShowSearchBox="false"
                                                                                        SearchBoxPosition="TopLeft"
                                                                                        SearchTextCssClass="GridHeaderText" PageSize="200"
                                                                                        ShowHeader="false"
                                                                                        FooterCssClass="GridFooter"
                                                                                        GroupingNotificationText=""
                                                                                        ScrollBar="Off"
                                                                                        ScrollTopBottomImagesEnabled="true"
                                                                                        ScrollTopBottomImageHeight="2"
                                                                                        ScrollTopBottomImageWidth="16"
                                                                                        ScrollImagesFolderUrl="../images/scroller/"
                                                                                        ScrollButtonWidth="16"
                                                                                        ScrollButtonHeight="17"
                                                                                        ShowFooter="false"
                                                                                        ScrollBarCssClass="ScrollBar"
                                                                                        ScrollGripCssClass="ScrollGrip"
                                                                                        ScrollBarWidth="16"
                                                                                        PagerTextCssClass="GridFooterText"
                                                                                        PagerButtonWidth="24"
                                                                                        PagerButtonHeight="24"
                                                                                        PagerButtonHoverEnabled="true"
                                                                                        ImagesBaseUrl="../images/"
                                                                                        LoadingPanelFadeDuration="1000"
                                                                                        LoadingPanelFadeMaximumOpacity="80"
                                                                                        LoadingPanelClientTemplateId="LoadingFeedbackTemplate"
                                                                                        LoadingPanelPosition="MiddleCenter"
                                                                                        Width="99%"
                                                                                        runat="server"
                                                                                        HeaderCssClass="GridHeader"
                                                                                        GroupingNotificationPosition="TopRight"
                                                                                        SearchBoxCssClass="EditTextBoxStyle">
                                                                                        <Levels>
                                                                                            <ComponentArt:GridLevel AllowGrouping="false"
                                                                                                DataMember="Service"
                                                                                                DataKeyField="id"
                                                                                                ShowTableHeading="false"
                                                                                                TableHeadingCssClass="GridHeader"
                                                                                                RowCssClass="Row"
                                                                                                HoverRowCssClass="HoverRow"
                                                                                                ColumnReorderIndicatorImageUrl="reorder.gif"
                                                                                                DataCellCssClass="DataCell"
                                                                                                HeadingCellCssClass="HeadingCell"
                                                                                                HeadingRowCssClass="HeadingRow"
                                                                                                HeadingTextCssClass="HeadingCellText"
                                                                                                EditCellCssClass="active"
                                                                                                SortedDataCellCssClass="SortedDataCell"
                                                                                                SelectedRowCssClass="SelectedRow"
                                                                                                SortAscendingImageUrl="col-asc.png"
                                                                                                SortDescendingImageUrl="col-desc.png"
                                                                                                SortImageWidth="10"
                                                                                                SortImageHeight="19">
                                                                                                <ConditionalFormats>
                                                                                                    <ComponentArt:GridConditionalFormat ClientFilter="((DataItem.get_index() + grdSASPAH.get_recordOffset()) % 2) > 0" RowCssClass="AltRow" SelectedRowCssClass="SelectedRow" />
                                                                                                </ConditionalFormats>
                                                                                                <Columns>
                                                                                                    <ComponentArt:GridColumn DataField="id" Align="left" HeadingText="id" AllowGrouping="false" Visible="false" />
                                                                                                    <ComponentArt:GridColumn DataField="name" Align="left" HeadingText="Service" AllowGrouping="false" Width="500" />
                                                                                                </Columns>
                                                                                            </ComponentArt:GridLevel>
                                                                                            <ComponentArt:GridLevel
                                                                                                AllowGrouping="false"
                                                                                                DataMember="Species"
                                                                                                DataKeyField="rec_id"
                                                                                                ShowTableHeading="false"
                                                                                                TableHeadingCssClass="GridHeader"
                                                                                                RowCssClass="Row"
                                                                                                HoverRowCssClass="HoverRow"
                                                                                                ColumnReorderIndicatorImageUrl="reorder.gif"
                                                                                                DataCellCssClass="DataCell"
                                                                                                HeadingCellCssClass="HeadingCell"
                                                                                                HeadingRowCssClass="HeadingRow"
                                                                                                HeadingTextCssClass="HeadingCellText"
                                                                                                SortedDataCellCssClass="SortedDataCell"
                                                                                                SelectedRowCssClass="SelectedRow"
                                                                                                SortAscendingImageUrl="col-asc.png"
                                                                                                SortDescendingImageUrl="col-desc.png"
                                                                                                SortImageWidth="10"
                                                                                                SortImageHeight="19">
                                                                                                <ConditionalFormats>
                                                                                                    <ComponentArt:GridConditionalFormat ClientFilter="((DataItem.get_index() + grdSASPAH.get_recordOffset()) % 2) > 0" RowCssClass="AltRow" SelectedRowCssClass="SelectedRow" />
                                                                                                </ConditionalFormats>
                                                                                                <Columns>
                                                                                                    <ComponentArt:GridColumn DataField="rec_id" Align="left" HeadingText="rec_id" AllowGrouping="false" Visible="false" DataCellCssClass="cell-row-update" />
                                                                                                    <ComponentArt:GridColumn DataField="service_id" Align="left" HeadingText="service_id" AllowGrouping="false" Visible="false" />
                                                                                                    <ComponentArt:GridColumn DataField="species_id" Align="left" HeadingText="species_id" AllowGrouping="false" Visible="false" />
                                                                                                    <ComponentArt:GridColumn DataField="species_name" Align="left" HeadingText="Species" AllowGrouping="false" Width="200" />
                                                                                                    <ComponentArt:GridColumn DataField="available" Align="left" HeadingText="Available ?" DataCellClientTemplateId="SVCAVBLSPAH" AllowGrouping="false" Width="70" />
                                                                                                    <ComponentArt:GridColumn DataField="message_display" Align="center" HeadingText="Message To Display" DataCellClientTemplateId="MSGSVCSPAH" AllowGrouping="false" Width="130" FixedWidth="true" AllowSorting="False" />
                                                                                                    <ComponentArt:GridColumn Align="center" HeadingText="Except Institution" DataCellClientTemplateId="EXINSTSPAH" AllowGrouping="false" Width="100" FixedWidth="true" AllowSorting="False" />
                                                                                                </Columns>
                                                                                            </ComponentArt:GridLevel>
                                                                                        </Levels>
                                                                                        <ClientEvents>
                                                                                            <RenderComplete EventHandler="grdSASPAH_onRenderComplete" />
                                                                                            <ItemExpand EventHandler="grdSASPAH_onItemExpand" />
                                                                                            <ItemCollapse EventHandler="grdSASPAH_onItemCollapse" />
                                                                                        </ClientEvents>
                                                                                        <ClientTemplates>
                                                                                            <ComponentArt:ClientTemplate ID="SVCAVBLSPAH">
                                                                                                <div class="grid_option optSwitch pull-left">
                                                                                                    <input type="checkbox" id="chkSASPAH_## DataItem.GetMember('rec_id').Value ##" onclick="javascript: UpdateSpeciesAfterHourAvailability('## DataItem.GetMember('service_id').Value ##','## DataItem.GetMember('rec_id').Value ##');" />
                                                                                                    <label for="chkSASPAH_## DataItem.GetMember('rec_id').Value ##" class="label-default"></label>
                                                                                                </div>
                                                                                            </ComponentArt:ClientTemplate>
                                                                                            <ComponentArt:ClientTemplate ID="EXINSTSPAH">
                                                                                                <button type="button" id="btnSASPExInstAH_## DataItem.GetMember('rec_id').Value ##" class="btn btn-primary btn_grd" title="click to view/update the exception institution" onclick="javascript:btnSASPExInstAH_OnClick('## DataItem.GetMember('service_id').Value ##','## DataItem.GetMember('species_id').Value ##','## DataItem.GetMember('rec_id').Value ##');">
                                                                                                    <i class="fa fa-hospital-o" aria-hidden="true"></i>
                                                                                                </button>
                                                                                            </ComponentArt:ClientTemplate>
                                                                                            <ComponentArt:ClientTemplate ID="MSGSVCSPAH">
                                                                                                <button type="button" id="btnSASPAHMsg_## DataItem.GetMember('rec_id').Value ##" class="btn btn-warning btn_grd" title="click to view/update the message to display" onclick="javascript:btnSASPAHMsg_OnClick('## DataItem.GetMember('service_id').Value ##','## DataItem.GetMember('rec_id').Value ##');">
                                                                                                    <i class="fa fa-exclamation-triangle" aria-hidden="true"></i>
                                                                                                </button>
                                                                                            </ComponentArt:ClientTemplate>
                                                                                        </ClientTemplates>
                                                                                    </ComponentArt:Grid>
                                                                                    <span id="spnSAAHSPERR" runat="server"></span>
                                                                                </Content>
                                                                                <LoadingPanelClientTemplate>
                                                                                    <table style="height: 400px; width: 100%;" border="0">
                                                                                        <tr>
                                                                                            <td style="text-align: center;">
                                                                                                <table border="0" style="width: 70px; display: inline-block;">
                                                                                                    <tr>
                                                                                                        <td>
                                                                                                            <img src="../images/Searching.gif" border="0" alt="" />
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                </table>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </LoadingPanelClientTemplate>
                                                                                <ClientEvents>
                                                                                    <CallbackComplete EventHandler="grdSASPAH_onCallbackComplete" />
                                                                                </ClientEvents>
                                                                            </ComponentArt:CallBack>
                                                                        </div>
                                                                    </div>
                                                                </div>

                                                            </div>
                                                        </div>
                                                        <div class="col-sm-6 col-xs-12">
                                                            <div class="searchSection">
                                                                <div class="row">
                                                                    <div class="col-sm-12 col-xs-12">
                                                                        <div class="pull-left">
                                                                            <h3 class="h3Text">
                                                                                Modality Wise
                                                                                <span style="font-size: 10px;">(Validation priority level 2)</span>
                                                                            </h3>
                                                                        </div>
                                                                        <div class="borderSearch pull-left"></div>
                                                                    </div>

                                                                </div>
                                                                <div class="sparkline10-graph">
                                                                    <div class="static-table-list">
                                                                        <div class="table-responsive">
                                                                            <ComponentArt:CallBack ID="CallBackSAAH" runat="server" OnCallback="CallBackSAAH_Callback">
                                                                                <Content>
                                                                                    <ComponentArt:Grid
                                                                                        ID="grdSAAH"
                                                                                        CssClass="Grid"
                                                                                        AutoTheming="true"
                                                                                        DataAreaCssClass=""
                                                                                        SearchOnKeyPress="true"
                                                                                        EnableViewState="true"
                                                                                        RunningMode="Client"
                                                                                        ShowSearchBox="false"
                                                                                        SearchBoxPosition="TopLeft"
                                                                                        SearchTextCssClass="GridHeaderText" PageSize="200"
                                                                                        ShowHeader="false"
                                                                                        FooterCssClass="GridFooter"
                                                                                        GroupingNotificationText=""
                                                                                        ScrollBar="Off"
                                                                                        ScrollTopBottomImagesEnabled="true"
                                                                                        ScrollTopBottomImageHeight="2"
                                                                                        ScrollTopBottomImageWidth="16"
                                                                                        ScrollImagesFolderUrl="../images/scroller/"
                                                                                        ScrollButtonWidth="16"
                                                                                        ScrollButtonHeight="17"
                                                                                        ShowFooter="false"
                                                                                        ScrollBarCssClass="ScrollBar"
                                                                                        ScrollGripCssClass="ScrollGrip"
                                                                                        ScrollBarWidth="16"
                                                                                        PagerTextCssClass="GridFooterText"
                                                                                        PagerButtonWidth="24"
                                                                                        PagerButtonHeight="24"
                                                                                        PagerButtonHoverEnabled="true"
                                                                                        ImagesBaseUrl="../images/"
                                                                                        LoadingPanelFadeDuration="1000"
                                                                                        LoadingPanelFadeMaximumOpacity="80"
                                                                                        LoadingPanelClientTemplateId="LoadingFeedbackTemplate"
                                                                                        LoadingPanelPosition="MiddleCenter"
                                                                                        Width="99%"
                                                                                        runat="server"
                                                                                        HeaderCssClass="GridHeader"
                                                                                        GroupingNotificationPosition="TopRight"
                                                                                        SearchBoxCssClass="EditTextBoxStyle">
                                                                                        <Levels>
                                                                                            <ComponentArt:GridLevel AllowGrouping="false"
                                                                                                DataMember="Service"
                                                                                                DataKeyField="id"
                                                                                                ShowTableHeading="false"
                                                                                                TableHeadingCssClass="GridHeader"
                                                                                                RowCssClass="Row"
                                                                                                HoverRowCssClass="HoverRow"
                                                                                                ColumnReorderIndicatorImageUrl="reorder.gif"
                                                                                                DataCellCssClass="DataCell"
                                                                                                HeadingCellCssClass="HeadingCell"
                                                                                                HeadingRowCssClass="HeadingRow"
                                                                                                HeadingTextCssClass="HeadingCellText"
                                                                                                EditCellCssClass="active"
                                                                                                SortedDataCellCssClass="SortedDataCell"
                                                                                                SelectedRowCssClass="SelectedRow"
                                                                                                SortAscendingImageUrl="col-asc.png"
                                                                                                SortDescendingImageUrl="col-desc.png"
                                                                                                SortImageWidth="10"
                                                                                                SortImageHeight="19">
                                                                                                <ConditionalFormats>
                                                                                                    <ComponentArt:GridConditionalFormat ClientFilter="((DataItem.get_index() + grdSAAH.get_recordOffset()) % 2) > 0" RowCssClass="AltRow" SelectedRowCssClass="SelectedRow" />
                                                                                                </ConditionalFormats>
                                                                                                <Columns>
                                                                                                    <ComponentArt:GridColumn DataField="id" Align="left" HeadingText="id" AllowGrouping="false" Visible="false" />
                                                                                                    <ComponentArt:GridColumn DataField="name" Align="left" HeadingText="Service" AllowGrouping="false" Width="500" />
                                                                                                </Columns>
                                                                                            </ComponentArt:GridLevel>
                                                                                            <ComponentArt:GridLevel
                                                                                                AllowGrouping="false"
                                                                                                DataMember="Modality"
                                                                                                DataKeyField="rec_id"
                                                                                                ShowTableHeading="false"
                                                                                                TableHeadingCssClass="GridHeader"
                                                                                                RowCssClass="Row"
                                                                                                HoverRowCssClass="HoverRow"
                                                                                                ColumnReorderIndicatorImageUrl="reorder.gif"
                                                                                                DataCellCssClass="DataCell"
                                                                                                HeadingCellCssClass="HeadingCell"
                                                                                                HeadingRowCssClass="HeadingRow"
                                                                                                HeadingTextCssClass="HeadingCellText"
                                                                                                SortedDataCellCssClass="SortedDataCell"
                                                                                                SelectedRowCssClass="SelectedRow"
                                                                                                SortAscendingImageUrl="col-asc.png"
                                                                                                SortDescendingImageUrl="col-desc.png"
                                                                                                SortImageWidth="10"
                                                                                                SortImageHeight="19">
                                                                                                <ConditionalFormats>
                                                                                                    <ComponentArt:GridConditionalFormat ClientFilter="((DataItem.get_index() + grdBrw.get_recordOffset()) % 2) > 0" RowCssClass="AltRow" SelectedRowCssClass="SelectedRow" />
                                                                                                </ConditionalFormats>
                                                                                                <Columns>
                                                                                                    <ComponentArt:GridColumn DataField="rec_id" Align="left" HeadingText="rec_id" AllowGrouping="false" Visible="false" DataCellCssClass="cell-row-update" />
                                                                                                    <ComponentArt:GridColumn DataField="service_id" Align="left" HeadingText="service_id" AllowGrouping="false" Visible="false" />
                                                                                                    <ComponentArt:GridColumn DataField="modality_id" Align="left" HeadingText="modality_id" AllowGrouping="false" Visible="false" />
                                                                                                    <ComponentArt:GridColumn DataField="modality_name" Align="left" HeadingText="Modality" AllowGrouping="false" Width="200" />
                                                                                                    <ComponentArt:GridColumn DataField="available" Align="left" HeadingText="Available ?" DataCellClientTemplateId="SVCAVBLAH" AllowGrouping="false" Width="70" />
                                                                                                    <ComponentArt:GridColumn DataField="message_display" Align="center" HeadingText="Message To Display" DataCellClientTemplateId="MSGSVCAH" AllowGrouping="false" Width="130" FixedWidth="true" AllowSorting="False" />
                                                                                                    <ComponentArt:GridColumn Align="center" HeadingText="Except Institution" DataCellClientTemplateId="EXINSTAH" AllowGrouping="false" Width="100" FixedWidth="true" AllowSorting="False" />
                                                                                                </Columns>
                                                                                            </ComponentArt:GridLevel>
                                                                                        </Levels>
                                                                                        <ClientEvents>
                                                                                            <RenderComplete EventHandler="grdSAAH_onRenderComplete" />
                                                                                            <ItemExpand EventHandler="grdSAAH_onItemExpand" />
                                                                                            <ItemCollapse EventHandler="grdSAAH_onItemCollapse" />
                                                                                        </ClientEvents>
                                                                                        <ClientTemplates>
                                                                                            <ComponentArt:ClientTemplate ID="SVCAVBLAH">
                                                                                                <div class="grid_option optSwitch pull-left">
                                                                                                    <input type="checkbox" id="chkSAAH_## DataItem.GetMember('rec_id').Value ##" onclick="javascript: UpdateAfterHourAvailability('## DataItem.GetMember('service_id').Value ##','## DataItem.GetMember('rec_id').Value ##');" />
                                                                                                    <label for="chkSAAH_## DataItem.GetMember('rec_id').Value ##" class="label-default"></label>
                                                                                                </div>
                                                                                            </ComponentArt:ClientTemplate>
                                                                                            <ComponentArt:ClientTemplate ID="EXINSTAH">
                                                                                                <button type="button" id="btnSAExInstAH_## DataItem.GetMember('rec_id').Value ##" class="btn btn-primary btn_grd" title="click to view/update the exception institution" onclick="javascript:btnSAExInstAH_OnClick('## DataItem.GetMember('service_id').Value ##','## DataItem.GetMember('modality_id').Value ##','## DataItem.GetMember('rec_id').Value ##');">
                                                                                                    <i class="fa fa-hospital-o" aria-hidden="true"></i>
                                                                                                </button>
                                                                                            </ComponentArt:ClientTemplate>
                                                                                            <ComponentArt:ClientTemplate ID="MSGSVCAH">
                                                                                                <button type="button" id="btnSAAHMsg_## DataItem.GetMember('rec_id').Value ##" class="btn btn-warning btn_grd" title="click to view/update the message to display" onclick="javascript:btnSAAHMsg_OnClick('## DataItem.GetMember('service_id').Value ##','## DataItem.GetMember('rec_id').Value ##');">
                                                                                                    <i class="fa fa-exclamation-triangle" aria-hidden="true"></i>
                                                                                                </button>
                                                                                            </ComponentArt:ClientTemplate>
                                                                                        </ClientTemplates>
                                                                                    </ComponentArt:Grid>
                                                                                    <span id="spnSAAHERR" runat="server"></span>
                                                                                </Content>
                                                                                <LoadingPanelClientTemplate>
                                                                                    <table style="height: 400px; width: 100%;" border="0">
                                                                                        <tr>
                                                                                            <td style="text-align: center;">
                                                                                                <table border="0" style="width: 70px; display: inline-block;">
                                                                                                    <tr>
                                                                                                        <td>
                                                                                                            <img src="../images/Searching.gif" border="0" alt="" />
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                </table>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </LoadingPanelClientTemplate>
                                                                                <ClientEvents>
                                                                                    <CallbackComplete EventHandler="grdSAAH_onCallbackComplete" />
                                                                                </ClientEvents>
                                                                            </ComponentArt:CallBack>
                                                                        </div>
                                                                    </div>
                                                                </div>

                                                            </div>
                                                        </div>
                                                        
                                                    </div>
                                                </div>

                                                <div class="row">

                                                    <div class="col-sm-12 col-xs-12 text-right" style="margin-left: -5px;">

                                                        <button type="button" id="btnSaveSAAH2" runat="server" class="btn btn-custon-four btn-success">
                                                            <i class="fa fa-floppy-o" aria-hidden="true"></i>&nbsp;Save</button>
                                                    </div>
                                                </div>

                                            </div>
                                        </ComponentArt:PageView>
                                        <ComponentArt:PageView ID="DASH" runat="server" Width="100%">
                                            <div class="col-sm-12 col-xs-12">
                                                <div class="row">
                                                    <div class="col-sm-12 col-xs-12 text-right" style="margin-left: -5px;">
                                                        <button type="button" id="btnSaveDashboard1" runat="server" class="btn btn-custon-four btn-success">
                                                            <i class="fa fa-floppy-o" aria-hidden="true"></i>&nbsp;Save
                                                        </button>
                                                    </div>
                                                </div>
                                                <div class="sparkline10-list mt-b-10">
                                                    <div class="searchSection">
                                                        <div class="sparkline10-graph">
                                                            <div class="static-table-list">
                                                                <div class="table-responsive">
                                                                    <ComponentArt:CallBack ID="CallBackDASH" runat="server" OnCallback="CallBackDASH_Callback">
                                                                        <Content>
                                                                            <ComponentArt:Grid
                                                                                ID="grdDASH"
                                                                                CssClass="Grid"
                                                                                AutoTheming="true"
                                                                                DataAreaCssClass=""
                                                                                SearchOnKeyPress="true"
                                                                                EnableViewState="true"
                                                                                RunningMode="Client"
                                                                                ShowSearchBox="false"
                                                                                SearchBoxPosition="TopLeft"
                                                                                SearchTextCssClass="GridHeaderText" PageSize="200"
                                                                                ShowHeader="false"
                                                                                FooterCssClass="GridFooter"
                                                                                GroupingNotificationText=""
                                                                                ScrollBar="Off"
                                                                                ScrollTopBottomImagesEnabled="true"
                                                                                ScrollTopBottomImageHeight="2"
                                                                                ScrollTopBottomImageWidth="16"
                                                                                ScrollImagesFolderUrl="../images/scroller/"
                                                                                ScrollButtonWidth="16"
                                                                                ScrollButtonHeight="17"
                                                                                ShowFooter="false"
                                                                                ScrollBarCssClass="ScrollBar"
                                                                                ScrollGripCssClass="ScrollGrip"
                                                                                ScrollBarWidth="16"
                                                                                PagerTextCssClass="GridFooterText"
                                                                                PagerButtonWidth="24"
                                                                                PagerButtonHeight="24"
                                                                                PagerButtonHoverEnabled="true"
                                                                                ImagesBaseUrl="../images/"
                                                                                LoadingPanelFadeDuration="1000"
                                                                                LoadingPanelFadeMaximumOpacity="80"
                                                                                LoadingPanelClientTemplateId="LoadingFeedbackTemplate"
                                                                                LoadingPanelPosition="MiddleCenter"
                                                                                Width="99%"
                                                                                runat="server"
                                                                                HeaderCssClass="GridHeader"
                                                                                GroupingNotificationPosition="TopRight"
                                                                                SearchBoxCssClass="EditTextBoxStyle">
                                                                                <Levels>
                                                                                    <ComponentArt:GridLevel AllowGrouping="false"
                                                                                        DataMember="parent_dashboard"
                                                                                        DataKeyField="id"
                                                                                        ShowTableHeading="false"
                                                                                        TableHeadingCssClass="GridHeader"
                                                                                        RowCssClass="Row"
                                                                                        HoverRowCssClass="HoverRow"
                                                                                        ColumnReorderIndicatorImageUrl="reorder.gif"
                                                                                        DataCellCssClass="DataCell"
                                                                                        HeadingCellCssClass="HeadingCell"
                                                                                        HeadingRowCssClass="HeadingRow"
                                                                                        HeadingTextCssClass="HeadingCellText"
                                                                                        EditCellCssClass="active"
                                                                                        SortedDataCellCssClass="SortedDataCell"
                                                                                        SelectedRowCssClass="SelectedRow"
                                                                                        SortAscendingImageUrl="col-asc.png"
                                                                                        SortDescendingImageUrl="col-desc.png"
                                                                                        SortImageWidth="10"
                                                                                        SortImageHeight="19">
                                                                                        <ConditionalFormats>
                                                                                            <ComponentArt:GridConditionalFormat ClientFilter="((DataItem.get_index() + grdDASH.get_recordOffset()) % 2) > 0" RowCssClass="AltRow" SelectedRowCssClass="SelectedRow" />
                                                                                        </ConditionalFormats>
                                                                                        <Columns>
                                                                                            <ComponentArt:GridColumn DataField="id" Align="left" HeadingText="id" AllowGrouping="false" Visible="false" />
                                                                                            <ComponentArt:GridColumn DataField="menu_desc" Align="left" HeadingText="Dashboard Group" AllowGrouping="false" Width="950" />
                                                                                        </Columns>
                                                                                    </ComponentArt:GridLevel>

                                                                                    <ComponentArt:GridLevel
                                                                                        AllowGrouping="false"
                                                                                        DataMember="dashboard_settings"
                                                                                        DataKeyField="parent_id"
                                                                                        ShowTableHeading="false"
                                                                                        TableHeadingCssClass="GridHeader"
                                                                                        RowCssClass="Row"
                                                                                        HoverRowCssClass="HoverRow"
                                                                                        ColumnReorderIndicatorImageUrl="reorder.gif"
                                                                                        DataCellCssClass="DataCell"
                                                                                        HeadingCellCssClass="HeadingCell"
                                                                                        HeadingRowCssClass="HeadingRow"
                                                                                        HeadingTextCssClass="HeadingCellText"
                                                                                        SortedDataCellCssClass="SortedDataCell"
                                                                                        SelectedRowCssClass="SelectedRow"
                                                                                        SortAscendingImageUrl="col-asc.png"
                                                                                        SortDescendingImageUrl="col-desc.png"
                                                                                        SortImageWidth="10"
                                                                                        SortImageHeight="19">
                                                                                        <ConditionalFormats>
                                                                                            <ComponentArt:GridConditionalFormat ClientFilter="((DataItem.get_index() + grdDASH.get_recordOffset()) % 2) > 0" RowCssClass="AltRow" SelectedRowCssClass="SelectedRow" />
                                                                                        </ConditionalFormats>
                                                                                        <Columns>
                                                                                            <ComponentArt:GridColumn DataField="id" Align="left" HeadingText="id" AllowGrouping="false" Visible="false" DataCellCssClass="cell-row-update" />
                                                                                            <ComponentArt:GridColumn DataField="parent_id" Align="left" HeadingText="parent_id" AllowGrouping="false" Visible="false" DataCellCssClass="cell-row-update" />
                                                                                            <ComponentArt:GridColumn DataField="menu_desc" Align="left" HeadingText="Menu Title" AllowGrouping="false" DataCellClientTemplateId="MENU_DESC" Width="120" />
                                                                                            <ComponentArt:GridColumn DataField="nav_url" Align="left" HeadingText="URL" AllowGrouping="false" Width="150" DataCellClientTemplateId="NAVURL" FixedWidth="true" />
                                                                                            <ComponentArt:GridColumn DataField="icon" Align="left" HeadingText="Icon" AllowGrouping="false" DataCellClientTemplateId="ICON" />
                                                                                            <ComponentArt:GridColumn DataField="display_index" Align="right" HeadingText="Display Order" DataCellClientTemplateId="DISPLAY_INDEX" AllowGrouping="false" Width="100" />
                                                                                            <ComponentArt:GridColumn DataField="refresh_time" Align="right" HeadingText="Refresh Time(Seconds)" DataCellClientTemplateId="REFRESH_TIME" AllowGrouping="false" Width="100" />
                                                                                            <ComponentArt:GridColumn DataField="is_enabled" Align="center" HeadingText="Active" DataCellClientTemplateId="IS_ENABLED" AllowGrouping="false" Width="80" FixedWidth="true" AllowSorting="False" />
                                                                                            <ComponentArt:GridColumn DataField="is_default" Align="center" HeadingText="Default" DataCellClientTemplateId="IS_DEFAULT" AllowGrouping="false" Width="80" FixedWidth="true" AllowSorting="False" />
                                                                                            <ComponentArt:GridColumn DataField="is_refresh_button" Align="center" HeadingText="Show Refresh" DataCellClientTemplateId="IS_REFRESH_BUTTON" AllowGrouping="false" Width="80" FixedWidth="true" AllowSorting="False" />
                                                                                            <ComponentArt:GridColumn DataField="title" Align="left" HeadingText="Report Title" AllowGrouping="false" DataCellClientTemplateId="TITLE_DESC" Width="150" />
                                                                                        </Columns>
                                                                                    </ComponentArt:GridLevel>
                                                                                    <ComponentArt:GridLevel
                                                                                        AllowGrouping="false"
                                                                                        DataMember="dashboard_settings_aging"
                                                                                        DataKeyField="dashboard_menu_id"
                                                                                        ShowTableHeading="false"
                                                                                        TableHeadingCssClass="GridHeader"
                                                                                        RowCssClass="Row"
                                                                                        HoverRowCssClass="HoverRow"
                                                                                        ColumnReorderIndicatorImageUrl="reorder.gif"
                                                                                        DataCellCssClass="DataCell"
                                                                                        HeadingCellCssClass="HeadingCell"
                                                                                        HeadingRowCssClass="HeadingRow"
                                                                                        HeadingTextCssClass="HeadingCellText"
                                                                                        SortedDataCellCssClass="SortedDataCell"
                                                                                        SelectedRowCssClass="SelectedRow"
                                                                                        SortAscendingImageUrl="col-asc.png"
                                                                                        SortDescendingImageUrl="col-desc.png"
                                                                                        SortImageWidth="10"
                                                                                        SortImageHeight="19">
                                                                                        <ConditionalFormats>
                                                                                            <ComponentArt:GridConditionalFormat ClientFilter="((DataItem.get_index() + grdDASH.get_recordOffset()) % 2) > 0" RowCssClass="AltRow" SelectedRowCssClass="SelectedRow" />
                                                                                        </ConditionalFormats>
                                                                                        <Columns>
                                                                                            <ComponentArt:GridColumn DataField="id" Align="left" HeadingText="id" AllowGrouping="false" Visible="false" DataCellCssClass="cell-row-update" />
                                                                                            <ComponentArt:GridColumn DataField="dashboard_menu_id" Align="left" HeadingText="dashboard_menu_id" AllowGrouping="false" Visible="false" DataCellCssClass="cell-row-update" />
                                                                                            <ComponentArt:GridColumn DataField="key" Align="left" HeadingText="Name" AllowGrouping="false" DataCellClientTemplateId="KEY_DESC" Width="200" />
                                                                                            <ComponentArt:GridColumn DataField="slot_count" Align="right" HeadingText="Slot Count(2 to 4)" AllowGrouping="false" DataCellClientTemplateId="SLOT_COUNT" />
                                                                                            <ComponentArt:GridColumn DataField="slot_1" Align="right" HeadingText="Slot 1" DataCellClientTemplateId="SLOT_1" AllowGrouping="false" />
                                                                                            <ComponentArt:GridColumn DataField="slot_2" Align="right" HeadingText="Slot 2" DataCellClientTemplateId="SLOT_2" AllowGrouping="false" />
                                                                                            <ComponentArt:GridColumn DataField="slot_3" Align="right" HeadingText="Slot 3" DataCellClientTemplateId="SLOT_3" AllowGrouping="false" />
                                                                                            <%--<ComponentArt:GridColumn DataField="slot_4" Align="right" HeadingText="Slot 4" DataCellClientTemplateId="SLOT_4" AllowGrouping="false" />--%>
                                                                                        </Columns>
                                                                                    </ComponentArt:GridLevel>
                                                                                </Levels>
                                                                                <ClientEvents>
                                                                                    <RenderComplete EventHandler="grdDASH_onRenderComplete" />
                                                                                    <ItemExpand EventHandler="grdDASH_onItemExpand" />
                                                                                    <ItemCollapse EventHandler="grdDASH_onItemCollapse" />
                                                                                </ClientEvents>
                                                                                <ClientTemplates>
                                                                                    <ComponentArt:ClientTemplate ID="MENU_DESC">
                                                                                        <input type="text" id="txtMenuDesc_## DataItem.GetMember('id').Value ##" class="GridTextBox"
                                                                                            value="## DataItem.GetMember('menu_desc').Value ##" style="width: 100%; text-align: left; padding-left: 5px;" maxlength="150"
                                                                                            onchange="javascript:txtMenuDesc_OnChange('## DataItem.GetMember('id').Value ##');"
                                                                                            onfocus="javascript:this.select();" />
                                                                                    </ComponentArt:ClientTemplate>
                                                                                    <ComponentArt:ClientTemplate ID="NAVURL">
                                                                                        <input type="text" id="txtNavUrl_## DataItem.GetMember('id').Value ##" class="GridTextBox"
                                                                                            value="## DataItem.GetMember('nav_url').Value ##" style="width: 100%; text-align: left; padding-left: 5px;" maxlength="500"
                                                                                            onkeypress="javascript:return parent.CheckNavUrl(event);"
                                                                                            onchange="javascript:txtNavUrl_OnChange('## DataItem.GetMember('id').Value ##');"
                                                                                            onfocus="javascript:this.select();" />
                                                                                    </ComponentArt:ClientTemplate>
                                                                                    <ComponentArt:ClientTemplate ID="ICON">
                                                                                        <input type="text" id="txtIcon_## DataItem.GetMember('id').Value ##" class="GridTextBox"
                                                                                            value="## DataItem.GetMember('icon').Value ##" style="width: 100%; text-align: left; padding-left: 5px;" maxlength="150"
                                                                                            onchange="javascript:txtIcon_OnChange('## DataItem.GetMember('id').Value ##');"
                                                                                            onfocus="javascript:this.select();" />
                                                                                    </ComponentArt:ClientTemplate>
                                                                                    <ComponentArt:ClientTemplate ID="REFRESH_TIME">
                                                                                        <input type="text" id="txtRefreshTime_## DataItem.GetMember('id').Value ##" class="GridTextBox"
                                                                                            value="## DataItem.GetMember('refresh_time').Value ##" style="width: 100%; text-align: right;"
                                                                                            onchange="javascript:txtRefreshTime_OnChange('## DataItem.GetMember('id').Value ##');"
                                                                                            onfocus="javascript:this.select();" />
                                                                                    </ComponentArt:ClientTemplate>
                                                                                    <ComponentArt:ClientTemplate ID="IS_REFRESH_BUTTON">
                                                                                        <input type="checkbox" id="chkIsRefreshButton_## DataItem.GetMember('id').Value ##" style="height: 18px; width: 18px;" onclick="javascript: IsRefreshButtonShow_OnClick('## DataItem.GetMember('id').Value ##');" />
                                                                                        <label for="chkIsRefreshButton_## DataItem.GetMember('id').Value ##" class="label-default"></label>
                                                                                    </ComponentArt:ClientTemplate>
                                                                                    <ComponentArt:ClientTemplate ID="IS_ENABLED">
                                                                                        <input type="checkbox" id="chkIsEnabled_## DataItem.GetMember('id').Value ##" style="height: 18px; width: 18px;" onclick="javascript: IsDashboardMenuEnabled_OnClick('## DataItem.GetMember('id').Value ##');" />
                                                                                        <label for="chkIsEnabled_## DataItem.GetMember('id').Value ##" class="label-default"></label>
                                                                                    </ComponentArt:ClientTemplate>
                                                                                    <ComponentArt:ClientTemplate ID="IS_DEFAULT">
                                                                                        <input type="checkbox" id="chkIsDefault_## DataItem.GetMember('id').Value ##" style="height: 18px; width: 18px;" onclick="javascript: IsDefault_OnClick('## DataItem.GetMember('id').Value ##');" />
                                                                                        <label for="chkIsDefault_## DataItem.GetMember('id').Value ##" class="label-default"></label>
                                                                                    </ComponentArt:ClientTemplate>
                                                                                    <ComponentArt:ClientTemplate ID="DISPLAY_INDEX">
                                                                                        <input type="text" id="txtDisplayIndex_## DataItem.GetMember('id').Value ##" class="GridTextBox"
                                                                                            value="## DataItem.GetMember('display_index').Value ##" style="width: 100%; text-align: right;"
                                                                                            onchange="javascript:txtDisplayIndex_OnChange('## DataItem.GetMember('id').Value ##');"
                                                                                            onfocus="javascript:this.select();" />
                                                                                    </ComponentArt:ClientTemplate>
                                                                                    <ComponentArt:ClientTemplate ID="TITLE_DESC">
                                                                                        <input type="text" id="txtTitleDesc_## DataItem.GetMember('id').Value ##" class="GridTextBox"
                                                                                            value="## DataItem.GetMember('title').Value ##" style="width: 100%; text-align: left; padding-left: 5px;" maxlength="150"
                                                                                            onchange="javascript:txtTitleDesc_OnChange('## DataItem.GetMember('id').Value ##');"
                                                                                            onfocus="javascript:this.select();" />
                                                                                    </ComponentArt:ClientTemplate>


                                                                                    <ComponentArt:ClientTemplate ID="KEY_DESC">
                                                                                        <input type="text" id="txtKeyDesc_## DataItem.GetMember('id').Value ##" class="GridTextBox"
                                                                                            value="## DataItem.GetMember('key').Value ##" style="width: 100%; text-align: left; padding-left: 5px;" maxlength="150"
                                                                                            onchange="javascript:txtKeyDesc_OnChange('## DataItem.GetMember('id').Value ##');"
                                                                                            onfocus="javascript:this.select();" />
                                                                                    </ComponentArt:ClientTemplate>
                                                                                    <ComponentArt:ClientTemplate ID="SLOT_COUNT">
                                                                                        <input type="text" id="txtSlotCount_## DataItem.GetMember('id').Value ##" class="GridTextBox"
                                                                                            value="## DataItem.GetMember('slot_count').Value ##" style="width: 100%; text-align: right; padding-right: 5px;"
                                                                                            onchange="javascript:txtSlotCount_OnChange('## DataItem.GetMember('id').Value ##');"
                                                                                            onblur="javascript:txtSlotCount_OnBlur('## DataItem.GetMember('id').Value ##');"
                                                                                            onfocus="javascript:this.select();" />
                                                                                    </ComponentArt:ClientTemplate>
                                                                                    <ComponentArt:ClientTemplate ID="SLOT_1">
                                                                                        <input type="text" id="txtSlot1_## DataItem.GetMember('id').Value ##" class="GridTextBox"
                                                                                            value="## DataItem.GetMember('slot_1').Value ##" style="width: 100%; text-align: right; padding-right: 5px;"
                                                                                            onchange="javascript:txtSlot1_OnChange('## DataItem.GetMember('id').Value ##');"
                                                                                            onfocus="javascript:this.select();" />
                                                                                    </ComponentArt:ClientTemplate>
                                                                                    <ComponentArt:ClientTemplate ID="SLOT_2">
                                                                                        <input type="text" id="txtSlot2_## DataItem.GetMember('id').Value ##" class="GridTextBox"
                                                                                            value="## DataItem.GetMember('slot_2').Value ##" style="width: 100%; text-align: right; padding-right: 5px;"
                                                                                            onchange="javascript:txtSlot2_OnChange('## DataItem.GetMember('id').Value ##');"
                                                                                            onfocus="javascript:this.select();" />
                                                                                    </ComponentArt:ClientTemplate>
                                                                                    <ComponentArt:ClientTemplate ID="SLOT_3">
                                                                                        <input type="text" id="txtSlot3_## DataItem.GetMember('id').Value ##" class="GridTextBox"
                                                                                            value="## DataItem.GetMember('slot_3').Value ##" style="width: 100%; text-align: right; padding-right: 5px;"
                                                                                            onchange="javascript:txtSlot3_OnChange('## DataItem.GetMember('id').Value ##');"
                                                                                            onfocus="javascript:this.select();" />
                                                                                    </ComponentArt:ClientTemplate>
                                                                                    <%--<ComponentArt:ClientTemplate ID="SLOT_4">
                                                                                        <input type="text" id="txtSlot4_## DataItem.GetMember('id').Value ##" class="GridTextBox"
                                                                                            value="## DataItem.GetMember('slot_4').Value ##" style="width: 100%; text-align: right; padding-right: 5px;"
                                                                                            onchange="javascript:txtSlot4_OnChange('## DataItem.GetMember('id').Value ##');"
                                                                                            onblur="javascript:txtSlot4_OnBlur('## DataItem.GetMember('id').Value ##');"
                                                                                            onfocus="javascript:this.select();" />
                                                                                    </ComponentArt:ClientTemplate>--%>
                                                                                </ClientTemplates>
                                                                            </ComponentArt:Grid>
                                                                            <span id="spnDashErr" runat="server"></span>
                                                                        </Content>
                                                                        <LoadingPanelClientTemplate>
                                                                            <table style="height: 400px; width: 100%;" border="0">
                                                                                <tr>
                                                                                    <td style="text-align: center;">
                                                                                        <table border="0" style="width: 70px; display: inline-block;">
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <img src="../images/Searching.gif" border="0" alt="" />
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </LoadingPanelClientTemplate>
                                                                        <ClientEvents>
                                                                            <CallbackComplete EventHandler="grdDASH_onCallbackComplete" />
                                                                        </ClientEvents>
                                                                    </ComponentArt:CallBack>


                                                                </div>
                                                            </div>
                                                        </div>

                                                    </div>
                                                </div>

                                                <div class="row">

                                                    <div class="col-sm-12 col-xs-12 text-right" style="margin-left: -5px;">

                                                        <button type="button" id="btnSaveDashboard2" runat="server" class="btn btn-custon-four btn-success">
                                                            <i class="fa fa-floppy-o" aria-hidden="true"></i>&nbsp;Save</button>
                                                    </div>
                                                </div>

                                            </div>
                                        </ComponentArt:PageView>
                                    </ComponentArt:MultiPage>
                                </div>
                            </div>


                        </div>
                    </div>
                </div>
            </div>

            <div class="sparklineHeader mt-b-10 marginTP10">
                <div class="sparkline10-hd">
                    <div class="row">
                        <div class="col-sm-6 col-xs-12">
                        </div>
                        <div class="col-sm-6 col-xs-12 text-right">



                            <button type="button" class="btn btn-custon-four btn-danger" id="btnClose2" runat="server">
                                <i class="fa fa-times" aria-hidden="true"></i>&nbsp;Close</button>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <input type="hidden" id="hdnError" runat="server" value="" />
        <input type="hidden" id="hdnID" runat="server" value="00000000-0000-0000-0000-000000000000" />
        <input type="hidden" id="hdnMode" runat="server" value="1" />
        <input type="hidden" id="hdnInfo" runat="server" value="" />
        <input type="hidden" id="hdnTZ" runat="server" value="" />
    </form>
</body>
<script type="text/javascript">
    var objhdnID = document.getElementById('<%=hdnID.ClientID %>');
    var objhdnError = document.getElementById('<%=hdnError.ClientID %>');
    var objhdnTZ = document.getElementById('<%=hdnTZ.ClientID %>');
    var strForm = "VRSConfig";


</script>
<script src="../scripts/custome-javascript.js"></script>
<script src="../scripts/AppPages.js?v=<%=DateTime.Now.Ticks%>"></script>
<script src="scripts/Config.js?v=<%=DateTime.Now.Ticks%>"></script>
</html>
