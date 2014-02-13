var HomePage = function ($) {
    return {
        init: function () {
            $('.dropdown-button').click(function () {
                var element = $(this);
                var popup = element.next();
                if (popup.is(":visible")) {
                    element.removeClass('icon-account-collapse').addClass('icon-account-expand');
                    popup.hide();
                    return;
                }
                $('.dropdown-popup').hide();
                $('.dropdown-button').removeClass('icon-account-collapse').addClass('icon-account-expand');
                element.addClass('icon-account-collapse');
                var url = element.attr('href');
                $.ajax({
                    url: url,
                    type: 'get',
                    beforeSend: function () {
                        CORE.blockUI();
                    },
                    success: function (data) {
                        popup.html(data);
                        popup.show();
                    },
                    complete: function () {
                        CORE.unblockUI();
                    }
                });
            });
        }
    };
} (jQuery);
$(function () {
    HomePage.init();
});