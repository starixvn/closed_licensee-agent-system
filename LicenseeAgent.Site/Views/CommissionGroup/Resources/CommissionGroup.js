/*global language, gridOptions, core, popup, numeral*/

var CommissionGroup = (function ($) {
    'use strict';
    return {
        init: function () {
            CommissionGroup.dataGridBinding();
            $('#newGroup').click(function () {
                CommissionGroup.newGroup();
            });
        },
        dataGridBinding: function () {
            $.ajax({
                url: core.resolveUrl("~/CommissionGroup/CommissionGroupList"),
                type: 'post',
                beforeSend: function () {
                    core.blockUI();
                },
                success: function (data) {
                    if (data.ErrCode === 0) {
                        gridOptions.pagerOption = {
                            itemsPerPage: core.pageSizes[2],
                            currentPage: 0,
                            itemsPerPageArray: core.pageSizes,
                            totalItem: data.List.length
                        };

                        gridOptions.bodyRows = core.genGridRows(data.List, gridOptions);
                        $('#grid').show().fanexGrid(gridOptions);
                        core.unblockUI();
                    }
                    else {
                        alert(data.ErrMsg);
                    }
                },
                complete: function () {
                    core.unblockUI();
                }
            });
        },
        newGroup: function () {
            popup.openFrame("CommissionGroup/Add");
        },
        editGroup: function (groupCommId) {
            popup.openFrame("CommissionGroup/Edit?groupCommId={0}".format(groupCommId));
        },
        deleteGroup: function (groupCommId, isDefault) {
            if (isDefault) {
                CommissionGroup.confirmDeleteGroup(groupCommId);
            }
            else {
                popup.openFrame("CommissionGroup/Delete?groupCommId={0}&isDefault=false".format(groupCommId));
            }
        },
        formatNumber: function (value) {
            return numeral(value * 100).format('0.00');
        },

        confirmDeleteGroup: function (groupCommId) {
            var buttons = [{
                label: language.getString('OK'),
                action: function () {
                    popup.openFrame("CommissionGroup/Delete?groupCommId={0}&isDefault=true".format(groupCommId));
                }
            },
            {
                label: language.getString('Cancel'),
                action: function () {
                    popup.close();
                }
            }
            ];

            popup.confirm({
                title: language.getString('Confirmation'),
                message: language.getString('ConfirmDeleteDefaultGroup'),
                buttons: buttons,
                top: 120,
                width: 420
            });

        }
    };
})(jQuery);

$(function () {
    'use strict';
    CommissionGroup.init();
});