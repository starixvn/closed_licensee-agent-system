var MESSAGE = (function ($) {
    return {
     initialize: function() {
            $('#fromDatePicker').datepicker();
            $('#toDatePicker').datepicker();
        },
    }
} (jQuery));
$(function () {
    MESSAGE.initialize();
});