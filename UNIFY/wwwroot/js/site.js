$(document).ready(function () {
    if ($.fn.DataTable.isDataTable("#table")) {
        $('#table').DataTable().destroy();
    }

    $('#table').DataTable({
        "paging": true,
        "pageLength": 5,
        "searching": true,
        "ordering": false,
        "info": true,
        "lengthChange": false,

    });
});
