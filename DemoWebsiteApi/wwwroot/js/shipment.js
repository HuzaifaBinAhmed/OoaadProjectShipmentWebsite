var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "autoWidth": false,
        "ajax": {
            "url": "/admin/shipment/getall"
        },
        "columns": [
            { "data": "consigneeName", "width": "10%" },
            { "data": "address", "width": "20%" },
            { "data": "codAmount", "width": "10%" },
            { "data": "city", "width": "10%" },
            { "data": "trackingNumber", "width": "20%" },
            { "data": "applicationUser.name", "width": "10%" },
            { "data": "shipmentStatus", "width": "10%" },
            {
                "data": "shipmentId",
                "render": function (data) {
                    return `
                        <div class="w-75 btn-group" role="group">
                            <a href="/admin/shipment/upsert?id=${data}" class="btn btn-primary mx-2">
                                <i class="bi bi-pencil-square"></i> Edit
                            </a>
                           <a href="/admin/shipment/delete?id=${data}" class="btn btn-danger mx-2">
                                <i class="bi bi-pencil-square"></i> Delete
                            </a>
                        </div>`;
                },
                "width": "10%"
            }
        ],
        "width": "100%"
    });
}