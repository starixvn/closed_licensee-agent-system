/*exported BREADCRUMB*/
'use strict';

var BREADCRUMB = (function ($this, $) {
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

    $this.init = function () {
        container = $('.breadcrumb');
        var item = container.data('item');
        if (isValid(item)) {
            items.push(item);
        }
        $this.render();
        return container;
    };

    $this.hide = function () {
        container.hide();
    };

    $this.show = function () {
        container.show();
    };

    $this.getContainer = function () {
        return container;
    };

    $this.getLastItem = function () {
        return items[items.length - 1];
    };

    $this.addItem = function (item) {
        if (isValid(item)) {
            items.push(item);
            $this.render();
        }
    };

    $this.render = function () {
        container.empty();

        if (items.length === 0 || (items.length === 1 && items[0].IsLink !== true)) {
            $this.hide();
            return;
        } else {
            $this.show();
        }

        var itemClicked = function () {
            var data = $(this).data('data');
            var guid = $(this).data('guid');
            for (var j = 0; j < items.length; j++) {
                var item = items[j];
                if (item.Guid === guid) {
                    var index = j + 1;
                    var total = items.length - index;
                    items.splice(index, total);
                    $this.render();
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
    };

    return $this;
} (BREADCRUMB || {}, jQuery));