function addToCart() {
    const productContainer = document.querySelector('.product-container')
    productContainer.addEventListener('click', e => {
        if(e.target.classList.contains('cart-btn')) {
            const id = parseInt(e.target.getAttribute('data-id'))
            const price =  document.querySelector('.product-price .price').innerText
            $.ajax({
                type: "POST",
                url: `${window.location.origin}/Cart/Add/${id}/${price}/` + 1 ,
                dataType: "json",
                contentType: 'application/json; charset=utf-8',
                data: {
                    productId: id,
                    price : price,
                    quantity: 1
                },
                success: function (respose) {
                    if(respose.success) {
                        const quantity = document.querySelector('.shopping-cart .quantity') 
                        quantity.innerHTML = parseInt(quantity.innerHTML) + 1
                    }
                    alert(respose.message);
                },
                error: function () {
                    alert("Đã xảy ra lỗi trong quá trình xử lý .");
                }
            });
        }
    })

}

function removeFromCart() {
    // alert(window.location.origin)
    const productContainer = document.querySelector('.cart-table')
    productContainer.addEventListener('click', e => {
        if(e.target.classList.contains('product-remove')) {
            const id = parseInt(e.target.getAttribute('data-id'))
            alert(id)

            // $.ajax({
            //     type: "POST",
            //     url: `${window.location.origin}/Cart/Remove/${id}`,
            //     dataType: "json",
            //     contentType: 'application/json; charset=utf-8',
            //     data: {
            //         productId: id,
            //     },
            //     success: function (respose) {
            //         if(respose.success) {
            //         }
            //         alert(respose.message);
            //     },
            //     error: function () {
            //         alert("Đã xảy ra lỗi trong quá trình xử lý .");
            //     }
            // });
        }
    })
}