var TRANSFER = function ($) {
    return {
        init: function () {
            $('.selectpicker').selectpicker();

            $('#search-form').submit(function (e) {
                TRANSFER.reRenderAfterPaged();
                e.preventDefault();
            });
        },
        reRenderAfterPaged: function () {
        }
    }
} (jQuery);

$(function () {
    TRANSFER.init();
});