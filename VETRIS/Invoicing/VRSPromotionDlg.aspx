<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VRSPromotionDlg.aspx.cs" Inherits="VETRIS.Invoicing.VRSPromotionDlg" %>

<%@ Register Assembly="ComponentArt.Web.UI" Namespace="ComponentArt.Web.UI" TagPrefix="ComponentArt" %>
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
    <link href="../css/grid_style.css?2" rel="stylesheet" />
    <link href="../css/CalendarStyle.css" rel="stylesheet" />--%>
     <link id="lnkSTYLE" runat="server" href = "../css/style.css" rel="stylesheet" type="text/css" />
    <link id="lnkTHEME" runat="server" href = "../css/theme.css" rel="stylesheet" type="text/css" />
    <link id="lnkGRID" runat="server" href = "../css/grid_style.css" rel="stylesheet" type="text/css" />
    <link id="lnkCAL" runat="server" href = "../css/CalendarStyle.css" rel="stylesheet" type="text/css" />
    <script src="../scripts/jquery-1.7.1.js"></script>
    <script src="scripts/PromotionDlgHdr.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
            <div class="sparklineHeader mt-b-10 marginTP10">
                <div class="sparkline10-hd">
                    <div class="row">
                        <div class="col-sm-6 col-xs-12">
                            <h2>Promotion Details</h2>
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
            <%--------------------------------%>
            <div class="sparkline10-list mt-b-10">
                <div class="searchSection">
                    <div class="row">
                        <div class="col-sm-12 col-xs-12">
                            <div class="pull-left">
                                <h3 class="h3Text">Promotion Details</h3>
                            </div>
                            <div class="borderSearch pull-left"></div>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-sm-4 col-xs-12">
                            <div class="form-group">
                                <label class="control-label">Promotion Type<span class="mandatory">*</span></label>
                                <asp:DropDownList ID="ddlType" runat="server" CssClass="form-control custom-select-value">
                                    <asp:ListItem Value="" Text="Select One" Selected="True"></asp:ListItem>
                                    <asp:ListItem Value="D" Text="Discount"></asp:ListItem>
                                    <asp:ListItem Value="F" Text="Free Credit"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-sm-2 col-xs-12">
                            <div class="form-group">
                                <label class="control-label">Date Created</label>
                                <asp:TextBox ID="txtCreatedDate" runat="server" ReadOnly="true" TabIndex="-1" CssClass="form-control" MaxLength="10" Width="60%"></asp:TextBox>
                            </div>

                        </div>
                        <div class="col-sm-6 col-xs-12">
                            <div class="form-group">
                                <label class="control-label">Reason<span class="mandatory">*</span></label>
                                <asp:DropDownList ID="ddlReason" runat="server" CssClass="form-control custom-select-value">
                                   
                                </asp:DropDownList>
                            </div>
                        </div>
                      
                               
                            
                             
                         
                        
                    </div>

                    <div class="row">
                        <div class="col-sm-4 col-xs-12">
                            <div class="form-group">
                                <label class="control-label" for="usermodality">Billing Account<span class="mandatory">*</span></label>
                                <div class="input-effect">
                                    <asp:DropDownList ID="ddlAccount" runat="server" CssClass="form-control custom-select-value" Enabled="false"></asp:DropDownList>
                                </div>

                            </div>
                        </div>
                          <div class="col-sm-2 col-xs-12">
                            <div class="form-group">
                                <div>
                                    <label class="control-label">Valid From</label>
                                </div>
                                <asp:TextBox ID="txtFromDate" runat="server" TabIndex="-1" CssClass="form-control" MaxLength="10" Width="90px" Style="float: left;"></asp:TextBox>

                                <img src="../images/cal.gif" id="imgFromDt" runat="server" alt="" style="cursor: pointer; margin-top: 5px; margin-left: 5px; display: inline; float: left;" />

                                <ComponentArt:Calendar runat="server" ID="CalFromDate" AllowMonthSelection="false" AllowMultipleSelection="false"
                                    AllowWeekSelection="false" CalendarCssClass="calendar" CalendarTitleCssClass="title"
                                    ControlType="Calendar" DayCssClass="day" DayHeaderCssClass="dayheader" DayHoverCssClass="dayhover"
                                    DayNameFormat="FirstTwoLetters" ImagesBaseUrl="../images/" MonthCssClass="month"
                                    NextImageUrl="cal_nextMonth.gif" NextPrevCssClass="nextprev" OtherMonthDayCssClass="othermonthday"
                                    PopUp="Custom" PopUpExpandControlId="imgFromDt" PrevImageUrl="cal_prevMonth.gif"
                                    SelectedDayCssClass="selectedday" SelectMonthCssClass="selector" SelectMonthText="&curren;"
                                    SelectWeekCssClass="selector" ReactOnSameSelection="true" SelectWeekText="&raquo;"
                                    SwapDuration="300" SwapSlide="Linear">
                                    <ClientEvents>
                                        <SelectionChanged EventHandler="CalFromDate_onSelectionChanged" />
                                    </ClientEvents>
                                </ComponentArt:Calendar>

                            </div>
                        </div>

                        <div class="col-sm-2 col-xs-12">
                            <div class="form-group">
                                 <div>
                                  <label class="control-label">Valid Till</label>
                                 </div>
                                <asp:TextBox ID="txtToDate" runat="server" TabIndex="-1" CssClass="form-control" MaxLength="10" Width="90px" Style="float: left;"></asp:TextBox>
                                <img src="../images/cal.gif" id="imgToDt" runat="server" alt="" style="cursor: pointer; margin-top: 5px; margin-left: 5px;" />
                                <ComponentArt:Calendar runat="server" ID="CalToDate" AllowMonthSelection="false" AllowMultipleSelection="false"
                                    AllowWeekSelection="false" CalendarCssClass="calendar" CalendarTitleCssClass="title"
                                    ControlType="Calendar" DayCssClass="day" DayHeaderCssClass="dayheader" DayHoverCssClass="dayhover"
                                    DayNameFormat="FirstTwoLetters" ImagesBaseUrl="../images/" MonthCssClass="month"
                                    NextImageUrl="cal_nextMonth.gif" NextPrevCssClass="nextprev" OtherMonthDayCssClass="othermonthday"
                                    PopUp="Custom" PopUpExpandControlId="imgToDt" PrevImageUrl="cal_prevMonth.gif"
                                    SelectedDayCssClass="selectedday" SelectMonthCssClass="selector" SelectMonthText="&curren;"
                                    SelectWeekCssClass="selector" ReactOnSameSelection="true" SelectWeekText="&raquo;"
                                    SwapDuration="300" SwapSlide="Linear">
                                    <ClientEvents>
                                        <SelectionChanged EventHandler="CalToDate_onSelectionChanged" />
                                    </ClientEvents>
                                </ComponentArt:Calendar>
                            </div>
                         </div>  
                        <div class="col-sm-4 col-xs-12">
                            <div class="form-group1">
                                <label class="control-label">Active?<span class="mandatory">*</span></label>
                            </div>
                            <div class="pull-left grid_option1 customRadio">
                                <asp:RadioButton ID="rdoStatYes" runat="server" Checked="true" GroupName="grpStat" />
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

                </div>
            </div>

            <div class="sparklineHeader mt-b-10 marginTP10">
                <div class="row marginTP10">
                    <div class="col-sm-12 col-xs-12">
                        <div class="searchSection">
                            <div class="row">
                                <div class="col-sm-12 col-xs-12">
                                    <div class="row">
                                        <div class="col-sm-8 col-xs-8">
                                            <div class="pull-left" style="margin-top: 8px;">
                                                <h3 class="h3Text">Institution Wise Promotion</h3>
                                            </div>
                                        </div>
                                        <div class="col-sm-4 col-xs-4">
                                            <div class="pull-right">
                                                <button type="button" class="btn btn_grd btn-primary" id="btnAddPromo" runat="server" title="click to add new row for promotion details">
                                                    <i class="fa fa-plus " aria-hidden="true"></i>
                                                </button>
                                            </div>
                                        </div>
                                        <div class="col-sm-12 col-xs-12">
                                            <div class="borderSearch pull-left"></div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-sm-12 col-xs-12">
                                    <div class="table-responsive">
                                        <ComponentArt:CallBack ID="CallBackPromo" runat="server" OnCallback="CallBackPromo_Callback">
                                            <Content>
                                                <ComponentArt:Grid
                                                    ID="grdPromo"
                                                    CssClass="Grid"
                                                    DataAreaCssClass="GridData6_1"
                                                    SearchOnKeyPress="true"
                                                    EnableViewState="true"
                                                    RunningMode="Client"
                                                    ShowSearchBox="false"
                                                    SearchBoxPosition="TopLeft"
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
                                                            DataKeyField="line_no"
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
                                                                <ComponentArt:GridConditionalFormat ClientFilter="((DataItem.get_index() + grdPromo.get_recordOffset()) % 2) > 0" RowCssClass="AltRow" SelectedRowCssClass="AltRow" />
                                                                <ComponentArt:GridConditionalFormat ClientFilter="((DataItem.get_index() + grdPromo.get_recordOffset()) % 2) == 0" RowCssClass="Row" SelectedRowCssClass="Row" />
                                                            </ConditionalFormats>
                                                            <Columns>
                                                                <ComponentArt:GridColumn DataField="line_no" Align="left" HeadingText="#" AllowGrouping="false" Width="30" />
                                                                <ComponentArt:GridColumn DataField="id" Align="left" HeadingText="id" AllowGrouping="false" Visible="false" />
                                                                <ComponentArt:GridColumn DataField="institution_id" Align="left" HeadingText="Institution" AllowGrouping="false" Width="300" DataCellClientTemplateId="INST" FixedWidth="true" />
                                                                <ComponentArt:GridColumn DataField="modality_id" Align="left" HeadingText="Modality" AllowGrouping="false" Width="250" DataCellClientTemplateId="MOD" FixedWidth="true" />
                                                                <ComponentArt:GridColumn DataField="discount_percent" Align="left" HeadingText="Discount (%)" AllowGrouping="false" Width="100" DataCellClientTemplateId="DISC" FixedWidth="true" Visible="false" />
                                                                <ComponentArt:GridColumn DataField="free_credits" Align="left" HeadingText="Free Credits" AllowGrouping="false" Width="100" DataCellClientTemplateId="CREDIT" FixedWidth="true" Visible="false" />
                                                                <ComponentArt:GridColumn DataField="del" Align="center" HeadingText=" " AllowGrouping="false" DataCellClientTemplateId="DEL" FixedWidth="True" Width="50" />
                                                            </Columns>

                                                        </ComponentArt:GridLevel>
                                                    </Levels>
                                                    <ClientEvents>
                                                        <RenderComplete EventHandler="grdPromo_onRenderComplete" />
                                                    </ClientEvents>

                                                    <ClientTemplates>
                                                        <ComponentArt:ClientTemplate ID="INST">
                                                            <select id="ddlInst_## DataItem.GetMember('line_no').Value ##" class="form-control custom-select-value" style="width: 99%;" onchange="javascript:ddlInstitution_OnChange('## DataItem.GetMember('line_no').Value ##');">
                                                            </select>

                                                        </ComponentArt:ClientTemplate>
                                                        <ComponentArt:ClientTemplate ID="MOD">
                                                            <select id="ddlModality_## DataItem.GetMember('line_no').Value ##" class="form-control custom-select-value" style="width: 99%;" onchange="javascript:ddlModality_OnChange('## DataItem.GetMember('line_no').Value ##');">
                                                            </select>

                                                        </ComponentArt:ClientTemplate>
                                                        <ComponentArt:ClientTemplate ID="DISC">
                                                            <input type="text" id="txtDisc_## DataItem.GetMember('line_no').Value ##" class="GridTextBox" value="## DataItem.GetMember('discount_percent').Value ##" maxlength="5" style="width: 95%; text-align: right;" onfocus="javascrpt:this.select();" onkeypress="javascript:parent.CheckDecimal(event);" onblur="javascript:ResetValueDecimal(this);" onchange="javascript:txtDisc_OnChange('## DataItem.GetMember('line_no').Value ##');" />
                                                        </ComponentArt:ClientTemplate>
                                                        <ComponentArt:ClientTemplate ID="CREDIT">
                                                            <input type="text" id="txtCredit_## DataItem.GetMember('line_no').Value ##" class="GridTextBox" value="## DataItem.GetMember('free_credits').Value ##" maxlength="3" style="width: 95%; text-align: right;" onfocus="javascrpt:this.select();" onkeypress="javascript:parent.CheckInteger(event);" onblur="javascript:ResetValueInteger(this);" onchange="javascript:txtCredit_OnChange('## DataItem.GetMember('line_no').Value ##');" />
                                                        </ComponentArt:ClientTemplate>
                                                        <ComponentArt:ClientTemplate ID="DEL">
                                                            <button type="button" id="btnDel_## DataItem.GetMember('line_no').Value ##" class="btn btn-danger btn_grd" onclick="javascript:DeleteRow('## DataItem.GetMember('line_no').Value ##')">
                                                                <i class="fa fa-trash" aria-hidden="true"></i>
                                                            </button>
                                                        </ComponentArt:ClientTemplate>
                                                    </ClientTemplates>
                                                </ComponentArt:Grid>
                                                <span id="spnERR" runat="server"></span>
                                                <span id="spnInst" runat="server"></span>
                                                <span id="spnMod" runat="server"></span>
                                            </Content>
                                            <LoadingPanelClientTemplate>
                                                <table style="height: 255px; width: 100%;" border="0">
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
                                                <CallbackComplete EventHandler="grdPromo_onCallbackComplete" />
                                            </ClientEvents>
                                        </ComponentArt:CallBack>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>


                </div>
            </div>






            <%--------------------------------%>
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

        <input type="hidden" id="hdnError" runat="server" value="" />
        <input type="hidden" id="hdnID" runat="server" value="00000000-0000-0000-0000-000000000000" />
    </form>
</body>
<script type="text/javascript">
    var objhdnID = document.getElementById('<%=hdnID.ClientID %>');
    var objhdnError = document.getElementById('<%=hdnError.ClientID %>');
    var objddlType = document.getElementById('<%=ddlType.ClientID %>');
    var objddlAccount = document.getElementById('<%=ddlAccount.ClientID %>');
    var objtxtCreatedDate = document.getElementById('<%=txtCreatedDate.ClientID %>');
    var objtxtFromDate = document.getElementById('<%=txtFromDate.ClientID %>');
    var objimgFromDt = document.getElementById('<%=imgFromDt.ClientID %>');
    var objtxtToDate = document.getElementById('<%=txtToDate.ClientID %>');
    var objrdoStatYes = document.getElementById('<%=rdoStatYes.ClientID %>');
    var objrdoStatNo = document.getElementById('<%=rdoStatNo.ClientID %>');
    var objddlReason = document.getElementById('<%=ddlReason.ClientID %>');
    var strForm = "VRSPromotionDlg";

</script>
<script src="../scripts/custome-javascript.js"></script>
<script src="../scripts/AppPages.js"></script>
<script src="scripts/PromotionDlg.js?01012020"></script>
</html>
