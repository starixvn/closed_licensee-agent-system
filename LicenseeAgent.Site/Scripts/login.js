var LOGIN = function ($) {
    return {
        init: function () {
            LOGIN.setFocus($('.btn-primary'));
            if ($('#Username').val().length == 0) {
                LOGIN.setFocus($('#Username'));
            } else {
                LOGIN.setFocus($('#Password'));
            }

            $('#reload').click(function () {
                var captchaUrl = CORE.resolveUrl($(this).attr('data-href')) + "?" + Date.now();
                $('#img-captcha').attr('src', captchaUrl);
            });
        },
        setFocus: function (element) {
            setTimeout(function () {
                element.focus();
            }, 1);
        },
        showAlert: function (isConfirm, confirmButtonText) {
            var buttons = new Array();
            buttons.push({
                text: confirmButtonText,
                click: function () {
                    LOGIN.redirect(true);
                }
            });

            $("#dialog-confirm").dialog({
                resizable: false,
                width: 400,
                modal: true,
                buttons: buttons,
                close: function (event, ui) {
                    LOGIN.redirect(false);
                }
            });
        },
        redirect: function (isChangePassword) {
            var url = $("#dialog-confirm").data('href');
            if (isChangePassword) {
                url += '?act=ChangePassword';
            }
            window.location.href = CORE.resolveUrl(url);
        }
    }
} (jQuery);

$(function () {
    LOGIN.init();
});