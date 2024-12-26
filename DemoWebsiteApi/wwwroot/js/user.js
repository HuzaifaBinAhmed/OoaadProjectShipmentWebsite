var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "/admin/user/getall"
        },
        "columns": [
            { "data": "name", "width": "20%" },
            { "data": "email", "width": "20%" },
            { "data": "phoneNumber", "width": "10%" },
            { "data": "address", "width": "20%" },
            { "data": "role", "width": "10%" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                        <div class="w-75 btn-group" role="group">
                            <a href="/admin/user/edit?id=${data}" class="btn btn-primary mx-2">
                                <i class="bi bi-pencil-square"></i> Edit
                            </a>
                           <a href="/admin/user/delete?id=${data}" class="btn btn-danger mx-2">
                                <i class="bi bi-pencil-square"></i> Delete
                            </a>
                        </div>`;
                },
                "width": "20%"
            }
        ],
        "width": "100%"
    });
}