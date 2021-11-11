<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VRSMailInvoice.aspx.cs" Inherits="VETRIS.Invoicing.VRSMailInvoice" %>

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
    <link id="lnkSTYLE" runat="server" href = "../css/style.css" rel="stylesheet" type="text/css" />
    <link id="lnkGRID" runat="server" href = "../css/grid_style.css" rel="stylesheet" type="text/css" />

    <script src="../CaseList/ckeditor/ckeditor.js"></script>
    <script src="../scripts/jquery-1.7.1.js"></script>
    <script src="../scripts/jquery.soverlay.min.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
            <div class="sparklineHeader mt-b-10 marginTP10">
                <div class="sparkline10-hd">
                    <div class="row">
                        <div class="col-sm-6 col-xs-12">
                            <h2>Send Invoice Mail</h2>
                        </div>

                        <div class="col-sm-6 col-xs-12 text-right">
                            <button type="button" class="btn btn-success" id="btnSend1" runat="server">
                                <i class="fa fa-paper-plane-o" aria-hidden="true"></i>&nbsp;Send       
                            </button>
                            <button type="button" class="btn btn-danger" id="btnClose1" runat="server">
                                <i class="fa fa-times" aria-hidden="true"></i>&nbsp;Close   
                            </button>

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
                                    <div class="row" style="margin-top: 5px;">
                                        <div class="col-sm-2 col-xs-12 marginTP5">
                                            To 
                                        </div>
                                        <div class="col-sm-10 col-xs-12 marginMobileTP5">
                                            <asp:TextBox ID="txtTo" runat="server" CssClass="form-control"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="row" style="margin-top: 5px;">
                                        <div class="col-sm-2 col-xs-12 marginTP5">
                                            Cc 
                                        </div>
                                        <div class="col-sm-10 col-xs-12 marginMobileTP5">
                                            <asp:TextBox ID="txtCC" runat="server" CssClass="form-control"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="row" style="margin-top: 5px;">
                                        <div class="col-sm-2 col-xs-12 marginTP5">
                                            Subject 
                                        </div>
                                        <div class="col-sm-10 col-xs-12 marginMobileTP5">
                                            <asp:TextBox ID="txtSubject" runat="server" CssClass="form-control"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="row" style="margin-top: 5px;">

                                        <div class="col-sm-12 col-xs-12 marginMobileTP5">
                                            <CKEditor:CKEditorControl ID="rtbMailText" runat="server" DisableNativeSpellChecker="false" RemovePlugins="scayt,contextmenu"
                                                Toolbar="Source|-|NewPage|Preview|-|Templates
                                                                                Cut|Copy|Paste|PasteText|PasteFromWord|-|Print
                                                                                Undo|Redo|-|Find|Replace|-|Bold|Italic|Underline|-|Subscript|Superscript
                                                                                NumberedList|BulletedList|-|Outdent|Indent
                                                                                JustifyLeft|JustifyCenter|JustifyRight|JustifyBlock
                                                                                Table|HorizontalRule|SpecialChar|PageBreak|-|TextColor|BGColor|-|Maximize|ShowBlocks|-|
                                                                                Link|Unlink|Image
                                                                                /
                                                                                Styles|Format|Font|FontSize"
                                                Height="200px" Width="100%">
                                            </CKEditor:CKEditorControl>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="sparklineHeader">
                <div class="sparkline10-hd">
                    <div class="row" style="margin-bottom: 10px;">

                        <div class="col-sm-12 col-xs-12 marginTP10">
                            <asp:Label ID="lblAttach" runat="server" Text="Invoice :&nbsp;"></asp:Label>
                        </div>

                    </div>
                </div>
            </div>

            <div class="sparklineHeader marginTP10" id="divMsg" style="display: none;">
                <div class="sparkline10-hd">
                    <div class="row">
                        <div class="col-sm-12 col-xs-12">
                            <asp:Label ID="lblMsg" runat="server"></asp:Label>
                        </div>
                    </div>
                </div>
            </div>

            <div class="sparklineHeader marginTP10">
                <div class="sparkline10-hd">
                    <div class="row" style="margin-bottom: 10px;">
                        <div class="col-sm-6 col-xs-12">
                            &nbsp;
                        </div>

                        <div class="col-sm-6 col-xs-12 text-right">
                            <button type="button" class="btn btn-success" id="btnSend2" runat="server" style="display: inline;">
                                <i class="fa fa-paper-plane-o" aria-hidden="true"></i>&nbsp;Send       
                            </button>
                            <button type="button" class="btn btn-danger" id="btnClose2" runat="server">
                                <i class="fa fa-times" aria-hidden="true"></i>&nbsp;Close   
                            </button>

                        </div>

                    </div>
                </div>
            </div>
        </div>
        <input type="hidden" id="hdnError" runat="server" value="" />
        <input type="hidden" id="hdnRootDir" runat="server" value="" />
        <input type="hidden" id="hdnUserID" runat="server" value="00000000-0000-0000-0000-000000000000" />
        <input type="hidden" id="hdnDivider" runat="server" value="0" />
        <input type="hidden" id="hdnInvFile" runat="server" value="" />
        <input type="hidden" id="hdnInvFilePath" runat="server" value="" />
        <input type="hidden" id="hdnCycleID" runat="server" value="00000000-0000-0000-0000-000000000000" />
        <input type="hidden" id="hdnAcctID" runat="server" value="00000000-0000-0000-0000-000000000000" />
        <input type="hidden" id="hdnInstID" runat="server" value="00000000-0000-0000-0000-000000000000" />
        <input type="hidden" id="hdnType" runat="server" value="" />
        <input type="hidden" id="hdnSENDMAILID" runat="server" value="" />
        <input type="hidden" id="hdnMAILSVRNAME" runat="server" value="" />
        <input type="hidden" id="hdnMAILSVRPORT" runat="server" value="0" />
        <input type="hidden" id="hdnMAILSVRUSRCODE" runat="server" value="" />
        <input type="hidden" id="hdnMAILSVRUSRPWD" runat="server" value="" />
        <input type="hidden" id="hdnMAILSSLENABLED" runat="server" value="" />
    </form>
</body>
<script type="text/javascript">
    var objhdnError = document.getElementById('<%=hdnError.ClientID %>');
    var objhdnUserID = document.getElementById('<%=hdnUserID.ClientID %>');
    var objhdnDivider = document.getElementById('<%=hdnDivider.ClientID %>');
    var objhdnInvFile = document.getElementById('<%=hdnInvFile.ClientID %>');
    var objhdnInvFilePath = document.getElementById('<%=hdnInvFilePath.ClientID %>');
    var objhdnRootDir = document.getElementById('<%=hdnRootDir.ClientID %>');
    var objhdnAcctID = document.getElementById('<%=hdnAcctID.ClientID %>');
    var objhdnCycleID = document.getElementById('<%=hdnCycleID.ClientID %>');
    var objhdnInstID = document.getElementById('<%=hdnInstID.ClientID %>');
    var objhdnType = document.getElementById('<%=hdnType.ClientID %>');
    var objlblMsg = document.getElementById('<%=lblMsg.ClientID %>');
    var objtxtTo = document.getElementById('<%=txtTo.ClientID %>');
    var objtxtCC = document.getElementById('<%=txtCC.ClientID %>');
    var objtxtSubject = document.getElementById('<%=txtSubject.ClientID %>');
    
    var objhdnSENDMAILID = document.getElementById('<%=hdnSENDMAILID.ClientID %>');
    var objhdnMAILSVRNAME = document.getElementById('<%=hdnMAILSVRNAME.ClientID %>');
    var objhdnMAILSVRPORT = document.getElementById('<%=hdnMAILSVRPORT.ClientID %>');
    var objhdnMAILSVRUSRCODE = document.getElementById('<%=hdnMAILSVRUSRCODE.ClientID %>');
    var objhdnMAILSVRUSRPWD = document.getElementById('<%=hdnMAILSVRUSRPWD.ClientID %>');
    var objhdnMAILSSLENABLED = document.getElementById('<%=hdnMAILSSLENABLED.ClientID %>');
    var strForm = "VRSMailInvoice";

</script>
<script src="scripts/MailInvoice.js?v=<%=DateTime.Now.Ticks%>"></script>
</html>
