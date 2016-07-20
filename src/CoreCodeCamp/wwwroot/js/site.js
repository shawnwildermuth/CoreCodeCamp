// Write your Javascript code.
$(document).ready(function () {

  $(".delete-star").on("click", function () {
    var $this = $(this);
    var id = $this.attr("data-id");

    toggleFavorite(id, function () {
      $this.parent().remove();
    }, function (error) {
        showAlert("Failed to remove favorite. Unknown reason.");
    });

  });

  $(".voteStar").on("click", function () {
    var $this = $(this);
    var id = $this.attr("data-id");

    toggleFavorite(id, function () {
      if ($this.hasClass("voted")) {
        $this.removeClass("voted");
      } else {
        $this.addClass("voted");
      }
    }, function (error) {
      if (error.status == 401) {
        showAlert("Must be logged in to set favorite sessions.");
      } else {
        showAlert("Failed to set favorite. Unknown reason.");
      }
    });
  });

  function toggleFavorite(id, success, fail) {
    var moniker = window.location.pathname.split('/')[1];
    $.ajax({
      url: "/" + moniker + "/api/me/favorites/" + id,
      type: 'PUT',
      success: success,
      error: fail
    });

  }

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