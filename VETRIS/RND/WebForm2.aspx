<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm2.aspx.cs" Inherits="VETRIS.RND.WebForm2" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="../scripts/jquery-1.12.4.min.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <center>
          <div id="box" style="border:solid 1px #bbb;height:100px;overflow:auto;">
              Drag & Drop files from your machine on this box.

          </div>
        <br />
          <input id="upload" type="button" value="Upload Selected Files" />
        </center>
    </form>
</body>
<script type="text/javascript">
    var selectedFiles;

    $(document).ready(function () {
        var box;
        box = document.getElementById("box");
        box.addEventListener("dragenter", OnDragEnter, false);
        box.addEventListener("dragover", OnDragOver, false);
        box.addEventListener("drop", OnDrop, false);

        function OnDragEnter(e) {
            e.stopPropagation();
            e.preventDefault();
        }

        function OnDragOver(e) {
            e.stopPropagation();
            e.preventDefault();
        }

        function OnDrop(e) {
            e.stopPropagation();
            e.preventDefault();
            debugger;
            selectedFiles = e.dataTransfer.files;
            $("#box").text(selectedFiles.length + " file(s) selected for uploading!");
        }



    });
    $("#upload").click(function () {
        var data = new FormData();
        debugger;
        for (var i = 0; i < selectedFiles.length; i++) {
            data.append(selectedFiles[i].name, selectedFiles[i]);
        }
        $.ajax({

            type: "POST",
            url: "FileHandler.ashx",
            contentType: false,
            processData: false,
            data: data,
            success: function (result) {
                alert(result);
            },
            error: function () {
                alert("There was error uploading files!");
            }
        });
    });
</script>
</html>
