/*global CORE, LANGUAGE, POPUP*/
'use strict';

var COMMISSION_SETTING = COMMISSION_SETTING || {};
COMMISSION_SETTING.GROUP_CRUD = (function ($this, $) {
    var isConfirmed = false;

    var confirmDefaultGroup = function () {
        var buttons = [{
            label: LANGUAGE.getString('Yes'),
            action: function () {
                POPUP.close();
                isConfirmed = true;
                $('button[type=submit]').click();
            }
        },
        {
            label: LANGUAGE.getString('No'),
            action: function () {
                POPUP.close();
            }
        }
        ];

        POPUP.confirm({
            title: LANGUAGE.getString('Confirmation'),
            message: LANGUAGE.getString('ConfirmDefaultGroup'),
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

        COMMISSION_SETTING.initProductCommissionSelect();
    };

    $this.checkAvailability = function () {
        var validator = { errorList: [] };
        if (!$.trim($('#GroupName').val())) {
            validator.errorList.push({ message: LANGUAGE.getString('GroupNameIsEmpty') });
        }
        if (validator.errorList.length > 0) {
            $('#iconCheck').removeClass('icon-input-valid').addClass('icon-input-invalid');
            CORE.showErrors(validator);
            return;
        }

        CORE.ajaxJSON({
            url: CORE.resolveUrl("~/CommissionSetting/GroupList/CheckGroupAvailability"),
            data: {
                groupName: $('#GroupName').val()
            }
        }, function () {
            $('#iconCheck').removeClass('icon-input-invalid').addClass('icon-input-valid');
            CORE.hideErrors();
        }, function (data) {
            CORE.showErrorMsg(data.ErrMsg);
        });
    };

    return $this;
} (COMMISSION_SETTING.GROUP_CRUD || {}, jQuery));