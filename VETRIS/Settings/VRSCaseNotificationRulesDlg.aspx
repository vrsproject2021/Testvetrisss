<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VRSCaseNotificationRulesDlg.aspx.cs" Inherits="VETRIS.Settings.VRSCaseNotificationRulesDlg" %>

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
    <link href="../css/theme.css" rel="stylesheet" />
    <link href="../css/grid_style.css" rel="stylesheet" />--%>
    <link id="lnkSTYLE" runat="server" href = "../css/style.css" rel="stylesheet" type="text/css" />
    <link id="lnkTHEME" runat="server" href="../css/theme.css" rel="stylesheet"  type="text/css"/>
    <link id="lnkGRID" runat="server" href = "../css/grid_style.css" rel="stylesheet" type="text/css" />
    <script src="../scripts/jquery-1.7.1.js"></script>
    <script src="scripts/NotificationRulesDlgHdr.js?04032020"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
            <div class="sparklineHeader mt-b-10 marginTP10">
                <div class="sparkline10-hd">
                    <div class="row">
                        <div class="col-sm-6 col-xs-12">
                            <h2>Notification Rule Details</h2>
                        </div>
                        <div class="col-sm-6 col-xs-12 text-right">
                            <button type="button" id="btnAdd1" runat="server" class="btn btn-custon-four btn-primary">
                                <i class="fa fa-plus" aria-hidden="true"></i>&nbsp;Add New</button>
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

            <div class="sparkline10-list mt-b-10">
                <div class="searchSection">
                    <div class="row">
                        <div class="col-sm-12 col-xs-12">
                            <div class="pull-left">
                                <h3 class="h3Text">Details</h3>
                            </div>
                            <div class="borderSearch pull-left"></div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-3 col-xs-12">
                            <div class="form-group">
                                <label class="control-label" for="usermodel">Rule #</label>
                                <asp:TextBox ID="txtRuleNo" runat="server" CssClass="form-control" ReadOnly="true" Text="0" MaxLength="3" Width="30%" Style="text-align: center;"></asp:TextBox>

                            </div>
                        </div>
                        <div class="col-sm-9 col-xs-12">
                            <div class="form-group">
                                <label class="control-label" for="usermodel">Description</label>
                                <asp:TextBox ID="txtDesc" runat="server" CssClass="form-control" MaxLength="500"></asp:TextBox>

                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-3 col-xs-12">
                            <div class="form-select-list">
                                <label class="control-label">Study Status<span class="mandatory">*</span></label>
                                <asp:DropDownList ID="ddlStudyStatus" runat="server" CssClass="form-control custom-select-value"></asp:DropDownList>
                            </div>

                        </div>
                        <div class="col-sm-3 col-xs-12">
                            <div class="form-group">
                                <label class="control-label" for="usermodel">Priority<span class="mandatory">*</span></label>
                                <asp:DropDownList ID="ddlPriority" runat="server" CssClass="form-control custom-select-value"></asp:DropDownList>

                            </div>
                        </div>
                        <div class="col-sm-3 col-xs-12">
                            <div class="form-group">
                                <%--<label class="control-label" for="usermodel">Notify After (Hours:Minutes)&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</label>--%>
                                <div class="pull-left grid_option1 customRadio">
                                    <asp:RadioButton ID="rdoNotifyE" runat="server" GroupName="grpNotify" />
                                    <label for="rdoNotifyE" class="label-default" style="width: auto; margin-top: 10px;"></label>
                                </div>
                                <span class="pull-left" style="padding-left: 0px; padding-top: 5px; padding-right: 5px;">Notify After (Hours:Minutes)&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp&nbsp;&nbsp;&nbsp;&nbsp;</span>
                                <asp:DropDownList ID="ddlEllapseHr" runat="server" CssClass="form-control custom-select-value pull-left" Width="25%"></asp:DropDownList>
                                <span class="pull-left" style="margin-top: 10px;">&nbsp;:&nbsp;</span>
                                <asp:DropDownList ID="ddlEllapseMin" runat="server" CssClass="form-control custom-select-value pull-left" Width="25%"></asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-sm-3 col-xs-12">
                            <div class="form-group">
                                <%--<label class="control-label" for="usermodel">Notify After (Hours:Minutes)&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</label>--%>
                                <div class="pull-left grid_option1 customRadio">
                                    <asp:RadioButton ID="rdoNotifyL" runat="server" GroupName="grpNotify" />
                                    <label for="rdoNotifyL" class="label-default" style="width: auto; margin-top: 10px;"></label>
                                </div>
                                <span class="pull-left" style="padding-left: 0px; padding-top: 5px; padding-right: 5px;">Notify Before (Hours:Minutes)&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>
                                <asp:DropDownList ID="ddlLeftHr" runat="server" CssClass="form-control custom-select-value pull-left" Width="25%"></asp:DropDownList>
                                <span class="pull-left" style="margin-top: 10px;">&nbsp;:&nbsp;</span>
                                <asp:DropDownList ID="ddlLeftMin" runat="server" CssClass="form-control custom-select-value pull-left" Width="25%"></asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-3 col-xs-12">
                            <div class="form-group1">
                                <label class="control-label">Active ?</label>
                            </div>
                            <div class="pull-left grid_option1 customRadio">
                                <asp:RadioButton ID="rdoStatYes" runat="server" GroupName="grpStat" />
                                <label for="rdoStatYes" class="label-default" style="width: auto; margin-top: 10px;"></label>
                            </div>
                            <span class="pull-left" style="padding-left: 0px; padding-top: 5px; padding-right: 5px;">Yes</span>
                            <div class="pull-left grid_option1 customRadio">
                                <asp:RadioButton ID="rdoStatNo" runat="server" GroupName="grpStat" />
                                <label for="rdoStatNo" class="label-default" style="width: auto; margin-top: 10px;"></label>
                            </div>
                            <span class="pull-left" style="padding-left: 0px; padding-top: 5px; padding-right: 5px;">No</span>
                        </div>

                    </div>

                    <div class="searchSection marginTP10">
                        <div class="row">
                            <div class="col-sm-12 col-xs-12">
                                <div class="pull-left">
                                    <h3 class="h3Text">Radiologists</h3>
                                </div>
                                <div class="borderSearch pull-left"></div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-12 col-xs-12">
                                <div class="table-responsive">
                                    <ComponentArt:CallBack ID="CallBackRadiologist" runat="server" OnCallback="CallBackRadiologist_Callback">
                                        <Content>
                                            <ComponentArt:Grid
                                                ID="grdRad"
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
                                                        SortAscendingImageUrl="col-asc.png"
                                                        SortDescendingImageUrl="col-desc.png"
                                                        SortImageWidth="10"
                                                        SortImageHeight="19">
                                                        <ConditionalFormats>
                                                            <ComponentArt:GridConditionalFormat ClientFilter="((DataItem.get_index() + grdRad.get_recordOffset()) % 2) > 0" RowCssClass="AltRow" SelectedRowCssClass="AltRow" />
                                                            <ComponentArt:GridConditionalFormat ClientFilter="((DataItem.get_index() + grdRad.get_recordOffset()) % 2) == 0" RowCssClass="Row" SelectedRowCssClass="Row" />
                                                        </ConditionalFormats>
                                                        <Columns>
                                                            <ComponentArt:GridColumn DataField="radiologist_id" Align="left" HeadingText="#" AllowGrouping="false" Width="30" FixedWidth="true" Visible="false" />
                                                            <ComponentArt:GridColumn DataField="user_id" Align="left" HeadingText="user_id" AllowGrouping="false" Visible="false" />
                                                            <ComponentArt:GridColumn DataField="name" Align="left" HeadingText="Name" AllowGrouping="false" Width="300" />
                                                            <ComponentArt:GridColumn DataField="notify_if_scheduled" Align="center" AllowGrouping="false" AllowSorting="false" DataCellClientTemplateId="RADSCHSEL" HeadingText="Notify If Scheduled ?" FixedWidth="True" Width="120" />
                                                            <ComponentArt:GridColumn DataField="notify_always" Align="center" AllowGrouping="false" AllowSorting="false" DataCellClientTemplateId="RADALWSEL" HeadingText="Notify Always ?" FixedWidth="True" Width="90" />
                                                        </Columns>

                                                    </ComponentArt:GridLevel>
                                                </Levels>

                                                <ClientEvents>
                                                    <RenderComplete EventHandler="grdRad_onRenderComplete" />
                                                </ClientEvents>
                                                <ClientTemplates>
                                                    <ComponentArt:ClientTemplate ID="RADSCHSEL">
                                                        <div class="grid_option">
                                                            <input type="checkbox" id="chkRadSchedule_## DataItem.GetMember('radiologist_id').Value ##" style="width: 18px; height: 18px;" onclick="javascript: chkRadSchedule_OnClick('## DataItem.GetMember('radiologist_id').Value ##');" />
                                                            <label for="chkRadSchedule_## DataItem.GetMember('radiologist_id').Value ##" class="label-default" style="width: auto; margin-top: 10px;"></label>
                                                        </div>
                                                    </ComponentArt:ClientTemplate>
                                                    <ComponentArt:ClientTemplate ID="RADALWSEL">
                                                        <div class="grid_option">
                                                            <input type="checkbox" id="chkRadAlways_## DataItem.GetMember('radiologist_id').Value ##" style="width: 18px; height: 18px;" onclick="javascript: chkRadAlways_OnClick('## DataItem.GetMember('radiologist_id').Value ##');" />
                                                            <label for="chkRadAlways_## DataItem.GetMember('radiologist_id').Value ##" class="label-default" style="width: auto; margin-top: 10px;"></label>
                                                        </div>
                                                    </ComponentArt:ClientTemplate>
                                                </ClientTemplates>
                                            </ComponentArt:Grid>
                                            <span id="spnRADErr" runat="server"></span>
                                        </Content>
                                        <LoadingPanelClientTemplate>
                                            <table style="height: 170px; width: 100%;" border="0">
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
                                            <CallbackComplete EventHandler="grdRad_onCallbackComplete" />
                                        </ClientEvents>
                                    </ComponentArt:CallBack>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="searchSection marginTP10">
                        <div class="row">
                            <div class="col-sm-12 col-xs-12">
                                <div class="pull-left">
                                    <h3 class="h3Text">Other Recipient Matrix</h3>
                                </div>
                                <div class="borderSearch pull-left"></div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-12 col-xs-12">
                                <div class="table-responsive">
                                    <ComponentArt:CallBack ID="CallBackMatrix" runat="server" OnCallback="CallBackMatrix_Callback">
                                        <Content>
                                            <ComponentArt:Grid
                                                ID="grdMatrix"
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
                                                        DataMember="UserRoles"
                                                        DataKeyField="user_role_id"
                                                        ShowTableHeading="false"
                                                        TableHeadingCssClass="GridHeader"
                                                        RowCssClass="Row"
                                                        HoverRowCssClass="HoverRow"
                                                        SelectedRowCssClass="SelectedRow"
                                                        ColumnReorderIndicatorImageUrl="reorder.gif"
                                                        DataCellCssClass="DataCell"
                                                        HeadingCellCssClass="HeadingCell"
                                                        HeadingRowCssClass="HeadingRow"
                                                        HeadingTextCssClass="HeadingCellText"
                                                        SortedDataCellCssClass="SortedDataCell"
                                                        SortAscendingImageUrl="col-asc.png"
                                                        SortDescendingImageUrl="col-desc.png"
                                                        SortImageWidth="10"
                                                        SortImageHeight="19"
                                                        SelectorCellWidth="20"
                                                        ShowSelectorCells="false">
                                                        <ConditionalFormats>
                                                            <ComponentArt:GridConditionalFormat ClientFilter="((DataItem.get_index() + grdMatrix.get_recordOffset()) % 2) > 0" RowCssClass="AltRow" SelectedRowCssClass="SelectedRow" HoverRowCssClass="HoverRow" />
                                                        </ConditionalFormats>
                                                        <Columns>
                                                            <ComponentArt:GridColumn DataField="user_role_id" Align="left" HeadingText="user_role_id" AllowGrouping="false" Visible="false" />
                                                            <ComponentArt:GridColumn DataField="code" Align="left" HeadingText="code" AllowGrouping="false" Visible="false" />
                                                            <ComponentArt:GridColumn DataField="name" Align="left" HeadingText="Recepient Type" AllowGrouping="false" Width="150" />
                                                            <ComponentArt:GridColumn DataField="scheduled" Align="center" HeadingText="Scheduled Only?" AllowGrouping="false" Width="100" DataCellClientTemplateId="SCHEDULE" FixedWidth="True" AllowSorting="False" Visible="false" />
                                                            <ComponentArt:GridColumn DataField="notify_all" Align="center" HeadingText="Notify All ?" AllowGrouping="false" Width="70" DataCellClientTemplateId="NOTIFY" FixedWidth="True" AllowSorting="False" />
                                                            <ComponentArt:GridColumn DataField="recepient_count" Align="center" HeadingText="Non Scheduled Recepient Count" AllowGrouping="false" Width="190" DataCellClientTemplateId="RCPTCOUNT" FixedWidth="True" AllowSorting="False" />
                                                        </Columns>

                                                    </ComponentArt:GridLevel>
                                                    <ComponentArt:GridLevel
                                                        AllowGrouping="false"
                                                        DataMember="Users"
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
                                                            <ComponentArt:GridConditionalFormat ClientFilter="((DataItem.get_index() + grdMatrix.get_recordOffset()) % 2) > 0" RowCssClass="AltRow" SelectedRowCssClass="SelectedRow" />

                                                        </ConditionalFormats>
                                                        <Columns>
                                                            <ComponentArt:GridColumn DataField="rec_id" Align="left" HeadingText="rec_id" AllowGrouping="false" Visible="false" />
                                                            <ComponentArt:GridColumn DataField="user_id" Align="left" HeadingText="user_id" AllowGrouping="false" Visible="false" />
                                                            <ComponentArt:GridColumn DataField="user_role_id" Align="left" HeadingText="user_role_id" AllowGrouping="false" Visible="false" />
                                                            <ComponentArt:GridColumn DataField="user_name" Align="left" HeadingText="Recepient" AllowGrouping="false" Width="200" />
                                                            <ComponentArt:GridColumn DataField="sel" Align="center" HeadingText="Select" AllowGrouping="false" AllowSorting="false" DataCellClientTemplateId="SEL" FixedWidth="True" Width="100" />

                                                        </Columns>

                                                    </ComponentArt:GridLevel>
                                                </Levels>
                                                <ClientEvents>
                                                    <RenderComplete EventHandler="grdMatrix_onRenderComplete" />
                                                    <ItemExpand EventHandler="grdMatrix_onItemExpand" />
                                                    <ItemCollapse EventHandler="grdMatrix_onItemCollapse" />
                                                </ClientEvents>
                                                <ClientTemplates>
                                                    <ComponentArt:ClientTemplate ID="SCHEDULE">
                                                        <div class="grid_option">
                                                            <input type="checkbox" id="chkSchedule_## DataItem.GetMember('user_role_id').Value ##" style="width: 18px; height: 18px; display: inline;" onclick="javascript: ChkSchedule_OnClick('## DataItem.GetMember('user_role_id').Value ##');" />
                                                        </div>
                                                    </ComponentArt:ClientTemplate>
                                                    <ComponentArt:ClientTemplate ID="NOTIFY">
                                                        <div class="grid_option">
                                                            <input type="checkbox" id="chkNotifyAll_## DataItem.GetMember('user_role_id').Value ##" style="width: 18px; height: 18px;" onclick="javascript: ChkNotifyAll_OnClick('## DataItem.GetMember('user_role_id').Value ##');" />
                                                        </div>
                                                    </ComponentArt:ClientTemplate>
                                                    <ComponentArt:ClientTemplate ID="RCPTCOUNT">

                                                        <a href="javascript:void(0);" id="lnkCnt_## DataItem.GetMember('user_role_id').Value ##" style="color: blue; text-decoration: underline;" onclick="javascript:Count_OnClick('## DataItem.GetMember('user_role_id').Value ##');">## DataItem.GetMember('recepient_count').Value ##</a>
                                                    </ComponentArt:ClientTemplate>
                                                    <ComponentArt:ClientTemplate ID="SEL">
                                                        <div class="grid_option">
                                                            <input type="checkbox" id="chkSel_## DataItem.GetMember('rec_id').Value ##" style="width: 18px; height: 18px;" onclick="javascript: ChkSel_OnClick('## DataItem.GetMember('rec_id').Value ##');" />
                                                        </div>
                                                    </ComponentArt:ClientTemplate>

                                                </ClientTemplates>
                                            </ComponentArt:Grid>
                                            <span id="spnERR" runat="server"></span>
                                        </Content>
                                        <LoadingPanelClientTemplate>
                                            <table style="height: 380px; width: 100%;" border="0">
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
                                            <CallbackComplete EventHandler="grdMatrix_onCallbackComplete" />
                                        </ClientEvents>
                                    </ComponentArt:CallBack>
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
                                <button type="button" class="btn btn-custon-four btn-primary" id="btnAdd2" runat="server">
                                    <i class="fa fa-plus" aria-hidden="true"></i>&nbsp;Add New</button>
                                <button type="button" class="btn btn-custon-four btn-success" id="btnSave2" runat="server">
                                    <i class="fa fa-floppy-o" aria-hidden="true"></i>&nbsp; Save</button>
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
        </div>
        <input type="hidden" id="hdnError" runat="server" value="" />
        <input type="hidden" id="hdnID" runat="server" value="0" />
    </form>
</body>
<script type="text/javascript">
    var objhdnID = document.getElementById('<%=hdnID.ClientID %>');
    var objhdnError = document.getElementById('<%=hdnError.ClientID %>');
    var objddlStudyStatus = document.getElementById('<%=ddlStudyStatus.ClientID %>');//--
    var objtxtRuleNo = document.getElementById('<%=txtRuleNo.ClientID %>');//--
    var objtxtDesc = document.getElementById('<%=txtDesc.ClientID %>');//--
    var objddlPriority = document.getElementById('<%=ddlPriority.ClientID %>');//--
    var objrdoNotifyE = document.getElementById('<%=rdoNotifyE.ClientID %>');
    var objddlEllapseHr = document.getElementById('<%=ddlEllapseHr.ClientID %>');
    var objddlEllapseMin = document.getElementById('<%=ddlEllapseMin.ClientID %>');
    var objrdoNotifyL = document.getElementById('<%=rdoNotifyL.ClientID %>');
    var objddlLeftHr = document.getElementById('<%=ddlLeftHr.ClientID %>');
    var objddlLeftMin = document.getElementById('<%=ddlLeftMin.ClientID %>');
    var objrdoStatYes = document.getElementById('<%=rdoStatYes.ClientID %>');//---
    var objrdoStatNo = document.getElementById('<%=rdoStatNo.ClientID %>');//---
    var strForm = "VRSCaseNotificationRulesDlg";
</script>
<script src="../scripts/custome-javascript.js"></script>
<script src="../scripts/AppPages.js"></script>
<script src="scripts/NotificationRulesDlg.js?04032020"></script>
</html>
