var AGENTWINLOSS = function ($) {
    return {
        init: function () {
            $("#tabs-area a:first").tab('show');
            $('#fromDatePicker').datepicker();
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

            // breadcrum
            $('.breadcrumb a').click(function () {
                var agentId = $(this).data('agent-id');
                var agentLevel = $(this).data('agent-level');
                var agentName = $(this).text();
                AGENTWINLOSS.ViewDownline(agentId, agentLevel, agentName);
            });

            AGENTWINLOSS.dataGridBinding();
        },

        dataGridBinding: function () {
            var submitOptions = $('form').serializeObject();
            $.ajax({
                url: CORE.resolveUrl("~/Report/GetAgentWinLossData"),
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

        ViewDownline: function (agentId, agentLevel, username) {
            //Reset bread crumb value
            var currentBreadCrumb = $("#BreadCrumb").val();
            var breadCrumb = agentId + "," + agentLevel + "," + username;
            var newBreadCrumb = "";
            if (currentBreadCrumb.indexOf(breadCrumb) >= 0)
                newBreadCrumb = currentBreadCrumb.substring(0, currentBreadCrumb.indexOf(breadCrumb) + breadCrumb.length);
            else
                newBreadCrumb = currentBreadCrumb + ";" + breadCrumb;
            $("#BreadCrumb").val(newBreadCrumb);

            location.href = CORE.resolveUrl("~/Report/AgentWinLoss?agentId=" + agentId + "&agentLevel=" + agentLevel + "&username=" + username + "&drawFilter=" + $('input[name=DrawFilter]:checked', '#search-form').val()
                                            + "&breadcrumb=" + $("#BreadCrumb").val() + "&FromDate=" + $("#FromDate").val() + "&ToDate=" + $("#ToDate").val());
        },
        ViewBetList: function (custId, agentLevel, username) {
            var currentBreadCrumb = $("#BreadCrumb").val();
            var breadCrumb = custId + "," + agentLevel + "," + username;
            $("#BreadCrumb").val(currentBreadCrumb + ';' + breadCrumb);
            location.href = CORE.resolveUrl("~/BetList/BetListWinlossReport?custId=" + custId + "&breadcrumb=" + $("#BreadCrumb").val() + "&FromDate=" + $("#FromDate").val() + "&ToDate=" + $("#ToDate").val() + "&drawFilter=" + $('input[name=DrawFilter]:checked', '#search-form').val());
        }
    }
} (jQuery);

$(function () {
    AGENTWINLOSS.init();
});