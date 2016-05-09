$('span.field-validation-valid, span.field-validation-error').each(function () {
    $(this).addClass('help-inline');
});

$('form').submit(function () {
    if ($(this).valid()) {
        $(this).find('div.control-group').each(function () {
            if ($(this).find('span.field-validation-error').length == 0) {
                $(this).removeClass('error');
            }
        });
    }
    else {
        $(this).find('div.control-group').each(function () {
            if ($(this).find('span.field-validation-error').length > 0) {
                $(this).addClass('error');
            }
        });
    }
});

$('form').each(function () {
    $(this).find('div.control-group').each(function () {
        if ($(this).find('span.field-validation-error').length > 0) {
            $(this).addClass('error');
        }
    });
});

$('div.validation-summary-errors').each(function () {
    $(this).find('ul').each(function () {
        if ($(this).find('li').length > 0 && $(this).find('li').text().length > 0) {
            $('div.validation-summary-errors').addClass("alert alert-block");
        }
    });
});

if ($.validator) {
    $.validator.setDefaults({
        highlight: function (element) {
            $(element).closest('.control-group').addClass('error');
        },
        unhighlight: function (element) {
            $(element).closest('.control-group').removeClass('error');
        }
    });
}