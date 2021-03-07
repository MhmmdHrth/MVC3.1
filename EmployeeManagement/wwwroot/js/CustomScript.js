function confirmDelete(uniqueId, isClicked) {
    var deleteSpan = `deleteSpan_${uniqueId}`;
    var confirmDeleteSpan = `confirmDeleteSpan_${uniqueId}`;

    if (isClicked) {
        $(`#${deleteSpan}`).hide()
        $(`#${confirmDeleteSpan}`).show()
    }
    else {
        $(`#${deleteSpan}`).show()
        $(`#${confirmDeleteSpan}`).hide()
    }
}