<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VRSWorkListDlg.aspx.cs" Inherits="VETRIS.CaseList.VRSWorkListDlg" %>

<%@ Register Assembly="ComponentArt.Web.UI" Namespace="ComponentArt.Web.UI" TagPrefix="ComponentArt" %>
<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>
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

    <script src="ckeditor/ckeditor.js"></script>
    <script src="../scripts/jquery.min.js"></script>
    <script src="scripts/WorkListDlgHdr.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
            <div class="sparklineHeader mt-b-10 marginTP10">
                <div class="sparkline10-hd">
                    <div class="row">
                        <div class="col-sm-2 col-xs-12">
                            <h2>Study Details </h2>
                        </div>
                        <div class="col-sm-2 col-xs-12">
                            <button type="button" class="btn btn-custon-four btn-danger" id="btnDel1" runat="server">
                                <i class="fa fa-trash-o edu-danger-error" aria-hidden="true"></i>&nbsp;DELETE CASE
                            </button>
                        </div>
                        <div class="col-sm-5 col-xs-12 text-center">
                            <button type="button" class="btn btn-custon-four btn-primary" id="btnSave1" runat="server">
                                <i class="fa fa-floppy-o edu-danger-error" aria-hidden="true"></i>&nbsp;Save       
                            </button>

                            <button type="button" class="btn btn-custon-four btn-warning" id="btnReset1" runat="server">
                                <i class="fa fa-repeat edu-danger-error" aria-hidden="true"></i>&nbsp;Reset   
                            </button>

                            <button type="button" class="btn btn-custon-four btn-danger" id="btnClose1" runat="server">
                                <i class="fa fa-times edu-danger-error" aria-hidden="true"></i>&nbsp;Close   
                            </button>

                        </div>
                        <div class="col-sm-3 col-xs-12 text-right">
                            <button type="button" class="btn btn-custon-four btn-success" id="btnView1" runat="server">
                                <i class="fa fa-eye edu-danger-error" aria-hidden="true"></i>&nbsp;VIEW STUDY & IMAGE(S)
                                       
                            </button>
                        </div>
                    </div>
                </div>
            </div>
            <div class="sparkline10-list mt-b-10" id="divStudyUID">
                <div class="sparkline10-hd">
                    <div class="row">
                        <div class="col-sm-8 col-xs-8">
                            Study UID : 
                            <asp:Label ID="lblSUID" runat="server"></asp:Label>
                        </div>
                        <div class="col-sm-4 col-xs-4">
                            &nbsp;
                        </div>

                    </div>
                </div>
            </div>
            <div class="sparkline10-list mt-b-10">
                <div class="row">
                    <div class="col-sm-6 col-xs-12">
                        <div class="searchSection">
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
                                        <div class="col-sm-4 col-xs-12">
                                            Study Date
                                        </div>
                                        <div class="col-sm-8 col-xs-12">
                                            <asp:Label ID="lblDOS" runat="server"></asp:Label>
                                        </div>

                                    </div>

                                    <div class="row marginTP10">
                                        <div class="col-sm-4 col-xs-12">
                                            Modality
                                        </div>
                                        <div class="col-sm-8 col-xs-12">
                                            <asp:Label ID="lblModality" runat="server"></asp:Label>
                                        </div>

                                    </div>
                                    <div class="row marginTP10">
                                        <div class="col-sm-4 col-xs-12">
                                            Category
                                        </div>
                                        <div class="col-sm-8 col-xs-12">
                                            <asp:Label ID="lblCategory" runat="server"></asp:Label>
                                        </div>

                                    </div>
                                    <div class="row marginTP10">
                                        <div class="col-sm-4 col-xs-12">
                                            Institution
                                        </div>
                                        <div class="col-sm-8 col-xs-12">
                                            <asp:Label ID="lblInstitution" runat="server"></asp:Label>
                                        </div>

                                    </div>

                                    <div class="row marginTP10">
                                        <div class="col-sm-4 col-xs-12">
                                            Referring Physician
                                        </div>
                                        <div class="col-sm-8 col-xs-12">
                                            <asp:Label ID="lblPhys" runat="server"></asp:Label>

                                        </div>

                                    </div>

                                    <div class="row marginTP10">
                                        <div class="col-sm-4 col-xs-12">
                                            Accession No.
                                        </div>
                                        <div class="col-sm-8 col-xs-12">
                                            <asp:Label ID="lblAccnNo" runat="server"></asp:Label>
                                        </div>

                                    </div>
                                    <div class="row marginTP10">
                                        <div class="col-sm-4 col-xs-12">
                                            Priority
                                        </div>
                                        <div class="col-sm-8 col-xs-12">
                                            <asp:Label ID="lblPriority" runat="server"></asp:Label>
                                        </div>

                                    </div>
                                    <div class="row marginTP10">
                                        <div class="col-sm-4 col-xs-12">
                                            Service(s)
                                        </div>
                                        <div class="col-sm-8 col-xs-12">
                                            <asp:Label ID="lblServices" runat="server"></asp:Label>
                                        </div>
                                    </div>

                                    <div class="row marginTP10">
                                        <div class="col-sm-4 col-xs-12">
                                            No. of Image(s)
                                        </div>
                                        <div class="col-sm-8 col-xs-12">
                                            <asp:Label ID="lblImgCnt" runat="server"></asp:Label>
                                        </div>

                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-6 col-xs-12">
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
                                <div class="col-sm-4 col-xs-12">
                                    Patient ID
                                </div>
                                <div class="col-sm-8 col-xs-12">
                                    <asp:Label ID="lblPatientID" runat="server"></asp:Label>
                                </div>

                            </div>

                            <div class="row marginTP10">
                                <div class="col-sm-4 col-xs-12">
                                    Patient Name
                                </div>
                                <div class="col-sm-8 col-xs-12">
                                    <asp:Label ID="lblPatientName" runat="server"></asp:Label>
                                </div>

                            </div>
                            <div class="row marginTP10">
                                <div class="col-sm-4 col-xs-12">
                                    Date Of Birth/Age
                                </div>
                                <div class="col-sm-4 col-xs-12">
                                    <asp:Label ID="lblDOB" runat="server"></asp:Label>
                                </div>
                                <div class="col-sm-4 col-xs-12">
                                    <asp:Label ID="lblAge" runat="server"></asp:Label>
                                </div>
                            </div>
                            <div class="row marginTP10">

                                <div class="col-sm-4 col-xs-12">
                                    Sex
                                </div>
                                <div class="col-sm-8 col-xs-12">
                                    <asp:Label ID="lblSex" runat="server"></asp:Label>
                                </div>
                            </div>
                            <div class="row marginTP10">

                                <div class="col-sm-4 col-xs-12">
                                    Spayed/Neutered
                                </div>
                                <div class="col-sm-8 col-xs-12">
                                    <asp:Label ID="lblSN" runat="server"></asp:Label>
                                </div>
                            </div>
                            <div class="row marginTP10">
                                <div class="col-sm-4 col-xs-12">
                                    Weight
                                </div>
                                <div class="col-sm-8 col-xs-12">
                                    <asp:Label ID="lblPWt" runat="server"></asp:Label>
                                </div>
                            </div>
                            <div class="row marginTP10">

                                <div class="col-sm-4 col-xs-12">
                                    Species
                                </div>
                                <div class="col-sm-8 col-xs-12">
                                    <asp:Label ID="lblSpecies" runat="server"></asp:Label>
                                </div>
                            </div>
                            <div class="row marginTP10">

                                <div class="col-sm-4 col-xs-12">
                                    Breed
                                </div>
                                <div class="col-sm-8 col-xs-12">
                                    <asp:Label ID="lblBreed" runat="server"></asp:Label>
                                </div>
                            </div>
                            <div class="row marginTP10">

                                <div class="col-sm-4 col-xs-12">
                                    Owner Name
                                </div>
                                <div class="col-sm-8 col-xs-12">
                                    <asp:Label ID="lblOwner" runat="server"></asp:Label>
                                </div>

                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-6 col-xs-12">
                        <div class="searchSection marginTP10">
                            <div class="row">
                                <div class="col-sm-12 col-xs-12">
                                    <div class="pull-left">
                                        <h3 style="color: #1e77bb;">History/Reason for study</h3>
                                    </div>
                                    <div class="borderSearch pull-left"></div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-12 col-xs-12 marginTP5">
                                    <div id="divHistory" runat="server" style="height: 80px; overflow: auto"></div>
                                </div>


                            </div>
                        </div>
                    </div>
                    <div class="col-sm-6 col-xs-12">
                        <div class="searchSection marginTP10">
                            <div class="row">
                                <div class="col-sm-12 col-xs-12">
                                    <div class="pull-left">
                                        <h3 style="color: #1e77bb;">Note For Physician</h3>
                                    </div>
                                    <div class="borderSearch pull-left"></div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-12 col-xs-12 marginTP5">
                                    <div id="divPhysNote" runat="server" style="height: 80px; overflow: auto"></div>
                                </div>


                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-6 col-xs-12">
                        <div class="searchSection marginTP10">
                            <div class="row">
                                <div class="col-sm-12 col-xs-12">
                                    <div class="pull-left">
                                        <h3 style="color: #1e77bb;">Study Types</h3>
                                    </div>
                                    <div class="borderSearch pull-left"></div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-12 col-xs-12 marginMobileTP5">
                                    <div class="table-responsive">
                                        <ComponentArt:CallBack ID="CallBackSelST" runat="server" OnCallback="CallBackSelST_Callback">
                                            <Content>
                                                <ComponentArt:Grid
                                                    ID="grdSelST"
                                                    CssClass="Grid"
                                                    DataAreaCssClass="GridData4"
                                                    SearchOnKeyPress="true"
                                                    EnableViewState="true"
                                                    ShowSearchBox="false"
                                                    SearchBoxPosition="TopRight"
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
                                                                <ComponentArt:GridConditionalFormat ClientFilter="((DataItem.get_index() + grdSelST.get_recordOffset()) % 2) > 0" RowCssClass="AltRow" SelectedRowCssClass="AltRow" />
                                                                <ComponentArt:GridConditionalFormat ClientFilter="((DataItem.get_index() + grdSelST.get_recordOffset()) % 2) == 0" RowCssClass="Row" SelectedRowCssClass="Row" />
                                                            </ConditionalFormats>
                                                            <Columns>
                                                                <ComponentArt:GridColumn DataField="srl_no" Align="left" HeadingText="#" AllowGrouping="false" Width="30" />
                                                                <ComponentArt:GridColumn DataField="study_type_id" Align="left" HeadingText="study_type_id" AllowGrouping="false" Visible="false" />
                                                                <ComponentArt:GridColumn DataField="study_type_name" Align="left" HeadingText="Study Type" AllowGrouping="false" Width="300" />

                                                            </Columns>

                                                        </ComponentArt:GridLevel>
                                                    </Levels>


                                                </ComponentArt:Grid>
                                                <span id="spnErrSelST" runat="server"></span>
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
                                                <CallbackComplete EventHandler="grdSelST_onCallbackComplete" />
                                            </ClientEvents>
                                        </ComponentArt:CallBack>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-6 col-xs-12">
                        <div class="searchSection marginTP10">
                            <div class="row">
                                <div class="col-sm-12 col-xs-12">
                                    <div class="row">
                                        <div class="col-sm-8 col-xs-8">
                                            <h3 style="color: #1e77bb;">Uploaded Document(s)</h3>
                                        </div>
                                        <div class="col-sm-4 col-xs-4 text-right">
                                            &nbsp;
                                        </div>
                                    </div>
                                </div>
                                <div class="col-sm-12 col-xs-12">
                                    <div class="borderSearch"></div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-12 col-xs-12 marginMobileTP5">
                                    <div class="table-responsive">
                                        <ComponentArt:CallBack ID="CallBackDoc" runat="server" OnCallback="CallBackDoc_Callback">
                                            <Content>
                                                <ComponentArt:Grid
                                                    ID="grdDoc"
                                                    CssClass="Grid"
                                                    DataAreaCssClass="GridData4"
                                                    SearchOnKeyPress="true"
                                                    EnableViewState="true"
                                                    ShowSearchBox="true"
                                                    SearchBoxPosition="TopRight"
                                                    SearchTextCssClass="GridHeaderText"
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
                                                                <ComponentArt:GridColumn DataField="document_link" Align="left" HeadingText="Document Link" AllowGrouping="false" DataCellClientTemplateId="FILE_LINK" FixedWidth="True" Width="150" />
                                                                <ComponentArt:GridColumn DataField="document_file_type" Align="left" HeadingText="document_file_type" AllowGrouping="false" Visible="false" />
                                                                <ComponentArt:GridColumn DataField="del_doc" Align="center" AllowGrouping="false" DataCellClientTemplateId="DELDOC" HeadingText=" " FixedWidth="True" Width="30" Visible="false" />
                                                            </Columns>

                                                        </ComponentArt:GridLevel>
                                                    </Levels>

                                                    <ClientEvents>
                                                        <RenderComplete EventHandler="grdDoc_onRenderComplete" />
                                                    </ClientEvents>
                                                    <ClientTemplates>
                                                        <ComponentArt:ClientTemplate ID="FILE_LINK">
                                                            <a href="#" style="color: blue; text-decoration: underline" id="lnkDoc_## DataItem.GetMember('document_srl_no').Value ##" title="## DataItem.GetMember('document_link').Value ##" onclick="ShowDocument('## DataItem.GetMember('document_srl_no').Value ##','## DataItem.GetMember('document_link').Value ##')">## DataItem.GetMember('document_link').Value ##</a>
                                                        </ComponentArt:ClientTemplate>
                                                        <ComponentArt:ClientTemplate ID="DELDOC">
                                                            <button type="button" id="btnDelDoc_## DataItem.GetMember('document_srl_no').Value ##" class="btn btn-danger btn_grd" onclick="javascript:DeleteDocument('## DataItem.GetMember('document_srl_no').Value ##')"><i class="fa fa-trash" aria-hidden="true"></i></button>
                                                        </ComponentArt:ClientTemplate>
                                                    </ClientTemplates>
                                                </ComponentArt:Grid>
                                                <span id="spnERRDoc" runat="server"></span>
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
                                                <CallbackComplete EventHandler="grdDoc_onCallbackComplete" />
                                            </ClientEvents>
                                        </ComponentArt:CallBack>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-12 col-xs-12">
                        <div class="searchSection marginTP10">
                            <div class="row">
                                <div class="col-sm-12 col-xs-12">

                                    <div class="pull-left">
                                        <h3 style="color: #1e77bb;">Study Text</h3>
                                    </div>

                                    <div class="borderSearch pull-left"></div>
                                </div>


                            </div>
                            <div class="row">

                                <div class="col-sm-12 col-xs-12 marginMobileTP5">
                                    <asp:TextBox ID="txtStudy" runat="server" CssClass="form-control" MaxLength="100"></asp:TextBox>
                                </div>

                            </div>

                        </div>

                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-12 col-xs-12">

                        <div class="searchSection marginTP10">
                            <div class="row">
                                <div class="col-sm-12 col-xs-12">

                                    <div class="pull-left">
                                        <h3 style="color: #1e77bb;">Findings</h3>
                                    </div>

                                    <div class="borderSearch pull-left"></div>
                                </div>
                            </div>
                            <div class="row">

                                <div class="col-sm-12 col-xs-12">

                                    <CKEditor:CKEditorControl ID="editorFindings" runat="server" DisableNativeSpellChecker="false" RemovePlugins="scayt,contextmenu"
                                        Toolbar="Source|-|NewPage|Preview|-|Templates
                                                                                Cut|Copy|Paste|PasteText|PasteFromWord|-|Print
                                                                                Undo|Redo|-|Find|Replace|Para
                                                                                /
                                                                                Bold|Italic|Underline|-|Subscript|Superscript
                                                                                NumberedList|BulletedList|-|Outdent|Indent
                                                                                JustifyLeft|JustifyCenter|JustifyRight|JustifyBlock
                                                                                Table|HorizontalRule|SpecialChar|PageBreak
                                                                                /
                                                                                Styles|Format|Font|FontSize
                                                                                TextColor|BGColor
                                                                                Maximize|ShowBlocks"
                                        Height="100px" Width="100%">
                                    </CKEditor:CKEditorControl>
                                </div>


                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-12 col-xs-12">
                        <div class="searchSection marginTP10">
                            <div class="row">
                                <div class="col-sm-12 col-xs-12">

                                    <div class="pull-left">
                                        <h3 style="color: #1e77bb;">Conclusions</h3>
                                    </div>

                                    <div class="borderSearch pull-left"></div>
                                </div>


                            </div>
                            <div class="row">

                                <div class="col-sm-12 col-xs-12">

                                    <CKEditor:CKEditorControl ID="editorConcclusion" runat="server" DisableNativeSpellChecker="false" RemovePlugins="scayt,contextmenu"
                                        Toolbar="Source|-|NewPage|Preview|-|Templates
                                                                                Cut|Copy|Paste|PasteText|PasteFromWord|-|Print
                                                                                Undo|Redo|-|Find|Replace
                                                                                /
                                                                                Bold|Italic|Underline|-|Subscript|Superscript
                                                                                NumberedList|BulletedList|-|Outdent|Indent
                                                                                JustifyLeft|JustifyCenter|JustifyRight|JustifyBlock
                                                                                Table|HorizontalRule|SpecialChar|PageBreak
                                                                                /
                                                                                Styles|Format|Font|FontSize
                                                                                TextColor|BGColor|Paragraph
                                                                                Maximize|ShowBlocks"
                                        Height="100px" Width="100%">
                                    </CKEditor:CKEditorControl>
                                </div>


                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-12 col-xs-12">
                        <div class="searchSection marginTP10">
                            <div class="row">
                                <div class="col-sm-12 col-xs-12">

                                    <div class="pull-left">
                                        <h3 style="color: #1e77bb;">Status</h3>
                                    </div>

                                    <div class="borderSearch pull-left"></div>
                                </div>


                            </div>
                            <div class="row">

                                <div class="col-sm-4 col-xs-12">
                                    <div class="col-sm-12 col-xs-12">
                                        <div class="row">
                                            <div class="col-sm-4 col-xs-12 marginTP5">
                                                Current Status
                                            </div>
                                            <div class="col-sm-8 col-xs-12 marginTP5">
                                                <asp:Label ID="lblCurrStat" runat="server"></asp:Label>
                                            </div>

                                        </div>
                                    </div>
                                </div>
                                <div class="col-sm-6 col-xs-12">
                                    <div class="col-sm-12 col-xs-12">
                                        <div class="row">
                                            <div class="col-sm-4 col-xs-12 marginTP5">
                                                Change Status To
                                            </div>
                                            <div class="col-sm-8 col-xs-12 marginMobileTP5">
                                                <div class="pull-left grid_option customRadio">
                                                    <asp:RadioButton ID="rdoDict" runat="server" GroupName="grpStat" />
                                                    <label for="rdoDict" class="label-default" style="width: auto; margin-top: 10px;"></label>
                                                </div>
                                                <span class="pull-left" style="padding-left: 0; padding-top: 5px; padding-right: 2px;">Dictated</span> &nbsp;&nbsp;
                                                <div class="pull-left grid_option customRadio marginLFT10">
                                                    <asp:RadioButton ID="rdoPrelim" runat="server" GroupName="grpStat" />
                                                    <label for="rdoPrelim" class="label-default" style="width: auto; margin-top: 10px;"></label>
                                                </div>
                                                <span class="pull-left" style="padding-left: 0; padding-top: 5px; padding-right: 2px;">Preliminary</span>&nbsp;&nbsp;
                                                <div class="pull-left grid_option customRadio marginLFT10">
                                                    <asp:RadioButton ID="rdoFinal" runat="server" GroupName="grpStat" />
                                                    <label for="rdoFinal" class="label-default" style="width: auto; margin-top: 10px;"></label>
                                                </div>
                                                <span class="pull-left" style="padding-left: 0; padding-top: 5px; padding-right: 2px;">Final</span>

                                            </div>

                                        </div>
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
                        <div class="col-sm-2 col-xs-12">
                        </div>
                        <div class="col-sm-2 col-xs-12">
                            <button type="button" class="btn btn-custon-four btn-danger" id="btnDel2" runat="server">
                                <i class="fa fa-trash-o edu-danger-error" aria-hidden="true"></i>&nbsp;DELETE CASE   
                            </button>
                        </div>
                        <div class="col-sm-5 col-xs-12 text-center">
                            <button type="button" class="btn btn-custon-four btn-primary" id="btnSave2" runat="server">
                                <i class="fa fa-floppy-o edu-danger-error" aria-hidden="true"></i>&nbsp;Save   
                            </button>

                            <button type="button" class="btn btn-custon-four btn-warning" id="btnReset2" runat="server">
                                <i class="fa fa-repeat edu-danger-error" aria-hidden="true"></i>&nbsp;Reset 
                            </button>

                            <button type="button" class="btn btn-custon-four btn-danger" id="btnClose2" runat="server">
                                <i class="fa fa-times edu-danger-error" aria-hidden="true"></i>&nbsp;Close     
                            </button>

                        </div>
                        <div class="col-sm-3 col-xs-12 text-right">
                            <button type="button" class="btn btn-custon-four btn-success" id="btnView2" runat="server">
                                <i class="fa fa-eye edu-danger-error" aria-hidden="true"></i>&nbsp;VIEW STUDY & IMAGE(S)
                                       
                            </button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <input type="hidden" id="hdnError" runat="server" value="" />
        <input type="hidden" id="hdnID" runat="server" value="00000000-0000-0000-0000-000000000000" />
        <input type="hidden" id="hdnCF" runat="server" value="" />
        <input type="hidden" id="hdnSUID" runat="server" value="" />
        <input type="hidden" id="hdnFilename" runat="server" value="" />
        <input type="hidden" id="hdnFilePath" runat="server" value="" />
        <input type="hidden" id="hdnModalityID" runat="server" value="0" />
        <input type="hidden" id="hdnStatusID" runat="server" value="0" />
        <input type="hidden" id="hdnPACSURL" runat="server" value="" />
        <input type="hidden" id="hdnImgVwrURL" runat="server" value="" />
        <input type="hidden" id="hdnWS8SRVPWD" runat="server" value="" />
        <input type="hidden" id="hdnWS8SYVWRURL" runat="server" value="" />
        <input type="hidden" id="hdnStudyDelUrl" runat="server" value="" />
    </form>
</body>
<script type="text/javascript">
    var objhdnID = document.getElementById('<%=hdnID.ClientID %>');
    var objhdnError = document.getElementById('<%=hdnError.ClientID %>');
    var objhdnCF = document.getElementById('<%=hdnCF.ClientID %>');
    var objhdnSUID = document.getElementById('<%=hdnSUID.ClientID %>');
    var objhdnFilename = document.getElementById('<%=hdnFilename.ClientID %>');
    var objhdnFilePath = document.getElementById('<%=hdnFilePath.ClientID %>');
    var objlblPatientID = document.getElementById('<%=lblPatientID.ClientID %>');
    var objlblAccnNo = document.getElementById('<%=lblAccnNo.ClientID %>');
    var objhdnModalityID = document.getElementById('<%=hdnModalityID.ClientID %>');
    var objhdnStatusID = document.getElementById('<%=hdnStatusID.ClientID %>');
    var objhdnWS8SRVPWD = document.getElementById('<%=hdnWS8SRVPWD.ClientID %>');
    var objhdnStudyDelUrl = document.getElementById('<%=hdnStudyDelUrl.ClientID %>');
    var objhdnWS8SYVWRURL = document.getElementById('<%=hdnWS8SYVWRURL.ClientID %>');
    var strForm = "VRSWorkListDlg";

</script>
<script src="../scripts/custome-javascript.js"></script>
<script src="../scripts/AppPages.js?1"></script>
<script src="scripts/WorkListDlg.js?11042020"></script>
</html>
