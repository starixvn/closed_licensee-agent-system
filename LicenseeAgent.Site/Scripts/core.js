/*global appSettings, language, moment, popup, pageContext */

var core = (function ($) {
    'use strict';
    return {
        DisableRecheckIn: false,
        UrlRoot: appSettings.UrlRoot,
        UrlSignOut: appSettings.UrlRoot + 'Account/SignOut',
        pageSizes: [10, 20, 50, 100, 500],
        TimeInterval: [15, 30, 45, 60, 75],
        DATE_FORMAT: "MM/DD/YYYY",
        DATE_FORMAT_FULL: "MM/DD/YYYY hh:mm:ss A",
        DATE_FORMAT_MONTH: "MM-YYYY",
        FORMATNUMBER: '0,0.00',
        FORMAT_INT_NUMBER: '0,0',
        MIN_CONTENT_WIDTH: '700',
        RELOAD_INTERVAL: 5000,
        REPLICATION_INTERVAL: 5000,
        EMPTY_TEXT_FORMAT: '---',
        ServerTime: appSettings.ServerTime,
        ServerDate: appSettings.ServerDate,
        ServerUtcOffset: appSettings.ServerUtcOffset,
        init: function () {
            // setup ajax.
            $.ajaxSetup({
                error: function (jqXHR) {
                    if (jqXHR.status === 0 || jqXHR.readyState === 0) {
                        // ajax request is aborted.
                        return;
                    }
                    var data = jqXHR.responseJSON;
                    core.handleError(data);
                }
            });

            // set behaviour with 3rd.
            $('input, textarea').placeholder();
            if ($.fn.datepicker) {
                $('.datepicker').data('datepicker-format', core.DATE_FORMAT);
            }

            // customize scrollbars.
            if ($.fn.niceScroll) {
                $('html').niceScroll({
                    cursorcolor: "#8a8a8a",
                    autohidemode: false
                });

                $('body').mutate('height width', function () {
                    $('html').getNiceScroll().resize();
                });
            }

            // set href for button.
            $("button[data-href]").click(function () {
                location.href = core.resolveUrl($(this).attr("data-href"));
            });

            // handle click in iframe.
            if ($('#frmMain').length === 0) {
                $('html').on('click', function () {
                    parent.$('#frmMain').trigger('click');
                });
            }

            // set language for 3rd.
            if (typeof moment !== 'undefined') {
                moment.lang(language.getSelectedLanguageCode().toLowerCase());
            }
            if ($.fn.fanexGrid) {
                //$.fn.fanexGrid.defaults.marginRight = 0;
                $.fn.fanexGrid.defaults.scrollWidth = 24;
                $.fn.fanexGrid.defaults.language = language.getSelectedLanguage();
                $.fn.fanexGrid.defaults.timeZone = core.ServerUtcOffset;
            }

            // handle tooltip.
            $(document).click(function (e) {
                if ($().tooltip) {
                    $('.tooltip').prev().each(function (index, element) {
                        if (element === e.target) {
                            return;
                        }
                        if ($(e.target).is('button')) {
                            return;
                        }
                        if ($(e.target).attr('title')) {
                            return;
                        }
                        $(element).tooltip('hide');
                    });
                }
            });

            // Override the fanexGrid
            var fanexGridOrigin = jQuery.fn.fanexGrid;
            if (fanexGridOrigin !== undefined) {
                jQuery.fn.fanexGrid = function () {
                    jQuery.fn.fanexGrid = fanexGridOrigin;
                    return fanexGridOrigin.apply(this, arguments);
                };
            }

            // call if page is not in frame & not be a popup
            if (window.parent.frmMain === undefined && !popup.isIframe() && !core.DisableRecheckIn) {
                setTimeout(function () { core.reCheckIn(); }, 1000);
            }
        },
        initInstruction: function () {
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
                    })
                        .click(function () {
                            popup.open({
                                title: title,
                                message: $this.html(),
                                top: 100,
                                clickout: true
                            });
                        });
                }
            });
        },
        reCheckIn: function () {
            $.ajax({
                type: "POST",
                url: core.resolveUrl("~/Account/ReCheckIn"),
                dataType: "json",
                async: true,
                timeout: 120000
            })
                .done(function (result) {
                    if (result.ErrCode !== 0) {
                        if (result.Reload) {
                            location.reload();
                            return;
                        }

                        alert(result.ErrMsg);
                        location.href = core.UrlRoot;
                    } else {
                        setTimeout(function () { core.reCheckIn(); }, 30000);
                    }
                });
        },
        resolveUrl: function (url) {
            var urlRoot = core.UrlRoot;
            if (url.substr(0, 2) === '~/') {
                url = url.substr(2);
            } else if (url.substr(0, 1) === '/') {
                url = url.substr(1);
            }

            return urlRoot + url;
        },
        console: {
            write: function (msg, consoleFunc) {
                if (typeof console !== 'undefined' && !core.detectBrowser().msie) {
                    consoleFunc(msg);
                }
            },
            log: function (msg) { core.console.write(msg, console.log); },
            info: function (msg) { core.console.write(msg, console.info); },
            warn: function (msg) { core.console.write(msg, console.warn); },
            error: function (msg) { core.console.write(msg, console.error); }
        },
        detectBrowser: function () {
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
        },
        blockUI: function () {
            if ($('body>.block-overlay').length === 0) {
                $('body').append('<div class="block-overlay"></div>');
                $('body').append('<div class="block-loading"></div>');
            }

            $('body>.block-overlay').show();
            $('body>.block-loading').show();
        },
        unblockUI: function () {
            $('body>.block-overlay').hide();
            $('body>.block-loading').hide();
        },
        getGridColumnId: function (gridOptions, columnName) {
            var length = gridOptions.columns.length;
            for (var i = 0; i < length; i++) {
                var column = gridOptions.columns[i];
                if (column.Name === columnName) {
                    return i;
                }
            }
            return -1;
        },
        rebuildGridOptions: function (options) {
            // always auto resize on alls columns that don't define CanResize
            if (options.autoResizeColumn) {
                var columns = options.columns;
                var calResizeFunction = function (value) {
                    return value === null ? 0 : value.toString().length * 10;
                };
                for (var j = 0; j < columns.length; j++) {
                    var column = columns[j];
                    if (column.CanResize === undefined) {
                        column.CanResize = true;
                        if (column.CalResizeFunction === undefined) {
                            column.CalResizeFunction = calResizeFunction;
                        }
                    }
                }
            }

            if (options.gridExpandHeight === undefined) {
                options.gridExpandHeight = Math.max($('.freegrid').offset().top, 2);
            }
        },
        genGridRows: function (list, options) {
            core.rebuildGridOptions(options);
            var columns = options.columns;
            var rowCssClass = options.rowCssClass; // custom option
            var tdCssClass = options.tdCssClass;
            var rowHeight = options.RowHeight;
            var indexNumberColumn = options.indexNumberColumn; // custom option

            var bodyRows = [];
            var len = list.length;
            var rowIndex = 0;
            if (typeof value !== 'undefined') {
                var currentPage = options.pagerOption.currentPage || 0;
                rowIndex = currentPage * options.pagerOption.itemsPerPage;
            }

            var rowIndexValue = rowIndex;
            if (options.pagerOption !== undefined) {
                rowIndexValue = options.pagerOption.itemsPerPage * options.pagerOption.currentPage + 1;
            }

            for (var i = 0; i < len; i++) {
                var record = list[i];
                var dataRow = [];

                for (var j = 0; j < columns.length; j++) {
                    dataRow.push(core.getGridColumn(j, indexNumberColumn, rowIndex, rowIndexValue, record, columns, tdCssClass));
                }
                rowIndexValue++;

                bodyRows.push({
                    Columns: dataRow,
                    CssClass: rowCssClass ? rowCssClass(rowIndex, record) : undefined,
                    RowHeight: rowHeight ? rowHeight(rowIndex, record) : undefined
                });

                rowIndex++;
            }
            return bodyRows;
        },
        getGridColumn: function (colIndex, numberColumn, rowIndex, rowIndexValue, record, columns, tdCssClass) {
            var rIndex = rowIndex;
            var rowData = record;
            var col = columns[colIndex];
            var colName = col.Name;

            return {
                Name: colName,
                Value: (numberColumn === colIndex) ? rowIndexValue : record[colName],
                CustomCellRender: function (cell, cellFormated) {
                    if (col.Formatter) {
                        return col.Formatter(rIndex, rowData, cell, cellFormated);
                    } else {
                        return cellFormated;
                    }
                },
                TdClass: tdCssClass ? tdCssClass(rowIndex, colName, record) : undefined
            };
        },
        genGridTotalRows: function (bodyRows, gridOptions) {
            if (gridOptions.footerRows) {
                var columns = gridOptions.footerRows[0].Columns;
                var length = columns.length;
                for (var i = 0; i < length; i++) {
                    var column = columns[i];
                    if (column.IsCalculated) {
                        var total = 0;
                        var colName = columns[i].Name;
                        var colId = core.getGridColumnId(gridOptions, colName);
                        for (var j = 0; j < bodyRows.length; j++) {
                            total += bodyRows[j].Columns[colId].Value;
                        }
                        column.Value = total;
                    }
                }
                return gridOptions.footerRows;
            }
            return undefined;
        },
        resizeHeightDataGrid: function (gridOptions) {
            var colHeight = $("#gridId").attr('colHeight'),
                expHeight = $("#gridId").attr('expHeight');
            if ($(".search-form").is(":visible")) {
                gridOptions.gridExpandHeight = expHeight;
            } else {
                gridOptions.gridExpandHeight = colHeight;
            }
            return gridOptions;
        },
        scrollTop: function () {
            $("html, body").animate({ scrollTop: 0 }, "fast");
        },
        scrollTo: function (element) {
            var viewportHeight = $(window).height(),
                $element = $(element),
                elHeight = $element.height(),
                elOffset = $element.offset();
            $("html, body").animate({ scrollTop: elOffset.top + (elHeight / 2) - (viewportHeight / 2) }, "fast");
        },
        isScrolledIntoView: function (elem) {
            var docViewTop = $(window).scrollTop();
            var docViewBottom = docViewTop + $(window).height();
            var elemTop = $(elem).offset().top;
            var elemBottom = elemTop + $(elem).height();
            return ((elemBottom <= docViewBottom) && (elemTop >= docViewTop));
        },
        parseBoolean: function (value) {
            var regex = new RegExp(/^true$/i);
            return regex.test(value);
        },
        showErrorMsg: function (msg) {
            var msgWrp = $('.validation-summary-valid');
            msgWrp.show();
            msgWrp.addClass('validation-summary-errors')
                .find('li:first').html(msg).show();
        },
        showSuccessMsg: function (msg) {
            var msgWrp = $('.validation-summary-valid');
            msgWrp.show();
            msgWrp.addClass('success').find('li:first').html(msg).show();
        },
        showErrors: function (validator) {  // 'this' is the form element
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
        },
        hideErrors: function () {  // 'this' is the form element
            var container = $('.validation-summary-valid');
            container.hide();
        },
        getParameterByName: function (name) {
            name = name.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
            var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
                results = regex.exec(location.search);
            return results == null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
        },
        removeURLParameter: function (url, parameter) {
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
        },
        gotoHomePage: function (timeout) {
            if (timeout === undefined) {
                timeout = core.RELOAD_INTERVAL;
            }
            setTimeout(function () {
                window.parent.location.href = core.UrlRoot;
            }, timeout);
        },
        reloadCurrentPage: function (timeout) {
            if (timeout === undefined) {
                timeout = core.RELOAD_INTERVAL;
            }
            setTimeout(function () {
                location.reload();
            }, timeout);
        },
        openWindow: function (url) {
            window.open(url, '_blank');
            window.focus();
        },
        submitPage: function (hasReplication) {
            if (popup.isIframe()) {
                window.parent.core.submitPage(hasReplication);
            } else {
                var timeout = 0;
                if (hasReplication === true) {
                    timeout = core.REPLICATION_INTERVAL;
                } else {
                    if ((typeof pageContext !== 'undefined') && pageContext.IsLastLevel) {
                        timeout = core.REPLICATION_INTERVAL;
                    }
                }
                popup.block();
                setTimeout(function () {
                    $('button[type=submit]').submit();
                    popup.unblock();
                }, timeout);
            }
        },
        handleError: function (data) {
            if (data && data.ErrCode) {
                if (data.ErrCode === -3) {
                    // UM
                    location.href = core.UrlRoot;
                    return;
                }

                alert(data.ErrMsg);
                if (data.ErrCode === -2) {
                    // Currency Permission

                    window.parent.location.href = core.UrlSignOut;
                }
                return;
            }

            alert('Your request is temporarily cannot be processed, please try again later!');
        },
        encodeMessages: function (messages) {
            var array = [];
            var length = messages.length;
            for (var i = 0; i < length; i++) {
                var message = messages[i];
                array.push(message.ErrCode.toString());
                array.push(message.ErrMsg);
            }
            return encodeURIComponent(array.join('|'));
        },
        decodeMessages: function (string) {
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
        }
    };
} (jQuery));

$(function () {
    'use strict';
    core.init();
});