this.createAccordion = function (options) {
    var settings = $.extend({
        speed: 300,
        accordionItemSelector: ".accordion-item",
        accordionItemHeaderSelector: ".accordion-item-header",
        accordionItemContentSelector: ".accordion-item-content"
    }, options);
    var $this = $(this);

    $(document).on('click', settings.accordionItemSelector, function (e) {
        var $this = $(this);

        if (!$this.hasClass("active")) {

            $this.find(settings.accordionItemContentSelector).slideDown(settings.speed);
            $this.addClass("active");

        } else {
            $this.find(settings.accordionItemContentSelector).slideUp(settings.speed);
            $this.removeClass("active");
        }
        setTimeout(function () {
            self.updateNiceScroll($this);
        }, 300);
    });
    $(document).on('click', settings.accordionItemHeaderSelector + " a", function (e) {
        e.stopPropagation();
    });
    $(document).on('click', settings.accordionItemContentSelector, function (e) {
        e.stopPropagation();
    });
    $(document).keypress(function (e) {
        if (e.which == 13 && $(e.target).parents($this)) {
            $(e.target).trigger('click');
        }
    });
}

if ($('.accordion').is(':not(.not-active)')) {
    $('.accordion').each(function () {
        self.createAccordion($(".accordion"));
    })
}