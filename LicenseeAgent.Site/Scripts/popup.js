/*global core */
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
        hasScroll: false,
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

            this.$modal = modal;

            if (this.options.buttons.length > 0) {
                var modalFooter = $('<div class="modal-footer"></div>');
                modalContent.append(modalFooter);
                var $this = this;
                $.each(this.options.buttons, function (index, button) {
                    $this.addFooterButton(button);
                });
            }

            if (this.options.onHide == null) {
                modal.on('hidden', this.options.onHide);
            }

            if (this.options.autoDestroy) {
                modal.on('hidden', function () {
                    modal.remove();
                });
            }

            return this;
        },
        showConfirm: function () {
            return this.init().setTitle(this.options.title).setBodyContent(this.options.message).show().calculate();
        },
        showFrame: function (url) {
            this.init();
            var iframe = $('<iframe id="{0}" src="{1}" width="100%" height="100%" frameborder="0" border="0"/>'.format(Popup.iframeId, url));
            this.setBodyContent(iframe);
            var modal = this.getModal();
            modal.css('display', 'none');
            $('body').append(modal);
            core.blockUI();
            var $this = this;
            $(iframe).load(function () {
                var popupDocument = this.contentDocument;
                $(popupDocument).find('#mainContainer').addClass("popup");
                var title = $(popupDocument).attr('title');
                $this.setTitle(title).calculate().show();
                core.unblockUI();

                $(this).resize(function () {
                    alert('test');
                });
            });

            return this;
        },
        showContent: function () {
            this.init().setTitle(this.options.title).setBodyContent(this.options.message).show().calculate();
        },
        getModal: function () {
            return this.$modal;
        },
        setTitle: function (title) {
            this.$modal.find('.modal-header h3').text(title);
            return this;
        },
        setBodyContent: function (content) {
            this.$modal.find('.modal-body').html(content);
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
        calculate: function () {
            var modalBody = this.getModalBody();
            var innerBody = modalBody.children();
            if (innerBody.length === 0) {
                innerBody = modalBody;
            } else if (modalBody.children().prop('tagName').toLowerCase() === 'iframe') {
                innerBody = innerBody.contents();
            }
            var width = this.options.width;
            if (width === 0) {
                width = innerBody.width();
                if (this.options.hasScroll) {
                    width += 20;
                }
                var maxWidth = $(window).width() - 24;
                width = Math.min(width, maxWidth);
            }
            var height = this.options.height;
            if (height === 0) {
                height = innerBody.height();
                var maxHeight = $(window).height() - 24;
                height = Math.min(height, maxHeight);
            }
            modalBody.css({
                width: width,
                height: height
            });

            var modal = this.getModal();
            var left = this.options.left;
            if (left === 0) {
                left = ($(window).width() - modal.outerWidth()) / 2;
            }
            var top = this.options.top;
            if (top === 0) {
                if ($(window).height() > 2 * modal.outerHeight()) {
                    top = 100;
                }
                else {
                    top = ($(window).height() - modal.outerHeight()) / 2;
                }
            }
            modal.css({
                left: left,
                top: top
            });

            return this;
        },
        show: function () {
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
            this.$modal.modal('hide');
            return this;
        },
        close: function () {
            this.$modal.modal('hide');
            this.$modal.remove();
            return this;
        }
    };

    var currentPopup = null;
    return {
        confirm: function (options) {
            currentPopup = new Popup(options).showConfirm();
        },
        openFrame: function (url, options) {
            currentPopup = new Popup(options).showFrame(core.resolveUrl(url));
        },
        open: function (options) {
            currentPopup = new Popup(options).showContent();
        },
        isIframe: function () {
            if (window !== window.parent && window.parent.document.getElementById(Popup.iframeId)) {
                return true;
            }
            return false;
        },
        close: function (timeout) {
            if (typeof timeout === 'undefined') {
                timeout = core.RELOAD_INTERVAL;
            }

            if (popup.isIframe()) {
                window.parent.popup.close(timeout);
            } else if (currentPopup) {
                setTimeout(function () {
                    currentPopup.close();
                }, timeout);
            }
        },
        closeAndReload: function (timeout) {
            if (typeof timeout === 'undefined') {
                timeout = core.RELOAD_INTERVAL;
            }

            if (popup.isIframe()) {
                window.parent.popup.closeAndReload(timeout);
            } else if (currentPopup) {
                setTimeout(function () {
                    currentPopup.close();
                    core.submitPage();
                }, timeout);
            }
        }
    };
} (jQuery));