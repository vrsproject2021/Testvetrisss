$('#hdnUrlReferrer').val(document.referrer);
var objDashboardIframePage = document.getElementById('iframeDashboard');
var strForm = "VRSDashboard";
if ($('#hdnIsRefreshBtn').val() == 'Y')
    $('#btnReset').css('display', 'inline');
else
    $('#btnReset').css('display', 'none');
if ($('#hdnDefaultDesc').val() !== '' && $('#hdnDefaultUrl').val() !== '') {
    loadDashboardPage($('#hdnDefaultDesc').val(),
                      $('#hdnDefaultUrl').val(),
                      $('#hdnMenuId').val(),
                      $('#hdnRefreshTime').val(),
                      $('#hdnIsRefreshBtn').val(),
                      $('#hdnreportTitle').val(),
                      $('#hdnTheme').val()
                      );
}
function loadDashboardPage(desc, url, id, sec, is_refresh_button, title,th) {
    objDashboardIframePage.src = url + "?uid=" + $('#hdnUserID').val() + "&mnuid=" + id + "&desc=" + desc + "&sec=" + sec + "&isrefresh=" + is_refresh_button + "&rt=" + title+'&th='+th;
}

function adjustDashboardFrameHeight() {
    var frame = getElement("iframeDashboard");
    var divBody = getElement("divFrame");
    var scrollHeight = divBody.scrollHeight;
    var frameDoc = getIFrameDocument("iframeDashboard");

    //offsetHeight = offsetHeight -123;
    var offsetHeight = divBody.offsetHeight;
    //alert(offsetHeight);
    if (frameDoc.body.scrollHeight > scrollHeight) {
        frame.style.height = frameDoc.body.scrollHeight.toString() + "px";
        //divBody.height = frameDoc.body.offsetHeight + 20;
    }
    else {
        frame.style.height = scrollHeight.toString() + "px";
        //divBody.height = "550px";
    }
    //doLoad();
}

function getElement(aID) {

    return (document.getElementById) ? document.getElementById(aID) : document.all[aID];
}
function getIFrameDocument(aID) {
    var rv = null;
    var frame = getElement(aID);
    // if contentDocument exists, W3C 

    //compliant(e.g.Mozilla)
    if (frame.contentDocument)
        rv = frame.contentDocument;
    else // bad Internet Explorer  ;)
        rv = document.frames[aID].document;
    return rv;
}

function closedashboardreport() {
    objDashboardIframePage.src = "";
    //parent.$('body').css('overflow', 'scroll');
    parent.closedashboardreport();
}