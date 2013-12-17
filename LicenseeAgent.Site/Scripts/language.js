var LANGUAGE = function ($) {
    return {
        init: function () {
            $('#nav li').hover(function () {
                //show its submenu
                $('ul', this).stop().slideDown(100);
            }, function () {
                //hide its submenu
                $('ul', this).stop().slideUp(100);
            }
            );

            $('#languages li').click(function () {
                var languageCode = $(this).attr('data-language');
                LANGUAGE.changeLanguage(languageCode);
            });
        },
        changeLanguage: function (language) {
            var data = {
                Language: language
            };
            $.ajax({
                type: "POST",
                data: $.param(data),
                url: CORE.resolveUrl("~/Account/ChangeLanguage"),
                dataType: "json",
                async: false,
                timeout: 20000
            })
            .done(function (result) {
                if (result.isChanged) {
                    location.reload();
                }
            });
        },
        getSelectedLanguage: function () {
            if ($('#AppSelectedLanguage').length == 1) {
                return $('#AppSelectedLanguage').val().toLowerCase();
            }
            return 'en-us';
        },
        getSelectedLanguageCode: function () {
            var selectedLanguage = LANGUAGE.getSelectedLanguage();
            var selectedLanguageCode = selectedLanguage.substr(0, 2);
            if (selectedLanguageCode == 'zh') {
                selectedLanguageCode = selectedLanguage;
            }
            return selectedLanguageCode;
        }
    };
} (jQuery);
$(function () {
    LANGUAGE.init();
});