<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VRSCaseNotificationRules.aspx.cs" Inherits="VETRIS.Settings.VRSCaseNotificationRules" %>
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
    <link href="../css/style.css" rel="stylesheet" />
    <link href="../css/grid_style.css" rel="stylesheet" />

    <script src="../scripts/jquery-1.7.1.js"></script>
    <script src="scripts/CaseNotificationRulesHdr.js"></script>
</head>
<body>
    <form id="form1" runat="server">
     <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
            <div class="sparklineHeader mt-b-10 marginTP10">
                <div class="sparkline10-hd">
                    <div class="row">
                        <div class="col-sm-4 col-xs-12">
                            <h2>Notification Rules</h2>
                        </div>
                        <div class="col-sm-8 col-xs-12 text-right">
                            <button type="button" class="btn btn-custon-four btn-success" id="btnSave" runat="server">
                                <i class="fa fa-floppy-o edu-danger-error" aria-hidden="true"></i>&nbsp;Save
                                       
                            </button>
                            <button type="button" class="btn btn-custon-four btn-danger" id="btnClose" runat="server">
                                <i class="fa fa-times edu-danger-error" aria-hidden="true"></i>&nbsp;Close
                                       
                            </button>
                        </div>
                    </div>
                </div>
            </div>

            



            <div class="sparkline10-list mt-b-10">
                <div class="sparkline10-graph">
                    <div class="static-table-list">
                        <div class="table-responsive">
                            <ComponentArt:CallBack ID="CallBackBrw" runat="server" OnCallback="CallBackBrw_Callback">
                                <Content>
                                    <ComponentArt:Grid
                                        ID="grdBrw"
                                        CssClass="Grid"
                                        DataAreaCssClass="GridData20"
                                        SearchOnKeyPress="true"
                                        EnableViewState="true"
                                        RunningMode="Client"
                                        ShowSearchBox="false"
                                        SearchBoxPosition="TopLeft"
                                        SearchTextCssClass="GridHeaderText"
                                        ShowHeader="false"
                                        FooterCssClass="GridFooter"
                                        GroupingNotificationText=""
                                        PageSize="23"
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
                                                DataKeyField="rule_no"
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
                                                    <ComponentArt:GridConditionalFormat ClientFilter="((DataItem.get_index() + grdBrw.get_recordOffset()) % 2) > 0" RowCssClass="AltRow" SelectedRowCssClass="AltRow" />
                                                </ConditionalFormats>
                                                <Columns>
                                                    <ComponentArt:GridColumn DataField="rule_no" Align="left" HeadingText="#" AllowGrouping="false" Width="20" />
                                                    <ComponentArt:GridColumn DataField="pacs_status_id" Align="left" HeadingText="pacs_status_id" AllowGrouping="false" Visible="false" />
                                                    <ComponentArt:GridColumn DataField="status_desc" Align="left" HeadingText="Study Status" AllowGrouping="false" Width="75" />
                                                    <ComponentArt:GridColumn DataField="priority_id" Align="left" HeadingText="priority_id" AllowGrouping="false" Visible="false" />
                                                    <ComponentArt:GridColumn DataField="priority_desc" Align="left" HeadingText="Priority" AllowGrouping="false" Width="100" />
                                                    <ComponentArt:GridColumn DataField="time_ellapsed_mins" Align="center" HeadingText="Time Ellapsed (Mins.)" AllowGrouping="true" DataCellClientTemplateId="TIME" FixedWidth="True" Width="130" />
                                                    <ComponentArt:GridColumn DataField="default_user_role" Align="left" HeadingText="default_user_role" AllowGrouping="false" Visible="false" />
                                                    <ComponentArt:GridColumn DataField="user_role" Align="left" HeadingText="Default Receipients" AllowGrouping="false" Width="120" />
                                                    <ComponentArt:GridColumn DataField="second_level_alert_receipient_id" Align="left" HeadingText="Second Level Receipients(Types)" AllowGrouping="true" DataCellClientTemplateId="USERS" FixedWidth="True" Width="300" />
                                                    <ComponentArt:GridColumn DataField="changed" Align="left" HeadingText="changed" AllowGrouping="false" Visible="false" />
                                                    <ComponentArt:GridColumn DataField="receipient_type" Align="left" HeadingText="receipient_type" AllowGrouping="false" Visible="false" />
                                                </Columns>

                                            </ComponentArt:GridLevel>
                                        </Levels>
                                        <ClientEvents>
                                            <RenderComplete EventHandler="grdBrw_onRenderComplete" />
                                        </ClientEvents>

                                        <ClientTemplates>

                                            <ComponentArt:ClientTemplate ID="TIME">
                                                <input type="text" id="txtTime_## DataItem.GetMember('rule_no').Value ##" class="GridTextBox" value="## DataItem.GetMember('time_ellapsed_mins').Value ##" maxlength="3" style="width: 95%;text-align:center;" onchange="javascript:txtTime_OnChange('## DataItem.GetMember('rule_no').Value ##');" onfocus="javascript:this.select();" onkeypress="javascript:return parent.CheckInteger(event);" onblur="javascript:ResetValueInteger(this,'0');" />
                                            </ComponentArt:ClientTemplate>
                                            <ComponentArt:ClientTemplate ID="USERS">
                                                <select id="ddlUser_## DataItem.GetMember('rule_no').Value ##" class="form-control custom-select-value" style="width:70%;float:left;" onchange="javascript:ddlUser_OnChange('## DataItem.GetMember('rule_no').Value ##');">
                                                </select>
                                                &nbsp;(<span id="spnUserType_## DataItem.GetMember('rule_no').Value ##">## DataItem.GetMember('receipient_type').Value ##</span>)
                                            </ComponentArt:ClientTemplate>
                                         
                                        </ClientTemplates>
                                    </ComponentArt:Grid>
                                    <span id="spnERR" runat="server"></span>
                                </Content>
                                <LoadingPanelClientTemplate>
                                    <table style="height: 830px; width: 100%;" border="0">
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

        <input type="hidden" id="hdnError" runat="server" value="" />
        <input type="hidden" id="hdnID" runat="server" value="00000000-0000-0000-0000-000000000000" />
        <input type="hidden" id="hdnAdmins" runat="server" value="" />
        <input type="hidden" id="hdnRadiologists" runat="server" value="" />
    </form>
</body>
<script type="text/javascript">
    var objhdnID = document.getElementById('<%=hdnID.ClientID %>');
    var objhdnError = document.getElementById('<%=hdnError.ClientID %>');
    var objhdnAdmins = document.getElementById('<%=hdnAdmins.ClientID %>');
    var objhdnRadiologists = document.getElementById('<%=hdnRadiologists.ClientID %>');
    var strForm = "VRSStudyTypes";
</script>
<script src="../scripts/custome-javascript.js"></script>
<script src="../scripts/AppPages.js"></script>
<script src="scripts/CaseNotificationRules.js"></script>
</html>
