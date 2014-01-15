var resetPassword = function ($) {
    return {
        init: function () {
            $('form').submit(function () {
                resetPassword.updatePassword();
                return false;
            });
            alert("hehe");
        },
        updatePassword: function () {
            var params = {
                CustId: $("#CustId").val(),
                Password: $("#Password").val(),
                ConfirmPassword: $("#ConfirmPassword").val()

            };

            $.ajax({
                url: CORE.resolveUrl("~/Customer/ResetPasswordAgent"),
                type: 'post',
                data: params,
                beforeSend: function () {
                    CORE.blockUI();
                },
                success: function (data) {
                    if (data.ErrCode != 0) {
                        alert(data.ErrMsg);
                    }
                },
                complete: function () {
                    CORE.unblockUI();
                }
            });
        }
    }
} (jQuery);
$(function () {
    resetPassword.init();
});