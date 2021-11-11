<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VRSCasePrelimDlg.aspx.cs" Inherits="VETRIS.CaseList.VRSCasePrelimDlg" %>

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
    <link href="../css/CalendarStyle.css" rel="stylesheet" />
    <link href="../css/grid_style.css" rel="stylesheet" />

    <script src="../scripts/jquery.min.js"></script>
    <script src="ckeditor/ckeditor.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="content-wrapper" style="margin-top: 20px;">
            <div class="static-table-area">
                <div class="container-fluid">
                    <div class="row">
                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                            <div class="sparklineHeader mt-b-10 marginTP10">
                                <div class="sparkline10-hd">
                                    <div class="row">
                                        <div class="col-sm-6 col-xs-12">
                                            <h2>Preliminary Report</h2>
                                        </div>
                                        <div class="col-sm-6 col-xs-12 text-right">

                                            <button type="button" class="btn btn-custon-four btn-warning" id="btnReset1" runat="server">
                                                <i class="fa fa-repeat edu-danger-error" aria-hidden="true"></i>&nbsp;Reset
                                       
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
                                        <div class="col-sm-6 col-xs-12">
                                            <a href="javascript:void(0)" style="color: blue; cursor: pointer;" onclick="javascript:NavigateToPACS();">Click here to view the study details in PACS</a>
                                            &nbsp;|&nbsp;
                                           <a href="javascript:void(0)" style="color: blue; cursor: pointer;" onclick="javascript:NavigateToImgViewer();">Click here to view the study images in PACS</a>
                                        </div>
                                        <div class="col-sm-6 col-xs-12 text-right">
                                            Study UID:
                                                <asp:Label ID="lblSUID" runat="server"></asp:Label>



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
                                                        <h3 style="color: #1e77bb;">Patient Info</h3>
                                                    </div>
                                                    <div class="borderSearch pull-left"></div>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-sm-12 col-xs-12">
                                                    <div class="static-table-list">
                                                        <div class="table-responsive">
                                                            <table class="table table-bordered">
                                                                <tbody>
                                                                    <tr>
                                                                        <td style="width: 20%;">ID</td>
                                                                        <td style="width: 30%;">
                                                                            <asp:Label ID="lblPatientID" runat="server"></asp:Label>
                                                                        </td>
                                                                        <td style="width: 20%;">Name</td>
                                                                        <td style="width: 30%;">
                                                                            <asp:Label ID="lblPatientName" runat="server"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>Sex</td>
                                                                        <td>
                                                                            <asp:Label ID="lblSex" runat="server"></asp:Label>
                                                                        </td>
                                                                        <td>Age</td>
                                                                        <td>
                                                                            <asp:Label ID="lblAge" runat="server"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>Species</td>
                                                                        <td>
                                                                            <asp:Label ID="lblSpecies" runat="server"></asp:Label>
                                                                        </td>
                                                                        <td>Breed</td>
                                                                        <td>
                                                                            <asp:Label ID="lblBreed" runat="server"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>Owner</td>
                                                                        <td colspan="3">
                                                                            <asp:Label ID="lblOwner" runat="server"></asp:Label>
                                                                        </td>

                                                                    </tr>
                                                                </tbody>
                                                            </table>
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
                                                    <div class="static-table-list">
                                                        <table class="table table-bordered">

                                                            <tbody>
                                                                <tr>
                                                                    <td style="width: 20%;">Modality</td>
                                                                    <td style="width: 30%;">
                                                                        <asp:Label ID="lblModality" runat="server"></asp:Label>
                                                                    </td>
                                                                    <td style="width: 20%;">Body Part</td>
                                                                    <td style="width: 30%;">
                                                                        <asp:Label ID="lblBodyPart" runat="server"></asp:Label>
                                                                    </td>

                                                                </tr>
                                                                <tr>
                                                                    <td>Study Type</td>
                                                                    <td colspan="3">
                                                                        <asp:Label ID="lblStudyType" runat="server"></asp:Label>
                                                                    </td>

                                                                </tr>
                                                                <tr>
                                                                    <td>Reason for Study</td>
                                                                    <td colspan="3">
                                                                        <asp:Label ID="lblReason" runat="server"></asp:Label>
                                                                    </td>

                                                                </tr>
                                                                <tr>
                                                                    <td>Institution</td>
                                                                    <td>
                                                                        <asp:Label ID="lblInstitution" runat="server"></asp:Label>
                                                                    </td>
                                                                    <td>Referring Physician</td>
                                                                    <td>
                                                                        <asp:Label ID="lblPhysician" runat="server"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                            </tbody>
                                                        </table>
                                                    </div>
                                                </div>
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
                                                    <div class="pull-left" style="margin-top: 10px;">
                                                        <h3 style="color: #1e77bb;">Report</h3>
                                                    </div>
                                                    <div class="pull-right">
                                                        <button type="button" class="btn btn-custon-four btn-primary" id="btnSave" runat="server">
                                                            <i class="fa fa-floppy-o edu-danger-error" aria-hidden="true"></i>&nbsp;Save
                                                        </button>
                                                        <button type="button" class="btn btn-custon-four btn-success" id="btnGen" runat="server">
                                                            <i class="fa fa-file-o edu-danger-error" aria-hidden="true"></i>&nbsp;Generate Report
                                                        </button>
                                                       <button type="button" class="btn btn-custon-four btn-warning" id="btnEmail" runat="server">
                                                            <i class="fa fa-envelope-o edu-danger-error" aria-hidden="true"></i>&nbsp;Email
                                                        </button>
                                                         <button type="button" class="btn btn-custon-four btn-primary" id="btnSMS" runat="server">
                                                            <i class="fa fa-comments-o edu-danger-error" aria-hidden="true"></i>&nbsp;SMS
                                                        </button>
                                                    </div>
                                                    <div class="borderSearch pull-left"></div>
                                                </div>


                                            </div>
                                            <div class="row">
                                            <div class="col-sm-12 col-xs-12">
                                                <CKEditor:CKEditorControl ID="CKEditor1" runat="server"
                                                    Toolbar="Source|-|NewPage|Preview|-|Templates
                                                                                Cut|Copy|Paste|PasteText|PasteFromWord|-|Print|SpellChecker|Scayt
                                                                                Undo|Redo|-|Find|Replace|-|SelectAll|RemoveFormat
                                                                                Form|Checkbox|Radio|TextField|Textarea|Select|Button|ImageButton|HiddenField
                                                                                /
                                                                                Bold|Italic|Underline|Strike|-|Subscript|Superscript
                                                                                NumberedList|BulletedList|-|Outdent|Indent|Blockquote|CreateDiv
                                                                                JustifyLeft|JustifyCenter|JustifyRight|JustifyBlock
                                                                                BidiLtr|BidiRtl
                                                                                Link|Unlink|Anchor
                                                                                Image|Flash|Table|HorizontalRule|Smiley|SpecialChar|PageBreak|Iframe
                                                                                /
                                                                                Styles|Format|Font|FontSize
                                                                                TextColor|BGColor
                                                                                Maximize|ShowBlocks|-|About"
                                                    Height="325px" Width="100%">
                                                </CKEditor:CKEditorControl>
                                            </div>
                                        </div>
                                        </div>
                                        
                                    </div>
                                </div>
                            </div>

                            <div class="sparklineHeader mt-b-10 marginTP10">
                                <div class="sparkline10-hd">
                                    <div class="row">
                                        <div class="col-sm-6 col-xs-12 marginTP10">
                                            &nbsp;
                                        </div>
                                        <div class="col-sm-6 col-xs-12 text-right">
                                            <button type="button" class="btn btn-custon-four btn-warning" id="btnReset2" runat="server">
                                                <i class="fa fa-repeat edu-danger-error" aria-hidden="true"></i>&nbsp;Reset
                                       
                                            </button>

                                            <button type="button" class="btn btn-custon-four btn-danger" id="btnClose2" runat="server">
                                                <i class="fa fa-times edu-danger-error" aria-hidden="true"></i>&nbsp;Close
                                       
                                            </button>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

            </div>
        </div>
        <input type="hidden" id="hdnError" runat="server" value="" />
        <input type="hidden" id="hdnID" runat="server" value="00000000-0000-0000-0000-000000000000" />
        <input type="hidden" id="hdnSUID" runat="server" value="" />
        <input type="hidden" id="hdnFilename" runat="server" value="" />
        <input type="hidden" id="hdnFilePath" runat="server" value="" />
        <input type="hidden" id="hdnPACSURL" runat="server" value="" />
        <input type="hidden" id="hdnImgVwrURL" runat="server" value="" />
    </form>
</body>
<script type="text/javascript">
    var objhdnID = document.getElementById('<%=hdnID.ClientID %>');
    var objhdnError = document.getElementById('<%=hdnError.ClientID %>');
    var objhdnSUID = document.getElementById('<%=hdnSUID.ClientID %>');
    var objhdnFilename = document.getElementById('<%=hdnFilename.ClientID %>');
    var objhdnFilePath = document.getElementById('<%=hdnFilePath.ClientID %>');
    var objlblPatientID = document.getElementById('<%=lblPatientID.ClientID %>');
    var objhdnPACSURL = document.getElementById('<%=hdnPACSURL.ClientID %>');
    var objhdnImgVwrURL = document.getElementById('<%=hdnImgVwrURL.ClientID %>');
    var strForm = "VRSCasePrelimDlg";

</script>
<script src="../scripts/custome-javascript.js"></script>
<script src="../scripts/AppPages.js?3?3"></script>
<script src="scripts/CasePrelimDlg.js?2"></script>
</html>
