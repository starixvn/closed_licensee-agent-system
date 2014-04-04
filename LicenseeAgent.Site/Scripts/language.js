/*global appSettings, fanexGridLanguage, fanexGridCurrentLanguage:true, Las_en_US*/

var language = (function($) {
    'use strict';
    return {
        init: function() {
            $('#nav li').hover(function() {
                    //show its submenu
                    $('ul', this).stop().slideDown(100);
                }, function() {
                    //hide its submenu
                    $('ul', this).stop().slideUp(100);
                }
            );

            $('#languages li').click(function() {
                var languageCode = $(this).attr('data-language');
                language.changeLanguage(languageCode);
            });

            var selectedLanguage = language.getSelectedLanguage();
            if (typeof fanexGridLanguage !== 'undefined') {
                fanexGridCurrentLanguage = fanexGridLanguage[selectedLanguage];
            }
        },
        changeLanguage: function(language) {
            var data = {
                Language: language
            };
            $.ajax({
                    type: "POST",
                    data: $.param(data),
                    url: $('.icon-language-arrow').data('href'),
                    dataType: "json",
                    async: false,
                    timeout: 20000
                })
                .done(function(result) {
                    if (result.isChanged) {
                        location.reload();
                    }
                });
        },
        getSelectedLanguage: function() {
            return appSettings.SelectedLanguage;
        },
        getSelectedLanguageCode: function() {
            var selectedLanguage = language.getSelectedLanguage();
            var selectedLanguageCode = selectedLanguage.substr(0, 2);
            if (selectedLanguageCode === 'zh') {
                selectedLanguageCode = selectedLanguage;
            }
            return selectedLanguageCode;
        },
        getString: function (enghlishName) {
            /* jshint camelcase: false */
            if (enghlishName == null || enghlishName === undefined) {
                return enghlishName;
            }
            var keyName = "Las_" + language.getSelectedLanguage().replace("-", "_");

            var resources = window[keyName];
            if (!resources) {
                resources = Las_en_US;
            }

            var mapResource = function(key) {
                var item = resources[key];
                if (item === undefined) {
                    item = Las_en_US[key];
                    return item !== undefined ? item : key;
                } else {
                    return item;
                }
            };
            if (enghlishName.indexOf("@") > -1) {
                var keys = enghlishName.match(/\@{1}\[{1}[a-zA-Z\d\_]+\]{1}/g);
                if (keys != null) {
                    var temp = enghlishName;
                    for (var i = 0; i < keys.length; i++) {
                        var realKey = keys[i].replace("@[", "").replace("]", "");
                        temp = temp.replace(keys[i], mapResource(realKey));
                    }
                    return temp;
                }
                return mapResource(enghlishName);
            } else {
                return mapResource(enghlishName);
            }
        }
    };
}(jQuery));
$(function () {
    'use strict';
    language.init();
});