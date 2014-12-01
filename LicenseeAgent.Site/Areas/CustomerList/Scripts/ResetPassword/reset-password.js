'use strict';

var CUSTOMER_LIST = CUSTOMER_LIST || {};
CUSTOMER_LIST.RESET_PASSWORD = (function ($this, $) {
    $this.init = function () {
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
    };

    return $this;
} (CUSTOMER_LIST.RESET_PASSWORD || {}, jQuery));