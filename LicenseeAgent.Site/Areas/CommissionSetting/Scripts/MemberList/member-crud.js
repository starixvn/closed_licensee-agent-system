'use strict';

var commissionSetting = commissionSetting || {};
commissionSetting.memberCrud = (function ($this, $) {
    $this.moveToGroup = function () {
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

    $this.updateMemberCommission = function () {
        commissionSetting.initProductCommissionSelect();
    };

    return $this;
} (commissionSetting.memberCrud || {}, jQuery));