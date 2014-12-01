/*exported CREDIT_BALANCE_POPUP*/
'use strict';

var CREDIT_BALANCE_POPUP = (function ($this, $) {
    var minAmount;
    var maxAmount;
    var validateAmount = function (target) {
        var newAmount = $(target).autoNumeric('get');
        if (newAmount < minAmount || (!isNaN(maxAmount) && newAmount > maxAmount)) {
            $(target).tooltip('show');
            return false;
        }
        else {
            $(target).tooltip('hide');
            return true;
        }
    };

    $this.init = function () {
        minAmount = parseFloat($('#GivenCredit').data('min'));
        maxAmount = parseFloat($('#GivenCredit').data('max'));

        $('#GivenCredit').tooltip({
            placement: 'top',
            trigger: 'manual'
        }).autoNumeric('init', {
            mDec: 0,
            vMin: 0,
            vMax: 999999999999,
            lZero: 'deny',
            wEmpty: 'zero',
            aPad: false
        }).on('keyup', function () {
            validateAmount(this);
        });

        $("form").validate().cancelSubmit = true;
        $('form').submit(function (e) {
            if (!validateAmount($('#GivenCredit'))) {
                e.preventDefault();
            }
            else {
                $('#GivenCredit').val($('#GivenCredit').autoNumeric('get'));
            }
        });
    };

    return $this;
} (CREDIT_BALANCE_POPUP || {}, jQuery));