<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VRSProdSchedule.aspx.cs" Inherits="VETRIS.Radiologist.VRSProdSchedule" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="UTF-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=1"/>
    <link href="https://fonts.googleapis.com/css?family=Roboto:100,300,400,700,900" rel="stylesheet" />
    <link href="../css/bootstrap.min.css" rel="stylesheet" />
    <link href="../css/font-awesome.min.css" rel="stylesheet" />
    <link href="../css/responsive.css" rel="stylesheet" />
    <link id="lnkSTYLE" runat="server" href = "../css/style.css" rel="stylesheet" type="text/css" />
    <link id="lnkTABLE" runat="server" href = "../css/table.css" rel="stylesheet" type="text/css" />
    <link id="lnkSEL" runat="server" href = "../css/select2.min.css" rel="stylesheet" type="text/css" />

    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/flatpickr/dist/flatpickr.min.css"/>
    <link href="https://cdn.jsdelivr.net/npm/select2@4.1.0-beta.1/dist/css/select2.min.css" rel="stylesheet" />
    

    <script src="../scripts/windows-iana/dist/windows-iana.esm.js" crossorigin="anonymous"></script>
    <script src="../scripts/moment.min.js" crossorigin="anonymous"></script>
    <script src="../scripts/moment-timezone-with-data.min.js" crossorigin="anonymous"></script>
    <script src="../scripts/jquery-1.12.4.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/select2@4.1.0-beta.1/dist/js/select2.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/flatpickr"></script>
    <script src="https://cdn.jsdelivr.net/npm/flatpickr/dist/plugins/rangePlugin.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/underscore.js/1.8.3/underscore.js"></script>
    <script src="../scripts/bootstrap.min.js"></script>
    <title>Schedule</title>
</head>
<body>

    <!-- Container begin -->
    <div class="main-container">
         <div class="sparklineHeader mt-b-10 marginTP10">
                <div class="sparkline10-hd">
                    <div class="row">
                        <div class="col-sm-6 col-xs-12">
                            <h2>Radiologist Productivity Schedule</h2>
                        </div>
                        <div class="col-sm-6 col-xs-12 text-right">
                            <button class="btn btn-success" id="create"><i class="fa fa-plus" aria-hidden="true"></i>&nbsp;Create Schedule</button>
                            <button type="button" class="btn btn-custon-four btn-danger" id="btnClose" onclick="javascript:btnClose_OnClick();">
                                <i class="fa fa-times edu-danger-error" aria-hidden="true"></i>&nbsp;Close
                                       
                            </button>
                        </div>
                    </div>
                </div>
            </div>
        <header>
            <div class="form-inline mt-2">
                <div class="form-group">
                    <label for="tz">Time Zone:</label>
                    <select id="tz" style="padding: 2px 4px; width: 220px;" class="form-control">
                    </select>
                </div>
                <div class="form-group">
                    <label for="fromDate">Date From:</label>
                    <input type="text" id="fromDate" placeholder="From" name="from" style="padding: 2px 4px; width: 80px;" class="date form-control" />
                </div>
                <div class="form-group">
                    <label for="next">Days:</label>
                    <select id="next" style="padding: 2px 4px; width: 50px;" class="form-control">
                        <option value="1">1</option>
                        <option value="3">3</option>
                        <option value="5">5</option>
                        <option value="7" selected>7</option>
                        <option value="15">15</option>
                        <option value="30">30</option>
                    </select>
                </div>
                <div class="form-group">
                    <label for="toDate">To:</label>
                    <input type="text" id="toDate" placeholder="To" name="to" style="padding: 2px 4px; width: 80px;" class="date form-control" readonly />
                </div>
                <div class="form-group">
                    <select id="option" style="padding: 2px 4px; width: 70px;" class="form-control">
                        <option value="group" selected>Group</option>
                        <option value="reader">Reader</option>
                    </select>
                </div>
                <div id="readers" class="form-group">
                    <select id="reader" style="padding: 2px 4px; width: 200px;" class="form-control">
                        <option value="" selected>All</option>
                    </select>
                </div>
                <div id="groups" class="form-group" style="display: none;">
                    <select id="group" style="padding: 2px 4px; width: 200px;" class="form-control">
                         <option value="" selected>All</option>
                    </select>
                </div>

                <div class="form-group">
                    <label for="suppress">Show:</label>
                    <select id="suppress" style="padding: 2px 2px; width: 130px;" class="form-control">
                         <option value="all" selected>All</option>
                         <option value="scheduled">Scheduled only</option>
                    </select>
                </div>

                <button  style="display: none;" class="apply btn btn-primary"><span>Apply</span></button>
                
            </div>

        </header>
        <div id="schedulediv" class="table-container"></div>

    </div>
    <!-- Container end -->

    <!-- Modal -->
    <div class="modal fade" id="editDialog" role="dialog" aria-labelledby="scheduleTitle" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                    <h5 class="modal-title" id="scheduleTitle">Edit Schedule</h5>

                </div>
                <div class="modal-body">
                    <form>
                        <div class="row">
                            <div class="form-group  col-md-6">
                                <label for="readerId">Reader <span class="mandatory">*</span>:</label>
                                <select id="readerId" style="width: 100%;" class="form-control">
                                </select>
                            </div>
                            <div class="col-md-1">
                                <div style="margin-top: 27px; margin-left: -20px;" id="rcolor">
                                </div>
                            </div>
                            <div class="col-md-5">
                                <div style="margin-top: 27px;" id="readertz">
                                    <span class="modal-span">Reader Timezone</span>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="form-group  col-md-12">
                                <label for="currentTz" class="col-form-label">Using Timezone:</label>
                                <select id="currentTz" style="padding: 2px 2px; width: 250px;" class="form-control">
                                </select>
                            </div>
                        </div>
                        <div class="row editmode">
                            <div class="form-group  col-md-4">
                                <label for="date" class="col-form-label">Date<span class="mandatory">*</span>:</label>
                                <input type="text" class="form-control" id="date" readonly />
                            </div>
                        </div>
                        <div class="row newmode" >
                            <div class="form-group  col-md-4">
                                <label for="date1" class="col-form-label">From Date<span class="mandatory">*</span>:</label>
                                <input type="text" class="form-control" id="date1" readonly />

                            </div>
                            <div class="form-group  col-md-4">
                                <label for="date2" class="col-form-label">To Date:</label>
                                <input type="text" class="form-control" id="date2" readonly />
                            </div>
                        </div>
                        <div class="row newmode">
                            <div class="col-sm-4 col-xs-12 marginTP5 text-right">
                                <label style="margin-right: 5px; font-weight: bold;">OR</label>
                            </div>
                            <div class="col-sm-2 col-xs-12 marginTP5">
                                <div class="grid_option1">
                                    <input type="checkbox" id="chkNext" style="width: 18px; height: 18px; float: left;" />
                                    <label for="chkNext" style="margin-top: 5px; margin-left: 5px; float: left;">For Next</label>
                                </div>
                            </div>
                            <div class="col-sm-2 col-xs-12 marginTP5">
                                <input id="ndays" class="form-control" MaxLength="10" Width="30%" readonly="true" placeholder="" style="text-align: center;"></input>
                            </div>
                            <div class="col-sm-2 col-xs-12 marginTP5">
                                <label style="margin-top: 8px; margin-left: 5px;">Day(s)</label>
                            </div>
                        </div>
                        <div class="row">
                            <div class="form-group col-md-6">
                                <label for="startTime" class="col-form-label">
                                    Start Time<span
                                        class="mandatory">*</span>:</label>
                                <div class="form-inline">
                                    <select id="hh1" class="form-control"></select>
                                    <select id="mm1" class="form-control"></select>
                                    <select id="ampm1" class="form-control">
                                        <option value="AM" selected>AM</option>
                                        <option value="PM">PM</option>
                                    </select>
                                </div>
                            </div>

                            <div class="form-group col-md-6">
                                <label for="endTime" class="col-form-label">
                                    End Time<span
                                        class="mandatory">*</span>:</label>
                                <div class="form-inline">
                                    <select id="hh2" class="form-control"></select>
                                    <select id="mm2" class="form-control"></select>
                                    <select id="ampm2" class="form-control">
                                        <option value="AM" selected>AM</option>
                                        <option value="PM">PM</option>
                                    </select>
                                </div>
                            </div>
                        </div>
                        <p id="defaultTz"></p>
                        
                        <div class="row newmode" style="margin-top: 4px;">
                                    <div class="col-sm-12 col-xs-12">
                                        <div class="searchSection">
                                            <div class="row">
                                                <div class="col-sm-12 col-xs-12">
                                                    <div class="pull-left">
                                                        <h3 class="h3Text">For Week Day(s)</h3>
                                                    </div>
                                                    <div class="borderSearch pull-left"></div>
                                                </div>
                                            </div>

                                            <div class="row">
                                                <div class="col-sm-12 col-xs-12 ">
                                                    <div class="row">
                                                        <div class="col-sm-12 col-xs-12 marginTP5">
                                                            <input type="checkbox" id="chkMon" style="width: 18px; height: 18px; float: left;" />
                                                            <label for="chkMon" style="margin-top: 5px; margin-left: 5px; float: left;">Monday</label>
                                                            <input type="checkbox" id="chkTue" style="margin-left: 10px; width: 18px; height: 18px; float: left;" />
                                                            <label for="chkTue" style="margin-top: 5px; margin-left: 5px; float: left;">Tuesday</label>
                                                            <input type="checkbox" id="chkWed"  style="margin-left: 10px; width: 18px; height: 18px; float: left;" />
                                                            <label for="chkWed" style="margin-top: 5px; margin-left: 5px; float: left;">Wednesday</label>
                                                            <input type="checkbox" id="chkThu"  style="margin-left: 10px; width: 18px; height: 18px; float: left;" />
                                                            <label for="chkThu" style="margin-top: 5px; margin-left: 5px; float: left;">Thursday</label>
                                                            <input type="checkbox" id="chkFri"  style="margin-left: 10px; width: 18px; height: 18px; float: left;" />
                                                            <label for="chkFri" style="margin-top: 5px; margin-left: 5px; float: left;">Friday</label>
                                                            <input type="checkbox" id="chkSat" style="margin-left: 10px; width: 18px; height: 18px; float: left;" />
                                                            <label for="chkSat" style="margin-top: 5px; margin-left: 5px; float: left;">Saturday</label>
                                                            <input type="checkbox" id="chkSun" style="margin-left: 10px; width: 18px; height: 18px; float: left;" />
                                                            <label for="chkSun" style="margin-top: 5px; margin-left: 5px; float: left;">Sunday</label>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                        <div class="form-group">
                            <label for="notes" class="col-form-label">Notes:</label>
                            <textarea class="form-control" id="notes" maxlength="250" style="height:48px;"></textarea>
                        </div>
                    </form>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-primary" id="savebtn"><i class="fa fa-floppy-o" aria-hidden="true"></i>&nbsp;Save</button>
                    <button type="button" class="btn btn-danger" id="deletebtn"><i class="fa fa-trash" aria-hidden="true"></i>&nbsp;Delete</button>
                    <button type="button" class="btn btn-secondary" data-dismiss="modal">Cancel</button>
                </div>
            </div>
        </div>
    </div>
    
    <%{

        var request = HttpContext.Current.Request;
        var appRootFolder = request.ApplicationPath;
        if (!appRootFolder.EndsWith("/")) {
            appRootFolder += "/";
        }
        this.baseUrl = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, appRootFolder);
    }%>
    <script>
        var baseUrl = "<%=this.baseUrl%>"
    </script>
    <script src="scripts/ProdSchedule.js?<%=string.Format("{0:yyyyMMddHHmmss}",DateTime.Now) %>"></script>
    <script src="scripts/ProdScheduleClient.js?<%=string.Format("{0:yyyyMMddHHmmss}",DateTime.Now) %>"></script>
</body>
</html>
