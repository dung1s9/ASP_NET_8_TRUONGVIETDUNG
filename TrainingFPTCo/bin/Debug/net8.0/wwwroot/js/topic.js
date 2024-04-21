$(function () {
    $(".js-delete-topic").on("click", function () {
        let self = $(this);
        let idTopic = self.attr("id").trim();

        if ($.isNumeric(idTopic)) {
            $.ajax({
                url: "/Topic/Delete",
                type: "post",
                data: { id: idTopic },
                beforeSend: function () {
                    self.text("Loading ...");
                },
                success: function (response) {
                    self.text("Delete");
                    if (response.cod == 200) {
                        alert(response.message);
                        $('.row-course-' + idTopic).hide();
                        location.reload(); // Reload the page
                    } else {
                        alert(response.message);
                        return;
                    }
                }
            });
        }
    });
});