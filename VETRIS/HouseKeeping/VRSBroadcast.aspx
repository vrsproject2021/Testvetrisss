<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VRSBroadcast.aspx.cs" Inherits="VETRIS.HouseKeeping.VRSBroadcast" %>
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

    <link href="../css/grid_style.css" rel="stylesheet" />
    <link href="../css/grid_style.css" rel="stylesheet" />--%>
    <link id="lnkSTYLE" runat="server" href = "../css/style.css" rel="stylesheet" type="text/css" />
    <link id="lnkGRID" runat="server" href = "../css/grid_style.css" rel="stylesheet" type="text/css" />
    <script src="../scripts/jquery.min.js"></script>
    <script src="scripts/BroadcastHdr.js?1"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12"> 
            <div class="sparklineHeader mt-b-10 marginTP10">
                <div class="sparkline10-hd">
                    <div class="row">
                        <div class="col-sm-6 col-xs-12">
                            <h2>Broadcasts</h2>
                        </div>
                        <div class="col-sm-6 col-xs-12 text-right">
                            
                            <button type="button" id="btnClose" runat="server" class="btn btn-custon-four btn-danger">
                                <i class="fa fa-times" aria-hidden="true"></i>&nbsp;Close</button>
                        </div>
                    </div>
                </div>
            </div>

            <div class="sparklineHeader mt-b-10 marginTP10">
                <div class="searchSection">
                    <div class="row">
                        <div class="col-sm-4 col-xs-4 margin-bottom-10 margin-top-10">
                            <b>Broadcast Option</b>
                        </div>
                        <%--<div class="col-sm-4 col-xs-12">
                            
                            <div class="pull-left">
                                <h3 style="color: #1e77bb;">Broadcast Option </h3>
                            </div>
                        </div>--%>
                        <div class="col-sm-8 col-xs-8">
                            
                            <div class="pull-left grid_option1 customRadio">
                                <asp:RadioButton ID="rdoEmail" runat="server"   Checked="true" GroupName="grpStat" />
                                <label for="rdoEmail" class="label-default" style="width: auto; margin-top: 10px;"></label>
                            </div>
                            <span class="pull-left" style="padding-left: 10px; padding-top: 5px; padding-right: 5px;">Email</span>
                            <div class="pull-left grid_option1 customRadio">
                                <asp:RadioButton ID="rdoSMS" runat="server"  GroupName="grpStat" />
                                <label for="rdoSMS" class="label-default" style="width: auto; margin-top: 10px;"></label>
                            </div>
                            <span class="pull-left" style="padding-left: 10px; padding-top: 5px; padding-right: 5px;">SMS</span>
                        </div>
                       
                    </div>

                    <div class="row">
                        <div class="col-sm-4 col-xs-4 margin-bottom-10 margin-top-10">
                            <b>Broadcast To</b>
                        </div>
                        <%--<div class="col-sm-4 col-xs-12">
                            <div class="pull-left">
                                <h3 style="color: #1e77bb;">Broadcast To </h3>
                            </div>
                            
                        </div>--%>
                        <div class="col-sm-8 col-xs-8">
                            <div class="form-group">
                                <asp:DropDownList ID="ddlInstitution" runat="server" CssClass="form-control">
                                    <asp:ListItem Text="Select"             Value="-1" Enabled="true" ></asp:ListItem>
                                    <asp:ListItem Text="Institution"        Value="1"></asp:ListItem>
                                    <asp:ListItem Text="Sales Person"       Value="2"></asp:ListItem>
                                    <asp:ListItem Text="Radiologist"        Value="3"></asp:ListItem>
                                    <asp:ListItem Text="Technician"         Value="4"></asp:ListItem>
                                    <asp:ListItem Text="Transcriptionist"   Value="5"></asp:ListItem>
                                </asp:DropDownList>
                                
                                <%--<asp:TextBox ID="TextBox8" runat="server" CssClass="form-control" MaxLength="80"></asp:TextBox>--%>

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
                                                <h3 class="h3Text">List Of Reciepients</h3>
                                            </div>
                                        </div>
                                        
                                    </div>
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-sm-12 col-xs-12">

                                    <div class="table-responsive">
                                        <ComponentArt:CallBack ID="CallBackInst" runat="server" OnCallback="CallBackInst_Callback" >
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
                                                                <ComponentArt:GridConditionalFormat ClientFilter="((DataItem.get_index() + grdInst.get_recordOffset()) % 2) > 0" RowCssClass="AltRow" />
                                                            </ConditionalFormats>
                                                            <Columns>
                                                                <ComponentArt:GridColumn DataField="id"             Align="left"    HeadingText="id"            AllowGrouping="false" Width="30" Visible="false" />
                                                                <ComponentArt:GridColumn DataField="checkbox"       Align="center"  HeadingText=" "             AllowGrouping="false" Width="30" FixedWidth="true"  HeadingCellClientTemplateId="ALLCHK" DataCellClientTemplateId="CHK" />
                                                                <ComponentArt:GridColumn DataField="name"           Align="left"    HeadingText="Name"          AllowGrouping="false" Width="200"  />
                                                                <ComponentArt:GridColumn DataField="email_id"       Align="left"    HeadingText="Email ID"      AllowGrouping="false" Width="200"  />
                                                                 <ComponentArt:GridColumn DataField="mobile"        Align="left"    HeadingText="Mobile #"      AllowGrouping="false" Width="80"   />
                                                            </Columns>

                                                        </ComponentArt:GridLevel>
                                                    </Levels>

                                                    <ClientEvents>
                                                        <RenderComplete EventHandler="grdInst_onRenderComplete" />
                                                        <%--<ColumnReorder EventHandler="grdInst_onColumnReorder" />--%>
                                                        
                                                    </ClientEvents>
                                                    <ClientTemplates>
                                                        <ComponentArt:ClientTemplate ID="ALLCHK" >
                                                            <div class="grid_option">
                                                                <input type="checkbox" id="chkSelect" style="width: 18px; height: 18px;"  onclick="javascript: chkSelect_Onclick('00000000-0000-0000-0000-000000000000');" />
                                                            </div>
                                                        </ComponentArt:ClientTemplate>
                                                        <ComponentArt:ClientTemplate ID="CHK">
                                                             <div class="grid_option">
                                                                 <input type="checkbox" id="chkSelect_## DataItem.GetMember('id').Value ##" style="width: 18px; height: 18px;" onclick="javascript: chkSelect_Onclick('## DataItem.GetMember('id').Value ##');" />
                                                             </div>
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

            <div id="dvEmail" class="sparkline10-list mt-b-10">
                <div class="searchSection">
                    <div class="row">
                        <div class="col-sm-12 col-xs-12">
                            <div class="pull-left">
                                <h3 class="h3Text">Email</h3>
                            </div>
                            <div class="borderSearch pull-left"></div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-2 col-xs-2 margin-bottom-10 margin-top-10">
                            <b>Subject</b>
                        </div>
                        <div class="col-sm-10 col-xs-10 margin-bottom-10 margin-top-10">
                                           
                            <asp:TextBox ID="txtEmailSubject" runat="server" CssClass="trans_bg_all_border form-control" placeholder=""  MaxLength="100"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-2 col-xs-2 margin-bottom-10 margin-top-10" style="top:10px;">
                            <b>Body</b>
                        </div>
                        <div class="col-sm-10 col-xs-10 margin-top-10 margin-bottom-10">
                            <div class="input-effect " style="top:10px;" >
                                <asp:TextBox ID="txtEmailBody" runat="server"  CssClass="trans_bg_all_border form-control" TextMode="MultiLine" Height="200px" ></asp:TextBox>
                                <label></label>
                                <span class="focus-border" style="width: 100%;"></span>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div id="dvSMS" class="sparkline10-list mt-b-10">
                <div class="searchSection">
                    <div class="row">
                        <div class="col-sm-12 col-xs-12">
                            <div class="pull-left">
                                <h3 class="h3Text">SMS</h3>
                            </div>
                            <div class="borderSearch pull-left"></div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-2 col-xs-2 margin-bottom-10 margin-top-10">
                            <b>Text</b>
                        </div>
                        <%--<div class="col-sm-2 col-xs-12">
                            <div class="form-group">
                                <label class="control-label" for="usermodel">Text<span class="mandatory">*</span></label>
                            </div>
                        </div>--%>
                         <div class="col-sm-10 col-xs-10">
                            <div class="form-group">
                                <asp:TextBox ID="txtSMS" runat="server" CssClass="form-control" MaxLength="160"></asp:TextBox>
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
                            <button type="button" class="btn btn-custon-four btn-primary" id="btnSend" runat="server">
                                <i class="fa fa-arrow" aria-hidden="true"></i>&nbsp;Send</button>
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
        <input type="hidden" id="hdnRights" runat="server" value="N" />
    </form>
</body>

    <script type="text/javascript">
        var objhdnID                = document.getElementById('<%=hdnID.ClientID %>');
        var objhdnError             = document.getElementById('<%=hdnError.ClientID %>');
        var objhdnRights            = document.getElementById('<%=hdnRights.ClientID %>');
        var objrdoEmail             = document.getElementById('<%=rdoEmail.ClientID %>');
        var objrdoSMS               = document.getElementById('<%=rdoSMS.ClientID %>');

        var objtxtEmailSubject      = document.getElementById('<%=txtEmailSubject.ClientID %>');
        var objtxtEmailBody         = document.getElementById('<%=txtEmailBody.ClientID %>');
        var objtxtSMS               = document.getElementById('<%=txtSMS.ClientID %>');


        var objddlInstitution       = document.getElementById('<%=ddlInstitution.ClientID %>');
        

        var strForm = "VRSBroadcast";

    </script>
    
    <script src="scripts/Broadcast.js?1"></script>
    <script src="../scripts/AppPages.js"></script>
    <script src="../scripts/custome-javascript.js"></script>
</html>
