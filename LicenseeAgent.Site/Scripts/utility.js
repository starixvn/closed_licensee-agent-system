if ($.fn.tooltip) {
    $.fn.tooltip.defaults.animation = false;
}

// inject validate unobtrusive
if (typeof (jQuery.validator.unobtrusive) != 'undefined') {
    jQuery.validator.unobtrusive.adapters.add('notequalto', ['other'], function (options) {
        options.rules['notEqualTo'] = '#' + options.params.other;
        if (options.message) {
            options.messages['notEqualTo'] = options.message;
        }
    });

    jQuery.validator.addMethod('notEqualTo', function (value, element, param) {
        return this.optional(element) || value != $(param).val();
    }, '');
}

// support function outerHTML
jQuery.fn.outerHTML = function (s) {
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
        if (o[this.name] !== undefined) {
        } else {
            o[this.name] = this.value || '';
        }
    });
    return o;
};

$.fn.setBreadCrumb = function (agentId, agentLevel, username) {
    var currentBreadCrumb = $(this).val();
    var breadCrumb = agentId + "," + agentLevel + "," + username;
    var newBreadCrumb = "";
    if (currentBreadCrumb.indexOf(breadCrumb) >= 0)
        newBreadCrumb = currentBreadCrumb.substring(0, currentBreadCrumb.indexOf(breadCrumb) + breadCrumb.length);
    else
        newBreadCrumb = currentBreadCrumb + ";" + breadCrumb;
    $(this).val(newBreadCrumb);
};

// First, checks if it isn't implemented yet.
if (!String.prototype.format) {
    String.prototype.format = function () {
        var args = arguments;
        return this.replace(/{(\d+)}/g, function (match, number) {
            return typeof args[number] != 'undefined'
        ? args[number]
        : match
      ;
        });
    };
}

// check whether a string is numberic or not
String.prototype.isNumber = function () {
    var regs = new RegExp(/^-?\d+(\.\d+)?\s*$/);
    return (regs.test(this));
};

// Getting the number of decimals in a number
Number.prototype.countDecimals = function () {
    if (Math.floor(this.valueOf()) === this.valueOf()) return 0;
    return this.toString().split(".")[1].length || 0;
}

Number.MinInt = -9007199254740992;
Number.MaxInt = 9007199254740992;