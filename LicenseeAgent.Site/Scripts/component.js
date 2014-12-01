/* jshint freeze:false */
/*global CORE, LANGUAGE, numeral, moment*/
'use strict';

// setup ajax.
$.ajaxSetup({
    error: function (jqXHR) {
        if (jqXHR.status === 0 || jqXHR.readyState === 0) {
            // ajax request is aborted.
            return;
        }
        var data = jqXHR.responseJSON;
        CORE.handleError(data);
    }
});

// Overwrite tooltip defaults.
if ($.fn.tooltip) {
    $.fn.tooltip.defaults.animation = false;
}

// inject validate unobtrusive
if (typeof ($.validator.unobtrusive) !== 'undefined') {
    $.validator.unobtrusive.adapters.add('notequalto', ['other'], function (options) {
        options.rules['notEqualTo'] = '#' + options.params.other;
        if (options.message) {
            options.messages['notEqualTo'] = options.message;
        }
    });

    $.validator.addMethod('notEqualTo', function (value, element, param) {
        return this.optional(element) || value !== $(param).val();
    }, '');

    $.validator.addMethod('requiredif', function (value, element, parameters) {
        var id = '#' + parameters['dependentproperty'];

        // get the target value (as a string,
        // as that's what actual value will be)
        var targetvalue = parameters['targetvalue'];
        targetvalue =
          (targetvalue == null ? '' : targetvalue).toString();

        // get the actual value of the target control
        // note - this probably needs to cater for more
        // control types, e.g. radios
        var control = $(id);
        var controltype = control.attr('type');
        var actualvalue =
            controltype === 'checkbox' ?
            control.attr('checked').toString() :
            control.val();

        // if the condition is true, reuse the existing
        // required field validator functionality
        if (targetvalue === actualvalue) {
            return $.validator.methods.required.call(this, value, element, parameters);
        }

        return true;
    });

    $.validator.unobtrusive.adapters.add('requiredif', ['dependentproperty', 'targetvalue'], function (options) {
        options.rules['requiredif'] = {
            dependentproperty: options.params['dependentproperty'],
            targetvalue: options.params['targetvalue']
        };
        options.messages['requiredif'] = options.message;
    });
}

// support function outerHTML
$.fn.outerHTML = function (s) {
    return s
        ? this.before(s).remove()
        : jQuery("<p>").append(this.eq(0).clone()).html();
};

// serialize a form into json object
$.fn.serializeObject = function () {
    var o = {};
    // find disabled inputs
    var inputs = this.find(':disabled');
    // enable them
    inputs.prop('disabled', false);
    var a = this.serializeArray();

    // then, disable them
    inputs.prop('disabled', true);

    $.each(a, function () {
        if (o[this.name] === undefined) {
            o[this.name] = this.value || '';
        }
    });
    return o;
};

// Implement string.format.
// Example: 'Hello {0}'.format('world') -> 'Hello world'
if (!String.prototype.format) {
    String.prototype.format = function () {
        var args = arguments;
        return this.replace(/{(\d+)}/g, function (match, number) {
            return typeof args[number] !== 'undefined' ? args[number] : match;
        });
    };
}

if ($.fn.fanexGrid) {
    //$.fn.fanexGrid.defaults.marginRight = 0;
    $.fn.fanexGrid.defaults.scrollWidth = 24;
    $.fn.fanexGrid.defaults.bodyRowHeight = 25;
    $.fn.fanexGrid.defaults.language = LANGUAGE.getSelectedLanguage();
    $.fn.fanexGrid.defaults.timeZone = CORE.ServerUtcOffset;

    var fanexGridOrigin = $.fn.fanexGrid;
    $.fn.fanexGrid = function () {
        $.fn.fanexGrid = fanexGridOrigin;
        return fanexGridOrigin.apply(this, arguments);
    };
}

// check whether a string is numberic or not
String.prototype.isNumber = function () {
    var regs = new RegExp(/^-?\d+(\.\d+)?\s*$/);
    return (regs.test(this));
};

// Getting the number of decimals in a number
Number.prototype.countDecimals = function () {
    if (Math.floor(this.valueOf()) === this.valueOf()) {
        return 0;
    }
    return this.toString().split(".")[1].length || 0;
};

Number.prototype.formatNumber = function (formatString) {
    if (typeof numeral === 'undefined') {
        alert('please include numeral.js');
        return '';
    }

    if (typeof formatString === 'undefined') {
        formatString = "0,0.00";
    }

    return numeral(this).format(formatString);
};

Number.prototype.formatNegative = function (formatString) {
    var formattedValue = this.formatNumber(formatString);
    return this < 0 ? '<div class="red-text">{0}</div>'.format(formattedValue) : formattedValue;
};

Number.MinInt = -9007199254740992;
Number.MaxInt = 9007199254740992;

var COMPONENT = (function ($this, $) {
    $this.init = function () {
        // placeholder.
        $('input, textarea').placeholder();

        // date picker
        if ($.fn.datepicker) {
            $.fn.datepicker.defaults = $.extend($.fn.datepicker.defaults, {
                autoclose: true,
                language: LANGUAGE.getSelectedLanguageCode().toLowerCase(),
                startDate: new Date('1/1/1900'),
                endDate: new Date(CORE.ServerDate)
            });

            $('.datepicker input').prop('readonly', true);
            var value = $('.datepicker input').val();
            if (value === '') {
                value = CORE.ServerDate;
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
            location.href = CORE.resolveUrl($(this).attr("data-href"));
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
            moment.lang(LANGUAGE.getSelectedLanguageCode().toLowerCase());
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
    };

    $this.registerCheckboxProductEvent = function () {
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
    };

    $this.setProductValue = function () {
        $('#Product').val(""); // Reset Product hidden to empty.
        $('.case:checked').each(function () {
            $('#Product').val($('#Product').val() + $(this).val() + ',');
        });
    };

    return $this;
} (COMPONENT || {}, jQuery));

$(function () {
    COMPONENT.init();
});