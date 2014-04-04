/*global core */

var logIn = (function ($) {
    'use strict';
    core.DisableRecheckIn = true;
    return {
        init: function () {
            if ($(window.parent.document).find('iframe').length > 0) {
                window.parent.location.href = location.href;
                return;
            }

            logIn.setFocus($('.btn-primary'));
            if ($('#Username').val().length === 0) {
                logIn.setFocus($('#Username'));
            } else {
                logIn.setFocus($('#Password'));
            }

            $('#reload').click(function (e) {
                e.preventDefault();
                var captchaUrl = $(this).attr('href') + "?" + Date.now();
                $('#img-captcha').attr('src', captchaUrl);
            });

            $('form').submit(function () {
                if ($(this).valid()) {
                    // disable button to avoid multiple submit.
                    $('#btnSubmit').attr('disabled', true);
                }
            });
        },
        setFocus: function (element) {
            setTimeout(function () {
                element.focus();
            }, 1);
        },
        showAlert: function (isConfirm, confirmButtonText) {
            var buttons = [];
            buttons.push({
                text: confirmButtonText,
                click: function () {
                    logIn.redirect(true);
                }
            });

            $("#dialog-confirm").dialog({
                resizable: false,
                width: 400,
                modal: true,
                buttons: buttons,
                /*jshint unused:false*/
                close: function (event, ui) {
                    logIn.redirect(false);
                }
            });
        },
        redirect: function (isChangePassword) {
            var url = $("#dialog-confirm").data('href');
            if (isChangePassword) {
                url += '?act=ChangePassword';
            }
            window.location.href = url;
        }
    };
} (jQuery));

$(function () {
    'use strict';
    logIn.init();
});