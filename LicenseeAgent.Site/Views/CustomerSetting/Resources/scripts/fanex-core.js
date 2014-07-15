/*global core, fanex:true */

var fanex = fanex || {};

// Arguments: number to round, number of decimal places
fanex.roundNumber = function (number, decimals) {
    'use strict';
    var newnumber = Number(number + '').toFixed(parseInt(decimals));
    return parseFloat(newnumber); // Output the result to the form field (change for your purposes)
};

fanex.ShowInvalidRangeMessage = function (element, minValue, maxValue) {
    'use strict';
    fanex.showHint(element, 'Invalid range, input must be in ['
                                + minValue
                                + ' - '
                                + maxValue
                                + ']');
};

fanex.showHint = function (element, content) {
    'use strict';
    var option = {
        alignX: 'center'
    };
    fanex.showHintOption(element, content, option);
};

fanex.showHintDown = function (element, content) {
    'use strict';
    var option = {
        alignX: 'center',
        alignY: 'bottom'
    };
    fanex.showHintOption(element, content, option);
};

fanex.showHintOption = function (element, content) {
    'use strict';
    if (typeof fanex.hintTarget !== 'undefined' && fanex.hintTarget[0] !== element[0]) {
        fanex.hideHint();
    }
    if (typeof $(element).data('tooltip') === 'undefined') {
        if (typeof content === 'undefined') {
            content = $(element).data('hint');
        }
        $(element).tooltip({
            title: content,
            placement: $(element).data('placement'),
            trigger: 'manual',
            delay: { show: 500, hide: 100 }
        });
    } else {
        if ($(element).data('tooltip').options.title !== content) {
            $(element).data('tooltip').options.title = content;
        }
    }
    if (fanex.hintTarget !== element) {
        if (!core.isScrolledIntoView(element)) {
            core.scrollTo(element);
        }
        $(element).tooltip('show');
    }
    fanex.hintTarget = $(element);
};

fanex.hideHint = function (element) {
    'use strict';
    if (typeof element === 'undefined') {
        element = fanex.hintTarget;
    }
    if (typeof element !== 'undefined') {
        $(element).tooltip('hide');
    }
};

fanex.cloneObject = function (obj) {
    'use strict';
    var newObj = (obj instanceof Array) ? [] : {};
    for (var i in obj) {
        if (i === 'clone') {
            continue;
        }
        if (obj[i] && typeof obj[i] === "object") {
            newObj[i] = fanex.cloneObject(obj[i]);
        } else {
            newObj[i] = obj[i];
        }
    }
    return newObj;
};

fanex.isChangedData = function (productSetting) {
    'use strict';
    var isChangedCommission = productSetting.CommissionSetting === undefined ? false :
                                (productSetting.CommissionSetting !== null &&
                                productSetting.CommissionSetting.CommissionItems !== null &&
                                productSetting.CommissionSetting.CommissionItems.length > 0);
    var isChangedBetSetting = productSetting.BettingLimitSetting === undefined ? false :
                                (productSetting.BettingLimitSetting !== null && productSetting.BettingLimitSetting.length > 0);
    var isChangedPositionTaking = productSetting.PositionTakingSetting === undefined ? false :
                                    (productSetting.PositionTakingSetting != null && productSetting.PositionTakingSetting.length > 0);

    return isChangedCommission || isChangedBetSetting || isChangedPositionTaking;
};