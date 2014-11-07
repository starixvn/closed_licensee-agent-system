/*global language, core, popup*/
'use strict';

var commissionSetting = commissionSetting || {};
commissionSetting.groupCrud = (function ($this, $) {
    var isConfirmed = false;

    var confirmDefaultGroup = function () {
        var buttons = [{
            label: language.getString('Yes'),
            action: function () {
                popup.close();
                isConfirmed = true;
                $('button[type=submit]').click();
            }
        },
        {
            label: language.getString('No'),
            action: function () {
                popup.close();
            }
        }
        ];

        popup.confirm({
            title: language.getString('Confirmation'),
            message: language.getString('ConfirmDefaultGroup'),
            buttons: buttons
        });
    };

    $this.init = function () {
        $('#check').click(function () {
            $this.checkAvailability();
        });

        $('form').submit(function (e) {
            var isShowConfirm = false;
            if ($('#IsDefault').is(":checked") && !isConfirmed) {
                if ($('#DefaultGroupId').length === 0 || $('#DefaultGroupId').val() !== $('#GroupCommId').val()) {
                    isShowConfirm = true;
                }
            }

            if (isShowConfirm) {
                confirmDefaultGroup();
                e.preventDefault();
            }
            else {
                isConfirmed = false;
            }
        });

        commissionSetting.initProductCommissionSelect();
    };

    $this.checkAvailability = function () {
        var validator = { errorList: [] };
        if (!$.trim($('#GroupName').val())) {
            validator.errorList.push({ message: language.getString('GroupNameIsEmpty') });
        }
        if (validator.errorList.length > 0) {
            $('#iconCheck').removeClass('icon-input-valid').addClass('icon-input-invalid');
            core.showErrors(validator);
            return;
        }

        core.ajaxJSON({
            url: core.resolveUrl("~/CommissionSetting/GroupList/CheckGroupAvailability"),
            data: {
                groupName: $('#GroupName').val()
            }
        }, function () {
            $('#iconCheck').removeClass('icon-input-invalid').addClass('icon-input-valid');
            core.hideErrors();
        }, function (data) {
            core.showErrorMsg(data.ErrMsg);
        });
    };

    return $this;
} (commissionSetting.groupCrud || {}, jQuery));