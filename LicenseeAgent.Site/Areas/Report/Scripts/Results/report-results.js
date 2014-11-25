/*global core */
"use strict";

var report = report || {};
report.results = (function ($this, $) {
    var generateSelect = function (element, array, valueField, textField) {
        $(element).html('');
        $.each(array, function () {
            var value = this[valueField];
            var text = this[textField];
            var optionTag = '<option value="{0}">{1}</option>'.format(value, text);
            $(element).append(optionTag);
        });
        $(element).selectpicker('refresh');
    };

    var submitForm = function (form) {
        var sportSelect = $(form).find('#Sport');
        var leagueSelect = $(form).find('#League');
        core.ajaxJSON({
            url: $(form).attr('action'),
            data: $(form).serializeObject()
        }, function (response) {
            if (response.Sports) {
                generateSelect(sportSelect, response.Sports, 'SportType', 'SportName');
            }
            if (response.Leagues) {
                generateSelect(leagueSelect, response.Leagues, 'LeagueId', 'LeagueName');
            }
            $('#ResultsArea').html(response.Results);
        });
    };

    $this.init = function () {
        $('.selectpicker').selectpicker();
        $('.datepicker').each(function () {
            var startDate = $(this).data('start-date');
            $(this).datepicker({
                startDate: startDate
            });
        });

        $('a[data-toggle="tab"]').on('shown', function (e) {
            var form;
            if ($(e.target).attr('href') === "#filter-tab-outright") {
                form = $('#filter-tab-outright form');
            }
            else {
                form = $('#filter-tab-match form');
            }
            submitForm(form);
        });

        $('select[name=Sport]').change(function () {
            var form = $(this).closest('form');
            form.find('#League').val(0);
            submitForm(form);
        });

        $('select[name=League]').change(function () {
            var form = $(this).closest('form');
            submitForm(form);
        });

        $('button[data-date]').click(function () {
            var date = $(this).data('date');
            var form = $(this).closest('form');
            form.find('.datepicker').datepicker('update', date);
            form.submit();
        });

        $('form').submit(function (e) {
            e.preventDefault();
            var form = $(this);
            var sportSelect = $(form).find('#Sport');
            sportSelect.html('');
            sportSelect.selectpicker('refresh');
            var leagueSelect = $(form).find('#League');
            leagueSelect.html('');
            leagueSelect.selectpicker('refresh');
            submitForm(form);
        });

        $('form:eq(0)').submit();
    };

    return $this;
} (report.results || {}, jQuery));