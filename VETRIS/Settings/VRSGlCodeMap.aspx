<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VRSGlCodeMap.aspx.cs" Inherits="VETRIS.Settings.VRSGlCodeMap" %>

<%@ Register Assembly="ComponentArt.Web.UI" Namespace="ComponentArt.Web.UI" TagPrefix="ComponentArt" %>
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
    <%--<link href="../css/style.css" rel="stylesheet" />
    <link href="../css/grid_style.css" rel="stylesheet" />--%>
    <link id="lnkSTYLE" runat="server" href = "../css/style.css" rel="stylesheet" type="text/css" />
    <link id="lnkGRID" runat="server" href = "../css/grid_style.css" rel="stylesheet" type="text/css" />
    <script src="../scripts/jquery-1.7.1.js"></script>
    <script src="scripts/GlCodeMapHdr.js?01112020" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
            <div class="sparklineHeader mt-b-10 marginTP10">
                <div class="sparkline10-hd">
                    <div class="row">
                        <div class="col-sm-6 col-xs-12">
                            <h2>G/L Code Mapping</h2>
                        </div>
                        <div class="col-sm-6 col-xs-12 text-right">

                            <button type="button" id="btnSave1" runat="server" class="btn btn-custon-four btn-success">
                                <i class="fa fa-floppy-o" aria-hidden="true"></i>&nbsp;Save</button>
                            <button type="button" class="btn btn-custon-four btn-warning" id="btnReset1" runat="server">
                                <i class="fa fa-repeat edu-danger-error" aria-hidden="true"></i>&nbsp;Reset
                            </button>
                            <button type="button" class="btn btn-custon-four btn-danger" id="btnClose1" runat="server">
                                <i class="fa fa-times" aria-hidden="true"></i>&nbsp;Close</button>
                        </div>
                    </div>


                </div>
            </div>
            <div class=" row mt-b-10 marginTP10">
                <div class="col-sm-12 col-xs-12">
                    <div class="sparkline10-list mt-b-10">
                        <div class="searchSection">
                            <div class="row">
                                <div class="col-sm-8 col-xs-12">
                                    <div class="pull-left" style="margin-top: 5px;">
                                        <h3 class="h3Text">Category Wise Modality</h3>
                                    </div>
                                </div>
                                <div class="col-sm-4 col-xs-12">
                                    <div class="pull-right">
                                        <button type="button" class="btn btn-grd btn-primary" id="btnAddModality" runat="server" title="click to add new row for Category Wise & Modality Wise G/L code mapping">
                                            <i class="fa fa-plus " aria-hidden="true"></i>
                                        </button>
                                    </div>
                                </div>
                                <div class="col-sm-12 col-xs-12">
                                    <div class="borderSearch pull-left"></div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-12 col-xs-12">
                                    <div class="table-responsive">
                                        <ComponentArt:CallBack ID="CallBackModality" runat="server" OnCallback="CallBackModality_Callback">
                                            <Content>
                                                <ComponentArt:Grid
                                                    ID="grdModality"
                                                    CssClass="Grid"
                                                    DataAreaCssClass="GridData5_1"
                                                    SearchOnKeyPress="true"
                                                    EnableViewState="true"
                                                    ShowSearchBox="true"
                                                    SearchBoxPosition="TopRight"
                                                    SearchTextCssClass="GridHeaderText"
                                                    ShowHeader="false"
                                                    FooterCssClass="GridFooter"
                                                    GroupingNotificationText=""
                                                    PageSize="5"
                                                    ScrollBar="Auto"
                                                    ScrollTopBottomImagesEnabled="true"
                                                    ScrollTopBottomImageHeight="2"
                                                    ScrollTopBottomImageWidth="16"
                                                    ScrollImagesFolderUrl="../images/scroller/"
                                                    ScrollButtonWidth="16"
                                                    ScrollButtonHeight="17" ShowFooter="false"
                                                    ScrollBarCssClass="ScrollBar"
                                                    ScrollGripCssClass="ScrollGrip"
                                                    ScrollBarWidth="16"
                                                    PagerTextCssClass="GridFooterText"
                                                    ImagesBaseUrl="../images/"
                                                    LoadingPanelFadeDuration="1000"
                                                    LoadingPanelFadeMaximumOpacity="80"
                                                    LoadingPanelClientTemplateId="LoadingFeedbackTemplate"
                                                    LoadingPanelPosition="MiddleCenter"
                                                    Width="99%"
                                                    runat="server" HeaderCssClass="GridHeader"
                                                    GroupingNotificationPosition="TopLeft">

                                                    <Levels>
                                                        <ComponentArt:GridLevel
                                                            AllowGrouping="false"
                                                            DataKeyField="row_id"
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
                                                            SortAscendingImageUrl="col-asc.png"
                                                            SortDescendingImageUrl="col-desc.png"
                                                            SortImageWidth="10"
                                                            SortImageHeight="19">
                                                            <ConditionalFormats>
                                                                <ComponentArt:GridConditionalFormat ClientFilter="((DataItem.get_index() + grdModality.get_recordOffset()) % 2) > 0" RowCssClass="AltRow" />
                                                            </ConditionalFormats>
                                                            <Columns>
                                                                <ComponentArt:GridColumn DataField="row_id" Align="left" HeadingText="#" AllowGrouping="false" Width="30" />
                                                                <ComponentArt:GridColumn DataField="category_id" Align="left" HeadingText="category_id" AllowGrouping="false" Visible="false" />
                                                                <ComponentArt:GridColumn DataField="category_name" Align="left" HeadingText="Category" AllowGrouping="false" Width="200" FixedWidth="true" DataCellClientTemplateId="CATEGORY" />
                                                                <ComponentArt:GridColumn DataField="modality_id" Align="left" HeadingText="modality_id" AllowGrouping="false" Visible="false" />
                                                                <ComponentArt:GridColumn DataField="modality_name" Align="left" HeadingText="Modality" AllowGrouping="false" Width="200" FixedWidth="true" DataCellClientTemplateId="MODALITY" />
                                                                <ComponentArt:GridColumn DataField="gl_code" Align="left" HeadingText="G/L Code" AllowGrouping="false" Width="80" FixedWidth="true" DataCellClientTemplateId="GLCODEMOD" />
                                                                <ComponentArt:GridColumn Align="center" AllowGrouping="false" DataCellClientTemplateId="DELMOD" HeadingText=" " FixedWidth="True" Width="50" />
                                                            </Columns>

                                                        </ComponentArt:GridLevel>
                                                    </Levels>

                                                    <ClientEvents>
                                                        <RenderComplete EventHandler="grdModality_onRenderComplete" />
                                                    </ClientEvents>
                                                    <ClientTemplates>
                                                        <ComponentArt:ClientTemplate ID="CATEGORY">
                                                            <select id="ddlCategory_## DataItem.GetMember('row_id').Value ##" class="form-control custom-select-value" style="width: 99%;" onchange="javascript:ddlCategory_OnChange('## DataItem.GetMember('row_id').Value ##');">
                                                            </select>

                                                        </ComponentArt:ClientTemplate>
                                                        <ComponentArt:ClientTemplate ID="MODALITY">
                                                            <select id="ddlModality_## DataItem.GetMember('row_id').Value ##" class="form-control custom-select-value" style="width: 99%;" onchange="javascript:ddlModality_OnChange('## DataItem.GetMember('row_id').Value ##');">
                                                            </select>
                                                        </ComponentArt:ClientTemplate>
                                                        <ComponentArt:ClientTemplate ID="GLCODEMOD">
                                                            <input type="text" id="txtGLCodeMod_## DataItem.GetMember('row_id').Value ##" maxlength="5" class="GridTextBox" value="## DataItem.GetMember('gl_code').Value ##" onchange="javascript:txtGLCodeMod_OnChange('## DataItem.GetMember('row_id').Value ##');" />
                                                        </ComponentArt:ClientTemplate>


                                                        <ComponentArt:ClientTemplate ID="DELMOD">
                                                            <button type="button" id="btnDelMod_## DataItem.GetMember('row_id').Value ##" class="btn btn-danger btn_grd" onclick="javascript:DeleteModalityRow('## DataItem.GetMember('row_id').Value ##')"><i class="fa fa-trash" aria-hidden="true"></i></button>
                                                        </ComponentArt:ClientTemplate>
                                                    </ClientTemplates>
                                                </ComponentArt:Grid>
                                                <span id="spnErrMod" runat="server"></span>
                                            </Content>
                                            <LoadingPanelClientTemplate>
                                                <table style="height: 205px; width: 100%;" border="0">
                                                    <tr>
                                                        <td style="text-align: center;">
                                                            <table border="0" style="width: 70px; display: inline-block;">
                                                                <tr>
                                                                    <td>
                                                                        <img src="../images/spinner-darkgrey.gif" width="50" height="65" border="0" alt="" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </LoadingPanelClientTemplate>
                                            <ClientEvents>
                                                <CallbackComplete EventHandler="grdModality_onCallbackComplete" />
                                            </ClientEvents>
                                        </ComponentArt:CallBack>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

            </div>
            <div class=" row mt-b-10 marginTP10">
                <div class="col-sm-12 col-xs-12">
                    <div class="sparkline10-list mt-b-10">
                        <div class="searchSection">
                            <div class="row">
                                <div class="col-sm-8 col-xs-12">
                                    <div class="pull-left" style="margin-top: 5px;">
                                        <h3 class="h3Text">Services</h3>
                                    </div>
                                </div>
                                <div class="col-sm-4 col-xs-12">
                                    <div class="pull-right">
                                        <button type="button" class="btn btn-grd btn-primary" id="btnAddService" runat="server" title="click to add new row for Service Wise G/L code mapping">
                                            <i class="fa fa-plus " aria-hidden="true"></i>
                                        </button>
                                    </div>
                                </div>
                                <div class="col-sm-12 col-xs-12">
                                    <div class="borderSearch pull-left"></div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-12 col-xs-12">
                                    <div class="table-responsive">
                                        <ComponentArt:CallBack ID="CallBackService" runat="server" OnCallback="CallBackService_Callback">
                                            <Content>
                                                <ComponentArt:Grid
                                                    ID="grdService"
                                                    CssClass="Grid"
                                                    DataAreaCssClass="GridData5_1"
                                                    SearchOnKeyPress="true"
                                                    EnableViewState="true"
                                                    ShowSearchBox="true"
                                                    SearchBoxPosition="TopRight"
                                                    SearchTextCssClass="GridHeaderText"
                                                    ShowHeader="false"
                                                    FooterCssClass="GridFooter"
                                                    GroupingNotificationText=""
                                                    PageSize="7"
                                                    ScrollBar="Auto"
                                                    ScrollTopBottomImagesEnabled="true"
                                                    ScrollTopBottomImageHeight="2"
                                                    ScrollTopBottomImageWidth="16"
                                                    ScrollImagesFolderUrl="../images/scroller/"
                                                    ScrollButtonWidth="16"
                                                    ScrollButtonHeight="17" ShowFooter="false"
                                                    ScrollBarCssClass="ScrollBar"
                                                    ScrollGripCssClass="ScrollGrip"
                                                    ScrollBarWidth="16"
                                                    PagerTextCssClass="GridFooterText"
                                                    ImagesBaseUrl="../images/"
                                                    LoadingPanelFadeDuration="1000"
                                                    LoadingPanelFadeMaximumOpacity="80"
                                                    LoadingPanelClientTemplateId="LoadingFeedbackTemplate"
                                                    LoadingPanelPosition="MiddleCenter"
                                                    Width="99%"
                                                    runat="server" HeaderCssClass="GridHeader"
                                                    GroupingNotificationPosition="TopLeft">

                                                    <Levels>
                                                        <ComponentArt:GridLevel
                                                            AllowGrouping="false"
                                                            DataKeyField="row_id"
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
                                                            SortAscendingImageUrl="col-asc.png"
                                                            SortDescendingImageUrl="col-desc.png"
                                                            SortImageWidth="10"
                                                            SortImageHeight="19">
                                                            <ConditionalFormats>
                                                                <ComponentArt:GridConditionalFormat ClientFilter="((DataItem.get_index() + grdService.get_recordOffset()) % 2) > 0" RowCssClass="AltRow" />
                                                            </ConditionalFormats>
                                                            <Columns>
                                                                <ComponentArt:GridColumn DataField="row_id" Align="left" HeadingText="#" AllowGrouping="false" Width="30" />
                                                                <ComponentArt:GridColumn DataField="service_id" Align="left" HeadingText="Service" AllowGrouping="false" Width="200" FixedWidth="true" DataCellClientTemplateId="SERVICE" />
                                                                <ComponentArt:GridColumn DataField="modality_id" Align="left" HeadingText="Modality" AllowGrouping="false" Width="200" FixedWidth="true" DataCellClientTemplateId="MODALITYSVC" />
                                                                <ComponentArt:GridColumn DataField="gl_code_default" Align="left" HeadingText="G/L Code" AllowGrouping="false" Width="100" FixedWidth="true" DataCellClientTemplateId="GLCODESVC" />
                                                                <ComponentArt:GridColumn DataField="gl_code_after_hrs" Align="left" HeadingText="G/L Code After Hrs." AllowGrouping="false" Width="130" FixedWidth="true" DataCellClientTemplateId="GLCODEAHSVC" />
                                                                <ComponentArt:GridColumn Align="center" AllowGrouping="false" DataCellClientTemplateId="DELSVC" HeadingText=" " FixedWidth="True" Width="50" />
                                                            </Columns>

                                                        </ComponentArt:GridLevel>
                                                    </Levels>
                                                    <ClientEvents>
                                                        <RenderComplete EventHandler="grdService_onRenderComplete" />
                                                    </ClientEvents>

                                                    <ClientTemplates>
                                                        <ComponentArt:ClientTemplate ID="SERVICE">
                                                            <select id="ddlService_## DataItem.GetMember('row_id').Value ##" class="form-control custom-select-value" style="width: 99%;" onchange="javascript:ddlService_OnChange('## DataItem.GetMember('row_id').Value ##');">
                                                            </select>
                                                        </ComponentArt:ClientTemplate>
                                                        <ComponentArt:ClientTemplate ID="MODALITYSVC">
                                                            <select id="ddlModalitySvc_## DataItem.GetMember('row_id').Value ##" class="form-control custom-select-value" style="width: 99%;" onchange="javascript:ddlModalitySvc_OnChange('## DataItem.GetMember('row_id').Value ##');">
                                                            </select>
                                                        </ComponentArt:ClientTemplate>
                                                        <ComponentArt:ClientTemplate ID="GLCODESVC">
                                                            <input type="text" id="txtGLCodeSvc_## DataItem.GetMember('row_id').Value ##" maxlength="80" class="GridTextBox" value="## DataItem.GetMember('gl_code_default').Value ##" onchange="javascript:txtGLCodeSvc_OnChange('## DataItem.GetMember('row_id').Value ##');" />
                                                        </ComponentArt:ClientTemplate>
                                                        <ComponentArt:ClientTemplate ID="GLCODEAHSVC">
                                                            <input type="text" id="txtGLCodeAHSvc_## DataItem.GetMember('row_id').Value ##" maxlength="80" class="GridTextBox" value="## DataItem.GetMember('gl_code_after_hrs').Value ##" onchange="javascript:txtGLCodeAHSvc_OnChange('## DataItem.GetMember('row_id').Value ##');" />
                                                        </ComponentArt:ClientTemplate>
                                                         <ComponentArt:ClientTemplate ID="DELSVC">
                                                            <button type="button" id="btnDelSvc_## DataItem.GetMember('row_id').Value ##" class="btn btn-danger btn_grd" onclick="javascript:DeleteServiceRow('## DataItem.GetMember('row_id').Value ##')"><i class="fa fa-trash" aria-hidden="true"></i></button>
                                                        </ComponentArt:ClientTemplate>
                                                    </ClientTemplates>
                                                </ComponentArt:Grid>
                                                <span id="spnErrSvc" runat="server"></span>
                                            </Content>
                                            <LoadingPanelClientTemplate>
                                                <table style="height: 205px; width: 100%;" border="0">
                                                    <tr>
                                                        <td style="text-align: center;">
                                                            <table border="0" style="width: 70px; display: inline-block;">
                                                                <tr>
                                                                    <td>
                                                                        <img src="../images/spinner-darkgrey.gif" width="50" height="65" border="0" alt="" />
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
                </div>

            </div>
            <div class=" row mt-b-10 marginTP10">
                <div class="col-sm-6 col-xs-12">
                    <div class="sparkline10-list mt-b-10">
                        <div class="searchSection">
                            <div class="row">
                                <div class="col-sm-12 col-xs-12">
                                    <div class="pull-left">
                                        <h3 class="h3Text">Other Accounting Heads</h3>
                                    </div>
                                    <div class="borderSearch pull-left"></div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-12 col-xs-12">
                                    <div class="table-responsive">
                                        <ComponentArt:CallBack ID="CallBackNRH" runat="server" OnCallback="CallBackNRH_Callback">
                                            <Content>
                                                <ComponentArt:Grid
                                                    ID="grdNRH"
                                                    CssClass="Grid"
                                                    DataAreaCssClass="GridData5_1"
                                                    SearchOnKeyPress="true"
                                                    EnableViewState="true"
                                                    ShowSearchBox="true"
                                                    SearchBoxPosition="TopRight"
                                                    SearchTextCssClass="GridHeaderText"
                                                    ShowHeader="false"
                                                    FooterCssClass="GridFooter"
                                                    GroupingNotificationText=""
                                                    PageSize="7"
                                                    ScrollBar="Auto"
                                                    ScrollTopBottomImagesEnabled="true"
                                                    ScrollTopBottomImageHeight="2"
                                                    ScrollTopBottomImageWidth="16"
                                                    ScrollImagesFolderUrl="../images/scroller/"
                                                    ScrollButtonWidth="16"
                                                    ScrollButtonHeight="17" ShowFooter="false"
                                                    ScrollBarCssClass="ScrollBar"
                                                    ScrollGripCssClass="ScrollGrip"
                                                    ScrollBarWidth="16"
                                                    PagerTextCssClass="GridFooterText"
                                                    ImagesBaseUrl="../images/"
                                                    LoadingPanelFadeDuration="1000"
                                                    LoadingPanelFadeMaximumOpacity="80"
                                                    LoadingPanelClientTemplateId="LoadingFeedbackTemplate"
                                                    LoadingPanelPosition="MiddleCenter"
                                                    Width="99%"
                                                    runat="server" HeaderCssClass="GridHeader"
                                                    GroupingNotificationPosition="TopLeft">

                                                    <Levels>
                                                        <ComponentArt:GridLevel
                                                            AllowGrouping="false"
                                                            DataKeyField="row_id"
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
                                                            SortAscendingImageUrl="col-asc.png"
                                                            SortDescendingImageUrl="col-desc.png"
                                                            SortImageWidth="10"
                                                            SortImageHeight="19">
                                                            <ConditionalFormats>
                                                                <ComponentArt:GridConditionalFormat ClientFilter="((DataItem.get_index() + grdNRH.get_recordOffset()) % 2) > 0" RowCssClass="AltRow" />
                                                            </ConditionalFormats>
                                                            <Columns>
                                                                <ComponentArt:GridColumn DataField="row_id" Align="left" HeadingText="#" AllowGrouping="false" Width="30" />
                                                                <ComponentArt:GridColumn DataField="control_code" Align="left" HeadingText="control_code" AllowGrouping="false" Visible="false" />
                                                                <ComponentArt:GridColumn DataField="control_desc" Align="left" HeadingText="Head" AllowGrouping="false" Width="200" />
                                                                <ComponentArt:GridColumn DataField="gl_code" Align="left" HeadingText="Last Name" AllowGrouping="false" Width="100" FixedWidth="true" DataCellClientTemplateId="GLCODENRH" />
                                                            </Columns>

                                                        </ComponentArt:GridLevel>
                                                    </Levels>

                                                    <ClientTemplates>

                                                        <ComponentArt:ClientTemplate ID="GLCODENRH">
                                                            <input type="text" id="txtGLCodeNRH_## DataItem.GetMember('row_id').Value ##" maxlength="80" class="GridTextBox" value="## DataItem.GetMember('gl_code').Value ##" onchange="javascript:txtGLCodeNRH_OnChange('## DataItem.GetMember('row_id').Value ##');" />
                                                        </ComponentArt:ClientTemplate>

                                                    </ClientTemplates>
                                                </ComponentArt:Grid>
                                                <span id="spnErrNRH" runat="server"></span>
                                            </Content>
                                            <LoadingPanelClientTemplate>
                                                <table style="height: 205px; width: 100%;" border="0">
                                                    <tr>
                                                        <td style="text-align: center;">
                                                            <table border="0" style="width: 70px; display: inline-block;">
                                                                <tr>
                                                                    <td>
                                                                        <img src="../images/spinner-darkgrey.gif" width="50" height="65" border="0" alt="" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </LoadingPanelClientTemplate>
                                            <ClientEvents>
                                                <CallbackComplete EventHandler="grdNRH_onCallbackComplete" />
                                            </ClientEvents>
                                        </ComponentArt:CallBack>
                                    </div>
                                </div>
                            </div>
                        </div>

                    </div>
                </div>
                <div class="col-sm-6 col-xs-12">
                    <div class="sparkline10-list mt-b-10">
                        <div class="searchSection">
                            <div class="row">
                                <div class="col-sm-12 col-xs-12">
                                    <div class="pull-left">
                                        <h3 class="h3Text">Radiologist Charges</h3>
                                    </div>
                                    <div class="borderSearch pull-left"></div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-12 col-xs-12">
                                    <div class="table-responsive">
                                        <ComponentArt:CallBack ID="CallBackRC" runat="server" OnCallback="CallBackRC_Callback">
                                            <Content>
                                                <ComponentArt:Grid
                                                    ID="grdRC"
                                                    CssClass="Grid"
                                                    DataAreaCssClass="GridData5_1"
                                                    SearchOnKeyPress="true"
                                                    EnableViewState="true"
                                                    ShowSearchBox="true"
                                                    SearchBoxPosition="TopRight"
                                                    SearchTextCssClass="GridHeaderText"
                                                    ShowHeader="false"
                                                    FooterCssClass="GridFooter"
                                                    GroupingNotificationText=""
                                                    PageSize="7"
                                                    ScrollBar="Auto"
                                                    ScrollTopBottomImagesEnabled="true"
                                                    ScrollTopBottomImageHeight="2"
                                                    ScrollTopBottomImageWidth="16"
                                                    ScrollImagesFolderUrl="../images/scroller/"
                                                    ScrollButtonWidth="16"
                                                    ScrollButtonHeight="17" ShowFooter="false"
                                                    ScrollBarCssClass="ScrollBar"
                                                    ScrollGripCssClass="ScrollGrip"
                                                    ScrollBarWidth="16"
                                                    PagerTextCssClass="GridFooterText"
                                                    ImagesBaseUrl="../images/"
                                                    LoadingPanelFadeDuration="1000"
                                                    LoadingPanelFadeMaximumOpacity="80"
                                                    LoadingPanelClientTemplateId="LoadingFeedbackTemplate"
                                                    LoadingPanelPosition="MiddleCenter"
                                                    Width="99%"
                                                    runat="server" HeaderCssClass="GridHeader"
                                                    GroupingNotificationPosition="TopLeft">

                                                    <Levels>
                                                        <ComponentArt:GridLevel
                                                            AllowGrouping="false"
                                                            DataKeyField="row_id"
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
                                                            SortAscendingImageUrl="col-asc.png"
                                                            SortDescendingImageUrl="col-desc.png"
                                                            SortImageWidth="10"
                                                            SortImageHeight="19">
                                                            <ConditionalFormats>
                                                                <ComponentArt:GridConditionalFormat ClientFilter="((DataItem.get_index() + grdRC.get_recordOffset()) % 2) > 0" RowCssClass="AltRow" />
                                                            </ConditionalFormats>
                                                            <Columns>
                                                                <ComponentArt:GridColumn DataField="row_id" Align="left" HeadingText="#" AllowGrouping="false" Width="30" />
                                                                <ComponentArt:GridColumn DataField="id" Align="left" HeadingText="id" AllowGrouping="false" Visible="false" />
                                                                <ComponentArt:GridColumn DataField="name" Align="left" HeadingText="Group" AllowGrouping="false" Width="250" />
                                                                <ComponentArt:GridColumn DataField="gl_code" Align="left" HeadingText="G/L Code" AllowGrouping="false" Width="100" FixedWidth="true" DataCellClientTemplateId="GLCODERC" />
                                                            </Columns>

                                                        </ComponentArt:GridLevel>
                                                    </Levels>


                                                    <ClientTemplates>

                                                        <ComponentArt:ClientTemplate ID="GLCODERC">
                                                            <input type="text" id="txtGLCodeRC_## DataItem.GetMember('row_id').Value ##" maxlength="80" class="GridTextBox" value="## DataItem.GetMember('gl_code').Value ##" onchange="javascript:txtGLCodeRC_OnChange('## DataItem.GetMember('row_id').Value ##');" />
                                                        </ComponentArt:ClientTemplate>

                                                    </ClientTemplates>
                                                </ComponentArt:Grid>
                                                <span id="spnErrRC" runat="server"></span>
                                            </Content>
                                            <LoadingPanelClientTemplate>
                                                <table style="height: 205px; width: 100%;" border="0">
                                                    <tr>
                                                        <td style="text-align: center;">
                                                            <table border="0" style="width: 70px; display: inline-block;">
                                                                <tr>
                                                                    <td>
                                                                        <img src="../images/spinner-darkgrey.gif" width="50" height="65" border="0" alt="" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </LoadingPanelClientTemplate>
                                            <ClientEvents>
                                                <CallbackComplete EventHandler="grdRC_onCallbackComplete" />
                                            </ClientEvents>
                                        </ComponentArt:CallBack>
                                    </div>
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

                            <button type="button" id="btnSave2" runat="server" class="btn btn-custon-four btn-success">
                                <i class="fa fa-floppy-o" aria-hidden="true"></i>&nbsp;Save</button>
                            <button type="button" class="btn btn-custon-four btn-warning" id="btnReset2" runat="server">
                                <i class="fa fa-repeat edu-danger-error" aria-hidden="true"></i>&nbsp;Reset
                            </button>
                            <button type="button" class="btn btn-custon-four btn-danger" id="btnClose2" runat="server">
                                <i class="fa fa-times" aria-hidden="true"></i>&nbsp;Close</button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <input type="hidden" id="hdnID" runat="server" value="0" />
        <input type="hidden" id="hdnError" runat="server" value="" />
        <input type="hidden" id="hdnModality" runat="server" value="" />
        <input type="hidden" id="hdnCategory" runat="server" value="" />
        <input type="hidden" id="hdnService" runat="server" value="" />
    </form>
</body>
<script type="text/javascript">
    var objhdnID = document.getElementById('<%=hdnID.ClientID %>');
    var objhdnError = document.getElementById('<%=hdnError.ClientID %>');
    var objhdnModality = document.getElementById('<%=hdnModality.ClientID %>');
    var objhdnCategory = document.getElementById('<%=hdnCategory.ClientID %>');
    var objhdnService = document.getElementById('<%=hdnService.ClientID %>');
    var objbtnSave1 = document.getElementById('<%=btnSave1.ClientID %>');
    var objbtnSave2 = document.getElementById('<%=btnSave2.ClientID %>');
    var strForm = "VRSGlCodeMap";
</script>
<script src="../scripts/custome-javascript.js" type="text/javascript"></script>
<script src="../scripts/AppPages.js" type="text/javascript"></script>
<script src="scripts/GlCodeMap.js?01022021" type="text/javascript"></script>
</html>
