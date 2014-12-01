/*global CORE */
'use strict';

var ACCOUNT = ACCOUNT || {};
ACCOUNT.HOMEPAGE = (function ($this, $) {
    $this.init = function () {
        $('.dropdown-button').click(function () {
            var element = $(this),
                popup = element.next(),
                url = element.data('href');
            if (popup.is(":visible")) {
                element.removeClass('icon-account-collapse').addClass('icon-account-expand');
                popup.hide();
                return;
            }
            $('.dropdown-popup').hide();
            $('.dropdown-button').removeClass('icon-account-collapse').addClass('icon-account-expand');
            element.addClass('icon-account-collapse');
            CORE.ajaxHTML({
                url: url
            }, function (response) {
                popup.html(response);
                popup.show();
                if (popup.height() < popup.children().height()) {
                    popup.width(popup.width() + CORE.getScrollBarWidth());
                }
            });
        });
    };

    return $this;
} (ACCOUNT.HOMEPAGE || {}, jQuery));