this.showLoader = function (el) {
     var curElParent = $(el).parent();
     if (!curElParent.find('.loader-wrapper').length) {
          curElParent.addClass('loader-parent');
          curElParent.append('<div class="loader-wrapper"><div class="loader"></div></div>');
     } else {
          $('.loader-wrapper').remove();
          curElParent.removeClass('loader-parent');
     }
}

this.hideLoader = function (el) {
     var curElParent = $(el).parent();
     if ($(curElParent).hasClass('loader-parent')) {
          $('.loader-wrapper').remove();
          curElParent.removeClass('loader-parent');
     }
}