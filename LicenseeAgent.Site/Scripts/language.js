/*global appSettings, CORE, fanexGridLanguage*/
'use strict';

var LANGUAGE = (function ($this, $) {
    var defaultLanguageCode = 'en-US';

    var getResourceString = function (object, key) {
        var langObj = object[$this.getSelectedLanguage()];
        var value = langObj[key.toString()];
        if (!value) {
            value = object[defaultLanguageCode][key];
            if (!value) {
                value = key;
            }
        }
        return value;
    };

    $this.init = function () {
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
            $this.changeLanguage(languageCode);
        });

        var selectedLanguage = $this.getSelectedLanguage();
        if (typeof fanexGridLanguage !== 'undefined') {
            window.fanexGridCurrentLanguage = fanexGridLanguage[selectedLanguage];
        }
    };

    $this.changeLanguage = function (language) {
        var data = {
            Language: language
        };
        $.ajax({
            type: "POST",
            data: $.param(data),
            url: CORE.resolveUrl('~/Account/ChangeLanguage'),
            dataType: "json",
            async: false,
            timeout: 20000
        }).done(function (result) {
            if (result.ErrCode !== 0) {
                alert(result.ErrMsg);
                return;
            }
            location.reload();
        });
    };

    $this.getSelectedLanguage = function () {
        return appSettings.SelectedLanguage;
    };

    $this.getSelectedLanguageCode = function () {
        var selectedLanguage = $this.getSelectedLanguage();
        var selectedLanguageCode = selectedLanguage.substr(0, 2);
        if (selectedLanguageCode === 'zh') {
            selectedLanguageCode = selectedLanguage;
        }
        return selectedLanguageCode;
    };

    $this.getString = function (englishName) {
        if (englishName == null || englishName === undefined) {
            return englishName;
        }

        return getResourceString(LANGUAGE.Las, englishName);
    };

    $this.getFormatString = function (englishName) {
        var value = LANGUAGE.getString(englishName);
        var args = Array.prototype.slice.call(arguments, 1);
        return String.prototype.format.apply(value, args);
    };

    $this.getSportName = function (sportTypeId) {
        return getResourceString(LANGUAGE.SportType, sportTypeId);
    };

    $this.getBetTypeName = function (betTypeId) {
        return getResourceString(LANGUAGE.BetType, betTypeId);
    };

    return $this;
} (LANGUAGE || {}, jQuery));

$(function () {
    LANGUAGE.init();
});