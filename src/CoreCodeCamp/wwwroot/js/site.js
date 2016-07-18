// Write your Javascript code.
$(document).ready(function () {

  $(".voteStar").on("click", function () {
    var $this = $(this);
    $.ajax({
      url: "/api/talks/" + $this.attr("data-id") + "/toggleStar",
      type: 'PUT',
      success: function () {
        var id = me.attr("data-id");
        if ($this.hasClass("voted")) {
          $this.removeClass("voted");
        } else {
          $this.addClass("voted");
        }
      },
      error: function (error) {
        if (error.status == 401) {
          showAlert("Must be logged in to set favorite sessions.");
        } else {
          showAlert("Failed to set favorite. Unknown reason.");
        }
      }
    });
  });

  var $topBar = $("#topBar");
  function showAlert(msg, alertType) {
    if (!alertType) alertType = "danger";
    var template = "<div class='alert alert-##ALERTTYPE## alert-dismissible'>" +
          "<button type='button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'><i class='fa fa-close'></i></span></button>" +
          "<p>##MSG##</p>" +
        "</div>";
    var $alert = (template.replace("##MSG##", msg).replace("##ALERTTYPE##", alertType));
    $topBar.after($alert);
    $alert.alert();
  }



});