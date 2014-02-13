var TRANSFER_POPUP = function ($) {
    var minAmount;
    var maxAmount;
    return {
        init: function () {
            minAmount = parseFloat($('#Amount').data('min'));
            maxAmount = parseFloat($('#Amount').data('max'));

            $('#Amount').tooltip({
                placement: 'top',
                trigger: 'manual'
            })
            .autoNumeric('init', {
                mDec: Math.max(minAmount.countDecimals(), maxAmount.countDecimals()),
                vMin: Number.MinInt,
                vMax: Number.MaxInt,
                lZero: 'deny',
                wEmpty: 'zero',
                aPad: false
            })
            .on('keyup', function () {
                TRANSFER_POPUP.validateAmount(this);
            });

            $("form").validate().cancelSubmit = true;
            $('form').submit(function (e) {
                if (!TRANSFER_POPUP.validateAmount($('#Amount'))) {
                    e.preventDefault();
                }
                else {
                    $('#Amount').val($('#Amount').autoNumeric('get'));
                }
            });
        },
        validateAmount: function (target) {
            var newAmount = $(target).autoNumeric('get');
            if (newAmount < minAmount || newAmount > maxAmount) {
                $(target).tooltip('show');
                return false;
            }
            else {
                $(target).tooltip('hide');
                return true;
            }
        }
    }
} (jQuery);

$(function () {
    TRANSFER_POPUP.init();
});