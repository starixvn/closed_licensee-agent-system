var TRANSFER_POPUP = function ($) {
    var minAmount;
    var maxAmount;
    var msgAmount;
    return {
        init: function () {
            $("form").validate().resetForm();

            minAmount = parseFloat($('#Amount').attr('data-val-range-min'));
            maxAmount = parseFloat($('#Amount').attr('data-val-range-max'));
            msgAmount = $('#Amount').attr('data-val-range');

            var poshytipOptions = {
                content: msgAmount,
                alignTo: 'target',
                alignX: 'inner-right',
                offsetY: 5,
                showOn: 'none',
                showTimeout: 0,
                className: 'tip-twitter'
            }
            $('#Amount').poshytip(poshytipOptions)
            .autoNumeric('init', {
                mDec: Math.max(minAmount.countDecimals(), maxAmount.countDecimals()),
                vMin: Number.MinInt,
                vMax: Number.MaxInt,
                lZero: 'deny',
                wEmpty: 'zero'
            })
            .on('keyup', function () {
                TRANSFER_POPUP.validateAmount(this);
            });

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
                $(target).poshytip('show');
                return false;
            }
            else {
                $(target).poshytip('hide');
                return true;
            }
        }
    }
} (jQuery);

$(function () {
    TRANSFER_POPUP.init();
});