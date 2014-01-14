var MENU = function ($) {
    return {
        init: function () {
            // init event on root item
            $('.leftmenu-box a.tree-toggler').click(function () {
                if ($(this).hasClass('active')) {
                    $(this).toggleClass('active');
                    $(this).parent().children('ul.tree').toggle(300);
                    return;
                };
                var navList = $(this).parent().parent();
                navList.find('a.tree-toggler').removeClass('active');
                navList.find('ul.tree').hide();
                $(this).toggleClass('active');
                $(this).parent().children('ul.tree').toggle(300);
            });

            // init event on sub item
            $('.leftmenu-box ul.tree > li > a').click(function () {
                MENU.clearActive();
                $(this).parent().addClass('active');
            });

            // init event on top menu items
            $('.banner-box ul.nav-pills li a').click(function (e) {
                MENU.clearActive();
                $(this).parent().addClass('active');
            })
        },
        clearActive: function () {
            $('.leftmenu-box ul.tree li.active').removeClass('active');
            $('.header-box ul.nav-pills li.active').removeClass('active');
        },
        activeMenu: function (menuItem) {
            MENU.clearActive();
            $(menuItem).trigger('click');
            var anchor = $(menuItem).get(0);
            window.open(anchor.href, anchor.target, '');
        }
    };
} (jQuery);
$(function () {
    MENU.init();
});