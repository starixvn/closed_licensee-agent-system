var PAGE = function ($) {
    return {
        init: function () {
            // call if page is not in frame & not be a popup
            // note: don't put this file in login page.
            if (window.parent.frmMain == undefined && window.parent.popup == undefined) {
                setTimeout(function () { CORE.reCheckIn(); }, 1000);
            }
        }
    }
} (jQuery);

// Shorthand for $( document ).ready()
$(function () {
    CORE.blockUI();
    PAGE.init();
});

$(window).load(function () {
    CORE.unblockUI();
});