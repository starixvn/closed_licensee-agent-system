/*global language, core, pageContext, popup*/

var CommissionGroupUtility = (function ($) {
    'use strict';
    return {
        isConfirm: false,
        init: function () {
            $('#check').click(function () {
                CommissionGroupUtility.checkAvailability();
            });

            $('form').submit(function () {
                if ($('#IsDefault').is(":checked") && !CommissionGroupUtility.isConfirm) {
                    if ($('#action').val() === "edit") {
                        if ($('#GroupCommId').val() !== $('#DefaultGroupId').val() && $('#DefaultGroupId').val() !== "0") {
                            CommissionGroupUtility.confirmDefaultGroup();
                            return CommissionGroupUtility.isConfirm;
                        }
                    }
                    else if ($('#DefaultGroupId').val() !== "0") {
                        CommissionGroupUtility.confirmDefaultGroup();
                        return CommissionGroupUtility.isConfirm;
                    }
                }
            });

            $('.product-comm').change(function () {
                var idComm2 = '#' + $(this).data("comm2");
                var idComm3 = '#' + $(this).data("comm3");

                if ($(this).val() === "15") {
                    $(idComm2).val("0");
                    $(idComm2).prop('disabled', 'disabled');
                    $(idComm3).val("0");
                    $(idComm3).prop('disabled', 'disabled');
                }
                else {
                    $(idComm2).prop('disabled', false);
                    $(idComm3).prop('disabled', false);
                }
            });

            $('.product-comm').change();
        },
        checkAvailability: function () {
            var validator = { errorList: [] };
            if (!$.trim($('#GroupName').val())) {
                validator.errorList.push({ message: language.getString('GroupNameIsEmpty') });
            }
            if (validator.errorList.length > 0) {
                $('#iconCheck').removeClass('icon-input-valid').addClass('icon-input-invalid');
                core.showErrors(validator);
                return;
            }

            $.ajax({
                url: core.resolveUrl("~/CommissionGroup/CheckAvailability"),
                type: 'post',
                data: 'groupName=' + $('#GroupName').val(),
                beforeSend: function () {
                    core.blockUI();
                },
                success: function (data) {
                    if (data.ErrCode === 0) {
                        $('#iconCheck').removeClass('icon-input-invalid').addClass('icon-input-valid');
                        core.hideErrors();
                    }
                    else {
                        $('#iconCheck').removeClass('icon-input-valid').addClass('icon-input-invalid');
                        core.showErrorMsg(data.ErrMsg);
                    }
                },
                complete: function () {
                    core.unblockUI();
                }
            });
        },
        ViewDetail: function (agentId, userName) {
            var url = core.resolveUrl('~/Report/Statement/Detail', {
                agentId: agentId,
                fromDate: $('#FromDate').val(),
                toDate: $('#ToDate').val(),
                userName: userName,
                isMember: pageContext.IsLastLevel,
                parentUsername: $('#Username').val()
            });
            location.href = url;
        },
        confirmDefaultGroup: function () {
            var buttons = [{
                label: language.getString('Yes'),
                action: function () {
                    $('#IsResetDefault').val("true");
                    CommissionGroupUtility.isConfirm = true;
                    popup.close();
                    $('#submit').click();
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
                buttons: buttons,
                top: 40,
                width: 420
            });
        }
    };
})(jQuery);

$(function () {
    'use strict';
    CommissionGroupUtility.init();
});