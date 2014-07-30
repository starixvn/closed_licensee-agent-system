/* jshint freeze:false */
/*global numeral*/

// Overwrite tooltip defaults.
if ($.fn.tooltip) {
    $.fn.tooltip.defaults.animation = false;
}

// inject validate unobtrusive
if (typeof ($.validator.unobtrusive) !== 'undefined') {
    $.validator.unobtrusive.adapters.add('notequalto', ['other'], function (options) {
        'use strict';
        options.rules['notEqualTo'] = '#' + options.params.other;
        if (options.message) {
            options.messages['notEqualTo'] = options.message;
        }
    });

    $.validator.addMethod('notEqualTo', function (value, element, param) {
        'use strict';
        return this.optional(element) || value !== $(param).val();
    }, '');

    $.validator.addMethod('requiredif', function (value, element, parameters) {
        'use strict';
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
        'use strict';
        options.rules['requiredif'] = {
            dependentproperty: options.params['dependentproperty'],
            targetvalue: options.params['targetvalue']
        };
        options.messages['requiredif'] = options.message;
    });
}

// support function outerHTML
jQuery.fn.outerHTML = function (s) {
    'use strict';
    return s
        ? this.before(s).remove()
        : jQuery("<p>").append(this.eq(0).clone()).html();
};

// serialize a form into json object
$.fn.serializeObject = function () {
    'use strict';
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

$.fn.setBreadCrumb = function (agentId, agentLevel, username) {
    'use strict';
    var currentBreadCrumb = $(this).val();
    var breadCrumb = agentId + "," + agentLevel + "," + username;
    var newBreadCrumb = "";
    if (currentBreadCrumb.indexOf(breadCrumb) >= 0) {
        {
            newBreadCrumb = currentBreadCrumb.substring(0, currentBreadCrumb.indexOf(breadCrumb) + breadCrumb.length);
        }
    } else {
        newBreadCrumb = currentBreadCrumb + ";" + breadCrumb;
    }
    $(this).val(newBreadCrumb);
};

// Implement string.format.
// Example: 'Hello {0}'.format('world') -> 'Hello world'
if (!String.prototype.format) {
    String.prototype.format = function () {
        'use strict';
        var args = arguments;
        return this.replace(/{(\d+)}/g, function (match, number) {
            return typeof args[number] !== 'undefined' ? args[number] : match;
        });
    };
}

// check whether a string is numberic or not
String.prototype.isNumber = function () {
    'use strict';
    var regs = new RegExp(/^-?\d+(\.\d+)?\s*$/);
    return (regs.test(this));
};

// Getting the number of decimals in a number
Number.prototype.countDecimals = function () {
    'use strict';
    if (Math.floor(this.valueOf()) === this.valueOf()) {
        return 0;
    }
    return this.toString().split(".")[1].length || 0;
};

Number.prototype.formatNumber = function (formatString) {
    'use strict';
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
    'use strict';
    var formattedValue = this.formatNumber(formatString);
    return this < 0 ? '<div class="red-text">{0}</div>'.format(formattedValue) : formattedValue;
};

Number.MinInt = -9007199254740992;
Number.MaxInt = 9007199254740992;