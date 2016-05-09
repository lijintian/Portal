function BindPermissionChange(containerSelector,permissionsSelector) {
    $(containerSelector).delegate('input[type="checkbox"]', 'change', function () { $.fn.ChangePermission($(this), permissionsSelector); });
}

function ShowOrHide(obj)
{
    if ($(obj).hasClass("fa fa-chevron-down")) {
        $(obj).removeClass("fa fa-chevron-down");
        $(obj).addClass("fa fa-chevron-up");
        $(obj).parents('.level1').find('.level2').slideDown(300);
    }
    else {
        $(obj).removeClass("fa fa-chevron-up");
        $(obj).addClass("fa fa-chevron-down");
        $(obj).parents('.level1').find('.level2').slideUp(300);
    }
}