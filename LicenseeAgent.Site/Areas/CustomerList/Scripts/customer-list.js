/*global core, language, pageContext, context */

var customerList = (function ($) {
    'use strict';
    var gridOptions;
    return {
        initCustomerList: function (_gridOptions) {
            gridOptions = _gridOptions;
            $('.selectpicker').selectpicker();

            customerList.createCheckBoxContext();

            // search
            $('#search-form').submit(function (e) {
                customerList.reRenderAfterPaged(0, core.pageSizes[2]);
                e.preventDefault();
            });

            // username suggestion
            $('#Username').typeahead({
                source: customerList.getSuggestion,
                minLength: 2
            });

            // breadcrum
            $('.breadcrumb a').click(function () {
                var agentId = $(this).data('agent-id');
                customerList.showAgent(agentId);
            });

            // get customer list
            $('#search-form').submit();
        },
        getSuggestion: function (query, process) {
            return $.ajax({
                url: $('#Username').data('autocomplete-url'),
                type: 'post',
                data: {
                    userName: query,
                    agentId: $("#AgentId").val(),
                    agentLevel: $("#AgentLevel").val()
                },
                success: function (data) {
                    if (data.ErrCode === 0) {
                        return process(data.List);
                    } else {
                        alert(data.ErrMsg);
                    }
                }
            });
        },
        reRenderAfterPaged: function (currentPage, itemsPerPage) {
            if (gridOptions && gridOptions.pagerOption) {
                currentPage = currentPage >= 0 ? currentPage : gridOptions.pagerOption.currentPage;
                itemsPerPage = itemsPerPage || gridOptions.pagerOption.itemsPerPage;
            }

            var params = $('#search-form').serializeObject();
            params.pageIndex = currentPage;
            params.pageSize = itemsPerPage;
            core.ajaxJSON({
                url: $('#search-form').attr('action'),
                data: params
            }, function (data) {
                gridOptions.pagerOption = {
                    itemsPerPage: data.ItemsPerPage,
                    itemsPerPageArray: core.pageSizes,
                    currentPage: data.CurrentPage,
                    totalItem: data.TotalItem,
                    pagerSelectCallBack: customerList.reRenderAfterPaged
                };

                gridOptions.bodyRows = core.genGridRows(data.List, gridOptions);
                $('#grid').show().fanexGrid(gridOptions);
                $("#boxsearch").width($("#grid").width());
                $("#breadcrumb").width($("#grid").width());
                if (!pageContext.IsDirectDownLine) {
                    $(".cust-status").addClass('cust-status-none');
                }
            });
        },
        showAgent: function (agentId) {
            $('#agent-form #AgentId').val(agentId);
            $('#agent-form').submit();
        },
        showMember: function (agentId) {
            $('#member-form #AgentId').val(agentId);
            $('#member-form').submit();
        },
        hidePopover: function () {
            $('.popover-link').each(function () {
                $(this).popover('hide');
            });
        },
        createCheckBoxContext: function () {
            context.attach('#select-dropdown', [
                {
                    text: language.getString('All'),
                    href: '#',
                    action: customerList.checkAll
                },
                {
                    text: language.getString('None'),
                    href: '#',
                    action: customerList.uncheckAll
                }
            ]);
        },
        checkAllItem: function (itemAll) {
            if ($(itemAll).hasClass("icon-uncheck-all")) {
                customerList.checkAll();
            } else {
                customerList.uncheckAll();
            }
        },
        checkAll: function () {
            $(".icon-uncheck-all").attr("class", "icon-checked-all");
            $(".icon-uncheck").attr("class", "icon-checked");
        },
        uncheckAll: function () {
            $(".icon-checked-all").attr("class", "icon-uncheck-all");
            $(".icon-checked").attr("class", "icon-uncheck");
        },
        selectClick: function (itemSelect) {
            if ($(itemSelect).hasClass("icon-uncheck")) {
                $(itemSelect).removeClass("icon-uncheck");
                $(itemSelect).addClass("icon-checked");
                if ($(".cell-select").length === $(".icon-checked").length) {
                    $(".icon-uncheck-all").attr("class", "icon-checked-all");
                }
            } else {
                $(itemSelect).removeClass("icon-checked");
                $(itemSelect).addClass("icon-uncheck");
                $(".icon-checked-all").attr("class", "icon-uncheck-all");
            }
        }
    };
} (jQuery));