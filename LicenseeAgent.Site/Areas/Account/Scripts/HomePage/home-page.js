/*global core */
'use strict';

var account = account || {};
account.homePage = (function ($this, $) {
    $this.init = function () {
        $('.dropdown-button').click(function () {
            var element = $(this),
                    popup = element.next(),
                    url = element.data('href');
            if (popup.is(":visible")) {
                element.removeClass('icon-account-collapse')
                        .addClass('icon-account-expand');
                popup.hide();
                return;
            }
            $('.dropdown-popup').hide();
            $('.dropdown-button').removeClass('icon-account-collapse')
                    .addClass('icon-account-expand');
            element.addClass('icon-account-collapse');
            $.ajax({
                url: url,
                type: 'get',
                beforeSend: function () {
                    core.blockUI();
                },
                success: function (data) {
                    popup.html(data);
                    popup.show();
                    if (popup.height() < popup.children().height()) {
                        popup.width(popup.width() + core.getScrollBarWidth());
                    }
                },
                complete: function () {
                    core.unblockUI();
                }
            });
        });
    };

    return $this;
} (account.homePage || {}, jQuery));