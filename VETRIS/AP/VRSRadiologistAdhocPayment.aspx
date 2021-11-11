<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VRSRadiologistAdhocPayment.aspx.cs" Inherits="VETRIS.AP.VRSRadiologistAdhocPayment" %>

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
    <script src="../scripts/jquery.soverlay.min.js"></script>
    <script src="scripts/RadiologistAdhocPaymentHdr.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
            <div class="sparklineHeader mt-b-10 marginTP10">
                <div class="sparkline10-hd">
                    <div class="row">
                        <div class="col-sm-7 col-xs-12">
                            <h2>Adhoc Payment
                                
                            </h2>
                        </div>

                        <div class="col-sm-5 col-xs-12 text-right">
                            <button type="button" class="btn btn-custon-four btn-primary" id="btnSave1" runat="server">
                                <i class="fa-floppy-o edu-danger-error" aria-hidden="true"></i>&nbsp;Save       
                            </button>

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

            <div class="sparkline10-list mt-b-10">
                <div class="row">

                    <div class="col-sm-12 col-xs-12">
                        <div class="searchSection">
                            <div class="row marginTP10">
                                <div class="col-sm-12 col-xs-12 marginMobileTP5">
                                    <div class="table-responsive">
                                        <ComponentArt:CallBack ID="CallBackPmt" runat="server" OnCallback="CallBackPmt_Callback">
                                            <Content>
                                                <ComponentArt:Grid
                                                    ID="grdPmt"
                                                    CssClass="Grid"
                                                    DataAreaCssClass="GridData5"
                                                    SearchOnKeyPress="true"
                                                    EnableViewState="true"
                                                    ShowSearchBox="true"
                                                    SearchBoxPosition="TopRight"
                                                    SearchTextCssClass="GridHeaderText" AutoFocusSearchBox="false"
                                                    ShowHeader="true"
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
                                                            SortedDataCellCssClass="SortedDataCell"
                                                            SortAscendingImageUrl="col-asc.png"
                                                            SortDescendingImageUrl="col-desc.png"
                                                            SortImageWidth="10"
                                                            SortImageHeight="19">
                                                            <ConditionalFormats>
                                                                <ComponentArt:GridConditionalFormat ClientFilter="((DataItem.get_index() + grdPmt.get_recordOffset()) % 2) > 0" RowCssClass="AltRow" SelectedRowCssClass="AltRow" />
                                                                <ComponentArt:GridConditionalFormat ClientFilter="((DataItem.get_index() + grdPmt.get_recordOffset()) % 2) == 0" RowCssClass="Row" SelectedRowCssClass="Row" />
                                                            </ConditionalFormats>
                                                            <Columns>
                                                                <ComponentArt:GridColumn DataField="id" Align="left" HeadingText="id" AllowGrouping="false" Visible="false" />
                                                                <ComponentArt:GridColumn DataField="name" Align="left" HeadingText="Adhoc Head" AllowGrouping="false" Width="150" />
                                                                <ComponentArt:GridColumn DataField="adhoc_payment" Align="right" HeadingText="Payment ($)" AllowGrouping="false" Width="100" AllowSorting="False" DataCellClientTemplateId="PAYMENT" FixedWidth="true" />
                                                                <ComponentArt:GridColumn DataField="remarks" Align="Left" HeadingText="Remarks" AllowGrouping="false" Width="250" AllowSorting="False" DataCellClientTemplateId="REMARKS" FixedWidth="true" />
                                                            </Columns>

                                                        </ComponentArt:GridLevel>
                                                    </Levels>
                                                    <ClientEvents>
                                                        <RenderComplete EventHandler="grdPmt_onRenderComplete" />
                                                    </ClientEvents>
                                                    <ClientTemplates>
                                                        <ComponentArt:ClientTemplate ID="PAYMENT">
                                                            <input type="text" id="txtAmt_## DataItem.GetMember('id').Value ##" class="GridTextBox" value="## DataItem.GetMember('adhoc_payment').Value ##" style="width: 95%; text-align: right;" onchange="javascript:txtAmt_OnChange('## DataItem.GetMember('id').Value ##');" onfocus="javascript:this.select();" onkeypress="javascript:return parent.CheckDecimal(event);" onblur="javascript:ResetValueDecimal(this);" />
                                                        </ComponentArt:ClientTemplate>
                                                        <ComponentArt:ClientTemplate ID="REMARKS">
                                                            <input type="text" id="txtRemarks_## DataItem.GetMember('id').Value ##" class="GridTextBox" value="## DataItem.GetMember('remarks').Value ##" maxlength="250" style="width: 99%;" onchange="javascript:txtRemarks_OnChange('## DataItem.GetMember('id').Value ##');" />
                                                        </ComponentArt:ClientTemplate>

                                                    </ClientTemplates>

                                                </ComponentArt:Grid>
                                                <span id="spnErr" runat="server"></span>
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
                                                <CallbackComplete EventHandler="grdPmt_onCallbackComplete" />
                                            </ClientEvents>
                                        </ComponentArt:CallBack>
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
                            <button type="button" class="btn btn-custon-four btn-primary" id="btnSave2" runat="server">
                                <i class="fa-floppy-o edu-danger-error" aria-hidden="true"></i>&nbsp;Save      
                            </button>

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
        <input type="hidden" id="hdnRadID" runat="server" value="00000000-0000-0000-0000-000000000000" />
        <input type="hidden" id="hdnCycleID" runat="server" value="00000000-0000-0000-0000-000000000000" />
        <input type="hidden" id="hdnUserID" runat="server" value="00000000-0000-0000-0000-000000000000" />
        <input type="hidden" id="hdnMenuID" runat="server" value="0" />
    </form>
</body>
<script type="text/javascript">
    var objhdnID = document.getElementById('<%=hdnID.ClientID %>');
    var objhdnError = document.getElementById('<%=hdnError.ClientID %>');
    var objhdnRadID = document.getElementById('<%=hdnRadID.ClientID %>');
    var objhdnCycleID = document.getElementById('<%=hdnCycleID.ClientID %>');
    var objhdnUserID = document.getElementById('<%=hdnUserID.ClientID %>');
    var objhdnMenuID = document.getElementById('<%=hdnMenuID.ClientID %>');
    var objlblMsg = document.getElementById('<%=lblMsg.ClientID %>');
    var strForm = "VRSRadiologistAdhocPayment";

</script>
<script src="../scripts/custome-javascript.js"></script>
<script src="scripts/RadiologistAdhocPayment.js"></script>
</html>
