var transferPopUp = (function ($) {
    'use strict';
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
                mDec: 4,
                vMin: Number.MinInt,
                vMax: Number.MaxInt,
                lZero: 'deny',
                wEmpty: 'zero',
                aPad: false
            })
            .on('keyup', function () {
                transferPopUp.validateAmount(this);
            });

            $("form").validate().cancelSubmit = true;
            $('form').submit(function (e) {
                if (!transferPopUp.validateAmount($('#Amount'))) {
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
    };
} (jQuery));

$(function () {
    'use strict';
    transferPopUp.init();
});