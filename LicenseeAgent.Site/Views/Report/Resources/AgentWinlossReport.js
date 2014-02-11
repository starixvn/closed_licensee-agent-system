var AGENTWINLOSS = function ($) {
    return {
        init: function () {
            $("#tabs-area a:first").tab('show');
            $('#fromDatePicker').datepicker("setDate", '12/01/2013');
            $('#toDatePicker').datepicker();

            $('#search').click(function () {
                AGENTWINLOSS.dataGridBinding();
                return false;
            });
            $('#today').click(function () {
                $('#FromDate').val($(this).data('date'));
                $('#ToDate').val($(this).data('date'));
                AGENTWINLOSS.dataGridBinding();
                return false;
            });
            $('#yesterday').click(function () {
                $('#FromDate').val($(this).data('date'));
                $('#ToDate').val($(this).data('date'));
                AGENTWINLOSS.dataGridBinding();
                return false;
            });
            AGENTWINLOSS.dataGridBinding($("#FromDate").val(), $("#ToDate").val());
        },

        dataGridBinding: function () {
            var submitOptions = $('form').serializeObject();
            $.ajax({
                url: CORE.resolveUrl("~/Report/AgentWinLoss"),
                type: 'post',
                data: jQuery.param(submitOptions),
                beforeSend: function () {
                    CORE.blockUI();
                },
                success: function (data) {
                    if (data.ErrCode == 0) {
                        gridOption.bodyRows = CORE.genGridRows(data.List, gridOption);
                        $('#grid').show().fanexGrid(gridOption);
                        CORE.unblockUI();
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
        },

        ViewDownline: function () {
            alert("test");
            //location.href = "/Report/AgentWinloss?agentId=" + agentId + "&agentLevel=" + agentLevel + "&username=" + username + "&breadcrumb=" + $("#BreadCrumb").val()
        }
    }
} (jQuery);

$(function () {
    AGENTWINLOSS.init();
});