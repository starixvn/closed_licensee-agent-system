var CORE = function ($) {
    return {
        UrlRoot: $('#AppRoot').val(),
        pageSizes: [10, 20, 50, 100, 500],
        TimeInterval: [15, 30, 45, 60, 75],
        DATE_FORMAT: "MM/DD/YYYY",
        DATE_FORMAT_FULL: "MM/DD/YYYY hh:mm:ss A",
        DATE_FORMAT_MONTH: "MM-YYYY",
        FORMATNUMBER: '0,0.00',
        FORMAT_INT_NUMBER: '0,0',
        MIN_CONTENT_WIDTH: '700',
        PASSWORD_FORMAT: new RegExp(/(?=^.{8,24}$)(?!.*\s)((?=.*\d)(?=.*[A-Z])(?=.*[a-z])|(?=.*\d)(?=.*[^A-Za-z0-9])(?=.*[a-z])|(?=.*[^A-Za-z0-9])(?=.*[A-Z])(?=.*[a-z])|(?=.*\d)(?=.*[A-Z])(?=.*[^A-Za-z0-9]))^.*/),

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

            // Override the fanexGrid
            var fanexGridOrigin = jQuery.fn.fanexGrid;
            if (fanexGridOrigin != undefined) {
                jQuery.fn.fanexGrid = function () {
                    jQuery.fn.fanexGrid = fanexGridOrigin;
                    return fanexGridOrigin.apply(this, arguments);
                    $("#mainContainer").css("overflow-x", "hidden");
                };
            }
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
        genGridRows: function (list, options) {
            var columns = options.columns;
            var rowCssClass = options.rowCssClass; // custom option
            var rowHeight = options.RowHeight;
            var indexNumberColumn = options.indexNumberColumn; // custom option

            var bodyRows = [];
            var len = list.length;

            var currentPage = options.pagerOption.currentPage || 0;
            var rowIndex = currentPage * options.pagerOption.itemsPerPage;
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
                            }
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
        genGridData: function (columns, list, rowCssClass) {
            var bodyRows = [];
            var len = list.length;
            for (var i = 0; i < len; i++) {
                var record = list[i];
                var dataRow = [];

                for (var j = 0; j < columns.length; j++) {
                    (function () {
                        var colName = columns[j].Name;
                        var col = columns[j];
                        dataRow.push({
                            Name: colName,
                            Value: record[colName],
                            CustomCellRender: function (cell, cellFormated) {
                                if (col.Formatter) {
                                    return col.Formatter(record, cell, cellFormated);
                                } else
                                    return cellFormated;
                            }
                        });
                    } ());
                }

                bodyRows.push({
                    Columns: dataRow,
                    CssClass: rowCssClass ? rowCssClass(record) : undefined
                });
            }
            return bodyRows;
        },
        renderGrid: function (gridObject, data, gridOption, callback) {
            gridObject.show();
            gridOption.pagerOption = {
                itemsPerPage: data.ItemsPerPage,
                itemsPerPageArray: CORE.pageSizes,
                currentPage: data.CurrentPage,
                totalItem: data.TotalItem,
                pagerSelectCallBack: callback
            };
            var bodyRows = CORE.genGridRows(data.List, gridOption);
            gridOption.bodyRows = bodyRows;

            gridObject.fanexGrid(gridOption);
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
        }
    };
} (jQuery);
$(function () {
    CORE.init();
});
$.ajaxSetup({
    error: function (jqXHR, textStatus, errorThrown) {
        alert('Your request is temporarily cannot be processed, please try again later!');
    }
});