/*global CORE*/
'use strict';

var UNDER_MAINTENANCE_PAGE = (function ($this, $) {
    CORE.DisableRecheckIn = true;

    $this.init = function () {
        if ($(window.parent.document).find('iframe').length > 0) {
            window.parent.location.href = location.href;
            return;
        }

        setTimeout(function () {
            reCheckUM();
        }, 30000);
    };

    var reCheckUM = function () {
        $.ajax({
            type: "POST",
            url: CORE.resolveUrl("~/Notify/UnderMaintenance"),
            dataType: "json",
            async: false,
            timeout: 120000
        }).done(function (result) {
            if (result.ErrCode !== 0) {
                alert(result.ErrMsg);
                setTimeout(function () { reCheckUM(); }, 30000);
                return;
            }

            if (result.Status) {
                setTimeout(function () { reCheckUM(); }, 30000);
            }
            else {
                location.href = CORE.UrlRoot;
            }
        });
    };

    return $this;
} (UNDER_MAINTENANCE_PAGE || {}, jQuery));