/*exported GRID*/
'use strict';

var GRID = (function ($this, $) {
    $this.PageSizes = [10, 20, 50, 100, 500];

    var getGridColumnId = function (gridOptions, columnName) {
        var length = gridOptions.columns.length;
        for (var i = 0; i < length; i++) {
            var column = gridOptions.columns[i];
            if (column.Name === columnName) {
                return i;
            }
        }
        return -1;
    };

    var getGridColumn = function (colIndex, numberColumn, rowIndex, rowIndexValue, record, columns, tdCssClass) {
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
    };

    var rebuildGridOptions = function (options) {
        // always auto resize on alls columns that don't define CanResize
        if (options.autoResizeColumn) {
            var columns = options.columns;
            var calResizeFunction = function (value) {
                return value === null || value === undefined ? 0 : value.toString().length * 10;
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
            options.gridExpandHeight = $('.freegrid').offset().top + 12;
        }
    };

    $this.generateBodyRows = function (list, options) {
        rebuildGridOptions(options);
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
                dataRow.push(getGridColumn(j, indexNumberColumn, rowIndex, rowIndexValue, record, columns, tdCssClass));
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
    };

    $this.generateTotalRows = function (bodyRows, gridOptions) {
        if (gridOptions.footerRows) {
            var columns = gridOptions.footerRows[0].Columns;
            var length = columns.length;
            for (var i = 0; i < length; i++) {
                var column = columns[i];
                if (column.NonCalculated !== true) {
                    var total = 0;
                    var colName = columns[i].Name;
                    var colId = getGridColumnId(gridOptions, colName);
                    for (var j = 0; j < bodyRows.length; j++) {
                        total += bodyRows[j].Columns[colId].Value;
                    }
                    column.Value = total;
                }
            }
            return gridOptions.footerRows;
        }
        return undefined;
    };

    return $this;
} (GRID || {}, jQuery));