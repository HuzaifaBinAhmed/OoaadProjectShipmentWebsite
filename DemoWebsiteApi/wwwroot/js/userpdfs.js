var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "autoWidth": false,
        "ajax": {
            "url": "/admin/finance/getuser"
        },
        "columns": [
            {
                "data": "pdfFilePath",
                "render": function (data) {
                    return `<a href="${data}" class="btn btn-info btn-sm" download>
                                <i class="bi bi-download"></i> Download
                            </a>`;
                },
                "width": "30%"
            },
            { "data": "pdfFilePath", "width": "70%" }
        ],
        "width": "100%"
    });
}
