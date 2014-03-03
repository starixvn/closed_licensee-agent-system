var creditBalancePopup = function ($) {
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
                mDec: 4,
                vMin: Number.MinInt,
                vMax: Number.MaxInt,
                lZero: 'deny',
                wEmpty: 'zero',
                aPad: false
            })
            .on('keyup', function () {
                creditBalancePopup.validateAmount(this);
            });

            $("form").validate().cancelSubmit = true;
            $('form').submit(function (e) {
                if (!creditBalancePopup.validateAmount($('#GivenCredit'))) {
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
    creditBalancePopup.init();
});