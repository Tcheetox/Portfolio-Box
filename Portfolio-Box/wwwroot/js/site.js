// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

const uploadFile = async form => {
    try {
        $("#uploadButton").prop("disabled", true);

        await fetch(`${window.location.href}/file/upload`, {
            method: 'POST',
            headers: { 'RequestVerificationToken': getCookie('RequestVerificationToken') },
            body: new FormData(form)
        });

        refreshFileList() // Reload partial _FileList to display updated results 
    } catch (error) {
        console.error('Error:', error);
    } finally {
        $("#uploadButton").prop("disabled", false);
    }
}

const clearInput = () => $("#file").val('')

const refreshFileList = () => $("#fileList").load(`${window.location.href}?handler=FileListPartial`)

const getCookie = name => {
    const parts = ("; " + document.cookie).split("; " + name + "=");
    if (parts.length == 2) return parts.pop().split(";").shift();
}

const adjustDownloadUrl = () => {
    const uri = $("#downloadUrl").val()
    $("#downloadUrl").val(`${window.location.href}/file/download/${uri}`)
}

const showDetails = id => {
    $("#detailsModal").modal("show")
    $("#detailsModal").load(`${window.location.href}/file/details/${id}`, adjustDownloadUrl)
}

const deleteFile = id => {
    $.ajax({
        url: `${window.location.href}/file/delete/${id}`,
        type: 'DELETE',
        headers: { 'RequestVerificationToken': getCookie('RequestVerificationToken') },
        success: () => $(`#fileTile-${id}`).remove()
    })
}

const createLink = async (id, expiry) => {
    $.ajax({
        url: `${window.location.href}/link/create?id=${id}&expiry=${expiry}`,
        type: 'POST',
        headers: { 'RequestVerificationToken': getCookie('RequestVerificationToken') },
        success: () => showDetails(id)
    })
}

const deleteLink = async (id, linkId) => {
    $.ajax({
        url: `${window.location.href}/link/delete/${linkId}`,
        type: 'DELETE',
        headers: { 'RequestVerificationToken': getCookie('RequestVerificationToken') },
        success: () => showDetails(id)
    })
}

const copyToClipboard = () => {
    navigator.clipboard.writeText($("#downloadUrl").val())
    $("#clipMe").text("(copied!)")
}