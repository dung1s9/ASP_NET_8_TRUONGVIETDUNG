$(function () {
    $(".js-delete-account").on("click", function () {
        let self = $(this);
        let idUser = self.attr("id").trim();
        if ($.isNumeric(idUser)) {
            $.ajax({
                url: "/Account/Delete",
                type: "post",
                data: { id: idUser },
                beforeSend: function () {
                    self.text("Loading ...");
                },
                success: function (response) {
                    self.text("Delete");
                    if (response.cod == 200) {
                        alert(response.message);
                        // An bo dong vua xoa
                        $('.row-course-' + idUser).hide();
                    } else {
                        alert(response.message);
                        return;
                    }
                }
            });
        }
    })
});