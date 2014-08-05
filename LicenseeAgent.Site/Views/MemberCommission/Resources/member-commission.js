/*global core, popup, gridOptions, groupCommissions, numeral */
var memberCommission = (function ($) {
    'use strict';
    var commMax = 15;
    return {
        init: function () {
            $('.selectpicker').selectpicker();

            // get member commission list
            if ($("#grid").length > 0) {
                memberCommission.reRenderAfterPaged(0, core.pageSizes[2]);
            }

            // search
            $('#search-form').submit(function (e) {
                memberCommission.reRenderAfterPaged(0, core.pageSizes[2]);
                e.preventDefault();
            });

            // move group
            $('#moveToGroup').click(function () {
                memberCommission.moveToGroup();
            });

            // update commission
            $('#updateCommission').click(function () {
                memberCommission.updateCommission();
            });

            // remove group
            $('#removeFromGroup').click(function () {
                memberCommission.removeGroup();
            });

            // close popup
            $("#cancel").click(function () {
                popup.close();
            });

            // go back history
            $("#membercomm-back").click(function () {
            });

            $('#action-form').submit(function (e) {
                memberCommission.changeMemberCommission();
                e.preventDefault();
            });

            $("#GroupCommId").change(function () {
                if (groupCommissions === undefined) {
                    return;
                }
                var numberGroup = groupCommissions.length;
                for (var index = 0; index < numberGroup; index++) {
                    if (groupCommissions[index].GroupCommId === parseInt($(this).val())) {
                        $("#SBComm").val(memberCommission.formatNumber(groupCommissions[index].SBComm));
                        $("#NGComm").val(memberCommission.formatNumber(groupCommissions[index].NGComm));
                    }
                }
            });

            $(".product-comm").change(function () {
                var siblings = $(this).siblings("select.selectpicker");
                if (parseInt($(this).val()) === commMax) {
                    siblings.each(function () {
                        $(this).selectpicker('val', 0);
                        $(this).prop('disabled', true);
                        $(this).selectpicker('refresh');
                    });
                }
                else {
                    siblings.each(function () {
                        $(this).prop('disabled', false);
                        $(this).selectpicker('refresh');
                    });
                }
            });
        },
        reRenderAfterPaged: function (currentPage, itemsPerPage) {
            if (gridOptions && gridOptions.pagerOption) {
                currentPage = currentPage >= 0 ? currentPage
                    : gridOptions.pagerOption.currentPage;
                itemsPerPage = itemsPerPage || gridOptions.pagerOption.itemsPerPage;
            }
            var params = {
                userName: $("#Username").val(),
                groupCommId: $("#GroupCommId").val(),
                pageIndex: currentPage,
                pageSize: itemsPerPage
            };

            $.ajax({
                url: $('#search-form').attr('action'),
                type: 'post',
                data: params,
                beforeSend: function () {
                    core.blockUI();
                },
                success: function (data) {
                    if (data.ErrCode !== 0) {
                        alert(data.ErrMsg);
                    } else {
                        gridOptions.pagerOption = {
                            itemsPerPage: data.ItemsPerPage,
                            itemsPerPageArray: core.pageSizes,
                            currentPage: data.CurrentPage,
                            totalItem: data.TotalItem,
                            pagerSelectCallBack: memberCommission.reRenderAfterPaged
                        };

                        gridOptions.bodyRows = core.genGridRows(data.List, gridOptions);
                        $('#grid').show().fanexGrid(gridOptions);
                    }
                },
                complete: function () {
                    core.unblockUI();
                    memberCommission.changeButtonState();
                }
            });
        },
        checkAllItem: function (checkAll) {
            if ($(checkAll).hasClass("icon-uncheck-all")) {
                $(checkAll).removeClass("icon-uncheck-all").addClass("icon-checked-all");
                $(".cell-select").removeClass("icon-uncheck").addClass("icon-checked");
            }
            else {
                $(checkAll).removeClass("icon-checked-all").addClass("icon-uncheck-all");
                $(".cell-select").removeClass("icon-checked").addClass("icon-uncheck");
            }
            memberCommission.changeButtonState();
        },
        selectClick: function (checkItem) {
            if ($(checkItem).hasClass("icon-uncheck")) {
                $(checkItem).removeClass("icon-uncheck").addClass("icon-checked");
            }
            else {
                $(checkItem).removeClass("icon-checked").addClass("icon-uncheck");
            }
            memberCommission.changeButtonState();
        },
        changeButtonState: function () {
            if ($(".cell-select").hasClass("icon-checked")) {
                $("#moveToGroup").removeClass("disabled");
                $("#updateCommission").removeClass("disabled");
                $("#removeFromGroup").removeClass("disabled");
            }
            else {
                $("#moveToGroup").addClass("disabled");
                $("#updateCommission").addClass("disabled");
                $("#removeFromGroup").addClass("disabled");
            }
        },
        getSelectedMembers: function () {
            var members = [];
            $(".icon-checked").each(function () {
                members.push($(this).data("cust-id"));
            });
            return members.join(',');
        },
        moveToGroup: function () {
            if (memberCommission.getSelectedMembers() !== '') {
                popup.openFrame("MemberCommission/MoveToGroup");
            }
        },
        updateCommission: function () {
            if (memberCommission.getSelectedMembers() !== '') {
                popup.openFrame("MemberCommission/UpdateMemberCommission");
            }
        },
        removeGroup: function () {
            if (memberCommission.getSelectedMembers() !== '') {
                popup.openFrame("MemberCommission/RemoveFromGroup");
            }
        },
        changeMemberCommission: function () {

            var memberIds = window.parent.memberCommission.getSelectedMembers();
            $("#MemberIds").val(memberIds);
            var submitOptions = $("#action-form").serializeObject();
            var actionName = $('#action-form').attr('action');

            $.ajax({
                url: actionName,
                type: 'post',
                data: jQuery.param(submitOptions),
                beforeSend: function () {
                    core.blockUI();
                },
                success: function (data) {
                    if (data.ErrCode !== 0) {
                        $(".messages").find("p").removeClass("success").addClass("error");
                        $(".messages").find("i").removeClass("icon-message-success").addClass("icon-message-error");
                        $(".messages").find("span").text(data.ErrMsg);
                    } else {
                        $(".messages").find("p").removeClass("error").addClass("success");
                        $(".messages").find("i").removeClass("icon-message-error").addClass("icon-message-success");
                        $(".messages").find("span").text(data.ErrMsg);
                        window.parent.memberCommission.updateGrid(data.Members);
                    }
                    $(".messages").show();
                },
                complete: function () {
                    core.unblockUI();
                }
            });
        },
        updateGrid: function (memberComms) {
            $("span.icon-checked").each(function () {
                var custId = $(this).data("cust-id");
                var memberComm = $.grep(memberComms, function (memberComm) {
                    return memberComm.CustId === custId;
                });
                var parent = $(this).parents("tr");
                parent.find(".group-comm").html(memberComm[0].GroupName);
                parent.find(".sb-comm").html(memberCommission.formatNumber(memberComm[0].SBComm));
                parent.find(".ng-comm").html(memberCommission.formatNumber(memberComm[0].NGComm));
            });
        },
        formatNumber: function (value) {
            return numeral(value * 100).format('0.00');
        }
    };
} (jQuery));

$(function () {
    'use strict';
    memberCommission.init();
});