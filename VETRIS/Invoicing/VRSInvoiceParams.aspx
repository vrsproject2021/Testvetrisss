<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VRSInvoiceParams.aspx.cs" Inherits="VETRIS.Invoicing.VRSInvoiceParams" %>
<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>
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


    <script src="../CaseList/ckeditor/ckeditor.js"></script>
    <script src="../scripts/jquery.min.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
            <div class="sparklineHeader mt-b-10 marginTP10">
                <div class="sparkline10-hd">
                    <div class="row">
                        <div class="col-sm-6 col-xs-12">
                            <h2>Invoice Parameters</h2>
                        </div>
                        <div class="col-sm-6 col-xs-12 text-right">

                            <button type="button" id="btnSave1" runat="server" class="btn btn-custon-four btn-success">
                                <i class="fa fa-floppy-o" aria-hidden="true"></i>&nbsp;Save</button>

                            <button type="button" id="btnClose1" runat="server" class="btn btn-custon-four btn-danger">
                                <i class="fa fa-times" aria-hidden="true"></i>&nbsp;Close</button>
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
                                    <h3 class="h3Text">Company Settings</h3>
                                </div>
                            </div>
                            <div class="col-sm-6 col-xs-6">
                                &nbsp;
                            </div>
                            <div class="col-sm-12 col-xs-12">
                                <div class="borderSearch pull-left"></div>
                            </div>


                        </div>
                        <div class="row">
                            <div class="col-sm-12 col-xs-12">
                                <div class="row">
                                    <div class="col-sm-4 col-xs-12" style="margin-top: 10px;">
                                        <label class="control-label" for="usermodel">Company Name</label>
                                    </div>
                                    <div class="col-sm-6 col-xs-12 marginTP5">
                                        <asp:TextBox ID="txtCOMPNAME" runat="server" CssClass="form-control" MaxLength="80"></asp:TextBox>
                                    </div>
                                </div>
                            </div>

                        </div>
                        <div class="row">
                            <div class="col-sm-12 col-xs-12">
                                <div class="row">
                                    <div class="col-sm-4 col-xs-12" style="margin-top: 10px;">
                                        <label class="control-label" for="usermodel">Company Address</label>
                                    </div>
                                    <div class="col-sm-6 col-xs-12 marginTP5">
                                        <asp:TextBox ID="txtCOMPADDR" runat="server" CssClass="form-control" MaxLength="1000" TextMode="MultiLine"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="sparklineHeader mt-b-10 marginTP10">
                <div class="sparkline10-graph">
                    <div class="searchSection">
                        <div class="row">
                            <div class="col-sm-6 col-xs-6">
                                <div class="pull-left">
                                    <h3 class="h3Text">Invoice Format Settings</h3>
                                </div>
                            </div>
                            <div class="col-sm-6 col-xs-6">
                                &nbsp;
                            </div>
                            <div class="col-sm-12 col-xs-12">
                                <div class="borderSearch pull-left"></div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-12 col-xs-12">
                                <div class="row">
                                    <div class="col-sm-4 col-xs-12" style="margin-top: 10px;">
                                        <label class="control-label" for="usermodel">Invoice Prefix</label>
                                    </div>
                                    <div class="col-sm-6 col-xs-12 marginTP5">
                                        <asp:TextBox ID="txtINVPRFX" runat="server" CssClass="form-control" MaxLength="80"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-12 col-xs-12">
                                <div class="row">
                                    <div class="col-sm-4 col-xs-12" style="margin-top: 10px;">
                                        <label class="control-label" for="usermodel">Pay Instruction</label>
                                    </div>
                                    <div class="col-sm-6 col-xs-12 marginTP5">
                                        <asp:TextBox ID="txtPAYINST" runat="server" CssClass="form-control" MaxLength="1000" TextMode="MultiLine"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-12 col-xs-12">
                                <div class="row">
                                    <div class="col-sm-4 col-xs-12" style="margin-top: 10px;">
                                        <label class="control-label" for="usermodel">Footer Text</label>
                                    </div>
                                    <div class="col-sm-6 col-xs-12 marginTP5">
                                        <asp:TextBox ID="txtFOOTTXT" runat="server" CssClass="form-control" MaxLength="200"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>

                    </div>
                </div>
            </div>

            <div class="sparklineHeader mt-b-10 marginTP10">
                <div class="sparkline10-graph">
                    <div class="searchSection">
                        <div class="row">
                            <div class="col-sm-6 col-xs-6">
                                <div class="pull-left">
                                    <h3 class="h3Text">Notification Settings</h3>
                                </div>
                            </div>
                            <div class="col-sm-6 col-xs-6">
                                &nbsp;
                            </div>
                            <div class="col-sm-12 col-xs-12">
                                <div class="borderSearch pull-left"></div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-sm-12 col-xs-12">
                                <div class="row">
                                    <div class="col-sm-4 col-xs-12" style="margin-top: 10px;">
                                        <label class="control-label" for="usermodel">Sender Email ID</label>
                                    </div>
                                    <div class="col-sm-6 col-xs-12 marginTP5">
                                        <asp:TextBox ID="txtSENDMAILID" runat="server" CssClass="form-control" MaxLength="200"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-12 col-xs-12">
                                <div class="row">
                                    <div class="col-sm-4 col-xs-12" style="margin-top: 10px;">
                                        <label class="control-label" for="usermodel">Sender Email Password</label>
                                    </div>
                                    <div class="col-sm-6 col-xs-12 marginTP5">
                                        <asp:TextBox ID="txtSENDMAILPWD" runat="server" CssClass="form-control" MaxLength="200" TextMode="Password"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-12 col-xs-12">
                                <div class="row">
                                    <div class="col-sm-4 col-xs-12" style="margin-top: 10px;">
                                        <label class="control-label" for="usermodel">Default CC Email ID</label>
                                    </div>
                                    <div class="col-sm-6 col-xs-12" style="margin-top: 10px;">
                                        <asp:TextBox ID="txtDEFCCMAILID" runat="server" CssClass="form-control" MaxLength="200"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-12 col-xs-12">
                                <div class="row">
                                    <div class="col-sm-4 col-xs-12" style="margin-top: 10px;">
                                        <label class="control-label" for="usermodel">Invoice Mail Sending Text</label>
                                    </div>
                                    <div class="col-sm-8 col-xs-12" style="margin-top: 10px;">
                                        <CKEditor:CKEditorControl ID="rtbINVMAILTXT" runat="server" DisableNativeSpellChecker="false" RemovePlugins="scayt,contextmenu"
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
                        <div class="row">
                            <div class="col-sm-12 col-xs-12">
                                <div class="row">
                                    <div class="col-sm-4 col-xs-12" style="margin-top: 10px;">
                                        <label class="control-label" for="usermodel">Online Payment Notification CC Email ID</label>
                                    </div>
                                    <div class="col-sm-6 col-xs-12 marginTP5">
                                        <asp:TextBox ID="txtOPMAILCC" runat="server" CssClass="form-control" MaxLength="200"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="sparklineHeader mt-b-10 marginTP10">
                <div class="sparkline10-graph">
                    <div class="searchSection">
                        <div class="row">
                            <div class="col-sm-6 col-xs-6">
                                <div class="pull-left">
                                    <h3 class="h3Text">Payment Settings</h3>
                                </div>
                            </div>
                            <div class="col-sm-6 col-xs-6">
                                &nbsp;
                            </div>
                            <div class="col-sm-12 col-xs-12">
                                <div class="borderSearch pull-left"></div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-12 col-xs-12">
                                <div class="row">
                                    <div class="col-sm-4 col-xs-12" style="margin-top: 10px;">
                                        <label class="control-label" for="usermodel">Payment Due Days</label>
                                    </div>
                                    <div class="col-sm-3 col-xs-12 marginTP5">
                                        <asp:TextBox ID="txtDUEDTDAYS" runat="server" CssClass="form-control pull-left" Width="40%"></asp:TextBox>
                                        <span class= "col-sm-2 col-xs-12 marginTP5 pull-left">days</span>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-12 col-xs-12">
                                <div class="row">
                                    <div class="col-sm-4 col-xs-12" style="margin-top: 10px;">
                                        <label class="control-label" for="usermodel">Payment Gateway URL</label>
                                    </div>
                                    <div class="col-sm-6 col-xs-12 marginTP5">
                                        <asp:TextBox ID="txtOLPMTURL" runat="server" CssClass="form-control" MaxLength="100"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-12 col-xs-12">
                                <div class="row">
                                    <div class="col-sm-4 col-xs-12" style="margin-top: 10px;">
                                        <label class="control-label" for="usermodel">Payment Gateway Token Key</label>
                                    </div>
                                    <div class="col-sm-6 col-xs-12 marginTP5">
                                        <asp:TextBox ID="txtTNTKNKEY" runat="server" CssClass="form-control" MaxLength="100"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-12 col-xs-12">
                                <div class="row">
                                    <div class="col-sm-4 col-xs-12" style="margin-top: 10px;">
                                        <label class="control-label" for="usermodel">Payment Gateway API Key</label>
                                    </div>
                                    <div class="col-sm-6 col-xs-12 marginTP5">
                                        <asp:TextBox ID="txtTNAPIKEY" runat="server" CssClass="form-control" MaxLength="100"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-12 col-xs-12">
                                <div class="row">
                                    <div class="col-sm-4 col-xs-12" style="margin-top: 10px;">
                                        <label class="control-label" for="usermodel">Payment Gateway User ID</label>
                                    </div>
                                    <div class="col-sm-6 col-xs-12 marginTP5">
                                        <asp:TextBox ID="txtOLPMTUID" runat="server" CssClass="form-control" MaxLength="100"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-12 col-xs-12">
                                <div class="row">
                                    <div class="col-sm-4 col-xs-12" style="margin-top: 10px;">
                                        <label class="control-label" for="usermodel">Payment Gateway Password</label>
                                    </div>
                                    <div class="col-sm-6 col-xs-12 marginTP5">
                                        <asp:TextBox ID="txtOLPMTPWD" runat="server" CssClass="form-control" MaxLength="200" TextMode="Password"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                         <div class="row">
                            <div class="col-sm-12 col-xs-12">
                                <div class="row">
                                    <div class="col-sm-4 col-xs-12" style="margin-top: 10px;">
                                        <label class="control-label" for="usermodel">Refund to be processed for payments made in last</label>
                                    </div>
                                    <div class="col-sm-3 col-xs-12 marginTP10">
                                       
                                        <asp:TextBox ID="txtREFUNDDAYS" runat="server" CssClass="form-control pull-left" Width="40%" ></asp:TextBox> 
                                         <span class= "col-sm-2 col-xs-12 marginTP5 pull-left">days</span>
                                        
                                    </div>
                                    <div class="col-sm-5 col-xs-12 marginTP5 pull-left">
                                           
                                       </div>
                                     
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="sparklineHeader mt-b-10 marginTP10">
                <div class="sparkline10-graph">
                    <div class="searchSection">
                        <div class="row">
                            <div class="col-sm-6 col-xs-6">
                                <div class="pull-left">
                                    <h3 class="h3Text">Serial Number Settings</h3>
                                </div>
                            </div>
                            <div class="col-sm-6 col-xs-6">
                                &nbsp;
                            </div>
                            <div class="col-sm-12 col-xs-12">
                                <div class="borderSearch pull-left"></div>
                            </div>


                        </div>
                        <div class="row">
                            <div class="col-sm-12 col-xs-12">
                                <div class="row">


                                    <div class="col-sm-4 col-xs-12" style="margin-top: 10px;">
                                        <label class="control-label" for="usermodel">Invoice Starting Serial #</label>
                                    </div>
                                    <div class="col-sm-3 col-xs-12 marginTP5">
                                        <asp:TextBox ID="txtSTARTINVSRL" runat="server" CssClass="form-control" MaxLength="80" Width="30%"></asp:TextBox>
                                    </div>

                                </div>

                            </div>
                            <div class="col-sm-12 col-xs-12">
                                <div class="row">
                                    <div class="col-sm-4 col-xs-12" style="margin-top: 10px;">
                                        <label class="control-label" for="usermodel">Radiologist Payment Starting Serial #</label>
                                    </div>
                                    <div class="col-sm-3 col-xs-12 marginTP5">
                                        <asp:TextBox ID="txtRADPMTSRL" runat="server" CssClass="form-control" MaxLength="80" Width="30%"></asp:TextBox>
                                    </div>
                                </div>
                            </div>

                        </div>
                    </div>
                </div>
            </div>

            <div class="sparklineHeader mt-b-10 marginTP10">
                <div class="sparkline10-graph">
                    <div class="searchSection">
                        <div class="row">
                            <div class="col-sm-6 col-xs-6">
                                <div class="pull-left">
                                    <h3 class="h3Text">Calculation Settings</h3>
                                </div>
                            </div>
                            <div class="col-sm-6 col-xs-6">
                                &nbsp;
                            </div>
                            <div class="col-sm-12 col-xs-12">
                                <div class="borderSearch pull-left"></div>
                            </div>


                        </div>
                        <div class="row">
                            <div class="col-sm-12 col-xs-12">
                                <div class="row">


                                    <div class="col-sm-4 col-xs-12" style="margin-top: 10px;">
                                        <label class="control-label" for="usermodel">Minute wise calculation factor</label>
                                    </div>
                                    <div class="col-sm-3 col-xs-12 marginTP5">
                                        <asp:TextBox ID="txtCALCMINUTEFACT" runat="server" CssClass="form-control" Width="30%"></asp:TextBox>
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

                            <button type="button" class="btn btn-custon-four btn-success" id="btnSave2" runat="server">
                                <i class="fa fa-floppy-o" aria-hidden="true"></i>&nbsp; Save</button>
                            <button type="button" class="btn btn-custon-four btn-danger" id="btnClose2" runat="server">
                                <i class="fa fa-times" aria-hidden="true"></i>&nbsp; Close</button>
                        </div>
                    </div>
                </div>
            </div>
            
            <input type="hidden" id="hdnError" runat="server" value="" />
            <input type="hidden" id="hdnID" runat="server" value="00000000-0000-0000-0000-000000000000" />
        </div>
    </form>
</body>
<script type="text/javascript">
    var objhdnID = document.getElementById('<%=hdnID.ClientID %>');
    var objhdnError = document.getElementById('<%=hdnError.ClientID %>');
    var objtxtCOMPNAME = document.getElementById('<%=txtCOMPNAME.ClientID %>');
    var objtxtCOMPADDR = document.getElementById('<%=txtCOMPADDR.ClientID %>');

    var objtxtOLPMTUID = document.getElementById('<%=txtOLPMTUID.ClientID %>');
    var objtxtOLPMTPWD = document.getElementById('<%=txtOLPMTPWD.ClientID %>');
    var objtxtOLPMTURL = document.getElementById('<%=txtOLPMTURL.ClientID %>');
    var objtxtDUEDTDAYS = document.getElementById('<%=txtDUEDTDAYS.ClientID %>');
    var objtxtFOOTTXT = document.getElementById('<%=txtFOOTTXT.ClientID %>');
    var objtxtINVPRFX = document.getElementById('<%=txtINVPRFX.ClientID %>');
    var objtxtSTARTINVSRL = document.getElementById('<%=txtSTARTINVSRL.ClientID %>');
    var objtxtPAYINST = document.getElementById('<%=txtPAYINST.ClientID %>');
    var objtxtRADPMTSRL = document.getElementById('<%=txtRADPMTSRL.ClientID %>');
    var objtxtDEFCCMAILID = document.getElementById('<%=txtDEFCCMAILID.ClientID %>');
    var objtxtOPMAILCC = document.getElementById('<%=txtOPMAILCC.ClientID %>');
    var objtxtSENDMAILID = document.getElementById('<%=txtSENDMAILID.ClientID %>');
    var objtxtSENDMAILPWD = document.getElementById('<%=txtSENDMAILPWD.ClientID %>');
    var objtxtTNTKNKEY = document.getElementById('<%=txtTNTKNKEY.ClientID %>');
    var objtxtTNAPIKEY = document.getElementById('<%=txtTNAPIKEY.ClientID %>');
    var objtxtREFUNDDAYS = document.getElementById('<%=txtREFUNDDAYS.ClientID %>');
    var objtxtCALCMINUTEFACT = document.getElementById('<%=txtCALCMINUTEFACT.ClientID %>');
    var strForm = "VRSInvoiceParams";

</script>
<script src="../scripts/custome-javascript.js"></script>
<script src="../scripts/AppPages.js"></script>
<script src="scripts/InvoiceParams.js?v=<%=DateTime.Now.Ticks%>"></script>
</html>
