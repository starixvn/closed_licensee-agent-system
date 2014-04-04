var resetPassword = (function ($) {
    'use strict';
    return {
        init: function () {
            var passwordRegex = new RegExp($("#Password").data('regex'));
            $("#Password").keyup(function () {
                if ($("#Password").val() !== "") {
                    if (passwordRegex.test($("#Password").val())) {
                        $("#iconValid").removeClass("icon-input-invalid")
                            .addClass("icon-input-valid");
                    } else {
                        $("#iconValid").removeClass("icon-input-valid")
                            .addClass("icon-input-invalid");
                    }
                }
            });
        }
    };
} (jQuery));

$(function() {
    'use strict';
    resetPassword.init();
});