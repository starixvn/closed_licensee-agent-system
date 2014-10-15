/*global language, moment */

var core = core || {};
core.components = (function ($) {
    'use strict';
    return {
        init: function () {
            // placeholder.
            $('input, textarea').placeholder();

            // date picker
            if ($.fn.datepicker) {
                $.fn.datepicker.defaults = $.extend($.fn.datepicker.defaults, {
                    autoclose: true,
                    language: language.getSelectedLanguageCode().toLowerCase(),
                    startDate: new Date('1/1/1900'),
                    endDate: new Date(core.ServerDate)
                });

                $('.datepicker input').prop('readonly', true);
                var value = $('.datepicker input').val();
                if (value === '') {
                    value = core.ServerDate;
                }
                //$('.datepicker').datepicker('update', new Date(value));
            }

            // customize scrollbars.
            if ($.fn.niceScroll) {
                $('html').niceScroll({
                    cursorcolor: "#8a8a8a",
                    autohidemode: false
                });

                $('body').mutate('height width', function () {
                    $('html').getNiceScroll().resize();
                });
            }

            // set href for button.
            $("button[data-href]").click(function () {
                location.href = core.resolveUrl($(this).attr("data-href"));
            });

            // handle click in iframe.
            if ($('#frmMain').length === 0) {
                $('html').on('click', function () {
                    if (parent !== null) {
                        parent.$('#frmMain').trigger('click');
                    }
                });
            }

            // moment
            if (typeof moment !== 'undefined') {
                moment.lang(language.getSelectedLanguageCode().toLowerCase());
            }

            // fanexGrid
            if ($.fn.fanexGrid) {
                //$.fn.fanexGrid.defaults.marginRight = 0;
                $.fn.fanexGrid.defaults.scrollWidth = 24;
                $.fn.fanexGrid.defaults.language = language.getSelectedLanguage();
                $.fn.fanexGrid.defaults.timeZone = core.ServerUtcOffset;
            }

            var fanexGridOrigin = jQuery.fn.fanexGrid;
            if (fanexGridOrigin !== undefined) {
                jQuery.fn.fanexGrid = function () {
                    jQuery.fn.fanexGrid = fanexGridOrigin;
                    return fanexGridOrigin.apply(this, arguments);
                };
            }

            // tooltip.
            $(document).click(function (e) {
                if ($().tooltip) {
                    $('.tooltip').prev().each(function (index, element) {
                        if (element === e.target) {
                            return;
                        }
                        if ($(e.target).is('button')) {
                            return;
                        }
                        if ($(e.target).attr('title')) {
                            return;
                        }
                        $(element).tooltip('hide');
                    });
                }
            });
        }
    };
} (jQuery));

$(function () {
    'use strict';
    core.components.init();
});