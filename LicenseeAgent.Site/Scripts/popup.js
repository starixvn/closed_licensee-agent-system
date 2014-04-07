/*global core */
/*exported popup*/
'use strict';

var popup = (function ($) {
    var currentOpenDialog = null;

    return {
        openDialog: function (content, title, options) {
            var target = $('#app-modal-dialog');
            var htmlBody = content.clone();
            htmlBody.show();
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