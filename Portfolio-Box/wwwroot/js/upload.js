
$("#uploadButton").click(function () {
    $("#files").val("")
    displayState(false)
})

$("#dragArea").click(function () {
    $("#files").click()
})
$("#dragArea").on("dragenter", function (e) {
    e.preventDefault()
    $(this).addClass("dragging")
    $(".drag-area .message").html("<strong>Drop</strong> me here")
})
$("#dragArea").on("dragleave", function () {
    const files = $("#files")[0].files
    $(this).removeClass("dragging")
    displayState(files.length > 0, files)
})
$("#dragArea").on("dragover", function (e) {
    e.preventDefault()
})
$("#dragArea").on("drop", function (e) {
    e.preventDefault()
    $(this).removeClass("dragging")
    $("#files")[0].files = e.originalEvent.dataTransfer.files
    filesChanged()
})

const displayState = (valid, files = null) => {
    $(".drag-area .info").css({ color: "darkgray" })
    if (!valid) {
        $(".drag-area .info").text("Max file size 10GB")
        $(".drag-area .message").html("<strong>Drop</strong> a file or <strong>click</strong> to upload")
        $("#submitUpload").prop("disabled", true)
    } else {
        $(".drag-area .message").html(`<strong>${files.length} file(s)</strong> selected`)
        var titles = ""
        for (let i = 0; i < files.length; i++) {
            titles += `${files.item(i).name} - `
        }
        $(".drag-area .info").text(titles.slice(0, -3))
        $("#submitUpload").prop("disabled", false)
    }
}

const filesChanged = () => {
    const files = $("#files")[0].files
    if (files.length === 0) {
        displayState(false)
    } else {
        let totalSize = 0
        for (let i = 0; i < files.length; i++) {
            totalSize += files.item(i).size
        }
        if (totalSize <= 10737418240) {
            displayState(true, files)
        }
        else {
            displayState(false)
            $(".drag-area .info").css({ color: "red" })
        }
    }
}
$("#files").change(filesChanged)
