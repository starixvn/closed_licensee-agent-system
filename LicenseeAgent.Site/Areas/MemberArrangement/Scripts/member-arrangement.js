/*global LANGUAGE, POPUP*/
/*exported MEMBER_ARRANGEMENT*/
'use strict';

var MEMBER_ARRANGEMENT = (function ($this, $) {
    var isConfirmed = false;
    var arrangementMode = {
        SwitchFromNormal: 0,
        AssignMember: 1
    };

    var getCheckedMode = function () {
        var checkedRadio = $('form input:radio:checked');
        return parseInt(checkedRadio.val());
    };

    var switchMode = function (arrangementMode) {
        $('tr[data-mode]').each(function () {
            var mode = parseInt($(this).data('mode'));
            if (mode === arrangementMode) {
                $(this).show();
            } else {
                $(this).hide();
            }
        });
    };

    var confirmSubmit = function () {
        var buttons = [{
            label: LANGUAGE.getString('Yes'),
            action: function () {
                isConfirmed = true;
                $('form').submit();
            }
        },
        {
            label: LANGUAGE.getString('No'),
            action: function () {
                POPUP.close();
            }
        }];

        var checkedMode = getCheckedMode();
        var confirmMessage = checkedMode === arrangementMode.SwitchFromNormal
            ? LANGUAGE.getString('ConfirmSwitchMember')
            : LANGUAGE.getString('ConfirmMemberReArangement');
        POPUP.confirm({
            title: LANGUAGE.getString('Confirmation'),
            message: confirmMessage,
            buttons: buttons,
            width: 400
        });
    };

    $this.init = function () {
        $('form input:radio').change(function () {
            var validator = $('form').data('validator');
            validator.resetForm();
            $(this).prop('checked', true);
            var mode = parseInt($(this).val());
            switchMode(mode);
        });

        $('form').find('input[data-message]').each(function () {
            var message = $(this).data('message');
            $(this).rules('add', {
                required: true,
                messages: {
                    required: message
                }
            });
        });

        $('form').submit(function () {
            var validator = $(this).data('validator');
            if (!isConfirmed && validator.form()) {
                confirmSubmit();
            }
            return isConfirmed;
        });

        var checkedMode = getCheckedMode();
        switchMode(checkedMode);
    };

    return $this;
} (MEMBER_ARRANGEMENT || {}, jQuery));