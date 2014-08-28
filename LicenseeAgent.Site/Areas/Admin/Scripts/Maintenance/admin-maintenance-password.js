var admin = admin || {};
admin.maintenance = admin.maintenance || {};
admin.maintenance.password = (function ($) {
    'use strict';

    return {
        init: function () {
            $("#encode").click(function () {
                $.ajax({
                    type: "POST",
                    url: $(this).data("url"),
                    data: { password: $("#password").val() }
                })
            .done(function (data) {
                if (data.ErrCode !== 0) {
                    alert(data.ErrMsg);
                }
                $("#encodepassword").val(data.Value);
            });
                return false;
            });
            $("#decode").click(function () {
                $.ajax({
                    type: "POST",
                    url: $(this).data("url"),
                    data: { encodedpassword: $("#encodepassword").val() }
                })
            .done(function (data) {
                if (data.ErrCode !== 0) {
                    alert(data.ErrMsg);
                }
                $("#password").val(data.Value);
            });
                return false;
            });
        }
    };
} (jQuery));