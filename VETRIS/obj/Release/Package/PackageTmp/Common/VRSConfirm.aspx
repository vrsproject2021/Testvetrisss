<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VRSConfirm.aspx.cs" Inherits="VETRIS.Common.VRSConfirm" %>
<%@ OutputCache Location="None" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <meta name="description" content="" />
    <title></title>
    <link href="../css/font-awesome.min.css" rel="stylesheet" />
    <link href="../css/bootstrap.min.css" rel="stylesheet" />
    <link href="../css/custome-css-style.css" rel="stylesheet" />
    <script type="text/javascript" src="../scripts/jquery-1.11.3.min.js"></script>
</head>
<body class="nav-sm" style="background: transparent;">
    <form id="form1" runat="server">
      <div class="container">
            <div class="main_container singleTask_page" style="margin: 10px;">
                <div class="row">
                    <div class="col-sm-12 col-xs-12 margin-bottom-10">
                        <div class="x_panel">
                            <div class="x_title">
                                <div class="row">
                                    <div class="col-sm-9 col-xs-9">
                                        <h4>
                                            <asp:Label ID="lblMsgHdr" runat="server" Font-Bold="true" Text=""></asp:Label>

                                        </h4>
                                    </div>
                                    <div class="col-sm-3 col-xs-3 text-right">
                                        <ul class="nav navbar-right panel_toolbox">
                                            <li class="pull-right">
                                                <button type="button" class="btn btn-danger btn_hdr" onclick="javascript: parent.HideConfirm();"><i class="fa fa-close" aria-hidden="true"></i></button>
                                            </li>
                                        </ul>
                                    </div>
                                     <div class="borderHdr pull-left"></div>
                                    <div class="clearfix"></div>
                                </div>

                            </div>
                            <div class="x_content">
                                <div class="row">
                                    <div class="col-sm-12 col-xs-12 margin-bottom-10 margin-top-10">
                                        <asp:Label ID="lblConfMsg" runat="server"></asp:Label>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-12 col-xs-12 margin-bottom-10 margin-top-10">
                                        &nbsp;
                                    </div>
                                </div>
                               <div class="row" style="text-align:center;">
                                    <div class="col-sm-12 col-xs-12 margin-bottom-10 margin-top-10">
                                        <button type="button" class="btn btn-success" id="btnYes" runat="server">Yes</button>&nbsp;
                                        &nbsp;<button type="button" class="btn btn-danger" id="btnNo" runat="server">No</button>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-12 col-xs-12 margin-bottom-10 margin-top-10">
                                        &nbsp;
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
<script type="text/javascript">
    $(document).ready($(function () {
        parent.adjusConfFrameHeight();
    }))
</script>

</html>
