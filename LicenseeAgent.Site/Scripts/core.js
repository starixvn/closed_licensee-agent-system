/*global POPUP, appSettings, pageContext */
'use strict';

var CORE = (function ($this, $) {
    var scrollBarWidth = 0;
    $this.DisableRecheckIn = false;
    $this.UrlRoot = appSettings.UrlRoot;
    $this.ShortDateFormat = "MM/DD/YYYY";
    $this.FullDateFormat = "MM/DD/YYYY hh:mm:ss A";
    $this.ReloadInterval = 5000;
    $this.ReplicationInterval = 5000;
    $this.ServerTime = appSettings.ServerTime;
    $this.ServerDate = appSettings.ServerDate;
    $this.ServerUtcOffset = appSettings.ServerUtcOffset;
    $this.Console = {
        write: function (msg, consoleFunc) {
            if (typeof console !== 'undefined' && !$this.detectBrowser().msie) {
                consoleFunc(msg);
            }
        },
        log: function (msg) { this.write(msg, console.log); },
        info: function (msg) { this.write(msg, console.info); },
        warn: function (msg) { this.write(msg, console.warn); },
        error: function (msg) { this.write(msg, console.error); }
    };

    $this.getScrollBarWidth = function () {
        if (scrollBarWidth === 0) {
            var inner = document.createElement('p');
            inner.style.width = "100%";
            inner.style.height = "200px";

            var outer = document.createElement('div');
            outer.style.position = "absolute";
            outer.style.top = "0px";
            outer.style.left = "0px";
            outer.style.visibility = "hidden";
            outer.style.width = "200px";
            outer.style.height = "150px";
            outer.style.overflow = "hidden";
            outer.appendChild(inner);

            document.body.appendChild(outer);
            var w1 = inner.offsetWidth;
            outer.style.overflow = 'scroll';
            var w2 = inner.offsetWidth;
            if (w1 === w2) {
                w2 = outer.clientWidth;
            }

            document.body.removeChild(outer);
            scrollBarWidth = w1 - w2;
        }

        return scrollBarWidth;
    };

    $this.init = function () {
        // call if page is not in frame & not be a popup
        if (window.parent.frmMain === undefined && !POPUP.isIframe() && !$this.DisableRecheckIn) {
            setTimeout(function () {
                $this.reCheckIn();
            }, 1000);
        }
    };

    $this.initInstruction = function () {
        var containers = $('.instruction-container');
        containers.each(function () {
            var $this = $(this),
                thisId = $this.attr('id'),
                title = $this.attr('title'),
                trigger;
            trigger = thisId === 'page-info'
                ? $('<div class="page-info" />').appendTo('#header_main')
                : $('#' + thisId.substr(0, thisId.length - 5)); // 5 is the length of "-info";

            if (trigger.length > 0) {
                trigger.tooltip({
                    position: { my: "right-10 center", at: "center center+5" },
                    show: null,
                    hide: null
                }).click(function () {
                    POPUP.open({
                        title: title,
                        message: $this.html(),
                        top: 100,
                        clickout: true
                    });
                });
            }
        });
    };

    $this.reCheckIn = function () {
        $.ajax({
            type: "POST",
            url: $this.resolveUrl("~/Account/ReCheckIn"),
            dataType: "json",
            async: true,
            timeout: 120000
        }).done(function (result) {
            if (result.ErrCode !== 0) {
                if (result.Reload) {
                    location.reload();
                    return;
                }

                alert(result.ErrMsg);
                location.href = CORE.UrlRoot;
            } else {
                setTimeout(function () {
                    $this.reCheckIn();
                }, 30000);
            }
        });
    };

    $this.resolveUrl = function (url, queryParams) {
        if (url.substr(0, 2) === '~/') {
            url = url.substr(2);
        } else if (url.substr(0, 1) === '/') {
            url = url.substr(1);
        }

        var absoluteUrl = $this.UrlRoot + url;
        if (queryParams !== undefined) {
            absoluteUrl = absoluteUrl + '?' + $.param(queryParams);
        }

        return absoluteUrl;
    };

    $this.ajaxJSON = function (settings, onSuccess, onFailure) {
        settings = $.extend(settings, {
            type: 'POST',
            dataType: 'json',
            beforeSend: function () {
                if ($('.validation-summary-valid').length > 0) {
                    $this.hideErrors();
                }
                $this.blockUI();
            },
            success: function (data) {
                if (!$this.isSuccessAjax(data)) {
                    var errorHandler = function (data) {
                        if ($('.validation-summary-valid').length > 0) {
                            $this.showErrorMsg(data.ErrMsg);
                        } else {
                            alert(data.ErrMsg);
                        }
                    };

                    if (onFailure != null) {
                        if (typeof (onFailure) === 'function') {
                            onFailure(data, errorHandler);
                        } else {
                            errorHandler(data);
                        }
                    }

                    return;
                }

                if (typeof (onSuccess) === 'function') {
                    onSuccess(data);
                }
            },
            complete: function () {
                $this.unblockUI();
            }
        });
        return $.ajax(settings);
    };

    $this.ajaxHTML = function (settings, onSuccess) {
        settings = $.extend(settings, {
            type: 'GET',
            dataType: 'html',
            beforeSend: function () {
                $this.blockUI();
            },
            success: function (data) {
                onSuccess(data);
            },
            complete: function () {
                $this.unblockUI();
            }
        });
        return $.ajax(settings);
    };

    $this.isSuccessAjax = function (data) {
        return typeof (data) !== 'undefined'
            && typeof (data.ErrCode) !== 'undefined'
            && data.ErrCode !== 0
            ? false
            : true;
    };

    $this.detectBrowser = function () {
        var matchedBrowser, browserDectect;

        // Use of jQuery.browser is frowned upon.
        // More details: http://api.jquery.com/jQuery.browser
        // jQuery.uaMatch maintained for back-compat
        function ubMatch(ua) {
            ua = ua.toLowerCase();

            var match = /(chrome)[ \/]([\w.]+)/.exec(ua) ||
                    /(webkit)[ \/]([\w.]+)/.exec(ua) ||
                    /(opera)(?:.*version|)[ \/]([\w.]+)/.exec(ua) ||
                    /(msie) ([\w.]+)/.exec(ua) ||
                    ua.indexOf("compatible") < 0 && /(mozilla)(?:.*? rv:([\w.]+)|)/.exec(ua) ||
                    [];

            return {
                browserDectect: match[1] || "",
                version: match[2] || "0"
            };
        }

        matchedBrowser = ubMatch(navigator.userAgent);
        browserDectect = {};

        if (matchedBrowser.browserDectect) {
            browserDectect[matchedBrowser.browserDectect] = true;
            browserDectect.version = matchedBrowser.version;
        }

        // Chrome is Webkit, but Webkit is also Safari.
        if (browserDectect.chrome) {
            browserDectect.webkit = true;
        } else if (browserDectect.webkit) {
            browserDectect.safari = true;
        }
        return browserDectect;
    };

    $this.blockUI = function () {
        if ($('body>.block-overlay').length === 0) {
            $('body').append('<div class="block-overlay"></div>');
            $('body').append('<div class="block-loading"></div>');
        }

        $('body>.block-overlay').show();
        $('body>.block-loading').show();
    };

    $this.unblockUI = function () {
        $('body>.block-overlay').hide();
        $('body>.block-loading').hide();
    };

    $this.scrollTop = function () {
        $("html, body").animate({ scrollTop: 0 }, "fast");
    };

    $this.scrollTo = function (element) {
        var viewportHeight = $(window).height(),
                $element = $(element),
                elHeight = $element.height(),
                elOffset = $element.offset();
        $("html, body").animate({ scrollTop: elOffset.top + (elHeight / 2) - (viewportHeight / 2) }, "fast");
    };

    $this.isScrolledIntoView = function (elem) {
        var docViewTop = $(window).scrollTop();
        var docViewBottom = docViewTop + $(window).height();
        var elemTop = $(elem).offset().top;
        var elemBottom = elemTop + $(elem).height();
        return ((elemBottom <= docViewBottom) && (elemTop >= docViewTop));
    };

    $this.showErrorMsg = function (msg) {
        var container = $('[data-valmsg-summary=true]');
        container.show();
        container.addClass('validation-summary-errors')
                .find('li:first').html(msg).show();
    };

    $this.showSuccessMsg = function (msg) {
        var container = $('[data-valmsg-summary=true]');
        container.show();
        container.addClass('success').find('li:first').html(msg).show();
    };

    $this.showErrors = function (validator) {
        var container = $('[data-valmsg-summary=true]');
        var list = container.find("ul");

        if (list && list.length && validator.errorList.length) {
            list.empty();
            container.addClass("validation-summary-errors");
            container.show();

            $.each(validator.errorList, function () {
                $("<li />").html(this.message).appendTo(list);
            });
        }
    };

    $this.hideErrors = function () {
        var container = $('[data-valmsg-summary=true]');
        container.hide();
    };

    $this.getQueryParamValue = function (name) {
        name = name.toLowerCase().replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
        var regex = new RegExp("[\\?&]" + name + "=([^&#]*)");
        var results = regex.exec(location.search.toLowerCase());
        return results == null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
    };

    $this.removeURLParameter = function (url, parameter) {
        //prefer to use l.search if you have a location/link object
        var urlparts = url.split('?');
        if (urlparts.length >= 2) {
            var prefix = encodeURIComponent(parameter) + '=';
            var pars = urlparts[1].split(/[&;]/g);

            //reverse iteration as may be destructive
            for (var i = pars.length; i-- > 0; ) {
                //idiom for string.startsWith
                if (pars[i].lastIndexOf(prefix, 0) !== -1) {
                    pars.splice(i, 1);
                }
            }

            url = urlparts[0] + '?' + pars.join('&');
            return url;
        } else {
            return url;
        }
    };

    $this.gotoHomePage = function (timeout) {
        if (timeout === undefined) {
            timeout = CORE.ReloadInterval;
        }
        setTimeout(function () {
            window.parent.location.href = CORE.UrlRoot;
        }, timeout);
    };

    $this.reloadCurrentPage = function (timeout) {
        if (timeout === undefined) {
            timeout = CORE.ReloadInterval;
        }
        setTimeout(function () {
            location.reload();
        }, timeout);
    };

    $this.openWindow = function (url) {
        window.open(url, '_blank');
        window.focus();
    };

    $this.submitPage = function (hasReplication) {
        if (POPUP.isIframe()) {
            window.parent.CORE.submitPage(hasReplication);
        } else {
            var timeout = 0;
            if (hasReplication === true) {
                timeout = CORE.ReplicationInterval;
            } else {
                if ((typeof pageContext !== 'undefined') && pageContext.IsLastLevel) {
                    timeout = CORE.ReplicationInterval;
                }
            }
            $this.blockUI();
            setTimeout(function () {
                $('button[type=submit]').submit();
                $this.unblockUI();
            }, timeout);
        }
    };

    $this.handleError = function (data) {
        if (data && data.ErrCode) {
            if (data.ErrCode === -3 || data.ErrCode === -5) {
                // UM
                location.href = CORE.UrlRoot;
                return;
            }

            alert(data.ErrMsg);
            if (data.ErrCode === -2) {
                // Currency Permission
                window.parent.location.href = $this.resolveUrl('~/Account/SignOut');
            }
            return;
        }

        alert('Your request is temporarily cannot be processed, please try again later!');
    };

    $this.encodeMessages = function (messages) {
        var array = [];
        var length = messages.length;
        for (var i = 0; i < length; i++) {
            var message = messages[i];
            array.push(message.ErrCode.toString());
            array.push(message.ErrMsg);
        }
        return encodeURIComponent(array.join('|'));
    };

    $this.decodeMessages = function (string) {
        var array = decodeURIComponent(string).split('|');
        if (array.length % 2 !== 0) {
            return string;
        }
        var length = array.length / 2;
        var messages = [];
        for (var i = 0; i < length; i++) {
            var errCode = parseInt(array[i * 2]);
            var errMsg = array[i * 2 + 1];
            var message = {
                ErrCode: errCode,
                ErrMsg: errMsg
            };
            messages.push(message);
        }
        return messages;
    };

    return $this;
} (CORE || {}, jQuery));

$(function () {
    CORE.init();
});