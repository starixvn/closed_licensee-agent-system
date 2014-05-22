/*global appSettings, core, fanexGridLanguage*/

var language = (function ($) {
    'use strict';
    var defaultLanguageCode = 'en-US';
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
                language.changeLanguage(languageCode);
            });

            var selectedLanguage = language.getSelectedLanguage();
            if (typeof fanexGridLanguage !== 'undefined') {
                window.fanexGridCurrentLanguage = fanexGridLanguage[selectedLanguage];
            }
        },
        changeLanguage: function (language) {
            var data = {
                Language: language
            };
            $.ajax({
                type: "POST",
                data: $.param(data),
                url: core.resolveUrl('~/Account/ChangeLanguage'),
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
            return appSettings.SelectedLanguage;
        },
        getSelectedLanguageCode: function () {
            var selectedLanguage = language.getSelectedLanguage();
            var selectedLanguageCode = selectedLanguage.substr(0, 2);
            if (selectedLanguageCode === 'zh') {
                selectedLanguageCode = selectedLanguage;
            }
            return selectedLanguageCode;
        },
        getString: function (englishName) {
            if (englishName == null || englishName === undefined) {
                return englishName;
            }
            
            return language.getResourceString(language.Las, englishName);
        },
        getSportName: function (sportTypeId) {
            return language.getResourceString(language.SportType, sportTypeId);
        },
        getBetTypeName: function (betTypeId) {
            return language.getResourceString(language.BetType, betTypeId);
        },
        getResourceString: function (object, key) {
            var langObj = object[language.getSelectedLanguage()];
            var value = langObj[key.toString()];
            if (!value) {
                value = object[defaultLanguageCode];
            }
            return value;
        }
    };
} (jQuery));
$(function () {
    'use strict';
    language.init();
});