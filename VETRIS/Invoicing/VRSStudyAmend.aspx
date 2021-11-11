<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VRSStudyAmend.aspx.cs" Inherits="VETRIS.Invoicing.VRSStudyAmend" %>

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
   <link id="lnkSTYLE" runat="server" href = "../css/style.css" rel="stylesheet" type="text/css" />
   <link id="lnkGRID" runat="server" href = "../css/grid_style.css" rel="stylesheet" type="text/css" />
   <link id="lnkTHEME" runat="server" href="../css/theme.css" rel="stylesheet"  type="text/css"/>
    <script src="../scripts/jquery.min.js"></script>
    <script src="scripts/StudyAmendHdr.js?v=<%=DateTime.Now.Ticks%>"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
            <div class="sparklineHeader mt-b-10 marginTP10">
                <div class="sparkline10-hd">
                    <div class="row">
                        <div class="col-sm-6 col-xs-12">
                            <h2>Study Amendments</h2>
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
                                <div class="col-sm-4 col-xs-12">
                                    <div class="form-group">
                                        <label class="control-label" for="usermodel">Billing Cycle<span class="mandatory">*</span></label>
                                        <div class="input-effect">
                                            <asp:DropDownList ID="ddlBillingCycle" runat="server" CssClass="form-control custom-select-value"></asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-sm-4 col-xs-12">
                                    <div class="form-group">
                                        <label class="control-label" for="usermodel">Institution</label>
                                        <div class="input-effect">
                                            <asp:DropDownList ID="ddlInstitution" runat="server" CssClass="form-control custom-select-value"></asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-sm-4 col-xs-12">
                                    <div class="form-group">
                                        <label class="control-label" for="usermodel">Patient Name</label>
                                        <asp:TextBox ID="txtName" runat="server" CssClass="form-control" MaxLength="100"></asp:TextBox>
                                    </div>
                                </div>


                            </div>
                            <div class="row">
                                <div class="col-sm-4 col-xs-12">
                                    <div class="form-group">
                                        <label class="control-label" for="usermodel">Modality</label>
                                        <div class="input-effect">
                                            <asp:DropDownList ID="ddlModality" runat="server" CssClass="form-control custom-select-value"></asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-sm-4 col-xs-12">
                                    <div class="form-group">
                                        <label class="control-label" for="usermodel">Category</label>
                                        <div class="input-effect">
                                            <asp:DropDownList ID="ddlCategory" runat="server" CssClass="form-control custom-select-value"></asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-sm-4 col-xs-12 text-center" style="margin-top: 22px;">
                                    <button type="button" id="btnOk" runat="server" class="btn btn-primary">
                                        <i class="fa fa-check" aria-hidden="true"></i>&nbsp;OK</button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="sparkline10-list mt-b-10">
                <div class="sparkline10-graph">
                    <div class="searchSection">
                        <div class="row">
                            <div class="col-sm-6 col-xs-6">
                                <div class="pull-left">
                                    <h3 class="h3Text">Study(ies)</h3>
                                </div>
                            </div>
                            <div class="col-sm-6 col-xs-6">
                                &nbsp;
                            </div>
                            <div class="col-sm-12 col-xs-12">
                                <div class="borderSearch pull-left"></div>
                            </div>


                        </div>


                        <div class="sparkline10-graph">
                            <div class="static-table-list">
                                <div class="table-responsive">
                                    <ComponentArt:CallBack ID="CallBackStudy" runat="server" OnCallback="CallBackStudy_Callback">
                                        <Content>
                                            <ComponentArt:Grid
                                                ID="grdStudy"
                                                CssClass="Grid"
                                                AutoTheming="true"
                                                DataAreaCssClass=""
                                                SearchOnKeyPress="true"
                                                EnableViewState="true"
                                                RunningMode="Client"
                                                ShowSearchBox="false"
                                                SearchBoxPosition="TopLeft"
                                                SearchTextCssClass="GridHeaderText"
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
                                                        DataMember="Details"
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
                                                        SortImageHeight="19"
                                                        SelectorCellWidth="20"
                                                        ShowSelectorCells="false">
                                                        <ConditionalFormats>
                                                            <ComponentArt:GridConditionalFormat ClientFilter="((DataItem.get_index() + grdStudy.get_recordOffset()) % 2) > 0" RowCssClass="AltRow" SelectedRowCssClass="SelectedRow" />
                                                        </ConditionalFormats>
                                                        <Columns>
                                                            <ComponentArt:GridColumn DataField="id" Align="left" HeadingText="id" AllowGrouping="false" Visible="false" />
                                                            <ComponentArt:GridColumn DataField="study_uid" Align="left" HeadingText="study_uid" AllowGrouping="false" Visible="false" />
                                                            <ComponentArt:GridColumn DataField="received_date" Align="left" HeadingText="Received Date" AllowGrouping="false" Width="100" />
                                                            <ComponentArt:GridColumn DataField="patient_name" Align="left" HeadingText="Patient Name" AllowGrouping="false" Width="100" />
                                                            <ComponentArt:GridColumn DataField="institution_id" Align="left" HeadingText="Institution" AllowGrouping="false" Width="200" DataCellClientTemplateId="INSTITUTE" FixedWidth="true" AllowSorting="False" />
                                                            <ComponentArt:GridColumn DataField="modality_id" Align="left" HeadingText="Modality" AllowGrouping="false" Width="150" DataCellClientTemplateId="MODALITY" FixedWidth="true" AllowSorting="False" />
                                                            <ComponentArt:GridColumn DataField="priority_id" Align="left" HeadingText="Priority" AllowGrouping="false" Width="135" DataCellClientTemplateId="PRIORITY" FixedWidth="true" AllowSorting="False" />
                                                            <ComponentArt:GridColumn DataField="category_id" Align="left" HeadingText="Category" AllowGrouping="false" Width="130" DataCellClientTemplateId="CATEGORY" FixedWidth="true" AllowSorting="False" />
                                                            <ComponentArt:GridColumn DataField="service_codes" Align="left" HeadingText="Service(s)" AllowGrouping="false" Width="200" DataCellClientTemplateId="SERVICE" FixedWidth="true" AllowSorting="False" />
                                                            <ComponentArt:GridColumn DataField="invoiced" Align="left" HeadingText="invoiced" AllowGrouping="false" Visible="false" />
                                                            <ComponentArt:GridColumn DataField="changed" Align="left" HeadingText="changed" AllowGrouping="false" Visible="false" />
                                                            <ComponentArt:GridColumn DataField="del" Align="center" HeadingText=" " AllowGrouping="false" Width="120" DataCellClientTemplateId="DEL" FixedWidth="true" AllowSorting="False" />
                                                            <ComponentArt:GridColumn DataField="promotion_id" Align="left" HeadingText="promotion_id" AllowGrouping="false" Visible="false" />
                                                        </Columns>

                                                    </ComponentArt:GridLevel>
                                                    <ComponentArt:GridLevel
                                                        AllowGrouping="false"
                                                        DataMember="Rates"
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
                                                        SortDescendingImageUrl="col-desc.png" ShowSelectorCells="false"
                                                        SortImageWidth="10"
                                                        SortImageHeight="19">
                                                        <ConditionalFormats>
                                                            <ComponentArt:GridConditionalFormat ClientFilter="((DataItem.get_index() + grdStudy.get_recordOffset()) % 2) > 0" RowCssClass="AltRow" SelectedRowCssClass="SelectedRow" />
                                                        </ConditionalFormats>
                                                        <Columns>
                                                            <ComponentArt:GridColumn DataField="rec_id" Align="left" HeadingText="study_id" AllowGrouping="false" Visible="false" />
                                                            <ComponentArt:GridColumn DataField="study_id" Align="left" HeadingText="study_id" AllowGrouping="false" Visible="false" />
                                                            <ComponentArt:GridColumn DataField="study_uid" Align="left" HeadingText="study_uid" AllowGrouping="false" Visible="false" />
                                                            <ComponentArt:GridColumn DataField="head_id" Align="left" HeadingText="head_id" AllowGrouping="false" Visible="false" />
                                                            <ComponentArt:GridColumn DataField="head_type" Align="left" HeadingText="head_type" AllowGrouping="false" Visible="false" />
                                                            <ComponentArt:GridColumn DataField="head_name" Align="left" HeadingText="Head" AllowGrouping="false" Width="200" DataCellClientTemplateId="RATEHEAD" FixedWidth="true" />
                                                            <ComponentArt:GridColumn DataField="image_count" Align="right" HeadingText="Image/Body Part #" AllowGrouping="false" Width="120" DataCellClientTemplateId="IMGCNT" FixedWidth="true"/>
                                                            <ComponentArt:GridColumn Align="left" HeadingText=" " AllowGrouping="false" Width="30" DataCellClientTemplateId="BPEDIT" FixedWidth="true" />
                                                            <ComponentArt:GridColumn DataField="amount" Align="right" HeadingText="Amount ($)" AllowGrouping="false" Width="80" DataCellClientTemplateId="AMT" FixedWidth="true" AllowSorting="False" />
                                                            <ComponentArt:GridColumn DataField="changed" Align="left" HeadingText="changed" AllowGrouping="false" Visible="false" />
                                                            <ComponentArt:GridColumn DataField="invoice_by" Align="left" HeadingText="invoice_by" AllowGrouping="false" Visible="false" />
                                                        </Columns>
                                                    </ComponentArt:GridLevel>
                                                </Levels>
                                                <ClientEvents>
                                                    <RenderComplete EventHandler="grdStudy_onRenderComplete" />
                                                    <ItemExpand EventHandler="grdStudy_onItemExpand" />
                                                    <ItemCollapse EventHandler="grdStudy_onItemCollapse" />
                                                </ClientEvents>
                                                <ClientTemplates>
                                                    <ComponentArt:ClientTemplate ID="INSTITUTE">
                                                        <select id="ddlInst_## DataItem.GetMember('id').Value ##" class="form-control custom-select-value" style="width: 95%; padding: 0px;" onchange="javascript:ddlInst_OnChange('## DataItem.GetMember('id').Value ##');">
                                                        </select>
                                                    </ComponentArt:ClientTemplate>
                                                    <ComponentArt:ClientTemplate ID="MODALITY">
                                                        <select id="ddlModality_## DataItem.GetMember('id').Value ##" class="form-control custom-select-value" style="width: 95%; padding: 0px;" onchange="javascript:ddlModality_OnChange('## DataItem.GetMember('id').Value ##');">
                                                        </select>
                                                    </ComponentArt:ClientTemplate>
                                                    <ComponentArt:ClientTemplate ID="PRIORITY">
                                                        <select id="ddlPriority_## DataItem.GetMember('id').Value ##" class="form-control custom-select-value" style="width: 95%; padding: 0px;" onchange="javascript:ddlPriority_OnChange('## DataItem.GetMember('id').Value ##');">
                                                        </select>
                                                    </ComponentArt:ClientTemplate>
                                                    <ComponentArt:ClientTemplate ID="CATEGORY">
                                                        <select id="ddlCategory_## DataItem.GetMember('id').Value ##" class="form-control custom-select-value" style="width: 95%; padding: 0px;" onchange="javascript:ddlCategory_OnChange('## DataItem.GetMember('id').Value ##');">
                                                        </select>
                                                    </ComponentArt:ClientTemplate>
                                                    <ComponentArt:ClientTemplate ID="SERVICE">
                                                        <input type="text" id="txtService_## DataItem.GetMember('id').Value ##" maxlength="500" style="width: 80%;" class="GridTextBoxNoBorder" readonly="readonly" value="## DataItem.GetMember('service_codes').Value ##" />
                                                        <button type="button" id="btnEditSvc_## DataItem.GetMember('id').Value ##" class="btn btn-warning btn_grd" onclick="javascript:btnEditSvc_OnClick('## DataItem.GetMember('id').Value ##','## DataItem.GetMember('service_codes').Value ##');" title="click to edit service(s)"><i class="fa fa-pencil" aria-hidden="true"></i></button>
                                                    </ComponentArt:ClientTemplate>
                                                    <ComponentArt:ClientTemplate ID="DEL">
                                                        <button type="button" id="btnFree_## DataItem.GetMember('id').Value ##" class="btn btn-info btn_grd" title="click to make the study free" onclick="javascript:btnFree_OnClick('## DataItem.GetMember('id').Value ##');"><i class="fa fa-circle-o" aria-hidden="true"></i></button>
                                                        <button type="button" id="btnDisc_## DataItem.GetMember('id').Value ##" class="btn btn-primary btn_grd" style="display: none;" title="click to view/apply/revert discount" onclick="javascript:btnDisc_OnClick('## DataItem.GetMember('id').Value ##');"><i class="fa fa-usd" aria-hidden="true"></i></button>
                                                        <button type="button" id="btnDel_## DataItem.GetMember('id').Value ##" class="btn btn-danger btn_grd" style="display: inline;" onclick="javascript:DeleteStudy('## DataItem.GetMember('id').Value ##','## DataItem.GetMember('study_uid').Value ##')"><i class="fa fa-trash" aria-hidden="true"></i></button>
                                                    </ComponentArt:ClientTemplate>
                                                    <ComponentArt:ClientTemplate ID="IMGCNT">
                                                        <input type="text" id="txtCnt_## DataItem.GetMember('rec_id').Value ##" style="width: 95%;text-align:right;" class="GridTextBoxNoBorder" readonly="readonly" tabindex="-1" value="## DataItem.GetMember('image_count').Value ##" />
                                                    </ComponentArt:ClientTemplate>
                                                    <ComponentArt:ClientTemplate ID="BPEDIT">
                                                        <button type="button" id="btnEditBP_## DataItem.GetMember('rec_id').Value ##" class="btn btn-success btn_grd" style="display:none;" title="click to view/update body part count" onclick="javascript:btnEditBP_OnClick('## DataItem.GetMember('rec_id').Value ##','## DataItem.GetMember('study_id').Value ##','## DataItem.GetMember('head_id').Value ##');">
                                                            <i class="fa fa-pencil-square-o" aria-hidden="true"></i>
                                                        </button>
                                                    </ComponentArt:ClientTemplate>
                                                    <ComponentArt:ClientTemplate ID="RATEHEAD">
                                                        <input type="text" id="txtHead_## DataItem.GetMember('rec_id').Value ##" style="width: 95%;" class="GridTextBoxNoBorder" readonly="readonly" tabindex="-1" value="## DataItem.GetMember('head_name').Value ##" />
                                                    </ComponentArt:ClientTemplate>
                                                    <ComponentArt:ClientTemplate ID="AMT">
                                                        <input type="text" id="txtAmt_## DataItem.GetMember('rec_id').Value ##" class="GridTextBox" value="## DataItem.GetMember('amount').Value ##" style="width: 90%; padding-right: 5px; text-align: right;" onchange="javascript:txtAmt_OnChange('## DataItem.GetMember('study_id').Value ##','## DataItem.GetMember('rec_id').Value ##')" />
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
                                                                    <img src="../images/spinner-darkgrey.gif" border="0" alt="" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </LoadingPanelClientTemplate>
                                        <ClientEvents>
                                            <CallbackComplete EventHandler="grdStudy_onCallbackComplete" />
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
        <input type="hidden" id="hdnInst" runat="server" value="" />
        <input type="hidden" id="hdnModality" runat="server" value="" />
        <input type="hidden" id="hdnPriority" runat="server" value="" />
        <input type="hidden" id="hdnCategory" runat="server" value="" />
        <input type="hidden" id="hdnAID" runat="server" value="00000000-0000-0000-0000-000000000000" />
        <input type="hidden" id="hdnBCID" runat="server" value="00000000-0000-0000-0000-000000000000" />
        <input type="hidden" id="hdnIID" runat="server" value="00000000-0000-0000-0000-000000000000" />
        <input type="hidden" id="hdnCF" runat="server" value="" />

        <input type="hidden" id="hdnAPIVER" runat="server" value="" />
        <input type="hidden" id="hdnWS8SRVIP" runat="server" value="" />
        <input type="hidden" id="hdnWS8CLTIP" runat="server" value="" />
        <input type="hidden" id="hdnWS8SRVUID" runat="server" value="" />
        <input type="hidden" id="hdnWS8SRVPWD" runat="server" value="" />
        <input type="hidden" id="hdnWS8Session" runat="server" value="" />
        <input type="hidden" id="hdnStudyDelUrl" runat="server" value="" />
    </form>
</body>
<script type="text/javascript">
    var objhdnID = document.getElementById('<%=hdnID.ClientID %>');
    var objhdnError = document.getElementById('<%=hdnError.ClientID %>');
    var objddlBillingCycle = document.getElementById('<%=ddlBillingCycle.ClientID %>');//--
    var objddlInstitution = document.getElementById('<%=ddlInstitution.ClientID %>');
    var objddlModality = document.getElementById('<%=ddlModality.ClientID %>');
    var objddlCategory = document.getElementById('<%=ddlCategory.ClientID %>');
    var objtxtName = document.getElementById('<%=txtName.ClientID %>');
    var objhdnInst = document.getElementById('<%=hdnInst.ClientID %>');
    var objhdnModality = document.getElementById('<%=hdnModality.ClientID %>');
    var objhdnPriority = document.getElementById('<%=hdnPriority.ClientID %>');
    var objhdnCategory = document.getElementById('<%=hdnCategory.ClientID %>');
    var objhdnAPIVER = document.getElementById('<%=hdnAPIVER.ClientID %>');
    var objhdnWS8SRVIP = document.getElementById('<%=hdnWS8SRVIP.ClientID %>');
    var objhdnWS8CLTIP = document.getElementById('<%=hdnWS8CLTIP.ClientID %>');
    var objhdnWS8SRVUID = document.getElementById('<%=hdnWS8SRVUID.ClientID %>');
    var objhdnWS8SRVPWD = document.getElementById('<%=hdnWS8SRVPWD.ClientID %>');
    var objhdnWS8Session = document.getElementById('<%=hdnWS8Session.ClientID %>');
    var objhdnStudyDelUrl = document.getElementById('<%=hdnStudyDelUrl.ClientID %>');
    var objhdnAID = document.getElementById('<%=hdnAID.ClientID %>');
    var objhdnBCID = document.getElementById('<%=hdnBCID.ClientID %>');
    var objhdnIID = document.getElementById('<%=hdnIID.ClientID %>');
    var objhdnCF = document.getElementById('<%=hdnCF.ClientID %>');
    var strForm = "VRSStudyAmend";

</script>
<script src="../scripts/custome-javascript.js"></script>
<script src="../scripts/AppPages.js"></script>
<script src="scripts/StudyAmend.js?v=<%=DateTime.Now.Ticks%>"></script>
</html>
