/*global language, core, pageContext, popup*/

var CommissionGroupUtility = (function ($) {
    'use strict';
    return {
        init: function () {
            $('#check').click(function () {
                CommissionGroupUtility.checkAvailability();
            });

            $('#cancel').click(function () {
                popup.close();
            });

            $("#submit").click(function () {
                if ($('#IsDefault').is(":checked")) {
                    CommissionGroupUtility.checkDefaultGroup();
                    return false;
                }
                else {
                    return true;
                }
            });
        },
        checkAvailability: function () {
            var validator = { errorList: [] };
            if (!$.trim($('#GroupName').val())) {
                validator.errorList.push({ message: language.getString('GroupNameIsEmpty') });
            }
            if (validator.errorList.length > 0) {
                core.showErrors(validator);
                return;
            }
            else {
                core.hideErrors();
            }

            $.ajax({
                url: core.resolveUrl("~/CommissionGroup/CheckAvailability"),
                type: 'post',
                data: 'groupName=' + $('#GroupName').val(),
                beforeSend: function () {
                    core.blockUI();
                },
                success: function (data) {
                    if (data.IsAvailable) {
                        $('#iconCheck').removeClass('icon-input-invalid').addClass('icon-input-valid');
                    }
                    else {
                        $('#iconCheck').removeClass('icon-input-valid').addClass('icon-input-invalid');
                    }
                },
                complete: function () {
                    core.unblockUI();
                }
            });
        },
        ViewDetail: function (agentId, username) {
            location.href = core.resolveUrl("~/Report/StatementDetail?agentId={0}&fromDate={1}&toDate={2}&username={3}&isMember={4}&parentUsername={5}"
                                            .format(agentId, $('#FromDate').val(), $('#ToDate').val(), username,
                                                    pageContext.IsLastLevel, $('#Username').val()));
        },
        checkDefaultGroup: function () {
            CommissionGroupUtility.confirmDefaultGroup();
        },
        confirmDefaultGroup: function () {
            var buttons = [{
                label: language.getString('Yes'),
                action: function () {
                    $('#actionForm').submit();
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