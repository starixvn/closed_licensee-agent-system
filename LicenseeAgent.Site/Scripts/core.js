var CORE = function ($) {
    return {
        UrlRoot: $('#AppRoot').val(),
        UrlSignOut: $('#AppRoot').val() + 'Account/SignOut',
        pageSizes: [10, 20, 50, 100, 500],
        TimeInterval: [15, 30, 45, 60, 75],
        DATE_FORMAT: "MM/DD/YYYY",
        DATE_FORMAT_FULL: "MM/DD/YYYY hh:mm:ss A",
        DATE_FORMAT_MONTH: "MM-YYYY",
        FORMATNUMBER: '0,0.00',
        FORMAT_INT_NUMBER: '0,0',
        MIN_CONTENT_WIDTH: '700',
        PASSWORD_FORMAT: new RegExp(/(?=^.{8,24}$)(?!.*\s)((?=.*\d)(?=.*[A-Z])(?=.*[a-z])|(?=.*\d)(?=.*[^A-Za-z0-9])(?=.*[a-z])|(?=.*[^A-Za-z0-9])(?=.*[A-Z])(?=.*[a-z])|(?=.*\d)(?=.*[A-Z])(?=.*[^A-Za-z0-9]))^.*/),
        RELOAD_INTERVAL: 2500,
        EMPTY_TEXT_FORMAT: '---',

        init: function () {
            $('input, textarea').placeholder();

            $("button[data-href]").click(function () {
                location.href = CORE.resolveUrl($(this).attr("data-href"));
            });

            if ($('#frmMain').length == 0) {
                $('html').on('click', function () {
                    parent.$('#frmMain').trigger('click');
                });
            }

            if (typeof moment != 'undefined') {
                var selectedLanguageCode = LANGUAGE.getSelectedLanguageCode();
                moment.lang(selectedLanguageCode);
            }

            $(document).click(function (e) {
                if ($().tooltip) {
                    $('.tooltip').prev().each(function (index, element) {
                        if (element == e.target) return;
                        if ($(e.target).is('button')) return;
                        if ($(e.target).attr('title')) return;
                        $(element).tooltip('hide');
                    });
                }
            });

            // Override the fanexGrid
            var fanexGridOrigin = jQuery.fn.fanexGrid;
            if (fanexGridOrigin != undefined) {
                jQuery.fn.fanexGrid = function () {
                    jQuery.fn.fanexGrid = fanexGridOrigin;
                    return fanexGridOrigin.apply(this, arguments);
                    $("#mainContainer").css("overflow-x", "hidden");
                };
            }

            $.ajaxSetup({
                error: function (jqXHR, textStatus, errorThrown) {
                    var data = jqXHR.responseJSON;
                    CORE.handleError(data);
                }
            });
        },
        initInstruction: function () {
            var containers = $('.instruction-container');
            containers.each(function () {
                var $this = $(this),
                        thisId = $this.attr('id'),
                        title = $this.attr('title'),
                        tipTitle = $this.attr('tip-title'),
                        trigger;

                trigger = thisId == 'page-info'
                        ? $('<div class="page-info" />').appendTo('#header_main')
                        : $('#' + thisId.substr(0, thisId.length - 5)); // 5 is the length of "-info";

                if (trigger.length > 0) {
                    trigger.attr('title', tipTitle ? tipTitle : title)
                        .tooltip({
                            position: { my: "right-10 center", at: "center center+5" },
                            show: null,
                            hide: null
                        })
                        .click(function () {
                            var dialog = POPUP.openDialog($this, title, { modal: false, clickout: true, openid: $(this).attr('id') });
                            $('.ui-tooltip').remove();
                        });
                }
            });
        },
        reCheckIn: function () {
            $.ajax({
                type: "POST",
                url: CORE.resolveUrl("~/Account/ReCheckIn"),
                dataType: "json",
                async: false,
                timeout: 120000
            })
            .done(function (result) {
                if (result.ErrCode != 0) {
                    if (result.Reload) {
                        location.reload();
                        return;
                    }

                    alert(result.ErrMsg);
                    location.href = CORE.UrlRoot;
                }
                else {
                    setTimeout(function () { CORE.reCheckIn(); }, 30000);
                }
            });
        },
        resolveUrl: function (url) {
            var urlRoot = $('#AppRoot');

            if (urlRoot.length == 0) {
                CORE.console.error('"AppRoot" does not exist! Please add the following code to your page (or _Layout page): @Html.Hidden("UrlRoot", Url.Content("~/"))');
            } else {
                if (url.substr(0, 2) == '~/') {
                    url = url.substr(2);
                } else if (url.substr(0, 1) == '/') {
                    url = url.substr(1);
                }

                return urlRoot.val() + url;
            }
        },
        console: {
            write: function (msg, consoleFunc) {
                if (typeof console != 'undefined' && !CORE.detectBrowser().msie) {
                }
            },
            log: function (msg) { CORE.console.write(msg, console.log) },
            info: function (msg) { CORE.console.write(msg, console.info) },
            warn: function (msg) { CORE.console.write(msg, console.warn) },
            error: function (msg) { CORE.console.write(msg, console.error) }
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
            };

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
            $('div.block-body').show();
            $('div.block-loading').show();
            $.blockUI({
                message: "",
                timeout: 30000
            });
        },
        unblockUI: function () {
            $('div.block-body').hide();
            $('div.block-loading').hide();
            $.unblockUI();
        },
        getGridColumnId: function (gridOption, columnName) {
            var length = gridOption.columns.length;
            for (var i = 0; i < length; i++) {
                var column = gridOption.columns[i];
                if (column.Name == columnName) {
                    return i;
                }
            }
            return -1;
        },
        genGridRows: function (list, options) {
            var columns = options.columns;
            var rowCssClass = options.rowCssClass; // custom option
            var tdCssClass = options.tdCssClass;
            var rowHeight = options.RowHeight;
            var indexNumberColumn = options.indexNumberColumn; // custom option

            var bodyRows = [];
            var len = list.length;
            var rowIndex = 0;
            if (typeof value != 'undefined') {
                var currentPage = options.pagerOption.currentPage || 0;
                rowIndex = currentPage * options.pagerOption.itemsPerPage;
            }

            for (var i = 0; i < len; i++) {
                var record = list[i];
                var dataRow = [];

                for (var j = 0; j < columns.length; j++) {
                    (function () {
                        var rIndex = rowIndex;
                        var rowData = record;
                        var col = columns[j];
                        var colName = col.Name;

                        dataRow.push({
                            Name: colName,
                            Value: (indexNumberColumn == j) ? rIndex + 1 : record[colName],
                            CustomCellRender: function (cell, cellFormated) {
                                if (col.Formatter) {
                                    return col.Formatter(rIndex, rowData, cell, cellFormated);
                                } else
                                    return cellFormated;
                            },
                            TdClass: tdCssClass ? tdCssClass(rowIndex, colName, record) : undefined
                        });
                    } ());
                }

                bodyRows.push({
                    Columns: dataRow,
                    CssClass: rowCssClass ? rowCssClass(rowIndex, record) : undefined,
                    RowHeight: rowHeight ? rowHeight(rowIndex, record) : undefined
                });

                rowIndex++;
            }
            return bodyRows;
        },
        genGridFooterRows: function (data, gridOption) {
            if (gridOption.footerRows) {
                var columns = gridOption.footerRows[0].Columns;
                var length = columns.length;
                for (var i = 0; i < length; i++) {
                    var colName = columns[i].Name;
                    var value = data[colName];
                    if (typeof value != 'undefined' && columns[i].Value !== '') {
                        columns[i].Value = value;
                    }
                }
                return gridOption.footerRows;
            }
            return undefined;
        },
        genGridTotalRows: function (bodyRows, gridOption) {
            if (gridOption.footerRows) {
                var columns = gridOption.footerRows[0].Columns;
                var length = columns.length;
                for (var i = 0; i < length; i++) {
                    var column = columns[i];
                    if (column.IsCalculated) {
                        var total = 0;
                        var colName = columns[i].Name;
                        var colId = CORE.getGridColumnId(gridOption, colName);
                        for (var j = 0; j < bodyRows.length; j++) {
                            total += bodyRows[j].Columns[colId].Value;
                        }
                        column.Value = total;
                    }
                }
                return gridOption.footerRows;
            }
            return undefined;
        },
        resizeHeightDataGrid: function (gridOption) {
            var colHeight = $("#gridId").attr('colHeight'),
                expHeight = $("#gridId").attr('expHeight');
            if ($(".search-form").is(":visible")) {
                gridOption.gridExpandHeight = expHeight;
            } else {
                gridOption.gridExpandHeight = colHeight;
            }
            return gridOption;
        },
        scrollTop: function () {
            $("html, body").animate({ scrollTop: 0 }, "fast");
        },
        scrollTo: function (element) {
            var viewportWidth = $(window).width(),
                viewportHeight = $(window).height(),
                $element = $(element),
                elWidth = $element.width(),
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
            return /^true$/i.test(value);
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
            msgWrp.addClass('success')
                    .find('li:first').html(msg).show();
        },
        getParameterByName: function (name) {
            name = name.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
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
        reload: function (timeout) {
            if (timeout == undefined) {
                timeout = CORE.RELOAD_INTERVAL;
            }
            setTimeout(function () {
                window.parent.location.href = CORE.UrlRoot;
            }, timeout);
        },
        reloadPage: function (timeout) {
            if (timeout == undefined) {
                timeout = CORE.RELOAD_INTERVAL;
            }
            setTimeout(function () {
                location.reload();
            }, timeout);
        },
        submitPage: function () {
            $('button[type=submit]').submit();
        },
        formatCustomerName: function (firstName, lastName) {
            if (firstName || lastName) {
                return '{0} {1}'.format(firstName, lastName);
            }
            return CORE.EMPTY_TEXT_FORMAT;
        },
        handleError: function (data) {
            if (data && data.ErrCode) {
                if (data.ErrCode == -3) {
                    // UM
                    location.href = CORE.UrlRoot;
                    return;
                }

                alert(data.ErrMsg);
                if (data.ErrCode == -2) {
                    // Currency Permission

                    window.parent.location.href = CORE.UrlSignOut;
                }
                return;
            }

            alert('Your request is temporarily cannot be processed, please try again later!');
        }
    };
} (jQuery);
$(function () {
    CORE.init();
});