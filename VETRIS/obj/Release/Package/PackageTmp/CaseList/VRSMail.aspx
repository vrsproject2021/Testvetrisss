<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VRSMail.aspx.cs" Inherits="VETRIS.CaseList.VRSMail" %>
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

    <script src="../scripts/jquery-1.7.1.js"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="content-wrapper" style="margin-top: 20px;">
            <div class="static-table-area">
                <div class="container-fluid">
                    <div class="row">
                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                            <div class="sparklineHeader mt-b-10">
                                
                                    <div class="row">
                                        <div class="col-sm-6 col-xs-12">
                                            <h2>Mail Preliminary Report</h2>
                                        </div>
                                        <div class="col-sm-6 col-xs-12 text-right">

                                            <button type="button" class="btn btn-custon-four btn-warning" id="btnSend1" runat="server">
                                                <i class="fa fa-envelope-o edu-danger-error" aria-hidden="true"></i>&nbsp;Send
                                       
                                            </button>

                                            <button type="button" class="btn btn-custon-four btn-danger" id="btnClose1" runat="server">
                                                <i class="fa fa-times edu-danger-error" aria-hidden="true"></i>&nbsp;Close
                                       
                                            </button>
                                        </div>
                                    </div>
                                
                            </div>

                            

                            <div class="sparkline10-list mt-b-10">
                                <div class="row">
                                    <div class="col-sm-12 col-xs-12">
                                       

                                        <div class="searchSection marginTP10">
                                            

                                            <div class="row">
                                                <div class="col-sm-12 col-xs-12">
                                                    <div class="static-table-list">
                                                        <table class="table table-bordered">

                                                            <tbody>
                                                                <tr>
                                                                    <td colspan="4" style="font-weight:bold;">Send To :</td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="width: 20%;">Institution</td>
                                                                    <td style="width: 30%;">
                                                                        <asp:Label ID="lblInstitution" runat="server"></asp:Label>
                                                                    </td>
                                                                    <td style="width: 20%;">Email ID</td>
                                                                    <td style="width: 30%;">
                                                                       <asp:TextBox ID="txtInstMail" runat="server" CssClass="form-control" MaxLength="100"></asp:TextBox>
                                                                    </td>

                                                                </tr>
                                                                
                                                                <tr>
                                                                    
                                                                    <td>Referring Physician</td>
                                                                    <td>
                                                                        <asp:Label ID="lblPhysician" runat="server"></asp:Label>
                                                                    </td>
                                                                    <td>Email ID</td>
                                                                    <td>
                                                                       <asp:TextBox ID="txtPhysMail" runat="server" CssClass="form-control" MaxLength="100"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    
                                                                    <td>Subject</td>
                                                                    <td colspan="3">
                                                                        <asp:TextBox ID="txtSubject" runat="server" CssClass="form-control" MaxLength="100"></asp:TextBox>
                                                                    </td>
                                                                    
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="4">
                                                                        <asp:TextBox ID="txtMailContent" runat="server" CssClass="form-control" TextMode="MultiLine" Height="200px"></asp:TextBox>
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

                            

                            <div class="sparklineHeader mt-b-10 marginTP10">
                                <div class="sparkline10-hd">
                                    <div class="row">
                                        <div class="col-sm-6 col-xs-12 marginTP10" id="divMsg">
                                            &nbsp;
                                        </div>
                                        <div class="col-sm-6 col-xs-12 text-right">
                                            <button type="button" class="btn btn-custon-four btn-warning" id="btnSend2" runat="server">
                                                <i class="fa fa-envelope-o edu-danger-error" aria-hidden="true"></i>&nbsp;Send
                                       
                                            </button>

                                            <button type="button" class="btn btn-custon-four btn-danger" id="btnClose2" runat="server" onclick="javascript: parent.HideDataList();">
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
    </form>
</body>
    <script type="text/javascript">
        var objhdnID = document.getElementById('<%=hdnID.ClientID %>');
        var objhdnError = document.getElementById('<%=hdnError.ClientID %>');
        var objhdnSUID = document.getElementById('<%=hdnSUID.ClientID %>');
        var objtxtInstMail = document.getElementById('<%=txtInstMail.ClientID %>');
        var objtxtPhysMail = document.getElementById('<%=txtPhysMail.ClientID %>');
        var objtxtSubject = document.getElementById('<%=txtSubject.ClientID %>');
        var objtxtMailContent = document.getElementById('<%=txtMailContent.ClientID %>');
        var objhdnFilename = document.getElementById('<%=hdnFilename.ClientID %>');
        var objhdnFilePath = document.getElementById('<%=hdnFilePath.ClientID %>');
        var objdivMsg = document.getElementById("divMsg");
        var strForm = "VRSMail";

</script>
<script src="../scripts/custome-javascript.js"></script>
<script src="scripts/Mail.js"></script>
</html>
