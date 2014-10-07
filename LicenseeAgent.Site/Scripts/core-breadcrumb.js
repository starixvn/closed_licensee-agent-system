var core = core || {};
core.breadcrumb = (function ($) {
    'use strict';
    var container;
    var items = [];
    var isValid = function (item) {
        if (item === null || typeof item === 'undefined') {
            return false;
        }

        if (typeof item.Text === 'undefined') {
            return false;
        }

        if (typeof item.Data === 'undefined') {
            return false;
        }

        item.Guid = (new Date()).getTime();
        return true;
    };

    return {
        init: function () {
            container = $('.breadcrumb');
            var item = container.data('item');
            if (isValid(item)) {
                items.push(item);
            }
            core.breadcrumb.render();
            return container;
        },
        hide: function () {
            container.hide();
        },
        show: function () {
            container.show();
        },
        getContainer: function () {
            return container;
        },
        getLastItem: function () {
            return items[items.length - 1];
        },
        addItem: function (item) {
            if (isValid(item)) {
                items.push(item);
                core.breadcrumb.render();
            }
        },
        render: function () {
            container.empty();

            if (items.length <= 1) {
                core.breadcrumb.hide();
                return;
            } else {
                core.breadcrumb.show();
            }

            var itemClicked = function () {
                var data = $(this).data('data');
                var guid = $(this).data('guid');
                for (var j = 0; j < items.length; j++) {
                    var item = items[j];
                    if (item.Guid === guid) {
                        items.splice(j + 1);
                        core.breadcrumb.render();
                        break;
                    }
                }

                container.trigger('itemClicked', data);
            };
            for (var i = 0; i < items.length; i++) {
                var item = items[i];
                var li = $('<li>');
                container.append(li);
                var element;
                if (i === items.length - 1 && !item.IsLink) {
                    element = $('<span>{0}</span>'.format(item.Text));
                } else {
                    element = $('<a href="#">{0}</a>'.format(item.Text));

                    element.click(itemClicked);
                }
                li.append(element);
                element.data('data', item.Data);
                if (i !== items.length - 1) {
                    li.append('<span class="divider">>></span>');
                }
                element.data('guid', item.Guid);
            }
        }
    };
} (jQuery));