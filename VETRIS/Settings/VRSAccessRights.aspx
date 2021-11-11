<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VRSAccessRights.aspx.cs" Inherits="VETRIS.Settings.VRSAccessRights" %>
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
    <%--<link href="../css/style.css" rel="stylesheet" />--%>
    
  
     <link id="lnkSTYLE" runat="server" href = "../css/style.css" rel="stylesheet" type="text/css" />
     <link id="lnkTHEME" runat="server" href = "../css/theme.css" rel="stylesheet" type="text/css" />
     <link id="lnkTV" runat="server" href = "../css/treeStyle.css" rel="stylesheet" type="text/css" />
    <link href="../css/menuStyle.css" rel="stylesheet" />

    <script src="../scripts/jquery-1.7.1.js"></script>
     <script src="scripts/AccessRightsHdr.js?05062020" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
            <div class="sparklineHeader mt-b-10 marginTP10">
                <div class="sparkline10-hd">
                    <div class="row">
                        <div class="col-sm-6 col-xs-12">
                            <h2>Access Rights</h2>
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
            
            <div class=" row mt-b-10 marginTP10">
                <div class="col-sm-4">
                    <div class="sparkline10-list mt-b-10">
                        <div class="searchSection">
                            <div class="row">
                                <div class="col-sm-12 col-xs-12">
                                    <div class="pull-left">
                                        <h3 class="h3Text">
                                            User Roles
                                            <asp:Label ID="lblUserGroupHelp" runat="server" Text="(select a node to assign/view rights)" CssClass="HelpText"></asp:Label>
                                        </h3>
                                    </div>
                                    <div class="borderSearch pull-left"></div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-12 col-xs-12" style="height: 360px; overflow-x: auto; overflow-y:auto;">
                                    <ComponentArt:CallBack ID="CallBackUserRole" runat="server" OnCallback="CallBackUserRole_Callback">
                                            <Content>
                                                <div id="divUserRole" runat="server" style="width: 100%; height:350px; overflow: auto;">
                                                    <ComponentArt:TreeView ID="tvUserRoles"
                                                        DragAndDropEnabled="false"
                                                        DragAndDropAcrossTreesEnabled="false"
                                                        NodeEditingEnabled="false"
                                                        AutoTheming="true" CssClass="TreeView4" EnableTheming="True"
                                                        MultipleSelectEnabled="False" AutoScroll="False" ShowLines="True"
                                                        runat="server" Width="200px">
                                                        <ClientEvents>
                                                            <NodeSelect EventHandler="tvUserRoles_OnSelect" />
                                                        </ClientEvents>
                                                    </ComponentArt:TreeView>
                                                </div>
                                            </Content>
                                            <LoadingPanelClientTemplate>
                                                <table style="height: 350px; width: 100%;" border="0">
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
                                                <CallbackComplete EventHandler="tvUserRoles_onCallbackComplete" />
                                            </ClientEvents>
                                        </ComponentArt:CallBack>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-sm-4">
                    <div class="sparkline10-list mt-b-10">
                        <div class="searchSection">
                            <div class="row">
                                <div class="col-sm-12 col-xs-12">
                                    <div class="pull-left">
                                        <h3 class="h3Text">
                                            Menu Rights
                                             <asp:Label ID="lblMenuListHelp" runat="server" Text="(Check a node checkbox to assign the right)" CssClass="HelpText"></asp:Label>
                                        </h3>
                                    </div>
                                    <div class="borderSearch pull-left"></div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-12 col-xs-12" style="height: 360px; overflow-x: auto; overflow-y:auto;">
                                    <ComponentArt:CallBack ID="CallBackRights" runat="server" OnCallback="CallBackRights_Callback">
                                            <Content>
                                                <div id="divRights" runat="server" style="width: 100%; height: 350px; overflow: auto;">
                                                    <ComponentArt:TreeView ID="tvRights"
                                                        DragAndDropEnabled="false"
                                                        DragAndDropAcrossTreesEnabled="false"
                                                        NodeEditingEnabled="false"
                                                        AutoTheming="true" CssClass="TreeView4" EnableTheming="True"
                                                        MultipleSelectEnabled="False" AutoScroll="False" ShowLines="True"
                                                        runat="server" Width="99%">
                                                        <ClientEvents>

                                                            <NodeCheckChange EventHandler="tvRights_onNodeCheckChanged" />
                                                        </ClientEvents>
                                                    </ComponentArt:TreeView>
                                                </div>
                                            </Content>
                                            <LoadingPanelClientTemplate>
                                                <table style="height: 350px; width: 100%;" border="0">
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
                                                <CallbackComplete EventHandler="tvRights_onCallbackComplete" />
                                            </ClientEvents>
                                        </ComponentArt:CallBack>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-sm-4">
                    <div class="sparkline10-list mt-b-10">
                        <div class="searchSection">
                            <div class="row">
                                <div class="col-sm-12 col-xs-12">
                                    <div class="pull-left">
                                        <h3 class="h3Text">
                                            Assigned Rights
                                             <asp:Label ID="lblRightsHelpText" runat="server" Text="(Right click on the node to unassign the right)" CssClass="HelpText"></asp:Label>
                                        </h3>
                                    </div>
                                    <div class="borderSearch pull-left"></div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-12 col-xs-12" style="height: 360px; overflow-x: auto; overflow-y:auto;">
                                   <ComponentArt:CallBack ID="CallBackAssign" runat="server" OnCallback="CallBackAssign_Callback">
                                            <Content>
                                                <div id="divAssigned" runat="server" style="width: 100%; height: 350px; overflow: auto;">
                                                    <ComponentArt:TreeView ID="tvAssignedRights"
                                                        DragAndDropEnabled="false"
                                                        DragAndDropAcrossTreesEnabled="false"
                                                        NodeEditingEnabled="false"
                                                        AutoTheming="true" CssClass="TreeView4" EnableTheming="True"
                                                        MultipleSelectEnabled="False" AutoScroll="False" ShowLines="True"
                                                        runat="server" Width="99%">
                                                        <ClientEvents>
                                                            <ContextMenu EventHandler="tvAssignedRights_onContextMenu" />
                                                        </ClientEvents>
                                                    </ComponentArt:TreeView>

                                                    <ComponentArt:Menu ID="removeMenu"
                                                        Orientation="Vertical"
                                                        DefaultGroupCssClass="MenuGroupContext"
                                                        SiteMapXmlFile="removeMenu.xml"
                                                        DefaultItemLookId="DefaultItemLook"
                                                        DefaultGroupItemSpacing="1"
                                                        ImagesBaseUrl="../images/"
                                                        EnableViewState="false"
                                                        ContextMenu="Custom"
                                                        runat="server">
                                                        <ClientEvents>
                                                            <ItemSelect EventHandler="removeMenu_onItemSelect" />
                                                        </ClientEvents>
                                                        <ItemLooks>
                                                            <ComponentArt:ItemLook LookId="DefaultItemLook" CssClass="MenuItemContext" HoverCssClass="MenuItemHoverContext" ExpandedCssClass="MenuItemHoverContext" LeftIconWidth="20" LeftIconHeight="18" LabelPaddingLeft="10" LabelPaddingRight="10" LabelPaddingTop="3" LabelPaddingBottom="4" />
                                                            <ComponentArt:ItemLook LookId="BreakItem" CssClass="MenuBreak" />
                                                        </ItemLooks>
                                                    </ComponentArt:Menu>
                                                </div>
                                            </Content>
                                            <LoadingPanelClientTemplate>
                                                <table style="height: 350px; width: 100%;" border="0">
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
                                                <CallbackComplete EventHandler="tvAssignedRights_onCallbackComplete" />
                                            </ClientEvents>
                                        </ComponentArt:CallBack>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="sparklineHeader mt-b-10 marginTP10">
                <div class="sparkline10-hd">
                    <div class="row">
                        <div class="col-sm-6 col-xs-12">
                            
                        </div>
                        <div class="col-sm-6 col-xs-12 text-right">

                            <button type="button" id="btnSave2" runat="server" class="btn btn-custon-four btn-success">
                                <i class="fa fa-floppy-o" aria-hidden="true"></i>&nbsp;Save</button>
                            <button type="button" class="btn btn-custon-four btn-warning" id="btnReset2" runat="server">
                                <i class="fa fa-repeat edu-danger-error" aria-hidden="true"></i>&nbsp;Reset
                            </button>
                            <button type="button" class="btn btn-custon-four btn-danger" id="btnClose2" runat="server">
                                <i class="fa fa-times" aria-hidden="true"></i>&nbsp;Close</button>
                        </div>
                    </div>
                </div>
            </div>

        </div>
    </form>
     <input type="hidden" id="hdnID" runat="server" value="0" />
        <input type="hidden" id="hdnError" runat="server" value="" />
</body>
<script type="text/javascript">
    var objhdnID = document.getElementById('<%=hdnID.ClientID %>');
    var objhdnError = document.getElementById('<%=hdnError.ClientID %>');
    var objbtnSave1 = document.getElementById('<%=btnSave1.ClientID %>');
    var objbtnSave2 = document.getElementById('<%=btnSave2.ClientID %>');
    var strForm = "VRSAccessRights";
</script>
<script src="../scripts/custome-javascript.js" type="text/javascript"></script>
<script src="../scripts/AppPages.js" type="text/javascript"></script>
<script src="scripts/AccessRights.js" type="text/javascript"></script>
</html>
