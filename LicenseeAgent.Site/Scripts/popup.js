/*global core */
/*exported popup*/
'use strict';

var popup = (function ($) {
    var currentOpenDialog = null;
    return {
        createModal: function (id) {
            var modal = $('#{0}'.format(id));
            if (modal.length === 0) {
                modal = $('<div id="{0}" class="modal fade" tabindex="-1"></div>'.format(id));
            }
            else {
                modal.html('');
            }
            var modalDialog = $('<div class="modal-dialog"></div>');
            modal.append(modalDialog);
            var modalContent = $('<div class="modal-dialog"></div>');
            modalDialog.append(modalContent);
            var modalHeader = $('<div class="modal-header"><button type="button" class="close" data-dismiss="modal"\
             aria-hidden="true"></button><h3></h3></div>');
            var modalBody = $('<div class="modal-body"></div>');
            var modalFooter = $('<div class="modal-footer"></div>');
            modalContent.append(modalHeader);
            modalContent.append(modalBody);
            modalContent.append(modalFooter);
            this.$modal = modal;
            return modal;
        },
        setModalTitle: function (title) {
            this.$modal.find('.modal-header h3').text(title);
        },
        setModalBody: function (content) {
            this.$modal.find('.modal-body').text(content);
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

            this.$modal.find('.modal-footer').append($button);

            return $button;
        },
        showModal: function () {
            this.$modal.modal();
        },
        closeModal: function () {
            this.$modal.modal('hide');
            this.$modal.remove();
        },
        confirm: function (content, title, buttons, onHide) {
            var $modal = this.createModal('confirm-dialog');
            this.setModalTitle(title);
            this.setModalBody(content);

            var $this = this;
            if (buttons) {
                $.each(buttons, function (index, button) {
                    $this.addFooterButton(button);
                });
            }

            if (onHide) {
                $modal.on('hide', onHide);
            }

            this.showModal();
        },
        openDialog: function (content, title, options) {
            var target = $('#app-modal-dialog');
            var htmlBody = jQuery.type(content) === "string" ? content : content.clone();
            if (jQuery.type(content) !== "string") {
                htmlBody.show();
            }

            if (target.length === 0) {
                target = $('<div id="app-modal-dialog" class="modal hide fade" tabindex="-1" role="dialog"></div>');
                var header = $('<div class="modal-header"><button type="button" class="close" data-dismiss="modal" ' +
                                'data-toggle="modal" aria-hidden="true"></button><h3>Loading ...</h3></div>');
                target.append(header);
                var body = $('<div class="modal-body"></div>');
                target.append(body);

                target.find('.modal-body').html('<iframe class="loading" id="popup" src="' +
                        '" width="100%" height="100%" frameborder="0" border="0">Test</iframe>');
                var iframe = target.find('iframe');

                $(iframe).load(function () {
                    var popupDocument = document.getElementById("popup").contentDocument;
                    $(popupDocument).find('#mainContainer').addClass("popup");
                    $('.modal-header h3').html(title);
                    $('.modal-body').html(htmlBody);
                    $('#app-modal-dialog').css({ width: options.width,
                        height: options.height,
                        left: ($(window).width() - options.width) / 2 + 290
                    });

                    $('.modal-body').css({ width: 'auto',
                        height: options.height - 50
                    });
                });
            }

            $('#app-modal-dialog').css({ width: options.width,
                height: options.height,
                left: ($(window).width() - options.width) / 2 + 290
            });

            $('.modal-body').css({ width: 'auto',
                height: options.height - 50
            });

            $('.modal-header h3').html(title);
            $('.modal-body').html(htmlBody);

            currentOpenDialog = target.modal('show');
            return currentOpenDialog;
        },
        openDialogFrame: function (url, options) {
            url = core.resolveUrl(url);
            var target = $('#app-modal-dialog');
            if (target.length === 0) {
                target = $('<div id="app-modal-dialog" class="modal hide fade" tabindex="-1" role="dialog" ' +
                                'data-backdrop="static" data-keyboard="false"></div>');
                var header = $('<div class="modal-header"><button type="button" class="close"  data-dismiss="modal"></button><h3>Loading ...</h3></div>');
                target.append(header);
                var body = $('<div class="modal-body"></div>');
                target.append(body);
            }
            target.find('.modal-body').html('<iframe class="loading" id="popup" src="' +
                        '" width="100%" height="100%" frameborder="0" border="0">Test</iframe>');
            var iframe = target.find('iframe');
            iframe.attr('src', url);
            $(iframe).load(function () {
                var popupDocument = document.getElementById("popup").contentDocument;
                //update popup title
                var popupTitle = popupDocument.title;
                $('.modal-header h3').html(popupTitle);
                //hide margin
                $(popupDocument).find('#mainContainer').addClass("popup");
                $('#app-modal-dialog').css({ width: options.width,
                    height: options.height,
                    left: ($(window).width() - options.width) / 2 + 290
                });

                $('.modal-body').css({ width: 'auto',
                    height: options.height - 50
                });
            });

            currentOpenDialog = target.modal('show');
            return currentOpenDialog;
        },
        closeDialog: function (target) {
            if (target === undefined) {
                target = currentOpenDialog;
            }
            if (target != null) {
                $('#app-modal-dialog').modal('hide');
            }
        },
        resizeDialog: function (width, height) {
            if (currentOpenDialog && currentOpenDialog.dialog("isOpen")) {
                currentOpenDialog.dialog("option", { width: width + 30, height: height + 50 });
            }
        },
        setDialogHeight: function (target, height) {
            target.dialog("option", "height", height);
        },
        openWindow: function (url) {
            window.open(url, '_blank');
            window.focus();
        },
        close: function (timeout) {
            if (window.parent && window.parent.popup) {
                if (timeout === undefined) {
                    timeout = core.RELOAD_INTERVAL;
                }
                setTimeout(function () {
                    window.parent.popup.closeDialog();
                }, timeout);
                return true;
            }
            return false;
        },
        closeAndReload: function (timeout) {
            if (window.parent && window.parent.popup) {
                if (timeout === undefined) {
                    timeout = core.RELOAD_INTERVAL;
                }
                setTimeout(function () {
                    window.parent.popup.closeDialog();
                    window.parent.core.submitPage();
                }, timeout);
                return true;
            }
            return false;
        }
    };
} (jQuery));