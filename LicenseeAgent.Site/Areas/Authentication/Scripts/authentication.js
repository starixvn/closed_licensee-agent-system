/*global CORE, LANGUAGE, POPUP */
/*exported AUTHENTICATION*/
'use strict';

var AUTHENTICATION = (function ($this, $) {
    CORE.DisableRecheckIn = true;

    var setFocus = function (element) {
        setTimeout(function () {
            element.focus();
        }, 1);
    };

    var gotoHome = function () {
        window.location.href = CORE.resolveUrl('~/');
    };

    $this.init = function () {
        if ($(window.parent.document).find('iframe').length > 0) {
            window.parent.location.href = location.href;
            return;
        }

        setFocus($('.btn-primary'));
        if ($('#Username').val().length === 0) {
            setFocus($('#Username'));
        } else {
            setFocus($('#Password'));
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
    };

    $this.showPasswordAlert = function (message) {
        var title = LANGUAGE.getString('PasswordAlert');
        var buttons = [{
            label: LANGUAGE.getString('ChangePassword'),
            action: function () {
                $.cookie('LasTopMenu', 'security', { path: '/' });
                gotoHome();
            }
        }];

        POPUP.confirm({
            title: title,
            message: message,
            buttons: buttons,
            top: 120,
            width: 420,
            onHide: gotoHome
        });
    };

    return $this;
} (AUTHENTICATION || {}, jQuery));