var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "/admin/shipment/getshiperadvice"
        },
        "columns": [
            { "data": "shipment.trackingNumber", "width": "30%" },
            { "data": "shipment.shipmentStatus", "width": "20%" },
            { "data": "adviceMessage", "width": "30%" },
            {
                "data": "id",
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
                "width": "20%"
            }
        ],
        "width": "100%"
    });
}