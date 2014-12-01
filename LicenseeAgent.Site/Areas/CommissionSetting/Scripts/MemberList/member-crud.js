'use strict';

var COMMISSION_SETTING = COMMISSION_SETTING || {};
COMMISSION_SETTING.MEMBER_CRUD = (function ($this, $) {
    $this.initMoveToGroup = function () {
        $('.selectpicker').selectpicker();

        $('#GroupCommId').change(function () {
            var selectedOption = $(this).find('option:selected');

            if ($(selectedOption).val() === '') {
                $('input[type=text]').val('');
                return;
            }

            $('input[type=text]').each(function () {
                var name = $(this).attr('name').toLowerCase();
                var comm = selectedOption.data(name);
                $(this).val(comm);
            });
        });
    };

    $this.initUpdateMemberCommission = function () {
        COMMISSION_SETTING.initProductCommissionSelect();
    };

    return $this;
} (COMMISSION_SETTING.MEMBER_CRUD || {}, jQuery));