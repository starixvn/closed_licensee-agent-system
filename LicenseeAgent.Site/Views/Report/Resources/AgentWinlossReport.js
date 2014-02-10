var AGENTWINLOSS = function ($) {
    return {
        init: function () {
            $("#tabs-area a:first").tab('show');
            $('#fromDatePicker').datepicker();
            $('#toDatePicker').datepicker();

            $('#search').click(function () {
                AGENTWINLOSS.dataGridBinding($("#FromDate").val(), $("#ToDate").val());
                return false;
            });
            $('#today').click(function () {
                AGENTWINLOSS.dataGridBinding($(this).data('date'), $(this).data('date'));
                return false;
            });
            $('#yesterday').click(function () {
                AGENTWINLOSS.dataGridBinding($(this).data('date'), $(this).data('date'));
                return false;
            });
        },

        dataGridBinding: function (fromDate, toDate) {
            var params = {
                FromDate: fromDate,
                ToDate: toDate,
                DrawFilter: $('#search-form input[name=DrawFilter]:checked').val()
            };
            $.ajax({
                url: CORE.resolveUrl("~/Report/AgentWinLoss"),
                type: 'post',
                data: params,
                beforeSend: function () {
                    CORE.blockUI();
                },
                success: function (data) {
                    if (data.ErrCode == 0) {
                        gridOption.bodyRows = CORE.genGridRows(data.List, gridOption);
                        $('#grid').show().fanexGrid(gridOption);
                        //customerList.resizeBox();
                    }
                    else {
                        alert(data.ErrMsg);
                    }
                },
                complete: function () {
                    CORE.unblockUI();
                }
            });
        }
    }
} (jQuery);

$(function () {
    AGENTWINLOSS.init();
});