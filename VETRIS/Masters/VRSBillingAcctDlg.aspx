<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VRSBillingAcctDlg.aspx.cs" Inherits="VETRIS.Masters.VRSBillingAcctDlg" %>

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

    <link id="lnkSTYLE" runat="server" href = "../css/style.css" rel="stylesheet" type="text/css" />
    <link id="lnkTHEME" runat="server" href="../css/theme.css" rel="stylesheet"  type="text/css"/>
    <link id="lnkGRID" runat="server" href = "../css/grid_style.css" rel="stylesheet" type="text/css" />
     <link id="lnkTAB" runat="server" href = "../css/tabStyle1.css" rel="stylesheet" type="text/css" />


    <script src="../scripts/jquery-1.7.1.js"></script>
    <script src="scripts/BillingAcctDlgHdr.js?10012021"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
            <div class="sparklineHeader mt-b-10 marginTP10">
                <div class="sparkline10-hd">
                    <div class="row">
                        <div class="col-sm-6 col-xs-12">
                            <h2>Billing Account Details</h2>
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
                        <div class="col-sm-4 col-xs-12">
                            <div class="form-group">
                                <label class="control-label" for="usermodality">Code<%--<span class="mandatory">*</span>--%></label>
                                <asp:TextBox ID="txtCode" runat="server" CssClass="form-control" MaxLength="5" ReadOnly="true"></asp:TextBox>

                            </div>
                        </div>
                        <div class="col-sm-4 col-xs-12">
                            <div class="form-group">
                                <label class="control-label" for="usermodality">Name<span class="mandatory">*</span></label>
                                <asp:TextBox ID="txtName" runat="server" CssClass="form-control" MaxLength="41"></asp:TextBox>

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
                                <label class="control-label" for="usermodality">Address 2</label>
                                <asp:TextBox ID="txtAddr2" runat="server" CssClass="form-control" MaxLength="100"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-sm-4 col-xs-12">
                            <div class="form-group">
                                <label class="control-label" for="usermodality">City</label>
                                <asp:TextBox ID="txtCity" runat="server" CssClass="form-control" MaxLength="100"></asp:TextBox>

                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-4 col-xs-12">
                            <div class="form-group">
                                <label class="control-label" for="usermodality">Country</label>
                                <asp:DropDownList ID="ddlCountry" runat="server" CssClass="form-control custom-select-value"></asp:DropDownList>

                            </div>
                        </div>
                        <div class="col-sm-4 col-xs-12">
                            <div class="form-group">
                                <label class="control-label" for="usermodality">State</label>
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
            </div>
            <div class="sparklineHeader mt-b-10 marginTP10">
                <div class="row marginTP10">
                    <div class="col-sm-12 col-xs-12">
                        <div class="searchSection">
                            <div class="row">
                                <div class="col-sm-12 col-xs-12">
                                    <div class="row">
                                        <div class="col-sm-12 col-xs-12">
                                            <div class="pull-left" style="margin-top: 8px;">
                                                <h3 class="h3Text">Institutions<span class="mandatory">*</span></h3>
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

                                                                <ComponentArt:GridColumn DataField="institution_id" Align="left" HeadingText="institution_id" AllowGrouping="false" Visible="false" />
                                                                <ComponentArt:GridColumn DataField="code" Align="left" HeadingText="Code" AllowGrouping="false" Width="60" />
                                                                <ComponentArt:GridColumn DataField="name" Align="left" HeadingText="Name" AllowGrouping="false" Width="250" />
                                                                <ComponentArt:GridColumn DataField="consult_applicable" Align="center" HeadingText="Consult Applicable?" AllowGrouping="false" DataCellClientTemplateId="SELCONS" FixedWidth="True" Width="120" />
                                                                <ComponentArt:GridColumn DataField="storage_applicable" Align="center" HeadingText="Storage Applicable?" AllowGrouping="false" DataCellClientTemplateId="SELSTORE" FixedWidth="True" Width="120" />
                                                                <ComponentArt:GridColumn DataField="sel" Align="center" HeadingText="Select" AllowGrouping="false" DataCellClientTemplateId="SELINST" FixedWidth="True" Width="40" />
                                                            </Columns>

                                                        </ComponentArt:GridLevel>
                                                    </Levels>
                                                    <ClientEvents>
                                                        <RenderComplete EventHandler="grdInst_onRenderComplete" />
                                                    </ClientEvents>

                                                    <ClientTemplates>
                                                        <ComponentArt:ClientTemplate ID="SELCONS">
                                                            <div class="grid_option">
                                                                <input type="checkbox" id="chkSelCons_## DataItem.GetMember('institution_id').Value ##" style="width: 18px; height: 18px;" onclick="javascript: chkSelCons_OnClick('## DataItem.GetMember('institution_id').Value ##');" />
                                                                <label for="chkSelCons_## DataItem.GetMember('institution_id').Value ##" class="label-default" style="width: auto; margin-top: 10px;"></label>
                                                            </div>
                                                        </ComponentArt:ClientTemplate>
                                                        <ComponentArt:ClientTemplate ID="SELSTORE">
                                                            <div class="grid_option">
                                                                <input type="checkbox" id="chkSelStore_## DataItem.GetMember('institution_id').Value ##" style="width: 18px; height: 18px;" onclick="javascript: chkSelStore_OnClick('## DataItem.GetMember('institution_id').Value ##');" />
                                                                <label for="chkSelStore_## DataItem.GetMember('institution_id').Value ##" class="label-default" style="width: auto; margin-top: 10px;"></label>
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
                    </div>


                </div>
            </div>
            <div class="sparklineHeader mt-b-10 marginTP10">
                <div class="searchSection marginTP10">
                    <div class="row">
                        <div class="col-sm-12 col-xs-12">
                            <div class="pull-left">
                                <h3 class="h3Text">Contacts</h3>
                            </div>
                            <div class="borderSearch pull-left"></div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-12 col-xs-12">

                            <div class="table-responsive">
                                <ComponentArt:CallBack ID="CallBackContact" runat="server" OnCallback="CallBackContact_Callback">
                                    <Content>
                                        <ComponentArt:Grid
                                            ID="grdContact"
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
                                                        <ComponentArt:GridConditionalFormat ClientFilter="((DataItem.get_index() + grdContact.get_recordOffset()) % 2) > 0" RowCssClass="AltRow" />
                                                    </ConditionalFormats>
                                                    <Columns>
                                                        <ComponentArt:GridColumn DataField="rec_id" Align="left" HeadingText="#" AllowGrouping="false" Width="30" Visible="false" />
                                                        <ComponentArt:GridColumn DataField="institution_id" Align="left" HeadingText="id" AllowGrouping="false" Visible="false" />
                                                        <ComponentArt:GridColumn DataField="institution_name" Align="left" HeadingText="Institution" AllowGrouping="false" Width="180" FixedWidth="true" DataCellClientTemplateId="INSTNAME" />
                                                        <ComponentArt:GridColumn DataField="phone_no" Align="left" HeadingText="Office Contact" AllowGrouping="false" Width="90" FixedWidth="true" DataCellClientTemplateId="PHONE" />
                                                        <ComponentArt:GridColumn DataField="fax_no" Align="left" HeadingText="Fax #" AllowGrouping="false" Width="80" FixedWidth="true" DataCellClientTemplateId="FAX" />
                                                        <ComponentArt:GridColumn DataField="contact_person_name" Align="left" HeadingText="Contact Person" AllowGrouping="false" Width="130" FixedWidth="true" DataCellClientTemplateId="CONTPER" />
                                                        <ComponentArt:GridColumn DataField="contact_person_mobile" Align="left" HeadingText="Mobile #" AllowGrouping="false" DataCellClientTemplateId="CONTMOB" FixedWidth="True" Width="100" />
                                                        <ComponentArt:GridColumn DataField="contact_person_email_id" Align="left" HeadingText="Email ID" AllowGrouping="false" Width="180" FixedWidth="true" DataCellClientTemplateId="CONTEMAIL" />

                                                    </Columns>

                                                </ComponentArt:GridLevel>
                                            </Levels>

                                            <ClientEvents>
                                                <RenderComplete EventHandler="grdContact_onRenderComplete" />
                                            </ClientEvents>
                                            <ClientTemplates>
                                                <ComponentArt:ClientTemplate ID="INSTNAME">
                                                    <span id="spnInstName_## DataItem.GetMember('rec_id').Value ##" title="## DataItem.GetMember('institution_name').Value ##">## DataItem.GetMember('institution_name').Value ##</span>

                                                </ComponentArt:ClientTemplate>
                                                <ComponentArt:ClientTemplate ID="PHONE">
                                                    <input type="text" id="txtPhone_## DataItem.GetMember('rec_id').Value ##" maxlength="30" class="GridTextBox" style="width: 95%;" value="## DataItem.GetMember('phone_no').Value ##" onchange="javascript:txtPhone_OnChange('## DataItem.GetMember('rec_id').Value ##');" />
                                                </ComponentArt:ClientTemplate>

                                                <ComponentArt:ClientTemplate ID="FAX">
                                                    <input type="text" id="txtFax_## DataItem.GetMember('rec_id').Value ##" maxlength="20" class="GridTextBox" style="width: 95%;" value="## DataItem.GetMember('fax_no').Value ##" onchange="javascript:txtFax_OnChange('## DataItem.GetMember('rec_id').Value ##');" />
                                                </ComponentArt:ClientTemplate>
                                                <ComponentArt:ClientTemplate ID="CONTPER">
                                                    <input type="text" id="txtContPer_## DataItem.GetMember('rec_id').Value ##" maxlength="100" class="GridTextBox" style="width: 95%;" value="## DataItem.GetMember('contact_person_name').Value ##" onchange="javascript:txtContPer_OnChange('## DataItem.GetMember('rec_id').Value ##');" />
                                                </ComponentArt:ClientTemplate>
                                                <ComponentArt:ClientTemplate ID="CONTEMAIL">
                                                    <input type="text" id="txtEmail_## DataItem.GetMember('rec_id').Value ##" maxlength="50" class="GridTextBox" style="width: 95%;" value="## DataItem.GetMember('contact_person_email_id').Value ##" onchange="javascript:txtEmail_OnChange('## DataItem.GetMember('rec_id').Value ##');" />
                                                </ComponentArt:ClientTemplate>
                                                <ComponentArt:ClientTemplate ID="CONTMOB">
                                                    <input type="text" id="txtMobile_## DataItem.GetMember('rec_id').Value ##" maxlength="20" class="GridTextBox" style="width: 95%;" value="## DataItem.GetMember('contact_person_mobile').Value ##" onchange="javascript:txtMobile_OnChange('## DataItem.GetMember('rec_id').Value ##');" />
                                                </ComponentArt:ClientTemplate>
                                            </ClientTemplates>
                                        </ComponentArt:Grid>
                                        <span id="spnErrCont" runat="server"></span>
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
                                        <CallbackComplete EventHandler="grdContact_onCallbackComplete" />
                                    </ClientEvents>
                                </ComponentArt:CallBack>
                            </div>


                        </div>
                    </div>


                </div>
            </div>
            <div class="sparklineHeader mt-b-10 marginTP10">
                <div class="searchSection marginTP10">
                    <div class="row">
                        <div class="col-sm-12 col-xs-12">
                            <div class="pull-left">
                                <h3 class="h3Text">Institution Wise Physicians</h3>
                            </div>
                            <div class="borderSearch pull-left"></div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-12 col-xs-12">

                            <div class="table-responsive">
                                <ComponentArt:CallBack ID="CallBackPhys" runat="server" OnCallback="CallBackPhys_Callback">
                                    <Content>
                                        <ComponentArt:Grid
                                            ID="grdPhys"
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
                                                    DataMember="Institutions"
                                                    DataKeyField="institution_id"
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
                                                    ShowSelectorCells="true">
                                                    <ConditionalFormats>
                                                        <ComponentArt:GridConditionalFormat ClientFilter="((DataItem.get_index() + grdPhys.get_recordOffset()) % 2) > 0" RowCssClass="AltRow" SelectedRowCssClass="SelectedRow" HoverRowCssClass="HoverRow" />
                                                    </ConditionalFormats>
                                                    <Columns>
                                                        <ComponentArt:GridColumn DataField="institution_id" Align="left" HeadingText="institution_id" AllowGrouping="false" Visible="false" />
                                                        <ComponentArt:GridColumn DataField="code" Align="left" HeadingText="Institution Code" AllowGrouping="false" Width="100" />
                                                        <ComponentArt:GridColumn DataField="name" Align="left" HeadingText="Institution Name" AllowGrouping="false" Width="250" />

                                                    </Columns>

                                                </ComponentArt:GridLevel>
                                                <ComponentArt:GridLevel
                                                    AllowGrouping="false"
                                                    DataMember="Physicians"
                                                    DataKeyField="physician_id"
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
                                                        <ComponentArt:GridConditionalFormat ClientFilter="((DataItem.get_index() + grdPhys.get_recordOffset()) % 2) > 0" RowCssClass="AltRow" SelectedRowCssClass="SelectedRow" />

                                                    </ConditionalFormats>
                                                    <Columns>
                                                        <ComponentArt:GridColumn DataField="institution_id" Align="left" HeadingText="#" AllowGrouping="false" Width="30" Visible="false" />
                                                        <ComponentArt:GridColumn DataField="physician_id" Align="left" HeadingText="physician_id" AllowGrouping="false" Visible="false" />
                                                        <ComponentArt:GridColumn DataField="physician_fname" Align="left" HeadingText="First Name" AllowGrouping="false" Width="100" FixedWidth="true" DataCellClientTemplateId="FNAME" />
                                                        <ComponentArt:GridColumn DataField="physician_lname" Align="left" HeadingText="Last Name" AllowGrouping="false" Width="100" FixedWidth="true" DataCellClientTemplateId="LNAME" />
                                                        <ComponentArt:GridColumn DataField="physician_credentials" Align="left" HeadingText="Credentials" AllowGrouping="false" Width="80" FixedWidth="true" DataCellClientTemplateId="CRED" />
                                                        <ComponentArt:GridColumn DataField="physician_email" Align="left" HeadingText="Email ID (Contact)" AllowGrouping="false" Width="250" FixedWidth="true" DataCellClientTemplateId="EMAIL" />
                                                        <ComponentArt:GridColumn DataField="physician_mobile" Align="left" HeadingText="Mobile #" AllowGrouping="false" Width="180" FixedWidth="true" DataCellClientTemplateId="MOBILE" />

                                                    </Columns>

                                                </ComponentArt:GridLevel>
                                            </Levels>
                                            <ClientEvents>
                                                <RenderComplete EventHandler="grdPhys_onRenderComplete" />
                                                <ItemExpand EventHandler="grdPhys_onItemExpand" />
                                                <ItemCollapse EventHandler="grdPhys_onItemCollapse" />
                                            </ClientEvents>
                                            <ClientTemplates>
                                                <ComponentArt:ClientTemplate ID="FNAME">
                                                    <input type="text" id="txtFname_## DataItem.GetMember('physician_id').Value ##" maxlength="80" class="GridTextBox" value="## DataItem.GetMember('physician_fname').Value ##" onchange="javascript:txtFname_OnChange('## DataItem.GetMember('institution_id').Value ##','## DataItem.GetMember('physician_id').Value ##');" />
                                                </ComponentArt:ClientTemplate>
                                                <ComponentArt:ClientTemplate ID="LNAME">
                                                    <input type="text" id="txtLname_## DataItem.GetMember('physician_id').Value ##" maxlength="80" class="GridTextBox" value="## DataItem.GetMember('physician_lname').Value ##" onchange="javascript:txtLname_OnChange('## DataItem.GetMember('institution_id').Value ##','## DataItem.GetMember('physician_id').Value ##');" />
                                                </ComponentArt:ClientTemplate>
                                                <ComponentArt:ClientTemplate ID="CRED">
                                                    <input type="text" id="txtCred_## DataItem.GetMember('physician_id').Value ##" maxlength="30" class="GridTextBox" value="## DataItem.GetMember('physician_credentials').Value ##" onchange="javascript:txtCred_OnChange('## DataItem.GetMember('institution_id').Value ##','## DataItem.GetMember('physician_id').Value ##');" />
                                                </ComponentArt:ClientTemplate>
                                                <ComponentArt:ClientTemplate ID="EMAIL">
                                                    <input type="text" id="txtPhysEmail_## DataItem.GetMember('physician_id').Value ##" maxlength="500" style="width: 99%;" class="GridTextBox" value="## DataItem.GetMember('physician_email').Value ##" onchange="javascript:txtPhysEmail_OnChange('## DataItem.GetMember('institution_id').Value ##','## DataItem.GetMember('physician_id').Value ##');" />
                                                </ComponentArt:ClientTemplate>
                                                <ComponentArt:ClientTemplate ID="MOBILE">
                                                    <input type="text" id="txtPhysMobile_## DataItem.GetMember('physician_id').Value ##" maxlength="500" style="width: 99%;" class="GridTextBox" value="## DataItem.GetMember('physician_mobile').Value ##" onchange="javascript:txtPhysMobile_OnChange('## DataItem.GetMember('institution_id').Value ##','## DataItem.GetMember('physician_id').Value ##');" />
                                                </ComponentArt:ClientTemplate>
                                            </ClientTemplates>
                                        </ComponentArt:Grid>
                                        <span id="spnErrPhys" runat="server"></span>
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
                                        <CallbackComplete EventHandler="grdPhys_onCallbackComplete" />
                                    </ClientEvents>
                                </ComponentArt:CallBack>
                            </div>


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
                                        <div class="col-sm-12 col-xs-12">
                                            <div class="pull-left" style="margin-top: 8px;">
                                                <h3 class="h3Text">Login Credentials & Details</h3>
                                            </div>
                                        </div>

                                        <div class="col-sm-12 col-xs-12">
                                            <div class="borderSearch pull-left"></div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-sm-3 col-xs-12">
                                    <div class="form-select-list">
                                        <label class="control-label">Login ID<span class="mandatory">*</span></label>
                                        <asp:TextBox ID="txtLoginID" runat="server" CssClass="form-control" MaxLength="50"></asp:TextBox>

                                    </div>
                                </div>
                                <div class="col-sm-3 col-xs-12">
                                    <div class="form-group">
                                        <label class="control-label" for="usermodel">Password<span class="mandatory">*</span></label>
                                        <asp:TextBox ID="txtPwd" runat="server" CssClass="form-control" MaxLength="200" TextMode="Password"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-sm-3 col-xs-12">
                                    <div class="form-group">
                                        <label class="control-label" for="usermodel">User's Email ID</label>
                                        <asp:TextBox ID="txtLoginEmail" runat="server" CssClass="form-control" MaxLength="100"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-sm-3 col-xs-12">
                                    <div class="form-group">
                                        <label class="control-label" for="usermodel">User's Mobile #</label>
                                        <asp:TextBox ID="txtUserMobile" runat="server" CssClass="form-control" MaxLength="50"></asp:TextBox>
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
                    </div>
                </div>
            </div>
            <%--***********************sales Person************************--%>
            <div class="sparklineHeader mt-b-10 marginTP10">
                <div class="row marginTP10">
                    <div class="col-sm-12 col-xs-12">
                        <div class="searchSection">
                            <div class="row">
                                <div class="col-sm-12 col-xs-12">
                                    <div class="row">
                                        <div class="col-sm-8 col-xs-6">
                                            <div class="pull-left" style="margin-top: 8px;">
                                                <h3 class="h3Text">Sales Person</h3>
                                            </div>
                                        </div>
                                        <div class="col-sm-4 col-xs-6">
                                            <div class="pull-right">
                                            </div>
                                        </div>
                                        <div class="col-sm-12 col-xs-12">
                                            <div class="borderSearch pull-left"></div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-sm-4 col-xs-12">
                                    <div class="form-group">
                                        <label class="control-label" for="usermodality">Name</label>
                                        <asp:DropDownList ID="ddlSalesPerson" runat="server" CssClass="form-control custom-select-value"></asp:DropDownList>

                                    </div>
                                </div>
                                <div class="col-sm-4 col-xs-12">
                                    <div class="form-group">
                                        <label class="control-label" for="usermodality">Commission for 1st Year ( % )</label>
                                        <asp:TextBox ID="txtCommission1stYr" runat="server" CssClass="form-control" MaxLength="6" Style="text-align: right;"></asp:TextBox>

                                    </div>
                                </div>
                                <div class="col-sm-4 col-xs-12">
                                    <div class="form-group">
                                        <label class="control-label" for="usermodality">Commission for 2nd Year ( % )</label>
                                        <asp:TextBox ID="txtCommission2ndYr" runat="server" CssClass="form-control" MaxLength="6" Style="text-align: right;"></asp:TextBox>

                                    </div>
                                </div>
                            </div>
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
                                        <div class="col-sm-4 col-xs-4">
                                            <div class="pull-left" style="margin-top: 8px;">
                                                <h3 class="h3Text">Fees Schedule<span class="mandatory">*</span></h3>
                                            </div>
                                        </div>
                                        <div class="col-sm-6 col-xs-6" style="margin-top: 10px;">
                                            <div class="pull-right">
                                                <asp:Label ID="lblAccName" runat="server" Text="Accountant Name"></asp:Label>

                                            </div>
                                        </div>
                                        <div class="col-sm-2 col-xs-2" style="margin-top: 5px;">
                                            <asp:TextBox ID="txtAccName" runat="server" CssClass="form-control" Width="95%"></asp:TextBox>
                                        </div>
                                        <%-- <div class="col-sm-2 col-xs-2" style="margin-top: 10px;">
                                            <div class="pull-right">
                                                <asp:Label ID="lblDisc" runat="server" Text="Apply Discount (%)"></asp:Label>

                                            </div>
                                        </div>
                                        <div class="col-sm-1 col-xs-1" style="margin-top: 5px;">
                                            <asp:TextBox ID="txtDisc" runat="server" CssClass="form-control" Text="0.00" Width="95%" Style="text-align: right; padding: 3px 6px;" MaxLength="6"></asp:TextBox>
                                        </div>--%>
                                        <%--<div class="col-sm-1 col-xs-4" style="margin-top: 5px;">
                                            <div class="pull-right">
                                                <button type="button" class="btn btn-success" id="btnApplyDisc" runat="server" title="click to apply discount">
                                                    APPLY DEFAULT FEE SCHEDULE
                                                </button>
                                            </div>
                                        </div>--%>

                                        <div class="col-sm-12 col-xs-12">
                                            <div class="borderSearch pull-left"></div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="sparkline10-list mt-b-10">
                                <div class=" row mt-b-10 marginTP10">
                                    <div class="col-sm-12 col-xs-12">
                                        <ComponentArt:TabStrip ID="tsFees"
                                            CssClass="TopGroup"
                                            SiteMapXmlFile="FeesTabData.xml"
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
                                                <TabSelect EventHandler="tsFees_OnSelect" />
                                            </ClientEvents>--%>
                                        </ComponentArt:TabStrip>
                                    </div>
                                </div>
                                <div class=" row mt-b-10 marginTP10">
                                    <div class="col-sm-12 col-xs-12">
                                        <ComponentArt:MultiPage ID="ImageManagerPages" runat="server" Width="100%">
                                            <ComponentArt:PageView ID="MF" runat="server" Width="100%">
                                                <div class="col-sm-12 col-xs-12">

                                                    <div class="row">

                                                        <div class="col-sm-12 col-xs-12 text-right" style="margin-left: -5px;">

                                                            <button type="button" id="btnApplyMF" runat="server" class="btn btn-success">
                                                                APPLY DEFAULT FEE SCHEDULE
                                                            </button>
                                                        </div>
                                                    </div>

                                                    <div class="sparkline10-list mt-b-10">
                                                        <div class="searchSection">

                                                            <div class="sparkline10-graph">
                                                                <div class="static-table-list">
                                                                    <div class="table-responsive">
                                                                        <ComponentArt:CallBack ID="CallBackMF" runat="server" OnCallback="CallBackMF_Callback">
                                                                            <Content>
                                                                                <ComponentArt:Grid
                                                                                    ID="grdMF"
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
                                                                                    PageSize="8"
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
                                                                                            SelectedRowCssClass=""
                                                                                            SortAscendingImageUrl="col-asc.png"
                                                                                            SortDescendingImageUrl="col-desc.png"
                                                                                            SortImageWidth="10"
                                                                                            SortImageHeight="19">
                                                                                            <ConditionalFormats>
                                                                                                <ComponentArt:GridConditionalFormat ClientFilter="((DataItem.get_index() + grdMF.get_recordOffset()) % 2) > 0" RowCssClass="AltRow" SelectedRowCssClass="AltRow" />
                                                                                                <ComponentArt:GridConditionalFormat ClientFilter="((DataItem.get_index() + grdMF.get_recordOffset()) % 2) == 0" RowCssClass="Row" SelectedRowCssClass="Row" />
                                                                                            </ConditionalFormats>
                                                                                            <Columns>
                                                                                                <ComponentArt:GridColumn DataField="row_id" Align="left" HeadingText="#" AllowGrouping="false" Width="30" />
                                                                                                <ComponentArt:GridColumn DataField="rate_id" Align="left" HeadingText="rate_id" AllowGrouping="false" Visible="false" />
                                                                                                <ComponentArt:GridColumn DataField="category_id" Align="left" HeadingText="category_id" AllowGrouping="false" Visible="false" />
                                                                                                <ComponentArt:GridColumn DataField="modality_id" Align="left" HeadingText="modality_id" AllowGrouping="false" Visible="false" />
                                                                                                <ComponentArt:GridColumn DataField="category_name" Align="left" HeadingText="Category" AllowGrouping="false" DataCellClientTemplateId="CATEGORY" FixedWidth="True" Width="150" />
                                                                                                <ComponentArt:GridColumn DataField="modality_name" Align="left" HeadingText="Modality" AllowGrouping="false" DataCellClientTemplateId="MODALITY" FixedWidth="True" Width="180" />
                                                                                                <ComponentArt:GridColumn DataField="invoice_by" Align="left" HeadingText="invoice_by" AllowGrouping="false" Visible="false" />
                                                                                                <ComponentArt:GridColumn DataField="invoice_by_desc" Align="left" HeadingText="Charge By" AllowGrouping="false" Width="100" />
                                                                                                <ComponentArt:GridColumn DataField="default_count_from" Align="center" HeadingText="Min. Value" AllowGrouping="false" Width="100" />
                                                                                                <ComponentArt:GridColumn DataField="default_count_to" Align="center" HeadingText="Max. Value" AllowGrouping="false" Width="100" />
                                                                                                <ComponentArt:GridColumn DataField="fee_amount" Align="right" HeadingText="Fee ($)" AllowGrouping="true" DataCellClientTemplateId="MFEES" FixedWidth="True" Width="100" />
                                                                                                <ComponentArt:GridColumn DataField="fee_amount_per_unit" Align="right" HeadingText="Add On Fee/Unit ($)" AllowGrouping="true" DataCellClientTemplateId="ADDON" FixedWidth="True" Width="130" />
                                                                                                <ComponentArt:GridColumn DataField="study_max_amount" Align="right" HeadingText="Max. Study Fee ($)" AllowGrouping="true" DataCellClientTemplateId="MAXSY" FixedWidth="True" Width="130" />
                                                                                            </Columns>
                                                                                        </ComponentArt:GridLevel>
                                                                                    </Levels>
                                                                                    <ClientEvents>
                                                                                        <RenderComplete EventHandler="grdMF_onRenderComplete" />
                                                                                    </ClientEvents>
                                                                                    <ClientTemplates>
                                                                                        <ComponentArt:ClientTemplate ID="CATEGORY">
                                                                                            <span id="spnCatg_## DataItem.GetMember('row_id').Value ##" title="## DataItem.GetMember('category_name').Value ##">## DataItem.GetMember('category_name').Value ##</span>
                                                                                        </ComponentArt:ClientTemplate>
                                                                                        <ComponentArt:ClientTemplate ID="MODALITY">
                                                                                            <span id="spnMod_## DataItem.GetMember('row_id').Value ##" title="## DataItem.GetMember('modality_name').Value ##">## DataItem.GetMember('modality_name').Value ##</span>
                                                                                        </ComponentArt:ClientTemplate>
                                                                                        <ComponentArt:ClientTemplate ID="MFEES">
                                                                                            <input type="text" id="txtMFees_## DataItem.GetMember('row_id').Value ##" class="GridTextBox" value="## DataItem.GetMember('fee_amount').Value ##" style="width: 95%; text-align: right;" onchange="javascript:txtMFees_OnChange('## DataItem.GetMember('row_id').Value ##');" onfocus="javascript:this.select();" onkeypress="javascript:return parent.CheckDecimal(event);" onblur="javascript:ResetValueDecimal(this);" />
                                                                                        </ComponentArt:ClientTemplate>
                                                                                        <ComponentArt:ClientTemplate ID="ADDON">
                                                                                            <input type="text" id="txtAddOn_## DataItem.GetMember('row_id').Value ##" class="GridTextBox" value="## DataItem.GetMember('fee_amount_per_unit').Value ##" style="width: 95%; text-align: right;" onchange="javascript:txtAddOn_OnChange('## DataItem.GetMember('row_id').Value ##');" onfocus="javascript:this.select();" onkeypress="javascript:return parent.CheckDecimal(event);" onblur="javascript:ResetValueDecimal(this);" />
                                                                                        </ComponentArt:ClientTemplate>
                                                                                        <ComponentArt:ClientTemplate ID="MAXSY">
                                                                                            <input type="text" id="txtMaxFee_## DataItem.GetMember('row_id').Value ##" class="GridTextBox" value="## DataItem.GetMember('study_max_amount').Value ##" style="width: 95%; text-align: right;" onchange="javascript:txtMaxFee_OnChange('## DataItem.GetMember('row_id').Value ##');" onfocus="javascript:this.select();" onkeypress="javascript:return parent.CheckDecimal(event);" onblur="javascript:ResetValueDecimal(this);" />
                                                                                        </ComponentArt:ClientTemplate>
                                                                                    </ClientTemplates>
                                                                                </ComponentArt:Grid>
                                                                                <span id="spnMFERR" runat="server"></span>
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
                                                                                <CallbackComplete EventHandler="grdMF_onCallbackComplete" />
                                                                            </ClientEvents>
                                                                        </ComponentArt:CallBack>
                                                                    </div>
                                                                </div>
                                                            </div>

                                                        </div>
                                                    </div>

                                                </div>
                                            </ComponentArt:PageView>
                                            <ComponentArt:PageView ID="SF" runat="server" Width="100%">
                                                <div class="col-sm-12 col-xs-12">
                                                    <div class="row">

                                                        <div class="col-sm-12 col-xs-12 text-right" style="margin-left: -5px;">
                                                            <div class="row">

                                                                <div class="col-sm-12 col-xs-12 text-right" style="margin-left: -5px;">

                                                                    <button type="button" id="btnApplySF" runat="server" class="btn btn-success">
                                                                        APPLY DEFAULT FEE SCHEDULE
                                                                    </button>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>

                                                    <div class="sparkline10-list mt-b-10">
                                                        <div class="searchSection">

                                                            <div class="sparkline10-graph">
                                                                <div class="static-table-list">
                                                                    <div class="table-responsive">
                                                                        <ComponentArt:CallBack ID="CallBackSF" runat="server" OnCallback="CallBackSF_Callback">
                                                                            <Content>
                                                                                <ComponentArt:Grid
                                                                                    ID="grdSF"
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
                                                                                    PageSize="8"
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
                                                                                            SelectedRowCssClass=""
                                                                                            SortAscendingImageUrl="col-asc.png"
                                                                                            SortDescendingImageUrl="col-desc.png"
                                                                                            SortImageWidth="10"
                                                                                            SortImageHeight="19">
                                                                                            <ConditionalFormats>
                                                                                                <ComponentArt:GridConditionalFormat ClientFilter="((DataItem.get_index() + grdMF.get_recordOffset()) % 2) > 0" RowCssClass="AltRow" SelectedRowCssClass="AltRow" />
                                                                                                <ComponentArt:GridConditionalFormat ClientFilter="((DataItem.get_index() + grdMF.get_recordOffset()) % 2) == 0" RowCssClass="Row" SelectedRowCssClass="Row" />
                                                                                            </ConditionalFormats>
                                                                                            <Columns>
                                                                                                <ComponentArt:GridColumn DataField="row_id" Align="left" HeadingText="#" AllowGrouping="false" Width="30" />
                                                                                                <ComponentArt:GridColumn DataField="rate_id" Align="left" HeadingText="rate_id" AllowGrouping="false" Visible="false" />
                                                                                                <ComponentArt:GridColumn DataField="service_id" Align="left" HeadingText="service_id" AllowGrouping="false" Visible="false" />
                                                                                                <ComponentArt:GridColumn DataField="modality_id" Align="left" HeadingText="modality_id" AllowGrouping="false" Visible="false" />
                                                                                                <ComponentArt:GridColumn DataField="service_name" Align="left" HeadingText="Service" AllowGrouping="false" DataCellClientTemplateId="SERVICE" FixedWidth="True" Width="200" />
                                                                                                <ComponentArt:GridColumn DataField="modality_name" Align="left" HeadingText="Modality" AllowGrouping="false" DataCellClientTemplateId="SMODALITY" FixedWidth="True" Width="200" />
                                                                                                <ComponentArt:GridColumn DataField="invoice_by" Align="left" HeadingText="invoice_by" AllowGrouping="false" Visible="false" />
                                                                                                <ComponentArt:GridColumn DataField="invoice_by_desc" Align="left" HeadingText="Charge By" AllowGrouping="false" Width="100" />
                                                                                                <ComponentArt:GridColumn DataField="default_count_from" Align="center" HeadingText="Min. Value" AllowGrouping="false" Width="100" />
                                                                                                <ComponentArt:GridColumn DataField="default_count_to" Align="center" HeadingText="Max. Value" AllowGrouping="false" Width="100" />
                                                                                                <ComponentArt:GridColumn DataField="fee_amount" Align="right" HeadingText="Default Fee ($)" AllowGrouping="true" DataCellClientTemplateId="SFEES" FixedWidth="True" Width="150" />
                                                                                                <ComponentArt:GridColumn DataField="fee_amount_after_hrs" Align="right" HeadingText="After Hrs. Fee ($)" AllowGrouping="false" DataCellClientTemplateId="SFEESAH" FixedWidth="True" Width="150" />
                                                                                            </Columns>

                                                                                        </ComponentArt:GridLevel>
                                                                                    </Levels>
                                                                                    <ClientEvents>
                                                                                        <RenderComplete EventHandler="grdSF_onRenderComplete" />
                                                                                    </ClientEvents>

                                                                                    <ClientTemplates>
                                                                                        <ComponentArt:ClientTemplate ID="SERVICE">
                                                                                            <span id="spnService_## DataItem.GetMember('row_id').Value ##" title="## DataItem.GetMember('service_name').Value ##">## DataItem.GetMember('service_name').Value ##</span>
                                                                                        </ComponentArt:ClientTemplate>
                                                                                        <ComponentArt:ClientTemplate ID="SMODALITY">
                                                                                            <span id="spnSMod_## DataItem.GetMember('row_id').Value ##" title="## DataItem.GetMember('modality_name').Value ##">## DataItem.GetMember('modality_name').Value ##</span>
                                                                                        </ComponentArt:ClientTemplate>
                                                                                        <ComponentArt:ClientTemplate ID="SFEES">
                                                                                            <input type="text" id="txtSFees_## DataItem.GetMember('row_id').Value ##" class="GridTextBox" value="## DataItem.GetMember('fee_amount').Value ##" style="width: 95%; text-align: right;" onchange="javascript:txtSFees_OnChange('## DataItem.GetMember('row_id').Value ##');" onfocus="javascript:this.select();" onkeypress="javascript:return parent.CheckDecimal(event);" onblur="javascript:ResetValueDecimal(this);" />
                                                                                        </ComponentArt:ClientTemplate>
                                                                                        <ComponentArt:ClientTemplate ID="SFEESAH">
                                                                                            <input type="text" id="txtSFeesAH_## DataItem.GetMember('row_id').Value ##" class="GridTextBox" value="## DataItem.GetMember('fee_amount_after_hrs').Value ##" style="width: 95%; text-align: right;" onchange="javascript:txtSFeesAH_OnChange('## DataItem.GetMember('row_id').Value ##');" onfocus="javascript:this.select();" onkeypress="javascript:return parent.CheckDecimal(event);" onblur="javascript:ResetValueDecimal(this);" />
                                                                                        </ComponentArt:ClientTemplate>

                                                                                    </ClientTemplates>
                                                                                </ComponentArt:Grid>
                                                                                <span id="spnSvcErr" runat="server"></span>
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
                                                                                <CallbackComplete EventHandler="grdSF_onCallbackComplete" />
                                                                            </ClientEvents>
                                                                        </ComponentArt:CallBack>
                                                                    </div>
                                                                </div>
                                                            </div>

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
        <input type="hidden" id="hdnError" runat="server" value="" />
        <input type="hidden" id="hdnID" runat="server" value="00000000-0000-0000-0000-000000000000" />
        <input type="hidden" id="hdnCF" runat="server" value="" />
    </form>
</body>
<script type="text/javascript">
    var objhdnID = document.getElementById('<%=hdnID.ClientID %>');
    var objhdnError = document.getElementById('<%=hdnError.ClientID %>');
    var objhdnCF = document.getElementById('<%=hdnCF.ClientID %>');
    var objtxtName = document.getElementById('<%=txtName.ClientID %>');//--
    var objtxtCode = document.getElementById('<%=txtCode.ClientID %>');//--
    var objtxtAddr1 = document.getElementById('<%=txtAddr1.ClientID %>');//--
    var objtxtAddr2 = document.getElementById('<%=txtAddr2.ClientID %>');//--
    var objtxtCity = document.getElementById('<%=txtCity.ClientID %>');//--
    var objtxtZip = document.getElementById('<%=txtZip.ClientID %>');//--
   
    var objddlCountry = document.getElementById('<%=ddlCountry.ClientID %>');//--
    var objddlState = document.getElementById('<%=ddlState.ClientID %>');//--
    var objrdoEmail= document.getElementById('<%=rdoEmail.ClientID %>');
    var objrdoSMS= document.getElementById('<%=rdoSMS.ClientID %>');
    var objrdoBoth= document.getElementById('<%=rdoBoth.ClientID %>');
    var objtxtLoginID = document.getElementById('<%=txtLoginID.ClientID %>');//---
    var objtxtPwd = document.getElementById('<%=txtPwd.ClientID %>');//---
    var objtxtLoginEmail= document.getElementById('<%=txtLoginEmail.ClientID %>');
    var objtxtUserMobile= document.getElementById('<%=txtUserMobile.ClientID %>');
    var objddlSalesPerson = document.getElementById('<%=ddlSalesPerson.ClientID %>');
    var objtxtCommission1stYr = document.getElementById('<%=txtCommission1stYr.ClientID %>');// Added on 4th SEP 2019 @BK
    var objtxtCommission2ndYr = document.getElementById('<%=txtCommission2ndYr.ClientID %>');// Added on 4th SEP 2019 @BK
    var objrdoStatYes = document.getElementById('<%=rdoStatYes.ClientID %>');//---
    var objrdoStatNo = document.getElementById('<%=rdoStatNo.ClientID %>');//---
    var objtxtAccName = document.getElementById('<%=txtAccName.ClientID %>');
    var strForm = "VRSBillingAcctDlg";

</script>
<script src="../scripts/custome-javascript.js"></script>
<script src="../scripts/AppPages.js"></script>
<script src="scripts/BillingAcctDlg.js?10012021"></script>
</html>
