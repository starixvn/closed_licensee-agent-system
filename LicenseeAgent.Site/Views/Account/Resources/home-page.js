/*global core */
var homePage = (function($) {
    'use strict';
    return {
        init: function() {
            $('.dropdown-button').click(function() {
                var element = $(this),
                    popup = element.next(),
                    url = element.attr('href');
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
                    beforeSend: function() {
                        core.blockUI();
                    },
                    success: function(data) {
                        popup.html(data);
                        popup.show();
                    },
                    complete: function() {
                        core.unblockUI();
                    }
                });
            });
        }
    };
}(jQuery));
$(function() {
    'use strict';
    homePage.init();
});