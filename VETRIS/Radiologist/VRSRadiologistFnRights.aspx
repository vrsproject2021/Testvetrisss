<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VRSRadiologistFnRights.aspx.cs" Inherits="VETRIS.Radiologist.VRSRadiologistFnRights" %>

<%@ Register Assembly="ComponentArt.Web.UI" Namespace="ComponentArt.Web.UI" TagPrefix="ComponentArt" %>
<%@ OutputCache Location="None" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta http-equiv="x-ua-compatible" content="ie=edge" />
    <meta name="description" content="" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title></title>
    <link href="https://fonts.googleapis.com/css?family=Roboto:100,300,400,700,900" rel="stylesheet" />
    <link href="../css/bootstrap.min.css" rel="stylesheet" />
    <link href="../css/font-awesome.min.css" rel="stylesheet" />
    <link href="../css/responsive.css" rel="stylesheet" />
    <%--<link href="../css/style.css" rel="stylesheet" />

    <link href="../css/theme.css" rel="stylesheet" />
    <link href="../css/grid_style.css?1" rel="stylesheet" />--%>
    <link id="lnkSTYLE" runat="server" href = "../css/style.css" rel="stylesheet" type="text/css" />
    <link id="lnkTHEME" runat="server" href="../css/theme.css" rel="stylesheet"  type="text/css"/>
    <link id="lnkGRID" runat="server" href = "../css/grid_style.css" rel="stylesheet" type="text/css" />
    <script src="../scripts/jquery.min.js"></script>
    <script src="scripts/RadiologistFnRightsHdr.js?v=<%=DateTime.Now.Ticks%>"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
            <div class="sparklineHeader mt-b-10 marginTP10">
                <div class="sparkline10-hd">
                    <div class="row">
                        <div class="col-sm-6 col-xs-12">
                            <h2>Functional Rights</h2>
                        </div>
                        <div class="col-sm-6 col-xs-12 text-right">
                            <button type="button" class="btn btn-custon-four btn-success" id="btnSave1" runat="server">
                                <i class="fa fa-floppy-o edu-danger-error" aria-hidden="true"></i>&nbsp;Save     
                            </button>
                            <button type="button" class="btn btn-custon-four btn-warning" id="btnReset1" runat="server">
                                <i class="fa fa-repeat edu-danger-error" aria-hidden="true"></i>&nbsp;Reset   
                            </button>
                            <button type="button" id="btnClose1" runat="server" class="btn btn-custon-four btn-danger">
                                <i class="fa fa-times" aria-hidden="true"></i>&nbsp;Close</button>
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

                                    <label class="control-label" for="usermodel" style="font-weight: bold;">Radiologist Name : &nbsp;</label>
                                    <asp:Label ID="lblName" runat="server"></asp:Label>

                                </div>


                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="sparkline10-list mt-b-10">
                <div class="row">
                    <div class="col-sm-6 col-xs-12">
                        <div class="searchSection">
                            <div class="row">
                                <div class="col-sm-12 col-xs-12">
                                    <div class="pull-left">
                                        <h3 class="h3Text">Functional Rights</h3>
                                    </div>
                                </div>

                                <div class="col-sm-12 col-xs-12">
                                    <div class="borderSearch pull-left"></div>
                                </div>
                            </div>

                            <div class="static-table-list">
                                <div class="table-responsive">
                                    <ComponentArt:CallBack ID="CallBackRights" runat="server" OnCallback="CallBackRights_Callback">
                                        <Content>
                                            <ComponentArt:Grid
                                                ID="grdRights"
                                                CssClass="Grid"
                                                DataAreaCssClass="GridData4"
                                                SearchOnKeyPress="true"
                                                EnableViewState="true"
                                                ShowSearchBox="true"
                                                SearchBoxPosition="TopLeft"
                                                SearchTextCssClass="GridHeaderText" AutoFocusSearchBox="false"
                                                ShowHeader="false"
                                                FooterCssClass="GridFooter"
                                                GroupingNotificationText=""
                                                PageSize="4"
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
                                                GroupingNotificationPosition="TopRight">

                                                <Levels>
                                                    <ComponentArt:GridLevel
                                                        AllowGrouping="false"
                                                        DataKeyField="right_code"
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
                                                            <ComponentArt:GridConditionalFormat ClientFilter="((DataItem.get_index() + grdRights.get_recordOffset()) % 2) > 0" RowCssClass="AltRow" SelectedRowCssClass="AltRow" />
                                                            <ComponentArt:GridConditionalFormat ClientFilter="((DataItem.get_index() + grdRights.get_recordOffset()) % 2) == 0" RowCssClass="Row" SelectedRowCssClass="Row" />
                                                        </ConditionalFormats>
                                                        <Columns>
                                                            <ComponentArt:GridColumn DataField="right_code" Align="left" HeadingText="right_code" AllowGrouping="false" Width="30" Visible="false" />
                                                            <ComponentArt:GridColumn DataField="sel" Align="center" AllowGrouping="false" AllowSorting="False" DataCellClientTemplateId="SELRIGHT"  HeadingCellClientTemplateId="SELRTHDR" HeadingText="Select" FixedWidth="True" Width="40" />
                                                            <ComponentArt:GridColumn DataField="right_desc" Align="left" HeadingText="Rights" AllowGrouping="false" Width="300" />
                                                        </Columns>

                                                    </ComponentArt:GridLevel>
                                                </Levels>

                                                <ClientEvents>
                                                    <RenderComplete EventHandler="grdRights_onRenderComplete" />
                                                </ClientEvents>
                                                <ClientTemplates>
                                                     <ComponentArt:ClientTemplate ID="SELRTHDR">
                                                            <div class="grid_option">
                                                                <input type="checkbox" id="chkSelRightHdr" style="width: 18px; height: 18px;" onclick="javascript:chkSelRightHdr_OnClick();" />
                                                                <label for="chkSelRightHdr" class="label-default" style="width: auto; margin-top: 10px;"></label>
                                                            </div>
                                                        </ComponentArt:ClientTemplate>
                                                    <ComponentArt:ClientTemplate ID="SELRIGHT">
                                                        <div class="grid_option">
                                                            <input type="checkbox" id="chkSelRight_## DataItem.GetMember('right_code').Value ##" style="width: 18px; height: 18px;" onclick="javascript: chkSelRight_OnClick('## DataItem.GetMember('right_code').Value ##');" />
                                                            <label for="chkSelRight_## DataItem.GetMember('right_code').Value ##" class="label-default" style="width: auto; margin-top: 10px;"></label>
                                                        </div>
                                                    </ComponentArt:ClientTemplate>
                                                </ClientTemplates>
                                            </ComponentArt:Grid>
                                            <span id="spnERR" runat="server"></span>

                                        </Content>
                                        <LoadingPanelClientTemplate>
                                            <table style="height: 170px; width: 100%;" border="0">
                                                <tr>
                                                    <td style="text-align: center;">
                                                        <table border="0" style="width: 70px; display: inline-block;">
                                                            <tr>
                                                                <td>
                                                                    <img src="../images/spinner-darkgrey.gif" border="0" alt="" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </LoadingPanelClientTemplate>
                                        <ClientEvents>
                                            <CallbackComplete EventHandler="grdRights_onCallbackComplete" />
                                        </ClientEvents>
                                    </ComponentArt:CallBack>


                                </div>
                            </div>

                        </div>
                    </div>
                    <div class="col-sm-6 col-xs-12">
                        <div class="searchSection">
                            <div class="row">
                                <div class="col-sm-12 col-xs-12">
                                    <div class="pull-left">
                                        <h3 class="h3Text">Modality Rights</h3>
                                    </div>
                                </div>

                                <div class="col-sm-12 col-xs-12">
                                    <div class="borderSearch pull-left"></div>
                                </div>
                            </div>

                            <div class="static-table-list">
                                <div class="table-responsive">
                                    <ComponentArt:CallBack ID="CallBackModality" runat="server" OnCallback="CallBackModality_Callback">
                                        <Content>
                                            <ComponentArt:Grid
                                                ID="grdModality"
                                                CssClass="Grid"
                                                DataAreaCssClass="GridData4"
                                                SearchOnKeyPress="true"
                                                EnableViewState="true"
                                                ShowSearchBox="true"
                                                SearchBoxPosition="TopLeft"
                                                SearchTextCssClass="GridHeaderText" AutoFocusSearchBox="false"
                                                ShowHeader="false"
                                                FooterCssClass="GridFooter"
                                                GroupingNotificationText=""
                                                PageSize="4"
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
                                                GroupingNotificationPosition="TopRight">

                                                <Levels>
                                                    <ComponentArt:GridLevel
                                                        AllowGrouping="false"
                                                        DataKeyField="modality_id"
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
                                                            <ComponentArt:GridConditionalFormat ClientFilter="((DataItem.get_index() + grdModality.get_recordOffset()) % 2) > 0" RowCssClass="AltRow" SelectedRowCssClass="AltRow" />
                                                            <ComponentArt:GridConditionalFormat ClientFilter="((DataItem.get_index() + grdModality.get_recordOffset()) % 2) == 0" RowCssClass="Row" SelectedRowCssClass="Row" />
                                                        </ConditionalFormats>
                                                        <Columns>
                                                            <ComponentArt:GridColumn DataField="modality_id" Align="left" HeadingText="modality_id" AllowGrouping="false" Width="30" Visible="false" />
                                                            <ComponentArt:GridColumn DataField="sel" Align="center" AllowGrouping="false" AllowSorting="False" DataCellClientTemplateId="SELMOD"  HeadingCellClientTemplateId="SELMODHDR" HeadingText="Select" FixedWidth="True" Width="40" />
                                                            <ComponentArt:GridColumn DataField="modality" Align="left" HeadingText="Modality" AllowGrouping="false" Width="300" />
                                                        </Columns>

                                                    </ComponentArt:GridLevel>
                                                </Levels>

                                                <ClientEvents>
                                                    <RenderComplete EventHandler="grdModality_onRenderComplete" />
                                                </ClientEvents>
                                                <ClientTemplates>
                                                    <ComponentArt:ClientTemplate ID="SELMODHDR">
                                                            <div class="grid_option">
                                                                <input type="checkbox" id="chkSelModHdr" style="width: 18px; height: 18px;" onclick="javascript:chkSelModHdr_OnClick();" />
                                                                <label for="chkSelModHdr" class="label-default" style="width: auto; margin-top: 10px;"></label>
                                                            </div>
                                                        </ComponentArt:ClientTemplate>
                                                    <ComponentArt:ClientTemplate ID="SELMOD">
                                                        <div class="grid_option">
                                                            <input type="checkbox" id="chkSelModality_## DataItem.GetMember('modality_id').Value ##" style="width: 18px; height: 18px;" onclick="javascript: chkSelModality_OnClick('## DataItem.GetMember('modality_id').Value ##');" />
                                                            <label for="chkSelModality_## DataItem.GetMember('modality_id').Value ##" class="label-default" style="width: auto; margin-top: 10px;"></label>
                                                        </div>
                                                    </ComponentArt:ClientTemplate>
                                                </ClientTemplates>
                                            </ComponentArt:Grid>
                                            <span id="spnModErr" runat="server"></span>

                                        </Content>
                                        <LoadingPanelClientTemplate>
                                            <table style="height: 170px; width: 100%;" border="0">
                                                <tr>
                                                    <td style="text-align: center;">
                                                        <table border="0" style="width: 70px; display: inline-block;">
                                                            <tr>
                                                                <td>
                                                                    <img src="../images/spinner-darkgrey.gif" border="0" alt="" />
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
                <div class="row marginTP10">
                    <div class="col-sm-6 col-xs-12">
                        <div class="searchSection">


                            <div class="row">
                                <div class="col-sm-12 col-xs-12">
                                    <div class="pull-left">
                                        <h3 class="h3Text">Study Type List</h3>
                                    </div>
                                </div>

                                <div class="col-sm-12 col-xs-12">
                                    <div class="borderSearch pull-left"></div>
                                </div>
                            </div>


                            <div class="static-table-list">
                                <div class="table-responsive">
                                    <ComponentArt:CallBack ID="CallBackST" runat="server" OnCallback="CallBackST_Callback">
                                        <Content>
                                            <ComponentArt:Grid
                                                ID="grdST"
                                                CssClass="Grid"
                                                DataAreaCssClass="GridData5_1"
                                                SearchOnKeyPress="true"
                                                EnableViewState="true"
                                                RunningMode="Client"
                                                ShowSearchBox="true"
                                                SearchBoxPosition="TopRight"
                                                SearchTextCssClass="GridHeaderText"
                                                ShowHeader="true"
                                                FooterCssClass="GridFooter"
                                                GroupingNotificationText=""
                                                PageSize="6"
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
                                                        DataKeyField="study_type_id"
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
                                                            <ComponentArt:GridConditionalFormat ClientFilter="((DataItem.get_index() + grdST.get_recordOffset()) % 2) > 0" RowCssClass="AltRow" SelectedRowCssClass="AltRow" />
                                                        </ConditionalFormats>
                                                        <Columns>

                                                            <ComponentArt:GridColumn DataField="study_type_id" Align="left" HeadingText="id" AllowGrouping="false" Visible="false" />
                                                            <ComponentArt:GridColumn DataField="sel" Align="center" HeadingText="Select" AllowGrouping="false" AllowSorting="false" DataCellClientTemplateId="SELST" HeadingCellClientTemplateId="SELSTHDR" FixedWidth="True" Width="50" />
                                                            <ComponentArt:GridColumn DataField="modality" Align="left" HeadingText="Modality" AllowGrouping="false" DataCellClientTemplateId="STModality" FixedWidth="True" Width="120" />
                                                            <ComponentArt:GridColumn DataField="study_type" Align="left" HeadingText="Study Type" AllowGrouping="false" Width="200" />

                                                        </Columns>

                                                    </ComponentArt:GridLevel>
                                                </Levels>
                                                <ClientEvents>
                                                    <RenderComplete EventHandler="grdST_onRenderComplete" />
                                                </ClientEvents>

                                                <ClientTemplates>
                                                    <ComponentArt:ClientTemplate ID="SELSTHDR">
                                                            <div class="grid_option">
                                                                <input type="checkbox" id="chkSelSTHdr" style="width: 18px; height: 18px;" onclick="javascript:chkSelSTHdr_OnClick();" />
                                                                <label for="chkSelSTHdr" class="label-default" style="width: auto; margin-top: 10px;"></label>
                                                            </div>
                                                        </ComponentArt:ClientTemplate>
                                                    <ComponentArt:ClientTemplate ID="STModality">
                                                        <span id="spnSTM_## DataItem.GetMember('study_type_id').Value ##" title="## DataItem.GetMember('modality').Value ##">## DataItem.GetMember('modality').Value ##</span>
                                                    </ComponentArt:ClientTemplate>
                                                    <ComponentArt:ClientTemplate ID="SELST">
                                                        <div class="grid_option">
                                                            <input type="checkbox" id="chkSelST_## DataItem.GetMember('study_type_id').Value ##" style="width: 18px; height: 18px;" onclick="javascript:chkSelST_OnClick('## DataItem.GetMember('study_type_id').Value ##');" />
                                                            <label for="chkSelST_## DataItem.GetMember('study_type_id').Value ##" class="label-default" style="width: auto; margin-top: 10px;"></label>
                                                        </div>
                                                    </ComponentArt:ClientTemplate>
                                                </ClientTemplates>
                                            </ComponentArt:Grid>
                                            <span id="spnSTErr" runat="server"></span>
                                        </Content>
                                        <LoadingPanelClientTemplate>
                                            <table style="height: 205px; width: 100%;" border="0">
                                                <tr>
                                                    <td style="text-align: center;">
                                                        <table border="0" style="width: 70px; display: inline-block;">
                                                            <tr>
                                                                <td>
                                                                    <img src="../images/spinner-darkgrey.gif" border="0" alt="" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </LoadingPanelClientTemplate>
                                        <ClientEvents>
                                            <CallbackComplete EventHandler="grdST_onCallbackComplete" />
                                        </ClientEvents>
                                    </ComponentArt:CallBack>
                                </div>
                            </div>


                        </div>

                    </div>
                    <div class="col-sm-6 col-xs-12">
                        <div class="searchSection">
                            <div class="row">
                                <div class="col-sm-12 col-xs-12">
                                    <div class="pull-left">
                                        <h3 class="h3Text">Exception Study Type List</h3>
                                    </div>
                                </div>

                                <div class="col-sm-12 col-xs-12">
                                    <div class="borderSearch pull-left"></div>
                                </div>
                            </div>

                            <div class="static-table-list">
                                <div class="table-responsive">
                                    <ComponentArt:CallBack ID="CallBackSelST" runat="server" OnCallback="CallBackSelST_Callback">
                                        <Content>
                                            <ComponentArt:Grid
                                                ID="grdSelST"
                                                CssClass="Grid"
                                                DataAreaCssClass="GridData5_1"
                                                SearchOnKeyPress="true"
                                                EnableViewState="true"
                                                RunningMode="Client"
                                                ShowSearchBox="false"
                                                SearchBoxPosition="TopRight"
                                                SearchTextCssClass="GridHeaderText"
                                                ShowHeader="true"
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
                                                        DataKeyField="study_type_id"
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
                                                            <ComponentArt:GridConditionalFormat ClientFilter="((DataItem.get_index() + grdSelST.get_recordOffset()) % 2) > 0" RowCssClass="AltRow" SelectedRowCssClass="AltRow" />
                                                        </ConditionalFormats>
                                                        <Columns>

                                                            <ComponentArt:GridColumn DataField="study_type_id" Align="left" HeadingText="id" AllowGrouping="false" Visible="false" />
                                                            <ComponentArt:GridColumn DataField="modality" Align="left" HeadingText="Modality" AllowGrouping="false" DataCellClientTemplateId="SELSTMOD" FixedWidth="True" Width="120" />
                                                            <ComponentArt:GridColumn DataField="study_type" Align="left" HeadingText="Study Type" AllowGrouping="false" Width="200" />

                                                        </Columns>

                                                    </ComponentArt:GridLevel>
                                                </Levels>


                                                <ClientTemplates>
                                                    <ComponentArt:ClientTemplate ID="SELSTMOD">
                                                        <span id="spnSTMOD_## DataItem.GetMember('study_type_id').Value ##" title="## DataItem.GetMember('modality').Value ##">## DataItem.GetMember('modality').Value ##</span>
                                                    </ComponentArt:ClientTemplate>

                                                </ClientTemplates>
                                            </ComponentArt:Grid>
                                            <span id="spnErrSelST" runat="server"></span>
                                        </Content>
                                        <LoadingPanelClientTemplate>
                                            <table style="height: 205px; width: 100%;" border="0">
                                                <tr>
                                                    <td style="text-align: center;">
                                                        <table border="0" style="width: 70px; display: inline-block;">
                                                            <tr>
                                                                <td>
                                                                    <img src="../images/spinner-darkgrey.gif" border="0" alt="" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </LoadingPanelClientTemplate>
                                        <ClientEvents>
                                            <CallbackComplete EventHandler="grdST_onCallbackComplete" />
                                        </ClientEvents>
                                    </ComponentArt:CallBack>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row marginTP10">

                    <div class="col-sm-6 col-xs-12">
                        <div class="searchSection">

                            <div class="row">
                                <div class="col-sm-12 col-xs-12">
                                    <div class="pull-left">
                                        <h3 class="h3Text">Institution List</h3>
                                    </div>
                                </div>

                                <div class="col-sm-12 col-xs-12">
                                    <div class="borderSearch pull-left"></div>
                                </div>
                            </div>


                            <div class="static-table-list">
                                <div class="table-responsive">
                                    <ComponentArt:CallBack ID="CallBackInst" runat="server" OnCallback="CallBackInst_Callback">
                                        <Content>
                                            <ComponentArt:Grid
                                                ID="grdInst"
                                                CssClass="Grid"
                                                DataAreaCssClass="GridData5_1"
                                                SearchOnKeyPress="true"
                                                EnableViewState="true"
                                                RunningMode="Client"
                                                ShowSearchBox="true"
                                                SearchBoxPosition="TopRight"
                                                SearchTextCssClass="GridHeaderText"
                                                ShowHeader="true"
                                                FooterCssClass="GridFooter"
                                                GroupingNotificationText=""
                                                PageSize="6"
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
                                                        DataKeyField="institution_id"
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
                                                            <ComponentArt:GridConditionalFormat ClientFilter="((DataItem.get_index() + grdInst.get_recordOffset()) % 2) > 0" RowCssClass="AltRow" SelectedRowCssClass="AltRow" />
                                                        </ConditionalFormats>
                                                        <Columns>

                                                            <ComponentArt:GridColumn DataField="institution_id" Align="left" HeadingText="id" AllowGrouping="false" Visible="false" />
                                                            <ComponentArt:GridColumn DataField="sel" Align="center" HeadingText="Select" AllowSorting="false" AllowGrouping="false" DataCellClientTemplateId="SELINST" HeadingCellClientTemplateId="SELINSTHDR" FixedWidth="True" Width="50" />
                                                            <ComponentArt:GridColumn DataField="institution_code" Align="left" HeadingText="Code" AllowGrouping="false" Width="80" />
                                                            <ComponentArt:GridColumn DataField="institution_name" Align="left" HeadingText="Name" AllowGrouping="false" Width="250" />

                                                        </Columns>
                                                    </ComponentArt:GridLevel>
                                                </Levels>
                                                <ClientEvents>
                                                    <RenderComplete EventHandler="grdInst_onRenderComplete" />
                                                </ClientEvents>

                                                <ClientTemplates>
                                                     <ComponentArt:ClientTemplate ID="SELINSTHDR">
                                                            <div class="grid_option">
                                                                <input type="checkbox" id="chkSelInstHdr" style="width: 18px; height: 18px;" onclick="javascript:chkSelInstHdr_OnClick();" />
                                                                <label for="chkSelInstHdr" class="label-default" style="width: auto; margin-top: 10px;"></label>
                                                            </div>
                                                        </ComponentArt:ClientTemplate>
                                                    <ComponentArt:ClientTemplate ID="SELINST">
                                                        <div class="grid_option">
                                                            <input type="checkbox" id="chkSelInst_## DataItem.GetMember('institution_id').Value ##" style="width: 18px; height: 18px;" onclick="javascript: chkSelInst_OnClick('## DataItem.GetMember('institution_id').Value ##');" />
                                                            <label for="chkSelInst_## DataItem.GetMember('institution_id').Value ##" class="label-default" style="width: auto; margin-top: 10px;"></label>
                                                        </div>
                                                    </ComponentArt:ClientTemplate>
                                                </ClientTemplates>
                                            </ComponentArt:Grid>
                                            <span id="spnInstERR" runat="server"></span>
                                        </Content>
                                        <LoadingPanelClientTemplate>
                                            <table style="height: 205px; width: 100%;" border="0">
                                                <tr>
                                                    <td style="text-align: center;">
                                                        <table border="0" style="width: 70px; display: inline-block;">
                                                            <tr>
                                                                <td>
                                                                    <img src="../images/spinner-darkgrey.gif" border="0" alt="" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </LoadingPanelClientTemplate>
                                        <ClientEvents>
                                            <CallbackComplete EventHandler="grdInst_onCallbackComplete" />
                                        </ClientEvents>
                                    </ComponentArt:CallBack>
                                </div>
                            </div>

                        </div>

                    </div>
                    <div class="col-sm-6 col-xs-12">
                        <div class="searchSection">

                            <div class="row">
                                <div class="col-sm-12 col-xs-12">
                                    <div class="pull-left">
                                        <h3 class="h3Text">Exception Institution List</h3>
                                    </div>
                                </div>

                                <div class="col-sm-12 col-xs-12">
                                    <div class="borderSearch pull-left"></div>
                                </div>
                            </div>


                            <div class="static-table-list">
                                <div class="table-responsive">
                                    <ComponentArt:CallBack ID="CallBackSelInst" runat="server" OnCallback="CallBackSelInst_Callback">
                                        <Content>
                                            <ComponentArt:Grid
                                                ID="grdSelInst"
                                                CssClass="Grid"
                                                DataAreaCssClass="GridData5_1"
                                                SearchOnKeyPress="true"
                                                EnableViewState="true"
                                                RunningMode="Client"
                                                ShowSearchBox="false"
                                                SearchBoxPosition="TopRight"
                                                SearchTextCssClass="GridHeaderText"
                                                ShowHeader="true"
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
                                                        DataKeyField="institution_id"
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
                                                            <ComponentArt:GridConditionalFormat ClientFilter="((DataItem.get_index() + grdSelInst.get_recordOffset()) % 2) > 0" RowCssClass="AltRow" SelectedRowCssClass="AltRow" />
                                                        </ConditionalFormats>
                                                        <Columns>

                                                            <ComponentArt:GridColumn DataField="institution_id" Align="left" HeadingText="id" AllowGrouping="false" Visible="false" />
                                                            <ComponentArt:GridColumn DataField="institution_code" Align="left" HeadingText="Code" AllowGrouping="false" Width="80" />
                                                            <ComponentArt:GridColumn DataField="institution_name" Align="left" HeadingText="Name" AllowGrouping="false" Width="250" />

                                                        </Columns>

                                                    </ComponentArt:GridLevel>
                                                </Levels>

                                            </ComponentArt:Grid>
                                            <span id="spnSelInstERR" runat="server"></span>
                                        </Content>
                                        <LoadingPanelClientTemplate>
                                            <table style="height: 205px; width: 100%;" border="0">
                                                <tr>
                                                    <td style="text-align: center;">
                                                        <table border="0" style="width: 70px; display: inline-block;">
                                                            <tr>
                                                                <td>
                                                                    <img src="../images/spinner-darkgrey.gif" border="0" alt="" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </LoadingPanelClientTemplate>
                                        <ClientEvents>
                                            <CallbackComplete EventHandler="grdSelInst_onCallbackComplete" />
                                        </ClientEvents>
                                    </ComponentArt:CallBack>
                                </div>
                            </div>

                        </div>
                    </div>


                </div>
                <div class="row marginTP10">

                    <div class="col-sm-6 col-xs-12">
                        <div class="searchSection">

                            <div class="row">
                                <div class="col-sm-12 col-xs-12">
                                    <div class="pull-left">
                                        <h3 class="h3Text">Radiologist List</h3>
                                    </div>
                                </div>

                                <div class="col-sm-12 col-xs-12">
                                    <div class="borderSearch pull-left"></div>
                                </div>
                            </div>


                            <div class="static-table-list">
                                <div class="table-responsive">
                                    <ComponentArt:CallBack ID="CallBackRad" runat="server" OnCallback="CallBackRad_Callback">
                                        <Content>
                                            <ComponentArt:Grid
                                                ID="grdRad"
                                                CssClass="Grid"
                                                DataAreaCssClass="GridData5_1"
                                                SearchOnKeyPress="true"
                                                EnableViewState="true"
                                                RunningMode="Client"
                                                ShowSearchBox="true"
                                                SearchBoxPosition="TopRight"
                                                SearchTextCssClass="GridHeaderText"
                                                ShowHeader="true"
                                                FooterCssClass="GridFooter"
                                                GroupingNotificationText=""
                                                PageSize="6"
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
                                                        DataKeyField="radiologist_id"
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
                                                            <ComponentArt:GridConditionalFormat ClientFilter="((DataItem.get_index() + grdRad.get_recordOffset()) % 2) > 0" RowCssClass="AltRow" SelectedRowCssClass="AltRow" />
                                                        </ConditionalFormats>
                                                        <Columns>

                                                            <ComponentArt:GridColumn DataField="radiologist_id" Align="left" HeadingText="id" AllowGrouping="false" Visible="false" />
                                                            <ComponentArt:GridColumn DataField="sel" Align="center" HeadingText="Select" AllowSorting="false" AllowGrouping="false" DataCellClientTemplateId="SELRAD" HeadingCellClientTemplateId="SELRADHDR" FixedWidth="True" Width="50" />
                                                            <ComponentArt:GridColumn DataField="radiologist_code" Align="left" HeadingText="Code" AllowGrouping="false" Width="80" />
                                                            <ComponentArt:GridColumn DataField="radiologist_name" Align="left" HeadingText="Name" AllowGrouping="false" Width="250" />

                                                        </Columns>
                                                    </ComponentArt:GridLevel>
                                                </Levels>
                                                <ClientEvents>
                                                    <RenderComplete EventHandler="grdRad_onRenderComplete" />
                                                </ClientEvents>

                                                <ClientTemplates>
                                                     <ComponentArt:ClientTemplate ID="SELRADHDR">
                                                            <div class="grid_option">
                                                                <input type="checkbox" id="chkSelRadHdr" style="width: 18px; height: 18px;" onclick="javascript:chkSelRadHdr_OnClick();" />
                                                                <label for="chkSelRadHdr" class="label-default" style="width: auto; margin-top: 10px;"></label>
                                                            </div>
                                                        </ComponentArt:ClientTemplate>
                                                    <ComponentArt:ClientTemplate ID="SELRAD">
                                                        <div class="grid_option">
                                                            <input type="checkbox" id="chkSelRad_## DataItem.GetMember('radiologist_id').Value ##" style="width: 18px; height: 18px;" onclick="javascript: chkSelRad_OnClick('## DataItem.GetMember('radiologist_id').Value ##');" />
                                                            <label for="chkSelRad_## DataItem.GetMember('radiologist_id').Value ##" class="label-default" style="width: auto; margin-top: 10px;"></label>
                                                        </div>
                                                    </ComponentArt:ClientTemplate>
                                                </ClientTemplates>
                                            </ComponentArt:Grid>
                                            <span id="spnRadERR" runat="server"></span>
                                        </Content>
                                        <LoadingPanelClientTemplate>
                                            <table style="height: 205px; width: 100%;" border="0">
                                                <tr>
                                                    <td style="text-align: center;">
                                                        <table border="0" style="width: 70px; display: inline-block;">
                                                            <tr>
                                                                <td>
                                                                    <img src="../images/spinner-darkgrey.gif" border="0" alt="" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </LoadingPanelClientTemplate>
                                        <ClientEvents>
                                            <CallbackComplete EventHandler="grdRad_onCallbackComplete" />
                                        </ClientEvents>
                                    </ComponentArt:CallBack>
                                </div>
                            </div>

                        </div>

                    </div>
                    <div class="col-sm-6 col-xs-12">
                        <div class="searchSection">

                            <div class="row">
                                <div class="col-sm-12 col-xs-12">
                                    <div class="pull-left">
                                        <h3 class="h3Text">Radiologist whose study can be worked on</h3>
                                    </div>
                                </div>

                                <div class="col-sm-12 col-xs-12">
                                    <div class="borderSearch pull-left"></div>
                                </div>
                            </div>


                            <div class="static-table-list">
                                <div class="table-responsive">
                                    <ComponentArt:CallBack ID="CallBackSelRad" runat="server" OnCallback="CallBackSelRad_Callback">
                                        <Content>
                                            <ComponentArt:Grid
                                                ID="grdSelRad"
                                                CssClass="Grid"
                                                DataAreaCssClass="GridData5_1"
                                                SearchOnKeyPress="true"
                                                EnableViewState="true"
                                                RunningMode="Client"
                                                ShowSearchBox="false"
                                                SearchBoxPosition="TopRight"
                                                SearchTextCssClass="GridHeaderText"
                                                ShowHeader="true"
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
                                                        DataKeyField="radiologist_id"
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
                                                            <ComponentArt:GridConditionalFormat ClientFilter="((DataItem.get_index() + grdSelRad.get_recordOffset()) % 2) > 0" RowCssClass="AltRow" SelectedRowCssClass="AltRow" />
                                                        </ConditionalFormats>
                                                        <Columns>

                                                            <ComponentArt:GridColumn DataField="radiologist_id" Align="left" HeadingText="id" AllowGrouping="false" Visible="false" />
                                                            <ComponentArt:GridColumn DataField="radiologist_code" Align="left" HeadingText="Code" AllowGrouping="false" Width="80" />
                                                            <ComponentArt:GridColumn DataField="radiologist_name" Align="left" HeadingText="Name" AllowGrouping="false" Width="250" />

                                                        </Columns>

                                                    </ComponentArt:GridLevel>
                                                </Levels>

                                            </ComponentArt:Grid>
                                            <span id="spnSelRadERR" runat="server"></span>
                                        </Content>
                                        <LoadingPanelClientTemplate>
                                            <table style="height: 205px; width: 100%;" border="0">
                                                <tr>
                                                    <td style="text-align: center;">
                                                        <table border="0" style="width: 70px; display: inline-block;">
                                                            <tr>
                                                                <td>
                                                                    <img src="../images/spinner-darkgrey.gif" border="0" alt="" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </LoadingPanelClientTemplate>
                                        <ClientEvents>
                                            <CallbackComplete EventHandler="grdSelRad_onCallbackComplete" />
                                        </ClientEvents>
                                    </ComponentArt:CallBack>
                                </div>
                            </div>

                        </div>
                    </div>


                </div>

                <div class="row marginTP10">
                    <div class="col-sm-6 col-xs-12">
                        <div class="searchSection">
                            <div class="row">
                                <div class="col-sm-12 col-xs-12">
                                    <div class="pull-left">
                                        <h3 class="h3Text">Species</h3>
                                    </div>
                                </div>

                                <div class="col-sm-12 col-xs-12">
                                    <div class="borderSearch pull-left"></div>
                                </div>
                            </div>

                            <div class="static-table-list">
                                <div class="table-responsive">
                                    <ComponentArt:CallBack ID="CallBackSpecies" runat="server" OnCallback="CallBackSpecies_Callback">
                                        <Content>
                                            <ComponentArt:Grid
                                                ID="grdSpecies"
                                                CssClass="Grid"
                                                DataAreaCssClass="GridData4"
                                                SearchOnKeyPress="true"
                                                EnableViewState="true"
                                                ShowSearchBox="true"
                                                SearchBoxPosition="TopLeft"
                                                SearchTextCssClass="GridHeaderText" AutoFocusSearchBox="false"
                                                ShowHeader="false"
                                                FooterCssClass="GridFooter"
                                                GroupingNotificationText=""
                                                PageSize="4"
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
                                                GroupingNotificationPosition="TopRight">

                                                <Levels>
                                                    <ComponentArt:GridLevel
                                                        AllowGrouping="false"
                                                        DataKeyField="species_id"
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
                                                            <ComponentArt:GridConditionalFormat ClientFilter="((DataItem.get_index() + grdSpecies.get_recordOffset()) % 2) > 0" RowCssClass="AltRow" SelectedRowCssClass="AltRow" />
                                                            <ComponentArt:GridConditionalFormat ClientFilter="((DataItem.get_index() + grdSpecies.get_recordOffset()) % 2) == 0" RowCssClass="Row" SelectedRowCssClass="Row" />
                                                        </ConditionalFormats>
                                                        <Columns>
                                                            <ComponentArt:GridColumn DataField="species_id" Align="left" HeadingText="species_id" AllowGrouping="false" Width="30" Visible="false" />
                                                            <ComponentArt:GridColumn DataField="sel" Align="center" AllowGrouping="false" AllowSorting="False" DataCellClientTemplateId="SELSPECIES"  HeadingCellClientTemplateId="SELSPECIESHDR" HeadingText="Select" FixedWidth="True" Width="50" />
                                                            <ComponentArt:GridColumn DataField="species_code" Align="left" HeadingText="Code" AllowGrouping="false" Width="80" />
                                                            <ComponentArt:GridColumn DataField="species_name" Align="left" HeadingText="Name" AllowGrouping="false" Width="250" />
                                                        </Columns>

                                                    </ComponentArt:GridLevel>
                                                </Levels>

                                                <ClientEvents>
                                                    <RenderComplete EventHandler="grdSpecies_onRenderComplete" />
                                                </ClientEvents>
                                                <ClientTemplates>
                                                    <ComponentArt:ClientTemplate ID="SELSPECIESHDR">
                                                            <div class="grid_option">
                                                                <input type="checkbox" id="chkSelSpeciesHdr" style="width: 18px; height: 18px;" onclick="javascript:chkSelSpeciesHdr_OnClick();" />
                                                                <label for="chkSelSpeciesHdr" class="label-default" style="width: auto; margin-top: 10px;"></label>
                                                            </div>
                                                        </ComponentArt:ClientTemplate>
                                                    <ComponentArt:ClientTemplate ID="SELSPECIES">
                                                        <div class="grid_option">
                                                            <input type="checkbox" id="chkSelSpecies_## DataItem.GetMember('species_id').Value ##" style="width: 18px; height: 18px;" onclick="javascript: chkSelSpecies_OnClick('## DataItem.GetMember('species_id').Value ##');" />
                                                            <label for="chkSelSpecies_## DataItem.GetMember('species_id').Value ##" class="label-default" style="width: auto; margin-top: 10px;"></label>
                                                        </div>
                                                    </ComponentArt:ClientTemplate>
                                                </ClientTemplates>
                                            </ComponentArt:Grid>
                                            <span id="spnSpeciesErr" runat="server"></span>

                                        </Content>
                                        <LoadingPanelClientTemplate>
                                            <table style="height: 170px; width: 100%;" border="0">
                                                <tr>
                                                    <td style="text-align: center;">
                                                        <table border="0" style="width: 70px; display: inline-block;">
                                                            <tr>
                                                                <td>
                                                                    <img src="../images/spinner-darkgrey.gif" border="0" alt="" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </LoadingPanelClientTemplate>
                                        <ClientEvents>
                                            <CallbackComplete EventHandler="grdSpecies_onCallbackComplete" />
                                        </ClientEvents>
                                    </ComponentArt:CallBack>


                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>



            <div class="sparklineHeader mt-b-10 marginTP10">
                <div class="sparkline10-hd">
                    <div class="row">
                        <div class="col-sm-6 hidden-xs">
                        </div>
                        <div class="col-sm-6 col-xs-12 text-right">
                            <button type="button" class="btn btn-custon-four btn-success" id="btnSave2" runat="server">
                                <i class="fa fa-floppy-o edu-danger-error" aria-hidden="true"></i>&nbsp;Save
                                       
                            </button>
                            <button type="button" class="btn btn-custon-four btn-warning" id="btnReset2" runat="server">
                                <i class="fa fa-repeat edu-danger-error" aria-hidden="true"></i>&nbsp;Reset 
                            </button>

                            <button type="button" class="btn btn-custon-four btn-danger" id="btnClose2" runat="server">
                                <i class="fa fa-times" aria-hidden="true"></i>&nbsp; Close</button>
                        </div>
                    </div>
                </div>
            </div>

        </div>

        <input type="hidden" id="hdnError" runat="server" value="" />
        <input type="hidden" id="hdnID" runat="server" value="00000000-0000-0000-0000-000000000000" />
    </form>
</body>
<script type="text/javascript">
    var objhdnID = document.getElementById('<%=hdnID.ClientID %>');
    var objhdnError = document.getElementById('<%=hdnError.ClientID %>');
    var strForm = "VRSRadiologistFnRights";

</script>
<script src="../scripts/custome-javascript.js"></script>
<script src="../scripts/AppPages.js"></script>
<script src="scripts/RadiologistFnRights.js?v=<%=DateTime.Now.Ticks%>"></script>
</html>
