var TRANSFER_POPUP = function ($) {
    return {
        init: function () {
            var minAmount = parseFloat($('#Amount').attr('data-val-range-min'));
            var maxAmount = parseFloat($('#Amount').attr('data-val-range-max'));
            $('form').validate({
                rules: {
                    Amount: {
                        required: true,
                        range: [minAmount, maxAmount]
                    }
                },
                invalidHandler: function (e, validator) {
                    alert('test');
                },
                submitHandler: function (form) { // for demo
                    alert('valid form');
                    return false;
                },
                onfocusout: true
            });
            $('form').submit(function (e) {
                //e.preventDefault();
            });
        }
    }
} (jQuery);

$(function () {
    TRANSFER_POPUP.init();
});