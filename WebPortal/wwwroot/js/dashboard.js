/* globals Chart:false, feather:false */


function ShowMessageWhileNavigating(title, body, url) {

    $("#inline-message").html('<i class="fas fa-cog fa-spin"></i> ' + title + '. <span class="text-muted-dark">' + body + '</span>. <a href="#" class="btn btn-sm btn-icon btn-warning ml-1" style="float:right !important;" aria-label="Close" onclick="$(this).parent().fadeOut()"><span aria-hidden="true"><i class="fa fa-times"></i></span></a>').show();

    if (url != null) {
        document.location.href = url;
    }

    return;
}

function gridSystemStatusQueryCellInfo(args) {
    if (args.column.field === 'SystemStatusId') {
        if (args.cell.textContent.trim() === "Deleted") {
            args.cell.querySelector(".badge").classList.add("bg-danger");
        }
        else if (args.cell.textContent.trim() === "Active") {
            args.cell.querySelector(".badge").classList.add("bg-success");
        }
        else if (args.cell.textContent.trim() === "Inactive") {
            args.cell.querySelector(".badge").classList.add("bg-warning");
        }
        else {
            args.cell.querySelector(".badge").classList.add("bg-dark");
        }
    }
}