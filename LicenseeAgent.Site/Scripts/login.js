/*global core, language, popup */

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
        showPasswordAlert: function (message) {
            var title = language.getString('PasswordAlert');
            var buttons = [{
                label: language.getString('ChangePassword'),
                action: function () {
                    logIn.gotoHome();
                }
            }];

            popup.confirm({
                title: title,
                message: message,
                button: buttons,
                onHide: logIn.gotoHome
            });
        },
        gotoHome: function () {
            window.location.href = core.resolveUrl('~/');
        }
    };
} (jQuery));

$(function () {
    'use strict';
    logIn.init();
});