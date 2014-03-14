var POPUP = function ($) {
    var currentOpenDialog = null;
    var isResizeDialog = true;
    var dialogOpt = {
        position: { my: "top+10%", at: "center top", of: window },
        modal: true,
        width: 'auto'
    };

    return {
        openDialog: function (target, title, options) {
            options = options || {};
            if (title) options.title = title;
            if (target instanceof jQuery == false) target = $(target);
            if (options.clickout) {
                dialogOpt.clickOutside = true;
                dialogOpt.clickOutsideTrigger = '#' + options.openid;
            }
            currentOpenDialog = target.dialog($.extend({}, dialogOpt, options));
            return currentOpenDialog;
        },
        openDialogFrame: function (url, options) {
            url = CORE.resolveUrl(url) + '&popup';
            var frame = $('#app-dialog-frame');
            if (frame.length == 0) {
                frame = $('<div id="app-dialog-frame"></div>');
            }
            frame.html('<iframe class="loading" id="popup" src="' + '" width="100%" height="100%" frameborder="0" border="0">Test</iframe>');
            var iframe = frame.children('iframe');
            iframe.attr('src', url);
            $(iframe).load(function () {
                var popupDocument = document.getElementById("popup").contentDocument;
                // update popup title
                var popupTitle = popupDocument.title;
                $('.ui-dialog-title').html(popupTitle);
                // hide margin
                $(popupDocument).find('#mainContainer').addClass("popup");

                //auto width and height
                if (options == null || options == undefined) {
                    var body = this.contentWindow.document.documentElement;
                    $(frame).dialog("option", "width", body.scrollWidth + 50);
                    $(frame).dialog("option", "height", body.scrollHeight + 60);
                }
            });
            return POPUP.openDialog(frame, 'Loading ...', options);
        },
        closeDialog: function (target) {
            if (target == undefined) {
                target = currentOpenDialog;
            }
            if (target != null) {
                target.dialog("close");
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
                if (timeout == undefined) {
                    timeout = CORE.RELOAD_INTERVAL;
                }
                setTimeout(function () {
                    window.parent.POPUP.closeDialog();
                }, timeout);
                return true;
            }
            return false;
        },
        closeAndReload: function (timeout) {
            if (window.parent && window.parent.popup) {
                if (timeout == undefined) {
                    timeout = CORE.RELOAD_INTERVAL;
                }
                setTimeout(function () {
                    window.parent.POPUP.closeDialog();
                    window.parent.CORE.submitPage();
                }, timeout);
                return true;
            }
            return false;
        }
    }
} (jQuery);

//var POPUP = function ($) {
//    var currentOpenDialog = null;
//    var isResizeDialog = true;
//    var dialogOpt = {
//        position: { my: "top+10%", at: "center top", of: window },
//        modal: true,
//        width: 'auto'
//    };

//    return {
//        openDialog: function (content, title, options) {
//            var target = $('#app-modal-dialog');
//            var htmlBody = content.clone();
//            htmlBody.show();
//            if (target.length == 0) {
//                target = $('<div id="app-modal-dialog" class="modal hide fade" tabindex="-1" role="dialog"></div>');
//                var header = $('<div class="modal-header"><button type="button" class="close" data-dismiss="modal" data-toggle="modal" aria-hidden="true"></button><h3>Loading ...</h3></div>');
//                target.append(header);
//                var body = $('<div class="modal-body"></div>');
//                target.append(body);
//            }

//            target.find('.modal-body').html('<iframe class="loading" id="popup" src="' + '" width="100%" height="100%" frameborder="0" border="0">Test</iframe>');
//            var iframe = target.find('iframe');
//            $(iframe).load(function () {
//                var popupDocument = document.getElementById("popup").contentDocument;
//                $('.modal-header h3').html(title);
//                $('.modal-body').html(htmlBody);
//                $(popupDocument).find('#mainContainer').addClass("popup");
//                $('#app-modal-dialog').css({ width: options.width,
//                    height: options.height
//                });

//                $('.modal-body').css({ width: 'auto',
//                    height: options.height - 50
//                });
//            });

//            $('.modal-header h3').html(title);
//            $('.modal-body').html(htmlBody);

//            currentOpenDialog = target.modal('show');
//            return currentOpenDialog;
//        },
//        openDialogFrame: function (url, options) {
//            url = CORE.resolveUrl(url);
//            var target = $('#app-modal-dialog');
//            if (target.length == 0) {
//                target = $('<div id="app-modal-dialog" class="modal hide fade" tabindex="-1" role="dialog" data-backdrop="static" data-keyboard="false"></div>');
//                var header = $('<div class="modal-header"><button type="button" class="close"  data-dismiss="modal"></button><h3>Loading ...</h3></div>');
//                target.append(header);
//                var body = $('<div class="modal-body"></div>');
//                target.append(body);
//            }
//            target.find('.modal-body').html('<iframe class="loading" id="popup" src="' + '" width="100%" height="100%" frameborder="0" border="0">Test</iframe>');
//            var iframe = target.find('iframe');
//            iframe.attr('src', url);
//            $(iframe).load(function () {
//                var popupDocument = document.getElementById("popup").contentDocument;
//                //update popup title
//                var popupTitle = popupDocument.title;
//                $('.modal-header h3').html(popupTitle);
//                //hide margin
//                $(popupDocument).find('#mainContainer').addClass("popup");
//                $('#app-modal-dialog').css({ width: options.width,
//                    height: options.height,
//                    left: ($(window).width() - options.width) / 2 + 290
//                });

//                $('.modal-body').css({ width: 'auto',
//                    height: options.height - 50
//                });
//            });

//            $('#app-modal-dialog').modal({ backdrop: 'static', keyboard: false });
//            currentOpenDialog = target.modal('show');
//            return currentOpenDialog;
//        },
//        closeDialog: function (target) {
//            if (target == undefined) {
//                target = currentOpenDialog;
//            }
//            if (target != null) {
//                $('#app-modal-dialog').modal('hide');
//            }
//        },
//        resizeDialog: function (width, height) {
//            if (currentOpenDialog && currentOpenDialog.dialog("isOpen")) {
//                currentOpenDialog.dialog("option", { width: width + 30, height: height + 50 });
//            }
//        },
//        setDialogHeight: function (target, height) {
//            target.dialog("option", "height", height);
//        },
//        openWindow: function (url) {
//            window.open(url, '_blank');
//            window.focus();
//        },
//        close: function (timeout) {
//            if (window.parent && window.parent.popup) {
//                if (timeout == undefined) {
//                    timeout = CORE.RELOAD_INTERVAL;
//                }
//                setTimeout(function () {
//                    window.parent.POPUP.closeDialog();
//                }, timeout);
//                return true;
//            }
//            return false;
//        },
//        closeAndReload: function (timeout) {
//            if (window.parent && window.parent.popup) {
//                if (timeout == undefined) {
//                    timeout = CORE.RELOAD_INTERVAL;
//                }
//                setTimeout(function () {
//                    window.parent.POPUP.closeDialog();
//                    window.parent.CORE.submitPage();
//                }, timeout);
//                return true;
//            }
//            return false;
//        }
//    }
//} (jQuery);