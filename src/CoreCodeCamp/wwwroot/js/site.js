// Write your Javascript code.
$(document).ready(function () {

  $(".voteStar").on("click", function () {
    var me = $(this);
    var id = me.attr("data-id");
    if (me.hasClass("voted")) {
      me.removeClass("voted");
    } else {
      me.addClass("voted");
    }
    $.ajax({
      url: "/api/talks/" + id + "/toggleStar",
      type: 'PUT',
      error: function (result) {
        console.log("Failed to PUT to toggling the star of a talk")
      }
    });
  });

});