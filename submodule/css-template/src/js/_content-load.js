this.tryLoadContainer = function (element, url) {
  self.showLoader(element);
  if (element) {
    self.updateNiceScroll();
    $(element).each(function (index, value) {
      var cUrl = url ? url : $(value).attr("data-tab-container-url");
      var replace = $(value).attr("data-replace");
      if (cUrl.length) {
        $.ajax({
          type: "Get",
          url: cUrl,
          success: function (data) {
            if (replace === "") { //todo доработать в случае, если замены нне предусматривается
              $.when($(value).replaceWith(data))
                .done(self.initMask(element), self.findFormatDateParagraph(element))
                .done(self.findGrids(element))
                .done(self.findPaging(element))
                .done(
                  function () {
                    self.manageOpenTab();
                    self.showHideFooter();
                    self.updateNiceScroll();
                    self.hideLoader(element);
                    self.setToolTipsForChild(element);
                    self.findValidateForm(element);
                    self.setDeleteContentListItem(element);
                    self.manageContentReload(element);
                    self.initTextareaAfterLoad();
                  }
                );
            } else {
              $.when($(value).html(data))
                .done(self.initMask(element), self.findFormatDateParagraph(element))
                .done(self.findGrids(element))
                .done(self.createCheckboxLabel(element))
                .done(self.findPaging(element))
                .done(
                  function () {
                    self.manageOpenTab();
                    self.showHideFooter();
                    self.updateNiceScroll();
                    self.hideLoader(element);
                    self.setToolTipsForChild(element);
                    self.findValidateForm(element);
                    self.setDeleteContentListItem(element);
                    self.manageContentReload(element);
                    self.initTextareaAfterLoad();
                    self.activateModalBtn($(this));
                  }
                );
            }
            //self.tryLoadContainer($($(value)[0]));
            //self.updateNiceScroll();

            return true;
          },
          error: function () {
            $(value).html('');
            self.hideLoader(element);
            self.closeModal();
            msg = "<p>Помилка! Перезагрузіть сторінку.</p>";
            self.createDialog(true, msg);
            return true;
          }
        });
      }
    });
  }
};