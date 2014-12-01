/*global LANGUAGE*/
'use strict';

var HOME_MENU = (function ($this, $) {
    var mainLeftCollapse = 0;

    $this.init = function () {
        // init event on root item
        $('.leftmenu-box a.tree-toggler').click(function () {
            if ($(this).hasClass('active')) {
                $(this).toggleClass('active');
                $(this).parent().children('ul.tree').toggle(300);
                return;
            }
            var navList = $(this).parent().parent();
            navList.find('a.tree-toggler').removeClass('active');
            navList.find('ul.tree').hide();
            $(this).toggleClass('active');
            $(this).parent().children('ul.tree').toggle(300);
        });

        // init event on sub item
        $('.leftmenu-box ul.tree > li > a').click(function () {
            clearActive();
            $(this).parent().addClass('active');
        });

        // init event on top menu items
        $('.banner-box ul.nav-pills li a').click(function () {
            clearActive();
            $(this).parent().addClass('active');
        });

        // collapse or exapnd main
        mainLeftCollapse = $('.main-box').offset().left;
        $('.control-box i').click(function () {
            toggleMain();
        });
        $('.leftmenu-box ul.nav > li > a').click(function () {
            if ($('.main-box').offset().left !== mainLeftCollapse) {
                toggleMain();
            }
        });
    };

    var clearActive = function () {
        $('.leftmenu-box ul.tree li.active').removeClass('active');
        $('.header-box ul.nav-pills li.active').removeClass('active');
    };

    $this.activeMenu = function (menuItem) {
        clearActive();
        $(menuItem).trigger('click');
        var anchor = $(menuItem).get(0);
        window.open(anchor.href, anchor.target, '');
    };

    var toggleMain = function () {
        var left = 0;
        if ($('.main-box').offset().left !== mainLeftCollapse) {
            left = mainLeftCollapse;
            $('.control-box i').attr('title', LANGUAGE.getString('Close'));
            $('.control-box i').removeClass('icon-collapse-home').addClass('icon-expand-home');
        }
        else {
            $('.control-box i').attr('title', LANGUAGE.getString('Open'));
            $('.control-box i').removeClass('icon-expand-home').addClass('icon-collapse-home');
        }
        $('.main-box').animate({
            left: '{0}px'.format(left)
        }, 'fast');
    };

    return $this;
} (HOME_MENU || {}, jQuery));

$(function () {
    HOME_MENU.init();
});