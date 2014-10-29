/*global core, language, popup */

var authentication = (function ($) {
    'use strict';
    core.DisableRecheckIn = true;
    return {
        init: function () {
            if ($(window.parent.document).find('iframe').length > 0) {
                window.parent.location.href = location.href;
                return;
            }

            authentication.setFocus($('.btn-primary'));
            if ($('#Username').val().length === 0) {
                authentication.setFocus($('#Username'));
            } else {
                authentication.setFocus($('#Password'));
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
                    $.cookie('LasTopMenu', 'security', { path: '/' });
                    authentication.gotoHome();
                }
            }];

            popup.confirm({
                title: title,
                message: message,
                buttons: buttons,
                top: 120,
                width: 420,
                onHide: authentication.gotoHome
            });
        },
        gotoHome: function () {
            window.location.href = core.resolveUrl('~/');
        }
    };
} (jQuery));