<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VRSDeleteStudyDlg.aspx.cs" Inherits="VETRIS.HouseKeeping.VRSDeleteStudyDlg" %>
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
    <link href="../css/style.css?1" rel="stylesheet" />
    <link href="../css/CalendarStyle.css" rel="stylesheet" />
    <link href="../css/grid_style.css?1" rel="stylesheet" />

    <script src="../scripts/jquery.min.js"></script>
    <script src="scripts/DeleteStudyDlgHdr.js"></script>
</head>
<body>
    <form id="form1" runat="server">
      <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
            <div class="sparklineHeader mt-b-10 marginTP10">
                <div class="sparkline10-hd">
                    <div class="row">
                        <div class="col-sm-4 col-xs-12">
                            <h2>Study Details </h2>
                        </div>

                        <div class="col-sm-8 col-xs-12 text-right">
                            <button type="button" class="btn btn-custon-four btn-primary" id="btnDel1" runat="server">
                                <i class="fa fa-trash-o edu-danger-error" aria-hidden="true"></i>&nbsp;Delete   
                            </button>
                            <button type="button" class="btn btn-custon-four btn-danger" id="btnClose1" runat="server">
                                <i class="fa fa-times edu-danger-error" aria-hidden="true"></i>&nbsp;Close   
                            </button>

                        </div>
                    </div>
                </div>
            </div>

            <div class="sparklineHeader mt-b-10 marginTP10">
                <div class="sparkline10-hd">
                    <div class="row">
                        <div class="col-sm-12 col-xs-12">
                            Study UID:&nbsp;<asp:Label ID="lblSUID" runat="server"></asp:Label>

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
                                    <div class="pull-left">
                                        <h3 style="color: #1e77bb;">Patient Details</h3>
                                    </div>
                                    <div class="borderSearch pull-left"></div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-12 col-xs-12">
                                    <div class="row">
                                        <div class="col-sm-6 col-xs-12">
                                            <div class="row">
                                                <div class="col-sm-3 col-xs-12">
                                                    ID<span class="mandatory">*</span>
                                                </div>
                                                <div class="col-sm-9 col-xs-12 marginMobileTP5">
                                                    <asp:Label ID="lblPID" runat="server" CssClass="form-control"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-sm-6 col-xs-12 marginMobileTP10">
                                            <div class="row">
                                                <div class="col-sm-3 col-xs-12">
                                                    Name<span class="mandatory">*</span>
                                                </div>
                                                <div class="col-sm-9 col-xs-12 marginMobileTP5">
                                                    <asp:Label ID="lblPName" runat="server" CssClass="form-control"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-sm-6 col-xs-12 marginTP10">
                                            <div class="row">
                                                <div class="col-sm-3 col-xs-12">
                                                    Sex
                                                </div>
                                                <div class="col-sm-9 col-xs-12 marginMobileTP5">
                                                    <asp:Label ID="lblSex" runat="server" CssClass="form-control">
                                                               
                                                    </asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-sm-6 col-xs-12 marginTP10">
                                            <div class="row">
                                                <div class="col-sm-3 col-xs-12">
                                                    Spayed/Neutered
                                                </div>
                                                <div class="col-sm-9 col-xs-12 marginMobileTP5">
                                                    <asp:Label ID="lblSN" runat="server" CssClass="form-control">
                                                               
                                                    </asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-sm-6 col-xs-12 marginTP10">
                                            <div class="row">
                                                <div class="col-sm-3 col-xs-12">
                                                    Date Of Birth
                                                </div>
                                                <div class="col-sm-9 col-xs-12 marginMobileTP5">
                                                    <asp:Label ID="lblFromDt" runat="server" CssClass="form-control" MaxLength="10" Width="60%" placeholder="" Style="float: left;"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-sm-6 col-xs-12 marginTP10">
                                            <div class="row">
                                                <div class="col-sm-3 col-xs-12">
                                                   Age
                                                </div>
                                                <div class="col-sm-9 col-xs-12 marginMobileTP5">
                                                   <asp:Label ID="lblAge" runat="server" CssClass="form-control"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-sm-6 col-xs-12 marginTP10">
                                            <div class="row">
                                                <div class="col-sm-3 col-xs-12">
                                                    Species
                                                </div>
                                                <div class="col-sm-9 col-xs-12 marginMobileTP5">
                                                    <asp:Label ID="lblSpecies" runat="server" CssClass="form-control"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-sm-6 col-xs-12 marginTP10">
                                            <div class="row">
                                                <div class="col-sm-3 col-xs-12">
                                                   Breed
                                                </div>
                                                <div class="col-sm-9 col-xs-12 marginMobileTP5">
                                                   <asp:Label ID="lblBreed" runat="server" CssClass="form-control"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-sm-6 col-xs-12 marginTP10">
                                            <div class="row">
                                                <div class="col-sm-3 col-xs-12">
                                                    Weight
                                                </div>
                                                <div class="col-sm-9 col-xs-12 marginMobileTP5">
                                                    <asp:Label ID="lblWt" runat="server" CssClass="form-control" Width="20%" Style="text-align: right; float: left;"></asp:Label>
                                                            <asp:Label ID="lblUOM" runat="server" CssClass="form-control custom-select-value" Width="15%" Style="float: left; margin-left: 5px;"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-sm-6 col-xs-12 marginTP10">
                                            <div class="row">
                                                <div class="col-sm-3 col-xs-12">
                                                   Owner - First Name 
                                                </div>
                                                <div class="col-sm-9 col-xs-12 marginMobileTP5">
                                                   <asp:Label ID="lblOwnerFN" runat="server" CssClass="form-control"></asp:Label> 
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-sm-6 col-xs-12 marginTP10">
                                            <div class="row">
                                                <div class="col-sm-3 col-xs-12">
                                                   Last Name
                                                </div>
                                                <div class="col-sm-9 col-xs-12 marginMobileTP5">
                                                    <asp:Label ID="lblOwnerLN" runat="server" CssClass="form-control"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="searchSection marginTP10">
                            <div class="row">
                                <div class="col-sm-12 col-xs-12">
                                    <div class="pull-left">
                                        <h3 style="color: #1e77bb;">Study Details</h3>
                                    </div>
                                    <div class="borderSearch pull-left"></div>
                                </div>
                            </div>


                            <div class="row">
                                <div class="col-sm-12 col-xs-12">
                                    <div class="row">
                                        <div class="col-sm-4 col-xs-12 marginMobileTP10">
                                            <div class="row">
                                                <div class="col-sm-3 col-xs-12">
                                                    Study Date
                                                </div>
                                                <div class="col-sm-9 col-xs-12 marginMobileTP5">
                                                    <asp:Label ID="lblStudyDt" runat="server" CssClass="form-control"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-sm-4 col-xs-12 marginMobileTP10">
                                            <div class="row">
                                                <div class="col-sm-4 col-xs-12">
                                                    Accession No.
                                                </div>
                                                <div class="col-sm-8 col-xs-12 marginMobileTP5">
                                                    <asp:Label ID="lblAccnNo" runat="server" CssClass="form-control"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-sm-4 col-xs-12 marginMobileTP10">
                                            <div class="row">
                                                <div class="col-sm-3 col-xs-12">
                                                    Priority
                                                </div>
                                                <div class="col-sm-9 col-xs-12 marginMobileTP5">
                                                   <asp:Label ID="lblPriority" runat="server" CssClass="form-control"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-sm-3 col-xs-12 marginTP10">
                                            History/Reason for study
                                        </div>
                                        <div class="col-sm-9 col-xs-12 marginTP10">
                                            <div id="divReason" style="width: 98%; height: 80px; overflow: auto" runat="server">
                                                            </div>
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-sm-6 col-xs-12 marginTP10">
                                            <div class="row">
                                                <div class="col-sm-3 col-xs-12">
                                                    Modality
                                                </div>
                                                <div class="col-sm-9 col-xs-12">
                                                    <asp:Label ID="lblModality" runat="server" CssClass="form-control"></asp:Label>
                                                            <input type="hidden" id="hdnModalityID" runat="server" value="0" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-sm-12 col-xs-12 marginTP10">
                                            Study Type
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-sm-12 col-xs-12 marginTP10">
                                            <div class="table-responsive">
                                                <ComponentArt:CallBack ID="CallBackST" runat="server" OnCallback="CallBackST_Callback">
                                                                <Content>
                                                                    <ComponentArt:Grid
                                                                        ID="grdST"
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
                                                                                DataKeyField="srl_no"
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
                                                                                    <ComponentArt:GridConditionalFormat ClientFilter="((DataItem.get_index() + grdST.get_recordOffset()) % 2) > 0" RowCssClass="AltRow" SelectedRowCssClass="AltRow" />
                                                                                    <ComponentArt:GridConditionalFormat ClientFilter="((DataItem.get_index() + grdST.get_recordOffset()) % 2) == 0" RowCssClass="Row" SelectedRowCssClass="Row" />
                                                                                </ConditionalFormats>
                                                                                <Columns>
                                                                                    <ComponentArt:GridColumn DataField="srl_no" Align="left" HeadingText="#" AllowGrouping="false" Width="30" />
                                                                                    <ComponentArt:GridColumn DataField="study_type_id" Align="left" HeadingText="study_type_id" AllowGrouping="false" Visible="false" />
                                                                                    <ComponentArt:GridColumn DataField="study_type_name" Align="left" HeadingText="Study Type" AllowGrouping="false" Width="200" />

                                                                                </Columns>

                                                                            </ComponentArt:GridLevel>
                                                                        </Levels>



                                                                    </ComponentArt:Grid>
                                                                    <span id="spnErrST" runat="server"></span>
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
                                                                    <CallbackComplete EventHandler="grdST_onCallbackComplete" />
                                                                </ClientEvents>
                                                            </ComponentArt:CallBack>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-sm-6 col-xs-12 marginTP10">
                                            <div class="row">
                                                <div class="col-sm-4 col-xs-12">
                                                    Image Count Received
                                                </div>
                                                <div class="col-sm-8 col-xs-12 marginMobileTP5">
                                                    <asp:Label ID="lblImgCnt" runat="server" CssClass="form-control"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-sm-6 col-xs-12 marginTP10">
                                            <div class="row">
                                                <div class="col-sm-4 col-xs-12">
                                                   Confirm Image Count?
                                                </div>
                                                <div class="col-sm-8 col-xs-12 marginMobileTP5">
                                                    <asp:Label ID="lblConfImgCnt" runat="server" CssClass="form-control"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-sm-6 col-xs-12 marginTP10">
                                            <div class="row">
                                                <div class="col-sm-4 col-xs-12">
                                                    Institution
                                                </div>
                                                <div class="col-sm-8 col-xs-12 marginMobileTP5">
                                                   <asp:Label ID="lblInstitution" runat="server" CssClass="form-control"></asp:Label> 
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-sm-6 col-xs-12 marginTP10">
                                            <div class="row">
                                                <div class="col-sm-4 col-xs-12">
                                                   Referring Physician
                                                </div>
                                                <div class="col-sm-8 col-xs-12 marginMobileTP5">
                                                    <asp:Label ID="lblPhys" runat="server" CssClass="form-control"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                </div>
            </div>
            <div class="sparkline10-list mt-b-10">
                <div class="searchSection">
                    <div class="row">
                        <div class="col-sm-12 col-xs-12">
                            <div class="row">
                                <div class="col-sm-6 col-xs-6">
                                    <div class="pull-left" style="margin-top: 8px;">
                                        <h3 style="color: #1e77bb;">Uploaded Document(s)</h3>
                                    </div>
                                </div>
                                <div class="col-sm-6 col-xs-6 text-right">
                                    &nbsp;
                                   
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-12 col-xs-12">
                            <div class="borderSearch"></div>
                        </div>
                    </div>
                    <div class="row">

                        <div class="col-sm-12 col-xs-12">
                            <div class="table-responsive">
                                <ComponentArt:CallBack ID="CallBackDoc" runat="server" OnCallback="CallBackDoc_Callback">
                                    <Content>
                                        <ComponentArt:Grid
                                            ID="grdDoc"
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
                                                    DataKeyField="document_srl_no"
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
                                                        <ComponentArt:GridConditionalFormat ClientFilter="((DataItem.get_index() + grdDoc.get_recordOffset()) % 2) > 0" RowCssClass="AltRow" SelectedRowCssClass="AltRow" />
                                                        <ComponentArt:GridConditionalFormat ClientFilter="((DataItem.get_index() + grdDoc.get_recordOffset()) % 2) == 0" RowCssClass="Row" SelectedRowCssClass="Row" />
                                                    </ConditionalFormats>
                                                    <Columns>
                                                        <ComponentArt:GridColumn DataField="document_srl_no" Align="left" HeadingText="#" AllowGrouping="false" Width="30" />
                                                        <ComponentArt:GridColumn DataField="document_id" Align="left" HeadingText="document_id" AllowGrouping="false" Visible="false" />
                                                        <ComponentArt:GridColumn DataField="document_name" Align="left" HeadingText="Document Name" AllowGrouping="false" Width="150" />
                                                        <ComponentArt:GridColumn DataField="document_link" Align="left" HeadingText="Document Link" AllowGrouping="false" DataCellClientTemplateId="FILE" FixedWidth="True" Width="150" />
                                                        <ComponentArt:GridColumn DataField="document_file_type" Align="left" HeadingText="document_file_type" AllowGrouping="false" Visible="false" />

                                                    </Columns>

                                                </ComponentArt:GridLevel>
                                            </Levels>

                                            <ClientEvents>
                                                <RenderComplete EventHandler="grdDoc_onRenderComplete" />
                                            </ClientEvents>
                                            <ClientTemplates>
                                                <ComponentArt:ClientTemplate ID="FILE">
                                                    <a href="#" style="color: blue; text-decoration: underline" id="lnkDoc_## DataItem.GetMember('document_srl_no').Value ##" title="## DataItem.GetMember('document_link').Value ##" onclick="ShowDocument('## DataItem.GetMember('document_srl_no').Value ##','## DataItem.GetMember('document_link').Value ##')">## DataItem.GetMember('document_link').Value ##</a>
                                                </ComponentArt:ClientTemplate>

                                            </ClientTemplates>
                                        </ComponentArt:Grid>
                                        <span id="spnERRDoc" runat="server"></span>
                                    </Content>
                                    <LoadingPanelClientTemplate>
                                        <table style="height: 235px; width: 100%;" border="0">
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
                                        <CallbackComplete EventHandler="grdDoc_onCallbackComplete" />
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
                        <div class="col-sm-4 col-xs-12 marginTP10">
                            &nbsp;
                        </div>

                        <div class="col-sm-8 col-xs-12 text-right">
                             <button type="button" class="btn btn-custon-four btn-primary" id="btnDel2"  runat="server">
                                <i class="fa fa-trash-o edu-danger-error" aria-hidden="true"></i>&nbsp;Delete   
                            </button>
                            <button type="button" class="btn btn-custon-four btn-danger" id="btnClose2" runat="server">
                                <i class="fa fa-times edu-danger-error" aria-hidden="true"></i>&nbsp;Close     
                            </button>


                        </div>
                    </div>
                </div>
            </div>
        </div>


        <asp:Label ID="lblPatientID" runat="server" Style="display: none;"></asp:Label>


        <input type="hidden" id="hdnError" runat="server" value="" />
        <input type="hidden" id="hdnID" runat="server" value="00000000-0000-0000-0000-000000000000" />
        <input type="hidden" id="hdnSUID" runat="server" value="" />
        <input type="hidden" id="hdnFilename" runat="server" value="" />
        <input type="hidden" id="hdnFilePath" runat="server" value="" />
    </form>
</body>
<script type="text/javascript">
    var objhdnID = document.getElementById('<%=hdnID.ClientID %>');
    var objhdnError = document.getElementById('<%=hdnError.ClientID %>');
    var objhdnSUID = document.getElementById('<%=hdnSUID.ClientID %>');
    var objlblPID = document.getElementById('<%=lblPID.ClientID %>');
    var objlblPName = document.getElementById('<%=lblPName.ClientID %>');
    var objlblWt = document.getElementById('<%=lblWt.ClientID %>');
    var objlblUOM = document.getElementById('<%=lblUOM.ClientID %>');
    var objlblFromDt = document.getElementById('<%=lblFromDt.ClientID %>');
    var objlblAge = document.getElementById('<%=lblAge.ClientID %>');
    var objlblSex = document.getElementById('<%=lblSex.ClientID %>');
    var objlblSN = document.getElementById('<%=lblSN.ClientID %>');
    var objlblSpecies = document.getElementById('<%=lblSpecies.ClientID %>');
    var objlblBreed = document.getElementById('<%=lblBreed.ClientID %>');
    var objlblOwnerFN = document.getElementById('<%=lblOwnerFN.ClientID %>');
    var objlblOwnerLN = document.getElementById('<%=lblOwnerLN.ClientID %>');
    var objlblAccnNo = document.getElementById('<%=lblAccnNo.ClientID %>');
    var objlblPriority = document.getElementById('<%=lblPriority.ClientID %>');

    var objlblModality = document.getElementById('<%=lblModality.ClientID %>');
    var objhdnModalityID = document.getElementById('<%=hdnModalityID.ClientID %>');
    var objlblImgCnt = document.getElementById('<%=lblImgCnt.ClientID %>');

    var objlblInstitution = document.getElementById('<%=lblInstitution.ClientID %>');
    var objlblPhys = document.getElementById('<%=lblPhys.ClientID %>');
    var objhdnFilename = document.getElementById('<%=hdnFilename.ClientID %>');
    var objhdnFilePath = document.getElementById('<%=hdnFilePath.ClientID %>');
    var objlblPatientID = document.getElementById('<%=lblPatientID.ClientID %>');
    var objlblAccnNo = document.getElementById('<%=lblAccnNo.ClientID %>');


    var strForm = "VRSDeleteStudyDlg";

</script>
<script src="../scripts/custome-javascript.js"></script>
<script src="../scripts/AppPages.js"></script>
<script src="scripts/DeleteStudyDlg.js"></script>
</html>
