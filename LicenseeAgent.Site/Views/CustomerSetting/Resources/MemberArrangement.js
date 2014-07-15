/*global core, language, popup */

var MemberArrangement = (function ($) {
    'use strict';
    return {
        isConfirm: false,
        init: function () {
            MemberArrangement.switchMode();
            $("form input:radio").click(function () {
                MemberArrangement.switchMode();
            });

            $('form').submit(function () {
                if (!MemberArrangement.isConfirm && MemberArrangement.validateForm()) {
                    MemberArrangement.confirmSubmit();
                }
                return MemberArrangement.isConfirm;
            });
        },

        switchMode: function () {
            core.hideErrors();
            if ($('input[name=Mode]:checked', 'form').val() === "0") {
                $('.normalCash').show();
                $('.assignMember').hide();
            }
            else {
                $('.normalCash').hide();
                $('.assignMember').show();
            }
        },

        validateForm: function () {
            var validator = { errorList: [] };
            if (!$.trim($('#MemberName').val())) {
                validator.errorList.push({ message: language.getString('MemberIsEmpty') });
            }
            if ($('input[name=Mode]:checked', 'form').val() === "0") {
                if (!$.trim($('#AgentName').val())) {
                    validator.errorList.push({ message: language.getString('LegengAgentIsEmpty') });
                }
            }
            else {
                if (!$.trim($('#OldAgentName').val())) {
                    validator.errorList.push({ message: language.getString('OldAgentIsEmpty') });
                }
                if (!$.trim($('#NewAgentName').val())) {
                    validator.errorList.push({ message: language.getString('NewAgentIsEmpty') });
                }
            }

            core.showErrors(validator);
            return (validator.errorList.length === 0);
        },

        confirmSubmit: function () {
            var buttons = [{
                label: language.getString('Yes'),
                action: function () {
                    MemberArrangement.isConfirm = true;
                    $('form').submit();
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
                message: language.getString('ConfirmMemberReArangement'),
                buttons: buttons,
                top: 120,
                width: 420
            });

        }
    };
} (jQuery));

$(function () {
    'use strict';
    MemberArrangement.init();
});