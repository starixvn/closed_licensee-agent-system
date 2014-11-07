/*global language, popup */
'use strict';

var memberArrangement = (function ($this, $) {
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
            label: language.getString('Yes'),
            action: function () {
                isConfirmed = true;
                $('form').submit();
            }
        },
        {
            label: language.getString('No'),
            action: function () {
                popup.close();
            }
        }];

        var checkedMode = getCheckedMode();
        var confirmMessage = checkedMode === arrangementMode.SwitchFromNormal
            ? language.getString('ConfirmSwitchMember')
            : language.getString('ConfirmMemberReArangement');
        popup.confirm({
            title: language.getString('Confirmation'),
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
} (memberArrangement || {}, jQuery));