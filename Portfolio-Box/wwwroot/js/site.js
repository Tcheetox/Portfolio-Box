// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

const getLocation = () => {
    const current = window.location.href
    return current.slice(-1) === "/" ? current.slice(0, -1) : current
}

const getCookie = name => {
    const parts = ("; " + document.cookie).split("; " + name + "=");
    if (parts.length == 2) return parts.pop().split(";").shift();
}

$("#uploadForm").submit(function (e) {
    e.preventDefault()
    uploadFile($(this)[0])
})
const uploadFile = async form => {
    $("#uploadButton").prop("disabled", true);
    $("#uploadButton .text-content").addClass("hidden")
    $("#uploadButton .spinner-border").removeClass("hidden")

    const files = $("input#files")[0].files
    let totalSize = 0
    for (let i = 0; i < files.length; i++) {
        totalSize += files.item(i).size
    }
    let progress = 0
    const progressBar = $("#progressBar")

    const xhr = new XMLHttpRequest();
    xhr.upload.onprogress = e => {
        const current = Math.round(100 * e.loaded / totalSize)
        if (current > progress) {
            progressBar.css({ background: `linear-gradient(90deg, rgba(0,123,255,1) 0%, rgba(0,123,255,1) ${current - 3}%, rgba(255,255,255,1) ${current + 2}%, rgba(255,255,255,0) 100%)` })
            progress = current
        }
    }
    xhr.open("POST", `${getLocation()}/file/upload`, true);
    xhr.setRequestHeader("RequestVerificationToken", getCookie('RequestVerificationToken'));
    xhr.onreadystatechange = function () {
        if (this.readyState === XMLHttpRequest.DONE) {
            if (this.status === 201)
                $("#fileList").load(`${getLocation()}?handler=FileListPartial`) // Reload partial _FileList to display updated results

            progressBar.css({ background: `linear-gradient(90deg, rgba(0,123,255,1) 0%, rgba(255,255,255,0) 0%)` })
            $("#uploadButton").prop("disabled", false)
            $("#uploadButton .spinner-border").addClass("hidden")
            $("#uploadButton .text-content").removeClass("hidden")
        }
    }
    xhr.send(new FormData(form));
}

const adjustDownloadUrl = () => {
    const uri = $("#downloadUrl").val()
    $("#downloadUrl").val(`${getLocation()}/file/download/${uri}`)
}

$("#fileList").on("click", ".file-tile", function () {
    $("#detailsModal").modal("show")
    $("#detailsModal").load(`${getLocation()}/file/details/${$(this).data("id")}`, adjustDownloadUrl)
})


$("#detailsModal").on("click", "#deleteFile", function () {
    $.ajax({
        url: `${getLocation()}/file/delete/${$(this).data("id")}`,
        type: 'DELETE',
        headers: { 'RequestVerificationToken': getCookie('RequestVerificationToken') },
        success: () => $(`#fileTile-${$(this).data("id")}`).remove()
    })
})

$("#detailsModal").on("submit", "#createLinkForm", function (e) {
    e.preventDefault()
    $.ajax({
        url: `${getLocation()}/link/create?id=${$(this).data("id")}&expiry=${$('#expiresIn').val()}`,
        type: 'POST',
        headers: { 'RequestVerificationToken': getCookie('RequestVerificationToken') },
        success: results => {
            $("#detailsModal").html(results)
            adjustDownloadUrl()
        }
    })
})

$("#detailsModal").on("click", "#deleteLink", function () {
    $.ajax({
        url: `${getLocation()}/link/delete/${$(this).data("id")}`,
        type: 'DELETE',
        headers: { 'RequestVerificationToken': getCookie('RequestVerificationToken') },
        success: results => $("#detailsModal").html(results)
    })
})

$("#detailsModal").on("click", "#downloadUrl", function () {
    navigator.clipboard.writeText($(this).val())
    $("#clipMe").text("(copied!)")
})