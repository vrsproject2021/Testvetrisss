<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VRSStatusAuditTrail.aspx.cs" Inherits="VETRIS.HouseKeeping.VRSStatusAuditTrail" %>

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

    <link href="../css/CalendarStyle.css" rel="stylesheet" />
    <link href="../css/grid_style.css?1" rel="stylesheet" />

    <script src="../scripts/jquery-1.7.1.js"></script>
    <script type="text/javascript">
        function grdStat_onCallbackComplete(sender, eventArgs) {
            grdStat.Width = "99%";
            parent.adjustFrameHeight();
            var strErr = parent.Trim(document.getElementById("hdnCBErr").value);
            if (strErr != "") {
                objdivMsg.innerHTML = "<font color='red'>" + strErr + "</font>";
                SlideDown();
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
            <div class="sparklineHeader mt-b-10 marginTP10">
                <div class="sparkline10-hd">
                    <div class="row">
                        <div class="col-sm-8 col-xs-12">
                            <h2>
                               Status Audit Trail
                            </h2>
                        </div>
                        <div class="col-sm-4 col-xs-12 text-right">
                            <button type="button" class="btn btn-custon-four btn-danger" id="btnClose" runat="server">
                                <i class="fa fa-times edu-danger-error" aria-hidden="true"></i>&nbsp;Close
                                       
                            </button>
                        </div>
                    </div>
                </div>
            </div>
            <div class="sparkline10-list mt-b-10">
                <div class="sparkline10-hd">
                    <div class="row">
                        <div class="col-sm-12 col-xs-12">
                            
                                <asp:Label ID="lblSUID" runat="server" Text="Study UID :&nbsp;" Font-Size="14px" Font-Bold="true"></asp:Label>
                                 <asp:Label ID="lblHdr" runat="server" Text="" Font-Size="14px"></asp:Label>
                            
                        </div>
                       
                    </div>
                </div>
                </div>
            <div class="sparkline10-list mt-b-10">
                <div class="sparkline10-graph">
                    <div class="static-table-list">
                        <div class="table-responsive">
                            <ComponentArt:CallBack ID="CallBackStat" runat="server" OnCallback="CallBackStat_Callback">
                                <Content>
                                    <ComponentArt:Grid
                                        ID="grdStat"
                                        CssClass="Grid"
                                        DataAreaCssClass="GridData5_1"
                                        SearchOnKeyPress="true"
                                        EnableViewState="true"
                                        RunningMode="Client"
                                        ShowSearchBox="false"
                                        SearchBoxPosition="TopLeft"
                                        SearchTextCssClass="GridHeaderText"
                                        ShowHeader="false"
                                        FooterCssClass="GridFooter"
                                        GroupingNotificationText=""
                                        PageSize="5"
                                        PagerPosition="BottomRight"
                                        PagerInfoPosition="BottomLeft"
                                        PagerStyle="Buttons"
                                        PagerTextCssClass="GridFooterText"
                                        PagerButtonWidth="24"
                                        PagerButtonHeight="24"
                                        PagerButtonHoverEnabled="true"
                                        SliderHeight="26"
                                        SliderWidth="150"
                                        SliderGripWidth="9"
                                        SliderPopupOffsetX="80"
                                        SliderPopupClientTemplateId="SliderTemplate"
                                        SliderPopupCachedClientTemplateId="CachedSliderTemplate"
                                        ImagesBaseUrl="../images/"
                                        PagerImagesFolderUrl="../images/pager/"
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
                                                DataKeyField="status_id_from"
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

                                                    <ComponentArt:GridConditionalFormat ClientFilter="((DataItem.get_index() + grdStat.get_recordOffset()) % 2) > 0" RowCssClass="AltRow" SelectedRowCssClass="SelectedRow" />
                                                </ConditionalFormats>
                                                <Columns>
                                                    <ComponentArt:GridColumn DataField="status_id_from" Align="left" HeadingText="status_id_from" AllowGrouping="false" Visible="false" />
                                                    <ComponentArt:GridColumn DataField="from_status" Align="left" HeadingText="From" AllowGrouping="false" Width="80" />
                                                    <ComponentArt:GridColumn DataField="status_id_to" Align="left" HeadingText="status_id_to" AllowGrouping="false" Visible="false" />
                                                    <ComponentArt:GridColumn DataField="to_status" Align="left" HeadingText="To" AllowGrouping="false" Width="80" />
                                                    <ComponentArt:GridColumn DataField="date_updated" Align="left" HeadingText="Updated On/At" AllowGrouping="false" Width="120" />
                                                    <ComponentArt:GridColumn DataField="updated_by" Align="left" HeadingText="Updated By" AllowGrouping="false" Width="150" />
                                                </Columns>

                                            </ComponentArt:GridLevel>
                                        </Levels>


                                    </ComponentArt:Grid>
                                    <span id="spnERR" runat="server"></span>
                                </Content>
                                <LoadingPanelClientTemplate>
                                    <table style="height: 235px; width: 100%;" border="0">
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
                                    <CallbackComplete EventHandler="grdStat_onCallbackComplete" />
                                </ClientEvents>
                            </ComponentArt:CallBack>
                        </div>
                    </div>
                </div>
            </div>

            <div class="sparkline10-hd">
                <div class="row">
                    <div class="col-sm-12 col-xs-12">
                        <div class="col-xs-12 text-center margin-top-20" style="display: none; color: red;" id="divMsg">
                        </div>
                        

                    </div>

                </div>
            </div>
        </div>
         <input type="hidden" id="hdnID" runat="server" value="00000000-0000-0000-0000-000000000000" />
        <input type="hidden" id="hdnError" runat="server" value="" />
    </form>
</body>
<script type="text/javascript">
    var objhdnID = document.getElementById('<%=hdnID.ClientID %>');
    var objhdnError = document.getElementById('<%=hdnError.ClientID %>');
    var objdivMsg = document.getElementById("divMsg");
    var strForm = "VRSStatusAuditTrail";

</script>
<script src="../scripts/custome-javascript.js"></script>
<script src="scripts/StatusAuditTrail.js"></script>
</html>
