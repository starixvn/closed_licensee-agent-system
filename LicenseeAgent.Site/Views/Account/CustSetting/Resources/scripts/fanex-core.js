/* File Created: October 21, 2013 */

var fanex = fanex || {};

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

// Arguments: number to round, number of decimal places
fanex.roundNumber = function (number, decimals) {
    var newnumber = new Number(number + '').toFixed(parseInt(decimals));
    return parseFloat(newnumber); // Output the result to the form field (change for your purposes)
};

fanex.ShowInvalidRangeMessage = function (element, minValue, maxValue) {
    fanex.showHint(element, 'Invalid range, input must be in ['
                                + minValue
                                + ' - '
                                + maxValue
                                + ']');
};

fanex.showHint = function (element, content) {
    var option = {
        alignX: 'center'
    }
    fanex.showHintOption(element, content, option);
};

fanex.showHintDown = function (element, content) {
    var option = {
        alignX: 'center',
        alignY: 'bottom'
    }
    fanex.showHintOption(element, content, option);
};

fanex.showHintOption = function (element, content, option) {
    if (typeof fanex.hintTarget != 'undefined' && fanex.hintTarget[0] != element[0]) {
        fanex.hideHint();
    }
    if (typeof $(element).data('poshytip') == 'undefined') {
        if (typeof content == 'undefined') {
            content = $(element).data('hint');
        }
        var defaultOption = {
            content: content,
            alignTo: 'target',
            offsetX: 0,
            offsetY: 5,
            showOn: 'none',
            showTimeout: 0,
            className: 'tip-twitter'
        }
        option = $.extend({}, defaultOption, option);
        $(element).poshytip(option);
        $(element).data('poshytip').$tip.click(function () {
            fanex.hideHint();
        });
    }
    else {
        if ($(element).data('poshytip').content != content) {
            $(element).poshytip('update', content);
        }
    }
    if (fanex.hintTarget != element) {
        $(element).poshytip('show');
    }
    fanex.hintTarget = $(element);
}

fanex.hideHint = function (element) {
    if (typeof element == 'undefined') {
        element = fanex.hintTarget;
    }
    if (typeof element != 'undefined') {
        $(element).poshytip('hide');
    }
}

fanex.cloneObject = function (obj) {
    var newObj = (obj instanceof Array) ? [] : {};
    for (var i in obj) {
        if (i == 'clone') continue;
        if (obj[i] && typeof obj[i] == "object") {
            newObj[i] = fanex.cloneObject(obj[i]);
        } else {
            newObj[i] = obj[i];
        }
    }
    return newObj;
};

$.fn.preventSpecialCharacters = function (allowedChars) {
    this.bind('keypress', function (event) {
        //"^[0-9]+$": One example for digit numbers
        var regex = new RegExp(allowedChars);
        var key = !event.charCode ? event.which : event.charCode;
        var keyChar = String.fromCharCode(key);

        if ((key == null) || (key == 0) || (key == 8) || (key == 9) || (key == 13) || (key == 27))
            return true;
        if (!regex.test(keyChar)) {
            event.preventDefault();
            return false;
        }
    });

    this.bind('keyup', function (event) {
        if ($(this).val() == '')
            $(this).val('0');
    });
}