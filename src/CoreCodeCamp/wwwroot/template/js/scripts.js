
function scroll_to(clicked_link, nav_height) {
  var element_class = clicked_link.attr('href').replace('#', '.');
  var scroll_to = 0;
  if (element_class != '.top-content') {
    element_class += '-container';
    scroll_to = $(element_class).offset().top - nav_height;
  }
  if ($(window).scrollTop() != scroll_to) {
    $('html, body').stop().animate({ scrollTop: scroll_to }, 1000);
  }
}


jQuery(document).ready(function () {

  /*
	    Navigation
	*/
  $('a.scroll-link').on('click', function (e) {
    e.preventDefault();
    scroll_to($(this), $('nav').outerHeight());
  });
  // toggle "navbar-no-bg" class
  var $top = $('.top-content .text');
  if ($top.length) {
    var waypoint = new Waypoint({
      element: $top[0],
      handler: function () {
        $('nav').toggleClass('navbar-no-bg');
      }
    });
  } else console.log("No top-content")

  /*
      Background slideshow
  */
  $('.top-content').backstretch([
    "../../img/backgrounds/top-1.jpg",
    "../../img/backgrounds/top-2.jpg",
    "../../img/backgrounds/top-3.jpg",
    "../../img/backgrounds/top-4.jpg"
  ], { duration: 3000, fade: 750 });

  $('.testimonials-container').backstretch([
    "../../img/backgrounds/cfs-1.jpg",
    "../../img/backgrounds/cfs-2.jpg",
    "../../img/backgrounds/cfs-3.jpg",
    "../../img/backgrounds/cfs-4.jpg"
  ], { duration: 3000, fade: 750 });

  $('.how-it-works-container').backstretch("assets/img/backgrounds/4.jpg");
  $('.call-to-action-container').backstretch("assets/img/backgrounds/1.jpg");
  $('.contact-container').backstretch("assets/img/backgrounds/2.jpg");

  $('#top-navbar-1').on('shown.bs.collapse', function () {
    $('.top-content').backstretch("resize");
  });
  $('#top-navbar-1').on('hidden.bs.collapse', function () {
    $('.top-content').backstretch("resize");
  });

  $('a[data-toggle="tab"]').on('shown.bs.tab', function () {
    $('.testimonials-container').backstretch("resize");
  });

  /*
      Wow
  */
  new WOW().init();

  /*
	    Modals
	*/
  $('.launch-modal').on('click', function (e) {
    e.preventDefault();
    $('#' + $(this).data('modal-id')).modal();
  });

  /*
	    Subscription form
	*/
  $('.subscribe .subscribe-email').on('focus', function () {
    $(this).val('').removeClass('subscribe-error');
  });
  $('.subscribe form').on('submit', function (e) {
    e.preventDefault();
    var this_form = $(this);
    var postdata = this_form.serialize();
    $.ajax({
      type: 'POST',
      url: 'assets/subscribe.php',
      data: postdata,
      dataType: 'json',
      success: function (json) {
        if (json.valid == 0) {
          $('.success-message').hide();
          this_form.find('.subscribe-email').addClass('subscribe-error').val(json.message);
          $('.subscribe form').addClass('animated shake').one('webkitAnimationEnd mozAnimationEnd MSAnimationEnd oanimationend animationend', function () {
            $(this).removeClass('animated shake');
          });
        }
        else {
          this_form.hide();
          $('.success-message').html(json.message).fadeIn('fast');
        }
      }
    });
  });

  /*
	    Contact form
	*/
  $('.contact-box form').on('submit', function (e) {
    e.preventDefault();
    var this_form_parent = $(this).parents('.contact-box');
    var postdata = $(this).serialize();
    $.ajax({
      type: 'POST',
      url: 'assets/contact.php',
      data: postdata,
      dataType: 'json',
      success: function (json) {
        $('.contact-box label[for="contact-email"] .contact-error').fadeOut('fast', function () {
          if (json.emailMessage != '') {
            $(this).html('(' + json.emailMessage + ')').fadeIn('fast');
          }
        });
        $('.contact-box label[for="contact-subject"] .contact-error').fadeOut('fast', function () {
          if (json.subjectMessage != '') {
            $(this).html('(' + json.subjectMessage + ')').fadeIn('fast');
          }
        });
        $('.contact-box label[for="contact-message"] .contact-error').fadeOut('fast', function () {
          if (json.messageMessage != '') {
            $(this).html('(' + json.messageMessage + ')').fadeIn('fast');
          }
        });
        if (json.emailMessage == '' && json.subjectMessage == '' && json.messageMessage == '') {
          this_form_parent.find('.contact-top').fadeOut('fast');
          this_form_parent.find('.contact-bottom').fadeOut('fast', function () {
            this_form_parent.append('<p>Thanks for contacting us! We will get back to you very soon.</p>');
            $('.contact-container').backstretch("resize");
          });
        }
      }
    });
  });


});


jQuery(window).on("load", function () {

  /*
		Hidden images
	*/
  $(".modal-body img, .testimonial-image img").attr("style", "width: auto !important; height: auto !important;");

});
