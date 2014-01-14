var REPORT = function ($) {
    return {
        init: function () {
            $("#filter-tabs").tabs();
        }
    }
} (jQuery);

$(function () {
    REPORT.init();
});