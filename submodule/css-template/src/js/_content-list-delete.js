//https://stackoverflow.com/questions/1318076/jquery-hasattr-checking-to-see-if-there-is-an-attribute-on-an-element

this.deleteContentListItem = function (btn) {
  $(btn).on("click", function (e) {
    e.preventDefault();

    var disableAttr = $(this).attr("disabled");
    if (typeof disableAttr !== typeof undefined && disableAttr !== false) {
      return;
    }

    $(this).attr("disabled", "disabled");

    var row = $(e.target).closest(".content-list");
    var url = $(this).attr("href");
    var msg = "<p>Ви впевнені що хочете видалити запис?</p>";

    self.createDialog(e, msg, confirmDialog, refuseDialog);

    function confirmDialog() {
      $.ajax({
        type: "Post",
        url: url,
        complete: function (jqXHR, textStatus, errorThrown) {
          console.log(jqXHR.status);
          console.log(textStatus);
          console.log(errorThrown);
        },
        error: function (data) {
          console.log('error', data);
          $(btn).removeAttr("disabled");
          msg = "<p>Помилка видалення</p>";
          self.createDialog(e, msg, refuseDialog);
        },
        success: function (data) {
          console.log('success', data);
          $(btn).removeAttr("disabled");
          row.remove();
          self.updateNiceScroll();
          if ($('.btn-add-state').length) {
            self.checkBtnState();
          }
          // if (data.success === true) {
          // } else {
          //   $(btn).removeAttr('disabled');
          //   msg = "<p>Помилка видалення</p>";
          //   self.createDialog(e, msg, refuseDialog);
          // }
        }
      });
    }

    function refuseDialog() {
      $(btn).removeAttr("disabled");
    }
  });
};

this.setDeleteContentListItem = function (el) {
  $(el)
    .find(".content-list-delete a")
    .each(function () {
      self.deleteContentListItem($(this));
    });
};

if ($("div").is(".content-list-delete")) {
  self.setDeleteContentListItem(body);
}

this.checkBtnState = function () {
  var urlBtnAddState = 'ApplicationSetting/IsPossibleToAddNew';

  checkState(urlBtnAddState);

  function checkState(url) {
    $.ajax({
      type: "Post",
      url: url,
      error: function (data) {},
      success: function (data) {
        if (data) {
          enableBtn();
        } else {
          disableBtn();
        }
      }
    });
  }
  var btnAdd = $(rightBtnMenu).find('.btn-add-state');

  function disableBtn() {
    $(btnAdd).addClass("btn-disabled");
  }

  function enableBtn() {
    $(btnAdd).removeClass("btn-disabled");
  }
}