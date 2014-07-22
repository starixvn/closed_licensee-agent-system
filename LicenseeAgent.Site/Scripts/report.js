/*exported report*/

var report = (function ($) {
    'use strict';
    return {
        Running: 0,
        Waiting: 1,
        Reject: 101, 
        Void: 102, 
        Refund: 103, 
        Draw: 111, 
        Won: 112,
        Lose: 113,
        SettledNoDraw: "112,113",
        SettledHasDraw: "111,112,113",
        Betting: "101,102,103,111,112,113",
        Outstanding: "0,1",
        registerCheckboxProductEvent: function () {
            // add multiple select / deselect functionality
            $("#selectAll").click(function () {
                $('.case').prop('checked', this.checked);
            });

            // if all checkbox are selected, check the selectall checkbox and viceversa
            $(".case").click(function () {
                if ($(".case").length === $(".case:checked").length) {
                    $("#selectAll").prop("checked", true);
                } else {
                    $("#selectAll").removeAttr("checked");
                }
            });
        },

        setProductValue: function () {
            $('#Product').val(""); // Reset Product hidden to empty.
            $('.case:checked').each(function () {
                $('#Product').val($('#Product').val() + $(this).val() + ',');
            });
        }
    };
} (jQuery));