
function deleteRow(type) { 
    const table = document.querySelector('.table')
    table.addEventListener('click', e => {
        if(e.target.classList.contains('delete-btn')) {
            const id = parseInt(e.target.getAttribute('data-id'))
            if(confirm('Are you sure about that')) {
                $.ajax({
                    type: "POST",
                    url: `${window.location.origin}/Admin/${type}/Delete/` + id,
                    dataType: "json",
                    contentType: 'application/json; charset=utf-8',
                    data: {id: id},
                    success: function (respose) {
                        if(respose.success) {
                            e.target.closest('tr').remove()
                        }
                        alert(respose.message);
                    },
                    error: function () {
                        alert("Đã xảy ra lỗi trong quá trình xử lý .");
                    }
                });
            }
        }
    })

}
// $(".delete-btn").click(() => {
//     alert("xóa")
//     var id = $(this).data("id");
//     console.log(id);

// })

