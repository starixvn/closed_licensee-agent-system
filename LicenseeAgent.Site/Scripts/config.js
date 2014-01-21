// override default blockUI message
$.blockUI.defaults.message = '<div class="loading"></div>';
$.blockUI.defaults.css = {
    padding: 0,
    margin: 0,
    width: '50px',
    top: '50%',
    left: '50%',
    textAlign: 'center',
    cursor: 'wait'
};

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