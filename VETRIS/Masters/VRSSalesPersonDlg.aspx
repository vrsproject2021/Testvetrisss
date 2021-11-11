<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VRSSalesPersonDlg.aspx.cs" Inherits="VETRIS.Masters.VRSSalesPersonDlg" %>

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
    <script src="scripts/SalesPersonDlgHdr.js?1"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
            <div class="sparklineHeader mt-b-10 marginTP10">
                <div class="sparkline10-hd">
                    <div class="row">
                        <div class="col-sm-6 col-xs-12">
                            <h2>Sales Person Details</h2>
                        </div>
                        <div class="col-sm-6 col-xs-12 text-right">
                            <button type="button" id="btnAdd1" runat="server" class="btn btn-custon-four btn-primary" style="display: none;">
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
                                <h3 style="color: #1e77bb;">Details</h3>
                            </div>
                            <div class="borderSearch pull-left"></div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-4 col-xs-12">
                            <div class="form-group">
                                <label class="control-label" for="usermodel">First Name<span class="mandatory">*</span></label>
                                <asp:TextBox ID="txtFName" runat="server" CssClass="form-control" MaxLength="80"></asp:TextBox>

                            </div>
                        </div>
                        <div class="col-sm-4 col-xs-12">
                            <div class="form-group">
                                <label class="control-label" for="usermodel">Last Name<span class="mandatory">*</span></label>
                                <asp:TextBox ID="txtLName" runat="server" CssClass="form-control" MaxLength="80"></asp:TextBox>

                            </div>
                        </div>
                        <div class="col-sm-4 col-xs-12">
                            <div class="form-group1">
                                <label class="control-label">Active ?<span class="mandatory">*</span></label>
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
                    <div class="row">
                        <div class="col-sm-4 col-xs-12">
                            <div class="form-select-list">
                                <label class="control-label">Address 1</label>
                                <asp:TextBox ID="txtAddr1" runat="server" CssClass="form-control" MaxLength="100"></asp:TextBox>

                            </div>
                        </div>
                        <div class="col-sm-4 col-xs-12">
                            <div class="form-group">
                                <label class="control-label" for="usermodel">Address 2</label>
                                <asp:TextBox ID="txtAddr2" runat="server" CssClass="form-control" MaxLength="100"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-sm-4 col-xs-12">
                            <div class="form-group">
                                <label class="control-label" for="usermodel">City</label>
                                <asp:TextBox ID="txtCity" runat="server" CssClass="form-control" MaxLength="100"></asp:TextBox>

                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-4 col-xs-12">
                            <div class="form-group">
                                <label class="control-label" for="usermodel">Country</label>
                                <asp:DropDownList ID="ddlCountry" runat="server" CssClass="form-control custom-select-value"></asp:DropDownList>

                            </div>
                        </div>
                        <div class="col-sm-4 col-xs-12">
                            <div class="form-group">
                                <label class="control-label" for="usermodel">State</label>
                                <asp:DropDownList ID="ddlState" runat="server" CssClass="form-control custom-select-value"></asp:DropDownList>

                            </div>
                        </div>
                        <div class="col-sm-4 col-xs-12">
                            <div class="form-group">
                                <label class="control-label" for="zip">Zip</label>
                                <asp:TextBox ID="txtZip" runat="server" CssClass="form-control" MaxLength="100"></asp:TextBox>

                            </div>
                        </div>
                    </div>
                </div>

                <div class="searchSection marginTP10">
                    <div class="row">
                        <div class="col-sm-12 col-xs-12">
                            <div class="pull-left">
                                <h3 style="color: #1e77bb;">Contacts</h3>
                            </div>
                            <div class="borderSearch pull-left"></div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-4 col-xs-12">
                            <div class="form-group">
                                <label class="control-label" for="usermodel">Email ID<span class="mandatory">*</span></label>
                                <asp:TextBox ID="txtEmailID" runat="server" CssClass="form-control" MaxLength="50"></asp:TextBox>

                            </div>
                        </div>
                        <div class="col-sm-4 col-xs-12">
                            <div class="form-group">
                                <label class="control-label" for="usermodel">Telephone #</label>
                                <asp:TextBox ID="txtTel" runat="server" CssClass="form-control" MaxLength="30"></asp:TextBox>

                            </div>
                        </div>
                        <div class="col-sm-4 col-xs-12">
                            <div class="form-select-list">
                                <label class="control-label">Mobile #<span class="mandatory">*</span></label>
                                <asp:TextBox ID="txtMobile" runat="server" CssClass="form-control" MaxLength="20"></asp:TextBox>

                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-4 col-xs-12">
                             <div class="form-group1">
                                <label class="control-label">Notification Preference</label>
                            </div>
                            <div class="pull-left grid_option1 customRadio">
                                <asp:RadioButton ID="rdoEmail" runat="server" GroupName="grpPref" />
                                <label for="rdoEmail" class="label-default" style="width: auto; margin-top: 10px;"></label>
                            </div>
                            <span class="pull-left" style="padding-left: 0px; padding-top: 5px; padding-right: 5px;">Email</span>
                            <div class="pull-left grid_option1 customRadio">
                                <asp:RadioButton ID="rdoSMS" runat="server" GroupName="grpPref" />
                                <label for="rdoSMS" class="label-default" style="width: auto; margin-top: 10px;"></label>
                            </div>
                            <span class="pull-left" style="padding-left: 0px; padding-top: 5px; padding-right: 5px;">SMS</span>
                            <div class="pull-left grid_option1 customRadio">
                                <asp:RadioButton ID="rdoBoth" runat="server" GroupName="grpPref" />
                                <label for="rdoBoth" class="label-default" style="width: auto; margin-top: 10px;"></label>
                            </div>
                            <span class="pull-left" style="padding-left: 0px; padding-top: 5px; padding-right: 5px;">Both</span>
                        </div>
                    </div>
                </div>

                <div class="searchSection marginTP10">
                    <div class="row">
                        <div class="col-sm-12 col-xs-12">
                            <div class="pull-left">
                                <h3 style="color: #1e77bb;">Login Credentials</h3>
                            </div>
                            <div class="borderSearch pull-left"></div>
                        </div>
                    </div>
                    <div class="row">
                         <div class="col-sm-3 col-xs-12">
                            <div class="form-select-list">
                                <label class="control-label">Login ID<span class="mandatory">*</span></label>
                                <asp:TextBox ID="txtLoginID" runat="server" CssClass="form-control" MaxLength="10"></asp:TextBox>

                            </div>
                        </div>
                        <div class="col-sm-3 col-xs-12">
                            <div class="form-group">
                                <label class="control-label" for="usermodel">Password<span class="mandatory">*</span></label>
                                <asp:TextBox ID="txtPwd" runat="server" CssClass="form-control" MaxLength="200" TextMode="Password"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-sm-3 col-xs-12">
                            <div class="form-select-list">
                                <label class="control-label">PACS User ID</label>
                                <asp:TextBox ID="txtPACSUserID" runat="server" CssClass="form-control" MaxLength="20"></asp:TextBox>

                            </div>
                        </div>
                        <div class="col-sm-3 col-xs-12">
                            <div class="form-group">
                                <label class="control-label" for="usermodel">PACS User Password</label>
                                <asp:TextBox ID="txtPACSPwd" runat="server" CssClass="form-control" MaxLength="200" extMode="Password"></asp:TextBox>
                            </div>
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
                                    <div class="row">
                                        <div class="col-sm-6 col-xs-6">
                                        <div class="pull-left" style="margin-top: 8px;">
                                            <h3 style="color: #1e77bb;">Institutions</h3>
                                        </div>
                                    </div>
                                        <div class="col-sm-6 col-xs-6">
                                            <div class="pull-right">
                                                <button type="button" class="btn btn_grd btn-primary" id="btnAddInst" runat="server" title="click to add new row for institution" style="display: none;">
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
                                        <ComponentArt:CallBack ID="CallBackInst" runat="server" OnCallback="CallBackInst_Callback">
                                            <Content>
                                                <ComponentArt:Grid
                                                    ID="grdInst"
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
                                                            SortAscendingImageUrl="col-asc.png"
                                                            SortDescendingImageUrl="col-desc.png"
                                                            SortImageWidth="10"
                                                            SortImageHeight="19">
                                                            <ConditionalFormats>
                                                                <ComponentArt:GridConditionalFormat ClientFilter="((DataItem.get_index() + grdInst.get_recordOffset()) % 2) > 0" RowCssClass="AltRow" />
                                                            </ConditionalFormats>
                                                            <Columns>
                                                                <ComponentArt:GridColumn DataField="rec_id"                 Align="left"    HeadingText="#"                              AllowGrouping="false" Width="30" />
                                                                <ComponentArt:GridColumn DataField="institution_id"         Align="left"    HeadingText="Institution"                    AllowGrouping="false" Width="300" FixedWidth="true" DataCellClientTemplateId="INSTITUTION" />
                                                                <ComponentArt:GridColumn DataField="salesperson_user_id"    Align="left"    HeadingText="Linked User"                    AllowGrouping="false" Width="200" FixedWidth="true" DataCellClientTemplateId="USER"             Visible="false"/>
                                                                <ComponentArt:GridColumn DataField="commission_1st_yr"      Align="right"   HeadingText="Commission for 1st Year (%)"  AllowGrouping="false" Width="170" FixedWidth="true" DataCellClientTemplateId="COMMISSION1YR"/>
                                                                <ComponentArt:GridColumn DataField="commission_2nd_yr"      Align="right"   HeadingText="Commission for 2nd Year (%)"  AllowGrouping="false" Width="170" FixedWidth="true" DataCellClientTemplateId="COMMISSION2YR"/>
                                                                <ComponentArt:GridColumn DataField="del"                    Align="center"  HeadingText=" "                              AllowGrouping="false" Width="50"  FixedWidth="True" DataCellClientTemplateId ="DEL"             Visible="false" />
                                                            </Columns>

                                                        </ComponentArt:GridLevel>
                                                    </Levels>

                                                    <ClientEvents>
                                                        <RenderComplete EventHandler="grdInst_onRenderComplete" />
                                                    </ClientEvents>
                                                    <ClientTemplates>
                                                        <ComponentArt:ClientTemplate ID="INSTITUTION">
                                                            <select id="ddlInst_## DataItem.GetMember('rec_id').Value ##" class="form-control custom-select-value" style="width: 95%;" disabled="disabled" onchange="javascript:ddlInst_OnChange('## DataItem.GetMember('rec_id').Value ##');">
                                                            </select>
                                                        </ComponentArt:ClientTemplate>
                                                        <ComponentArt:ClientTemplate ID="USER">
                                                            <select id="ddlUser_## DataItem.GetMember('rec_id').Value ##" class="form-control custom-select-value" style="width: 95%;" disabled="disabled" onchange="javascript:ddlUser_OnChange('## DataItem.GetMember('rec_id').Value ##');">
                                                            </select>
                                                        </ComponentArt:ClientTemplate>
                                                        <ComponentArt:ClientTemplate ID="DEL">
                                                            <button type="button" id="btnDel_## DataItem.GetMember('rec_id').Value ##" class="btn btn-danger btn_grd" onclick="javascript:DeleteRow('## DataItem.GetMember('rec_id').Value ##')"><i class="fa fa-trash" aria-hidden="true"></i></button>
                                                        </ComponentArt:ClientTemplate>
                                                        <ComponentArt:ClientTemplate ID="COMMISSION1YR">
                                                            <input type="text" id="txtCommission1Yr_## DataItem.GetMember('rec_id').Value ##" class="GridTextBox" value="## DataItem.GetMember('commission_1st_yr').Value ##" style="width: 95%; text-align: center;" onchange="javascript:txtCommission_OnChange('## DataItem.GetMember('rec_id').Value ##','1yr');"  onfocus="javascript:this.select();" onkeypress="javascript:return parent.CheckDecimal(event);"  onblur="javascript:ResetValueDecimal(this);"/>
                                                        </ComponentArt:ClientTemplate>
                                                        <ComponentArt:ClientTemplate ID="COMMISSION2YR">
                                                            <input type="text" id="txtCommission2Yr_## DataItem.GetMember('rec_id').Value ##" class="GridTextBox" value="## DataItem.GetMember('commission_2nd_yr').Value ##" style="width: 95%; text-align: center;" onchange="javascript:txtCommission_OnChange('## DataItem.GetMember('rec_id').Value ##','2yr');"    onfocus="javascript:this.select();" onkeypress="javascript:return parent.CheckDecimal(event);" onblur="javascript:ResetValueDecimal(this);"/>
                                                            
                                                        </ComponentArt:ClientTemplate>
                                                    </ClientTemplates>
                                                </ComponentArt:Grid>
                                                <span id="spnErrInst" runat="server"></span>
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
                                                <CallbackComplete EventHandler="grdInst_onCallbackComplete" />
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
                        <div class="col-sm-6 hidden-xs">
                        </div>
                        <div class="col-sm-6 col-xs-12 text-right">
                            <button type="button" class="btn btn-custon-four btn-primary" style="display: none;" id="btnAdd2" runat="server">
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
        <asp:TextBox ID="txtCode" runat="server" CssClass="form-control" MaxLength="5" Style="display: none;"></asp:TextBox>
        <input type="hidden" id="hdnError" runat="server" value="" />
        <input type="hidden" id="hdnID" runat="server" value="00000000-0000-0000-0000-000000000000" />
        <input type="hidden" id="hdnInstitutions" runat="server" value="" />
        <input type="hidden" id="hdnUsers" runat="server" value="" />
    </form>
</body>
<script type="text/javascript">
    var objhdnID = document.getElementById('<%=hdnID.ClientID %>');
    var objhdnError = document.getElementById('<%=hdnError.ClientID %>');
    var objtxtFName = document.getElementById('<%=txtFName.ClientID %>');//--
    var objtxtLName = document.getElementById('<%=txtLName.ClientID %>');//--
    var objtxtCode = document.getElementById('<%=txtCode.ClientID %>');//--
    var objtxtAddr1 = document.getElementById('<%=txtAddr1.ClientID %>');//--
    var objtxtAddr2 = document.getElementById('<%=txtAddr2.ClientID %>');//--
    var objtxtCity = document.getElementById('<%=txtCity.ClientID %>');//--
    var objtxtZip = document.getElementById('<%=txtZip.ClientID %>');//--
    var objtxtEmailID = document.getElementById('<%=txtEmailID.ClientID %>');//--
    var objtxtTel = document.getElementById('<%=txtTel.ClientID %>');//--
    var objtxtMobile = document.getElementById('<%=txtMobile.ClientID %>');//--
    var objddlCountry = document.getElementById('<%=ddlCountry.ClientID %>');//--
    var objddlState = document.getElementById('<%=ddlState.ClientID %>');//--
    var objhdnInstitutions = document.getElementById('<%=hdnInstitutions.ClientID %>');//-
    var objhdnUsers = document.getElementById('<%=hdnUsers.ClientID %>');//-
    var objrdoStatYes = document.getElementById('<%=rdoStatYes.ClientID %>');//---
    var objrdoStatNo = document.getElementById('<%=rdoStatNo.ClientID %>');//---
    var objrdoEmail = document.getElementById('<%=rdoEmail.ClientID %>');
    var objrdoSMS = document.getElementById('<%=rdoSMS.ClientID %>');
    var objrdoBoth = document.getElementById('<%=rdoBoth.ClientID %>');
    var objtxtLoginID = document.getElementById('<%=txtLoginID.ClientID %>');//---
    var objtxtPwd = document.getElementById('<%=txtPwd.ClientID %>');//---
    var objtxtPACSUserID = document.getElementById('<%=txtPACSUserID.ClientID %>');//--
    var objtxtPACSPwd = document.getElementById('<%=txtPACSPwd.ClientID %>');//--
    var strForm = "VRSSalesPersonDlg";

</script>
<script src="../scripts/custome-javascript.js"></script>
<script src="../scripts/AppPages.js"></script>
<script src="scripts/SalesPersonDlg.js?01102019"></script>
</html>
