<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm3.aspx.cs" Inherits="VETRIS.RND.WebForm3" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/4.3.1/css/bootstrap.min.css" />
    <link href="bootstrap-fileinput-master/css/fileinput.css" rel="stylesheet" />
    <link href="bootstrap-fileinput-master/css/fileinput.min.css" rel="stylesheet" />
    <link href="bootstrap-fileinput-master/css/fileinput-rtl.css" rel="stylesheet" />
    <link href="bootstrap-fileinput-master/css/fileinput-rtl.min.css" rel="stylesheet" />
    <link href="bootstrap-fileinput-master/themes/explorer/theme.css" rel="stylesheet" />

     <script src="https://code.jquery.com/jquery-3.3.1.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-fileinput/5.0.9/js/plugins/piexif.min.js" type="text/javascript"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-fileinput/5.0.9/js/plugins/sortable.min.js" type="text/javascript"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-fileinput/5.0.9/js/plugins/purify.min.js" type="text/javascript"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.14.7/umd/popper.min.js"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/4.3.1/js/bootstrap.min.js" type="text/javascript"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-fileinput/5.0.9/js/fileinput.min.js"></script>    
    
    <script src="bootstrap-fileinput-master/js/fileinput.js"></script>
    <script src="bootstrap-fileinput-master/js/fileinput.min.js"></script>
    <script src="bootstrap-fileinput-master/themes/fa/theme.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <label for="input-folder-3">Select files/folders</label>
            <div class="file-input file-input-ajax-new">
               
                
                <div >
                    
                    <div class="input-group-btn input-group-append">
                        <button type="button" tabindex="500" title="Clear all unprocessed files" class="btn btn-default btn-secondary fileinput-remove fileinput-remove-button">
                            <i class="glyphicon glyphicon-trash"></i>
                            <span class="hidden-xs">Remove</span>
                        </button>
                        <button type="button" tabindex="500" title="Abort ongoing upload" class="btn btn-default btn-secondary kv-hidden fileinput-cancel fileinput-cancel-button">
                            <i class="glyphicon glyphicon-ban-circle"></i>
                            <span class="hidden-xs">Cancel</span>

                        </button>

                        <button tabindex="500" title="Upload selected files" class="btn btn-default btn-secondary fileinput-upload fileinput-upload-button">
                            <i class="glyphicon glyphicon-upload"></i>
                            <span class="hidden-xs">Upload</span>

                        </button>
                        <div tabindex="500" >
                            
                            <input id="input-folder-3" name="input-folder-3[]" type="file" multiple="" />

                        </div>
                    </div>
                </div>
               <%--  <div >
                     <input type="button" id="btnUpload" value="Upload" onclick="javascript: btnUpload_OnClick();" />
                </div>--%>
            </div>
        </div>
    </form>
</body>
<script type="text/javascript">
    $(document).ready(function () {
        $("#input-folder-3").fileinput({
            //uploadUrl: "/file-upload-batch/2",
            uploadUrl: "/VETRIS/RND/UploadedFiles",
            hideThumbnailContent: true // hide image, pdf, text or other content in the thumbnail preview
        });
    });
    $("#input-id").fileinput();

    //// with plugin options
    $("#input-id").fileinput({ 'showUpload': false, 'previewFileType': 'any' });

    function btnUpload_OnClick() {

    }

</script>
</html>
