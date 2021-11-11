<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VRSBillingAcctDlg.aspx.cs" Inherits="VETRIS.Profile.VRSBillingAcctDlg" %>
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
    <link href="../css/grid_style.css?2" rel="stylesheet" />--%>
    <link id="lnkSTYLE" runat="server" href = "../css/style.css" rel="stylesheet" type="text/css" />
    <link id="lnkGRID" runat="server" href = "../css/grid_style.css" rel="stylesheet" type="text/css" />
    <link id="lnkTHEME" runat="server" href="../css/theme.css" rel="stylesheet"  type="text/css"/>
    <script src="../scripts/jquery-1.7.1.js"></script>
    <script src="scripts/BillingAcctDlgHdr.js"></script>
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
                        <div class="col-sm-6 col-xs-12">
                            <div class="row">
                                <div class="col-sm-4 col-xs-12" style="font-weight: bold;">
                                    Code
                                </div>
                                <div class="col-sm-8 col-xs-12">
                                    <asp:Label ID="lblCode" runat="server"></asp:Label>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-4 col-xs-12" style="font-weight: bold;">
                                    Name
                                </div>
                                <div class="col-sm-8 col-xs-12">
                                    <asp:Label ID="lblName" runat="server"></asp:Label>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-4 col-xs-12" style="font-weight: bold;">
                                    Status
                                </div>
                                <div class="col-sm-8 col-xs-12">
                                    <asp:Label ID="lblStatus" runat="server"></asp:Label>
                                </div>
                            </div>
                         
                        </div>
                        <div class="col-sm-6 col-xs-12">
                            <div class="row">
                                <div class="col-sm-4 col-xs-12" style="font-weight: bold;">
                                    Address
                                </div>
                                <div class="col-sm-8 col-xs-12">
                                    <asp:Label ID="lblAddr1" runat="server"></asp:Label>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-4 col-xs-12" style="font-weight: bold;">
                                    &nbsp;
                                </div>
                                <div class="col-sm-8 col-xs-12">
                                    <asp:Label ID="lblAddr2" runat="server"></asp:Label>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-4 col-xs-12" style="font-weight: bold;">
                                    City
                                </div>
                                <div class="col-sm-8 col-xs-12">
                                    <asp:Label ID="lblCity" runat="server"></asp:Label>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-4 col-xs-12" style="font-weight: bold;">
                                    State
                                </div>
                                <div class="col-sm-8 col-xs-12">
                                    <asp:Label ID="lblState" runat="server"></asp:Label>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-4 col-xs-12" style="font-weight: bold;">
                                    Country
                                </div>
                                <div class="col-sm-8 col-xs-12">
                                    <asp:Label ID="lblCountry" runat="server"></asp:Label>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-4 col-xs-12" style="font-weight: bold;">
                                    Zip
                                </div>
                                <div class="col-sm-8 col-xs-12">
                                    <asp:Label ID="lblZip" runat="server"></asp:Label>
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
                                        <div class="col-sm-12 col-xs-12">
                                            <div class="pull-left" style="margin-top: 8px;">
                                                <h3 class="h3Text">Institutions</h3>
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
                                                                <ComponentArt:GridColumn DataField="code" Align="left" HeadingText="Code" AllowGrouping="false" Width="80" />
                                                                <ComponentArt:GridColumn DataField="name" Align="left" HeadingText="Name" AllowGrouping="false" Width="250" />
                                                                <ComponentArt:GridColumn DataField="consult_applicable" Align="center" HeadingText="Consult Applicable?" AllowGrouping="false" Width="120" />
                                                                <ComponentArt:GridColumn DataField="storage_applicable" Align="center" HeadingText="Storage Applicable?" AllowGrouping="false" Width="120" />    
                                                            </Columns>

                                                        </ComponentArt:GridLevel>
                                                    </Levels>
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
                                                        <ComponentArt:GridColumn DataField="institution_id" Align="left" HeadingText="#" AllowGrouping="false" Width="30" Visible="false"/>
                                                        <ComponentArt:GridColumn DataField="physician_id" Align="left" HeadingText="physician_id" AllowGrouping="false" Visible="false" />
                                                        <ComponentArt:GridColumn DataField="physician_fname" Align="left" HeadingText="First Name" AllowGrouping="false" Width="130" FixedWidth="true" DataCellClientTemplateId="FNAME" />
                                                        <ComponentArt:GridColumn DataField="physician_lname" Align="left" HeadingText="Last Name" AllowGrouping="false" Width="130" FixedWidth="true" DataCellClientTemplateId="LNAME" />
                                                        <ComponentArt:GridColumn DataField="physician_credentials" Align="left" HeadingText="Credentials" AllowGrouping="false" Width="80" FixedWidth="true" DataCellClientTemplateId="CRED" />
                                                        <ComponentArt:GridColumn DataField="physician_email" Align="left" HeadingText="Email ID (Contact)" AllowGrouping="false" Width="350" FixedWidth="true" DataCellClientTemplateId="EMAIL" />
                                                        <ComponentArt:GridColumn DataField="physician_mobile" Align="left" HeadingText="Mobile #" AllowGrouping="false" Width="200" FixedWidth="true" DataCellClientTemplateId="MOBILE" />

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
                                                    <input type="text" id="txtPhysEmail_## DataItem.GetMember('physician_id').Value ##" maxlength="500" style="width: 85%;" class="GridTextBoxNoBorder" readonly="readonly" value="## DataItem.GetMember('physician_email').Value ##" onchange="javascript:txtPhysEmail_OnChange('## DataItem.GetMember('institution_id').Value ##','## DataItem.GetMember('physician_id').Value ##');" />
                                                    <button type="button" id="btnEditPhysEmail_## DataItem.GetMember('physician_id').Value ##" class="btn btn-warning btn_grd" onclick="javascript:btnEditPhysEmail_OnClick('## DataItem.GetMember('institution_id').Value ##','## DataItem.GetMember('physician_id').Value ##');" title="click to add/update the email id(s)"><i class="fa fa-pencil" aria-hidden="true"></i></button>
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
                                        <asp:TextBox ID="txtLoginEmail" runat="server" CssClass="form-control" MaxLength="50"></asp:TextBox>
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
       

            <div class="sparklineHeader mt-b-10 marginTP10">
                <div class="sparkline10-hd">
                    <div class="row">
                        <div class="col-sm-6 hidden-xs">
                        </div>
                        <div class="col-sm-6 col-xs-12 text-right">
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
    var objrdoEmail= document.getElementById('<%=rdoEmail.ClientID %>');
    var objrdoSMS= document.getElementById('<%=rdoSMS.ClientID %>');
    var objrdoBoth= document.getElementById('<%=rdoBoth.ClientID %>');
    var objtxtLoginID = document.getElementById('<%=txtLoginID.ClientID %>');//---
    var objtxtPwd = document.getElementById('<%=txtPwd.ClientID %>');//---
    var objtxtLoginEmail= document.getElementById('<%=txtLoginEmail.ClientID %>');
    var objtxtUserMobile= document.getElementById('<%=txtUserMobile.ClientID %>');
    var strForm = "VRSBillingAcctDlg";

</script>
<script src="../scripts/custome-javascript.js"></script>
<script src="../scripts/AppPages.js"></script>
<script src="scripts/BillingAcctDlg.js"></script>
</html>
