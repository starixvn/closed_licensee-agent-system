/*global core, language */
/*exported popup*/

var popup = (function ($) {
    'use strict';
    var Popup = function (options) {
        this.options = $.extend({}, Popup.defaultOptions, options);
    };

    Popup.defaultOptions = {
        title: null,
        message: null,
        buttons: [],
        onHide: null,
        autoDestroy: true,
        draggable: false,
        clickout: false,
        top: 0, // auto-top
        left: 0, //auto-left
        width: 0, // auto-width
        height: 0 // auto-height
    };

    Popup.defaultName = 'popup-dialog';
    Popup.iframeId = 'popup-iframe';
    Popup.prototype = {
        init: function () {
            var modal = $('#{0}'.format(Popup.defaultName));
            if (modal.length === 0) {
                modal = $('<div id="{0}" class="modal" tabindex="-1"></div>'.format(Popup.defaultName));
            }
            else {
                modal.html('');
            }
            var modalDialog = $('<div class="modal-dialog"></div>');
            modal.append(modalDialog);
            var modalContent = $('<div class="modal-content"></div>');
            modalDialog.append(modalContent);
            var modalHeader = $('<div class="modal-header"><button type="button" class="close" data-dismiss="modal"\
             aria-hidden="true"></button><h3>&nbsp;</h3></div>');
            var modalBody = $('<div class="modal-body">&nbsp;</div>');
            modalContent.append(modalHeader);
            modalContent.append(modalBody);
            modal.css({
                'visibility': 'hidden',
                'top': 0,
                'left': 0
            });
            $('body').append(modal);

            var $this = this;
            this.$modal = modal;
            this.$paddingWidth = modalContent.width() - modalBody.width() + 4;
            this.$paddingHeight = modalContent.height() - modalBody.height();
            this.$timeout = null;
            this.$onWindowResize = function () {
                $this.calculate();
            };
            $(window).on('resize', this.$onWindowResize);

            if (this.options.buttons.length > 0) {
                var modalFooter = $('<div class="modal-footer"></div>');
                modalContent.append(modalFooter);
                $.each(this.options.buttons, function (index, button) {
                    $this.addFooterButton(button);
                });
            }

            if (this.options.onHide != null) {
                modal.on('hidden', this.options.onHide);
            }

            if (this.options.autoDestroy) {
                modal.on('hidden', function () {
                    $this.remove();
                });
            }

            return this;
        },
        showConfirm: function () {
            return this.init().setTitle(this.options.title).setBodyContent(this.options.message).show();
        },
        showFrame: function (url) {
            this.init();
            var iframe = $('<iframe id="{0}" src="{1}" width="100%" height="100px" frameborder="0" border="0"/>'.format(Popup.iframeId, url));
            this.setBodyContent(iframe);
            this.getModalBody().css({
                width: 0,
                height: 0
            });
            core.blockUI();
            var $this = this;
            $(iframe).load(function () {
                var popupDocument = this.contentDocument;
                var mainContainer = $(popupDocument).find('#mainContainer');
                mainContainer.addClass("popup");
                var popupBody = $(iframe).contents().find('body');
                popupBody.css('overflow', 'hidden'); // trick IE
                var title = $(popupDocument).attr('title');
                $this.setTitle(title).show();
                core.unblockUI();
                $this.clearTimeout();
                popupBody.css('overflow', ''); // trick IE
                $this.calculate();
                $this.autoSize();
            });

            return this;
        },
        showContent: function () {
            this.init().setTitle(this.options.title).setBodyContent(this.options.message, true).show();
        },
        getModal: function () {
            return this.$modal;
        },
        setTitle: function (title) {
            this.$modal.find('.modal-header h3').text(title);
            return this;
        },
        setBodyContent: function (content, hasWrapper) {
            var html = content;
            if (hasWrapper) {
                html = '<div class="modal-body-wrapper">' + content + '</div>';
            }
            this.$modal.find('.modal-body').html(html);
            return this;
        },
        getModalBody: function () {
            return this.$modal.find('.modal-body');
        },
        getModalHeader: function () {
            return this.$modal.find('.modal-header');
        },
        getModalFooter: function () {
            return this.$modal.find('.modal-footer');
        },
        addFooterButton: function (button) {
            var $button = $('<button class="btn"></button>');

            // Label
            if (typeof button.label !== undefined) {
                $button.append(button.label);
            }

            // Css class
            if (typeof button.cssClass !== undefined && $.trim(button.cssClass) !== '') {
                $button.addClass(button.cssClass);
            } else {
                $button.addClass('btn-primary');
            }

            // Button on click
            $button.on('click', { dialog: this, $button: $button, button: button }, function (event) {
                var dialog = event.data.dialog;
                var $button = event.data.$button;
                var button = event.data.button;
                if (typeof button.action === 'function') {
                    button.action.call($button, dialog);
                }
            });

            this.getModalFooter().append($button);
            return $button;
        },
        getInnerBody: function () {
            var modalBody = this.getModalBody();
            var innerBody = modalBody.children();
            if (innerBody.length === 0) {
                innerBody = modalBody;
            } else if (modalBody.children().prop('tagName').toLowerCase() === 'iframe') {
                innerBody = modalBody.children().contents();
            }
            return innerBody;
        },
        getInnerBodySize: function () {
            var innerBody = this.getInnerBody();
            if (innerBody.length === 0) {
                return null;
            }
            if (innerBody[0].body === null) {
                return null;
            }
            var width = innerBody.width();
            var height = innerBody.height();
            return {
                width: width,
                height: height
            };
        },
        calculate: function () {
            var innerBodySize = this.getInnerBodySize();
            if (innerBodySize == null) {
                return;
            }

            var maxWidth = $(window).width() - this.$paddingWidth;
            var width = this.options.width;
            if (width === 0) {
                width = innerBodySize.width;
            }
            width = Math.min(width, maxWidth);

            var maxHeight = $(window).height() - this.$paddingHeight;
            var height = this.options.height;
            if (height === 0) {
                height = innerBodySize.height;
            }
            height = Math.min(height, maxHeight);

            var modalBody = this.getModalBody();
            modalBody.css({
                width: width,
                height: height
            });

            var modal = this.getModal();
            var left = this.options.left;
            if (left === 0) {
                left = Math.max(($(window).width() - modal.width()) / 2, 0);
            }
            var top = this.options.top;
            if (top === 0) {
                top = Math.min(Math.max(($(window).height() - modal.height()) / 2, 0), 100);
            }
            modal.css({
                left: left,
                top: top
            });

            return this;
        },
        show: function () {
            this.calculate();
            this.$modal.css('visibility', 'visible');
            if (this.options.clickout) {
                this.$modal.modal();
            }
            else {
                this.$modal.modal({
                    backdrop: 'static',
                    keyboard: false
                });
            }
            return this;
        },
        hide: function () {
            if (this.$modal != null) {
                this.$modal.modal('hide');
            }
            this.remove();
            return this;
        },
        close: function () {
            this.hide();
            this.remove();
            return this;
        },
        remove: function () {
            $(window).off('resize', this.$onWindowResize);
            if (this.$modal) {
                this.$modal.remove();
                this.$modal = null;
            }
            return this;
        },
        clearTimeout: function () {
            if (this.$timeout != null) {
                window.clearTimeout(this.$timeout);
                this.$timeout = null;
            }
        },
        autoSize: function () {
            if (this.options.width !== 0 && this.options.height !== 0) {
                return;
            }
            var $this = this;
            var currentSize = this.getInnerBodySize();
            var internalAutoSize = function () {
                if ($this.$modal == null) {
                    return;
                }
                var newSize = $this.getInnerBodySize();
                if (newSize != null && (currentSize.width !== newSize.width || currentSize.height !== newSize.height)) {
                    $this.calculate();
                    currentSize = newSize;
                }
                $this.$timeout = setTimeout(function () {
                    internalAutoSize();
                }, 200);
            };
            internalAutoSize();
        },
        setHideCallback: function (callback) {
            if (this.$modal != null) {
                this.$modal.on('hidden', callback);
            }
        }
    };

    var $currentPopup = null;
    var $closeTimeout = null;
    return {
        error: function (options) {
            var defaultOptions = {
                title: language.getString('Error'),
                buttons: [{
                    label: language.getString('OK'),
                    action: function () {
                        popup.close(0);
                    }
                }]
            };
            var $options = $.extend({}, defaultOptions, options);
            popup.confirm($options);
        },
        confirm: function (options) {
            $currentPopup = new Popup(options).showConfirm();
        },
        openFrame: function (url, options) {
            $currentPopup = new Popup(options).showFrame(core.resolveUrl(url));
        },
        open: function (options) {
            $currentPopup = new Popup(options).showContent();
        },
        isIframe: function () {
            if ($(document).find('#' + Popup.iframeId).length === 0 &&
                $(window.parent.document).find('#' + Popup.iframeId).length === 1) {
                return true;
            }
            return false;
        },
        close: function (timeout) {
            window.clearTimeout($closeTimeout);
            if (typeof timeout === 'undefined') {
                timeout = core.RELOAD_INTERVAL;
            }

            if (popup.isIframe()) {
                window.parent.popup.close(timeout);
            } else if ($currentPopup) {
                this.onHide();
                $closeTimeout = setTimeout(function () {
                    $currentPopup.close();
                }, timeout);
            }
        },
        closeAndReload: function (timeout) {
            window.clearTimeout($closeTimeout);
            if (typeof timeout === 'undefined') {
                timeout = core.RELOAD_INTERVAL;
            }

            if (popup.isIframe()) {
                window.parent.popup.closeAndReload(timeout);
            } else if ($currentPopup) {
                this.onHide();
                $closeTimeout = setTimeout(function () {
                    $currentPopup.close();
                }, timeout);
                setTimeout(function () {
                    core.submitPage();
                }, timeout);
            }
        },
        onHide: function () {
            if ($currentPopup != null) {
                $currentPopup.setHideCallback(function () {
                    if ($closeTimeout != null) {
                        window.clearTimeout($closeTimeout);
                        $closeTimeout = null;
                    }
                });
            }
        }
    };
} (jQuery));