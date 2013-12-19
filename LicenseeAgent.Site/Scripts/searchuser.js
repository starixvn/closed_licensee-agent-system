var searchUser = function ($) {
    return {
        init: function () {
            
            var options = {
                size: 'mini',
                currentPage: 1,
                numberOfPages: 3,
                totalPages: 10,
                pageUrl: function () {},
                onPageChanged: null
            };

            $('#paging').bootstrapPaginator(options);
        }
    }
} (jQuery);

$(function () {
    searchUser.init();
});