this.createPaging = function (el) {
  var pagingContainer = $(el).find('[data-pgparams]');

  if (!pagingContainer)
    return;

  var pgparams = pagingContainer.data('pgparams');
  if (pgparams) {
    if ($(el).find('.pagination')) {
      $(el).find('.pagination a').each(function () {
        var link = $(this).attr('href') + '&' + pgparams;
        $(this).on('click', function (e) {
          e.preventDefault();
          self.showLoader(el);
          self.doPaging(link, el);
          self.updateNiceScroll();
        })
      })
    }
    if ($(el).find('.content-list-sortable')) {
      $(el).find('.content-list-sortable a').each(function () {
        var link = $(this).attr('href') + '&' + pgparams;
        $(this).on('click', function (e) {
          
          e.preventDefault();
          e.stopPropagation();

          self.showLoader(el);
          self.doPaging(link, el);
          self.updateNiceScroll();
        })
      })
    }
  }
}


this.doPaging = function (link, el) {
  $.ajax({
    url: link,
    type: "GET",
    success: function (data) {
      $.when(self.clearPaging(el))
        .done($(el).html(data))
        .done(self.createPaging(el))
        .done(self.findGrids($(el)), self.initMask(el))
        .done(self.setDeleteContentListItem($(el)))
        .done(self.createCheckboxLabel(el))
        .done(self.updateNiceScroll())
        .done(self.hideLoader(el));
    },
    error: function (data) {
      console.error('doPaging error', data)
    }
  });

}



this.clearPaging = function (el) {
  var pagingParent;
  if ($(el).data('ajax-update')) {
    pagingParent = $(el).data('ajax-update');
  } else {
    pagingParent = $(el);
  }
  self.clearElEventListeners(pagingParent);
}


this.findPaging = function () {
  if ($('*').is('[data-pgparams]')) {
    $('[data-pgparams]').each(function () {
      self.createPaging($(this));
    })
  }
}