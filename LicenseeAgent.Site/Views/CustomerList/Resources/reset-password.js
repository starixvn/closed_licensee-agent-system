/*global core */
var resetPassword = (function($) {
    'use strict';
    return {
        init: function() {
            $("#Password").keyup(function() {
                if ($("#Password").val() !== "") {
                    if (core.PASSWORD_FORMAT.test($("#Password").val())) {
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
}(jQuery));

$(function() {
    'use strict';
    resetPassword.init();
});