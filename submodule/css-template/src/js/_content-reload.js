this.onAjaxComplete = function(xhr, status, el, url) {
  var typeStatus, errorsMsg, currError, errorsArr, validationEl, invalidMsg;
  errorsArr = [];
  typeStatus = JSON.parse(xhr.responseText);
  if (status === "success" && typeStatus.success === true) {
    self.getAjaxContainer(el, url);
  } else if (status === "success" && typeStatus.success === false) {
    for (key in typeStatus.errors) {
      currError = typeStatus.errors[key];
      invalidMsg = "<li>" + currError.item1 + " " + currError.item2 + "</li>";
      errorsArr.push(invalidMsg);
    }
    errorsMsg = errorsArr.join("");
    showValidationErrors(el, errorsMsg);
  }

  function showValidationErrors(element, msg) {
    validationEl = $(element).find(".validation-summary-errors ul");
    validationEl.html("" + msg + "");
  }
};

this.getAjaxContainer = function(el, url) {
  self.showLoader();
  el.load(url, function() {
    self.manageContentReload(el);
  });
};

this.manageContentReload = function(el) {
  //init template elements&components after reload
  self.findGrids(el);
  self.createCheckboxLabel(el);
  self.initTextareaAfterLoad();
  self.setDeleteContentListItem(el);

  if ($(el).find("data-pgparams")) {
    $("[data-pgparams]").each(function () {
      self.createPaging($(el));
    });
  }
  if ($(el).find("select").length) {
    $(el)
      .find("select")
      .not(".multiselect")
      .not(".standard-select")
      .each(function() {
        $(this).addClass("select");
        self.selectWrap($(this));
      });
  }

  if ($("input").is(".input")) {
    $(".input").each(function() {
      self.initInputAfterLoad($(this));
    });
  }
  if ($("input").is(".input")) {
    $(".input").each(function() {
      self.onLoadCheckAllInputs($(this));
    });
  }

  if ($("input").is(".datepicker")) {
    $(".datepicker").each(function() {
      self.createDatePicker($(this));
    });
  }
  if ($(el).find("[multiple].multiselect").length) {
    $(el)
      .find("[multiple].multiselect")
      .each(function() {
        $("[multiple].multiselect").each(function() {
          var selectPlaceholder = $(this).attr("data-placeholder");
          $(this).multiselect({
            columns: 1,
            selectAll: true,
            texts: {
              placeholder: selectPlaceholder,
              selectAll: "Обрати все",
              unselectAll: "Видалити всі позначки"
            }
          });
          self.setLabelMultiSelect($(this));
        });
      });
  }
  self.updateNiceScroll(el);
  self.hideLoader();
};
