var resetPassword = function ($) {
    return {
        init: function () {            
            $("#Password").keyup(function (event) {
                if ($("#Password").val() != "") {
                    if (CORE.PASSWORD_FORMAT.test($("#Password").val())) {
                        $("#iconValid").removeClass("icon-input-invalid").addClass("icon-input-valid");
                    } else {
                        $("#iconValid").removeClass("icon-input-valid").addClass("icon-input-invalid");
                    }
                }
            });
        }
    }
} (jQuery);
$(function () {
    resetPassword.init();
});