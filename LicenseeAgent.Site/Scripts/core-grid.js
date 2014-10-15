var core = core || {};
core.grid = (function () {
    'use strict';

    var Grid = function (options) {
        this.options = $.extend({}, Grid.defaultOptions, options);
    };

    Grid.defaultOptions = {
        hasIndexColumn: false
    };

    return {
        buildBodyRows: function (list, options) {
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
        }
    };
} ());