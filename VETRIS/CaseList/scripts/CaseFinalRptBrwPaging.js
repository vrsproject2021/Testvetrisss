// - Paging --
function Render_Paging() {
    var ul = $(".pagination");
    ul.empty();
    var num_pages = parseInt(parseInt(objhdnTotalRecords.value) / parseInt(objhdnPageSize.value));
    var reminder = parseInt(objhdnTotalRecords.value) % parseInt(objhdnPageSize.value);
    if (reminder > 0) num_pages += 1;
    if (num_pages === 0) return;
    var current = parseInt(objhdnPageNo.value);
    var arr_pages = buildArr(current, num_pages);
    ul.append('<li class="page-item"><a class="page-link" href="javascript:loadPage(' + 1 +')">First</a></li>')
    arr_pages.forEach(item => {
        if (item !== "...") {
            ul.append('<li class="page-item ' + ((item === current) ? 'active' : '') +'"><a class="page-link" href="javascript:loadPage('+item+')">' + item + '</a></li>');
} else {

            ul.append('<li class="page-item"><a class="page-link" href="javascript:void">' + item + '</a></li>');
}
});
ul.append('<li class="page-item"><a class="page-link" href="javascript:loadPage(' + num_pages +')">Last</a></li>')

}

function loadPage(num) {
    objhdnPageNo.value = num.toString();
    SearchRecord();
}

function buildArr(c, n) {
    if (n <= 7) {
        return [...Array(n)].map((_, i) => i + 1);
    } else {
        if (c < 3 || c > n - 2) {
            return [1, 2, 3, "...", n - 2, n - 1, n];
        } else {
            return [1, "...", c - 1, c, c + 1, "...", n];
        }
    }
}
