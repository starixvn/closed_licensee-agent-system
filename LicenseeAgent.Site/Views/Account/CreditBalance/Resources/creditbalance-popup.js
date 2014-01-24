var TRANSFER_POPUP = function ($) {
    var minAmount;
    var maxAmount;
    return {
        init: function () {
            minAmount = parseFloat($('#GivenCredit').data('min'));
            maxAmount = parseFloat($('#GivenCredit').data('max'));

            $('#GivenCredit').tooltip({
                placement: 'top',
                trigger: 'manual'
            })
            .autoNumeric('init', {
                mDec: Math.max(minAmount.countDecimals(), !isNaN(maxAmount) ? maxAmount.countDecimals() : minAmount.countDecimals()),
                vMin: Number.MinInt,
                vMax: Number.MaxInt,
                lZero: 'deny',
                wEmpty: 'zero'
            })
            .on('keyup', function () {
                TRANSFER_POPUP.validateAmount(this);
            });

            $("form").validate().cancelSubmit = true;
            $('form').submit(function (e) {
                if (!TRANSFER_POPUP.validateAmount($('#GivenCredit'))) {
                    e.preventDefault();
                }
                else {
                    $('#GivenCredit').val($('#GivenCredit').autoNumeric('get'));
                }
            });
        },
        validateAmount: function (target) {
            var newAmount = $(target).autoNumeric('get');
            if (newAmount < minAmount || (!isNaN(maxAmount) && newAmount > maxAmount)) {
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