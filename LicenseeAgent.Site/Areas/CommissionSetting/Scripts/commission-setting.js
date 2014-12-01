/*exported COMMISSION_SETTING*/
'use strict';

var COMMISSION_SETTING = (function ($this, $) {
    $this.initProductCommissionSelect = function () {
        $('[data-commission-product-id] select').change(function () {
            var parent = $(this).parent();
            var productId = parent.data('commission-product-id');
            var index = parent.data('index');
            var selectComm1 = $('#comm1_' + productId);
            var selectComm2 = $('#comm2_' + productId);
            var selectComm3 = $('#comm3_' + productId);

            if (selectComm1[0] === this) {
                if ($(this).val() === "15") {
                    selectComm2.val("0");
                    selectComm2.prop('disabled', true);
                    selectComm3.val("0");
                    selectComm3.prop('disabled', true);
                }
                else {
                    selectComm2.prop('disabled', false);
                    selectComm3.prop('disabled', false);
                }
            }

            var commission = parseFloat(selectComm1.val() + '.' + selectComm2.val() + selectComm3.val()) * 100 / 10000;
            var name = 'ProductCommissions[{0}].Commission'.format(index);
            $('input[name="{0}"]'.format(name)).val(commission);
        });

        $('[data-commission-product-id]').find('select:eq(0)').change();
    };

    return $this;
} (COMMISSION_SETTING || {}, jQuery));