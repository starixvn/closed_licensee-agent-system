/* global CORE*/
'use strict';

var ADMIN = ADMIN || {};
ADMIN.MAINTENANCE = ADMIN.MAINTENANCE || {};
ADMIN.MAINTENANCE.PASSWORD = (function ($this, $) {
    $this.init = function () {
        $("#encode").click(function () {
            CORE.ajaxJSON({
                url: $(this).data("url"),
                data: {
                    password: $("#password").val()
                }
            }, function (data) {
                $("#encodepassword").val(data.Value);
            });
            return false;
        });

        $("#decode").click(function () {
            CORE.ajaxJSON({
                url: $(this).data("url"),
                data: {
                    encodedpassword: $("#encodepassword").val()
                }
            }, function (data) {
                if (data.ErrCode !== 0) {
                    alert(data.ErrMsg);
                }
                $("#password").val(data.Value);
            });
            return false;
        });
    };

    return $this;
} (ADMIN.MAINTENANCE.PASSWORD || {}, jQuery));